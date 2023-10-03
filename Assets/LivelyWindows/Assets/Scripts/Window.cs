using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace LivelyWindows
{
	[Serializable]
	public class WindowProperties
	{
		[Header("Setup")]
		[Tooltip("到帧图像开始位置的偏移（以像素为单位）。 某些 9 宫格切片图像的边缘可能会出现透明阴影")]
		public float FrameOffset = 0;

		[Tooltip("当允许调整大小属性设置为 true 时，可以抓取的边框厚度（以像素为单位）")]
		public float FrameThickness = 4;

		[Tooltip("窗口的最小尺寸（以像素为单位）")]
		public Vector2 MinSize = new Vector2(200, 100);

		public Vector2 MaxSize = new Vector2(2000, 1000);
		
		public Vector2 MinPos = Vector2.zero;

		public Vector2 MaxPos = Vector2.zero;

		[Header("Movement")]
		[Tooltip("如果可以通过抓取标题来移动面板 (RectTransform)，则为 true。 确保标题高度大于零")]
		public bool AllowMoveByCaption;
		
		[Tooltip("当“AllowMoveByCaption”属性设置为 true 时，可以抓取的顶部标题的厚度（以像素为单位）")]
		public float CaptionHeight = 0;

		[Header("Resize")]
		[Tooltip("如果面板 (RectTransform) 可以在任何方向上调整大小，则为 true")]
		public bool AllowResize;
	}

	[RequireComponent(typeof(RectTransform), typeof(EventTrigger))]
	public class Window : MonoBehaviour
	{

		public WindowProperties Properties;

		private Canvas _canvas;
		public Canvas Canvas { get { if (!_canvas) _canvas = GetComponentInParent<Canvas>(); return _canvas; } }

		private EventTrigger _eventTrigger;
		public EventTrigger EventTrigger { get { if (!_eventTrigger) _eventTrigger = GetComponent<EventTrigger>(); return _eventTrigger; } }

		private RectTransform _rectTransform;
		public RectTransform RectTransform { get { if (!_rectTransform) _rectTransform = GetComponent<RectTransform>(); return _rectTransform; } }

		private Vector3 _correctedLossyScale;
		public Vector3 CorrectedLossyScale
		{
			get
			{
				if (_correctedLossyScale == Vector3.zero)
					_correctedLossyScale = Canvas.CorrectLossyScale();
				return _correctedLossyScale;
			}
		}

		private Vector3[] _frameOuterCorners;
		public Vector3[] FrameOuterCorners
		{
			get
			{
				if (_frameOuterCorners == null || _frameOuterCorners.Length != 4)
				{
					_frameOuterCorners = new Vector3[4];
					RectTransform.GetScreenCorners(_frameOuterCorners, Canvas, Properties.FrameOffset);
				}
				return _frameOuterCorners;
			}
		}

		private Vector3[] _frameInnerCorners;
		public Vector3[] FrameInnerCorners
		{
			get
			{
				if (_frameInnerCorners == null || _frameInnerCorners.Length != 4)
				{
					_frameInnerCorners = new Vector3[4];
					RectTransform.GetScreenCorners(_frameInnerCorners, Canvas, Properties.FrameOffset + Properties.FrameThickness);
				}
				return _frameInnerCorners;
			}
		}

		private Vector3[] _captionCorners;
		public Vector3[] CaptionCorners
		{
			get
			{
				if (_captionCorners == null || _captionCorners.Length != 4)
				{
					_captionCorners = new Vector3[4];
					for (int i = 0; i < 4; i++)
						_captionCorners[i] = FrameInnerCorners[i];

					_captionCorners[0].y = _captionCorners[1].y - Properties.CaptionHeight * CorrectedLossyScale.y;
					_captionCorners[3].y = _captionCorners[2].y - Properties.CaptionHeight * CorrectedLossyScale.y;
				}
				return _captionCorners;
			}
		}

		private bool IsProcessingCursorChanges;
		private bool IsProcessingMovement;
		private bool IsProcessingResize;

		private bool IsResizingOverTop;
		private bool IsResizingOverBottom;
		private bool IsResizingOverLeft;
		private bool IsResizingOverRight;

		private Vector3 LastScreenCursorPosition;

		private void Start()
		{
			if (Properties.MaxPos == Vector2.zero)
			{
				Properties.MaxPos = new Vector2(Canvas.pixelRect.width, Canvas.pixelRect.height);
			}
			
			EventTrigger.Entry entry;

			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerEnter;
			entry.callback.AddListener((d) => { OnPointerEnter(d as PointerEventData); });
			EventTrigger.triggers.Add(entry);

			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerExit;
			entry.callback.AddListener((d) => { OnPointerExit(d as PointerEventData); });
			EventTrigger.triggers.Add(entry);

			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener((d) => { OnPointerDown(d as PointerEventData); });
			EventTrigger.triggers.Add(entry);

			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerUp;
			entry.callback.AddListener((d) => { OnPointerUp(d as PointerEventData); });
			EventTrigger.triggers.Add(entry);

			RecalculatePositionCaches();
		}

		private void OnValidate()
		{
			RecalculatePositionCaches();
		}

		private void RecalculatePositionCaches()
		{
			_frameInnerCorners = null;
			_frameOuterCorners = null;
			_captionCorners = null;
		}

		private void Update()
		{
			if (IsProcessingCursorChanges)
				ProcessCursorChanges();
			if (IsProcessingMovement)
				ProcessMovement();
			if (IsProcessingResize)
				ProcessResize();

			if (IsProcessingCursorChanges || IsProcessingMovement || IsProcessingResize)
			{
				LastScreenCursorPosition = Input.mousePosition;
				LastScreenCursorPosition.x = Mathf.Clamp(LastScreenCursorPosition.x, 0, Canvas.pixelRect.width);
				LastScreenCursorPosition.y = Mathf.Clamp(LastScreenCursorPosition.y, 0, Canvas.pixelRect.height);
			}

		}

		private void OnPointerEnter(PointerEventData data)
		{
			IsProcessingCursorChanges = !Input.GetMouseButton(0);
		}

		private void OnPointerExit(PointerEventData data)
		{
			IsProcessingCursorChanges = false;
			if (!IsProcessingResize && !Input.GetMouseButton(0))
				Cursors.Instance.SetCursor(Cursors.Instance.Arrow);
		}

		private void OnPointerDown(PointerEventData data)
		{
			// bring us to the front
			transform.SetAsLastSibling();

			IsProcessingCursorChanges = false;
			IsProcessingMovement = Properties.AllowMoveByCaption && CursorOverCaption();

			if (!IsProcessingMovement)
			{
				IsResizingOverTop = Properties.AllowResize && CursorOverTopBorder();
				IsResizingOverBottom = Properties.AllowResize && CursorOverBottomBorder();
				IsResizingOverLeft = Properties.AllowResize && CursorOverLeftBorder();
				IsResizingOverRight = Properties.AllowResize && CursorOverRightBorder();
			}

			IsProcessingResize = IsResizingOverTop || IsResizingOverBottom || IsResizingOverLeft || IsResizingOverRight;
		}

		private void OnPointerUp(PointerEventData data)
		{
			RecalculatePositionCaches();

			IsProcessingCursorChanges = true;
			IsProcessingMovement = false;
			IsProcessingResize = false;
			IsResizingOverTop = false;
			IsResizingOverBottom = false;
			IsResizingOverLeft = false;
			IsResizingOverRight = false;
		}

		private void ProcessCursorChanges()
		{
			if (Properties.AllowMoveByCaption && CursorOverCaption())
				Cursors.Instance.SetCursor(Cursors.Instance.Move);
			else if (Properties.AllowResize && ((CursorOverTopBorder() && CursorOverLeftBorder()) || (CursorOverBottomBorder() && CursorOverRightBorder())))
				Cursors.Instance.SetCursor(Cursors.Instance.ResizeNWSE);
			else if (Properties.AllowResize && ((CursorOverTopBorder() && CursorOverRightBorder()) || (CursorOverBottomBorder() && CursorOverLeftBorder())))
				Cursors.Instance.SetCursor(Cursors.Instance.ResizeNESW);
			else if (Properties.AllowResize && (CursorOverTopBorder() || CursorOverBottomBorder()))
				Cursors.Instance.SetCursor(Cursors.Instance.ResizeNS);
			else if (Properties.AllowResize && (CursorOverLeftBorder() || CursorOverRightBorder()))
				Cursors.Instance.SetCursor(Cursors.Instance.ResizeWE);
			else
				Cursors.Instance.SetCursor(Cursors.Instance.Arrow);
		}

		private void ProcessMovement()
		{
			var mousePosition = Input.mousePosition;
			mousePosition.x = Mathf.Clamp(mousePosition.x, 0, Canvas.pixelRect.width);
			mousePosition.y = Mathf.Clamp(mousePosition.y, 0, Canvas.pixelRect.height);

			var lastMousePosition = LastScreenCursorPosition;

			if (Canvas.renderMode == RenderMode.ScreenSpaceCamera)
			{
				mousePosition = Canvas.worldCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Canvas.planeDistance));
				lastMousePosition = Canvas.worldCamera.ScreenToWorldPoint(new Vector3(lastMousePosition.x, lastMousePosition.y, Canvas.planeDistance));
			}

			RectTransform.Translate(mousePosition - lastMousePosition);
		}

		private void ProcessResize()
		{
			var mousePosition = Input.mousePosition;
			mousePosition.x = Mathf.Clamp(mousePosition.x, Properties.MinPos.x, Properties.MaxPos.x);
			mousePosition.y = Mathf.Clamp(mousePosition.y, Properties.MinPos.y, Properties.MaxPos.y);
			var lastMousePosition = LastScreenCursorPosition;

			var deltaPixels = mousePosition - lastMousePosition;
			if (deltaPixels.x == 0 && deltaPixels.y == 0)
				return;

			// if in camera space then the 
			// RectTransform.position must be adjusted in world coordinates
			// RectTransform.size is still in pixel space
			if (Canvas.renderMode == RenderMode.ScreenSpaceCamera)
			{
				mousePosition = Canvas.worldCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Canvas.planeDistance));
				lastMousePosition = Canvas.worldCamera.ScreenToWorldPoint(new Vector3(lastMousePosition.x, lastMousePosition.y, Canvas.planeDistance));
			}

			var deltaWorldUnits = mousePosition - lastMousePosition;
			var rectPosition = RectTransform.position;
			var rectSize = RectTransform.sizeDelta;
			var size = RectTransform.rect.size;
			var changeInPosition = Vector3.zero;
			var changeInSize = Vector2.zero;

			// if anchor min/max are != then x/sizeX represent (X=offset in pixels rightward to the left border)			(sizeX=offset in pixels leftward to the right border)
			// if anchor min/max are == then x/sizeX represent (X=offset in pixels to the anchor X)		(sizeX=width in pixels of the element)

			if (IsResizingOverTop || IsResizingOverBottom)
			{
				if (RectTransform.anchorMin.y == RectTransform.anchorMax.y)
				{
					if (IsResizingOverTop)
					{
						// accounts for going to far down
						if (rectSize.y + deltaPixels.y > Properties.MinSize.y && rectSize.y + deltaPixels.y < Properties.MaxSize.y)
						{
							//deltaPixels.y = Properties.MinSize.y - size.y;

							// accounts for the way back up making sure the cursor is above
							if (!CursorOverTopBorder(true))
							{
								deltaWorldUnits.y = 0;
								deltaPixels.y = 0;
							}

							changeInPosition.y = deltaWorldUnits.y * RectTransform.pivot.y;
							changeInSize.y = deltaPixels.y;
						}
					}
					else if (IsResizingOverBottom)
					{
						// accounts for going to far down
						if (rectSize.y - deltaPixels.y > Properties.MinSize.y && rectSize.y - deltaPixels.y < Properties.MaxSize.y)
						{
							//deltaPixels.y = -(Properties.MinSize.y - size.y);

							// accounts for the way back down making sure the cursor is below
							if (!CursorOverBottomBorder(true))
							{
								deltaWorldUnits.y = 0;
								deltaPixels.y = 0;
							}

							changeInPosition.y = deltaWorldUnits.y * (1 - RectTransform.pivot.y);
							changeInSize.y = -deltaPixels.y;
						}
					}

					rectPosition.y += changeInPosition.y;
					rectSize.y += changeInSize.y / CorrectedLossyScale.y;
				}
				else
				{
					if (IsResizingOverTop)
					{
						// accounts for going to far down
						if (size.y + deltaPixels.y > Properties.MinSize.y && size.y + deltaPixels.y < Properties.MaxSize.y)
						{
							//deltaPixels.y = Properties.MinSize.y - size.y;

							// accounts for the way back up making sure the cursor is above
							if (!CursorOverTopBorder(true))
							{
								deltaWorldUnits.y = 0;
								deltaPixels.y = 0;
							}

							changeInPosition.y = deltaWorldUnits.y * RectTransform.pivot.y;
							changeInSize.y = deltaPixels.y;
						}
					}
					else if (IsResizingOverBottom)
					{
						// accounts for going to far down
						if (size.y - deltaPixels.y > Properties.MinSize.y && size.y - deltaPixels.y < Properties.MaxSize.y)
						{
							//deltaPixels.y = -(Properties.MinSize.y - size.y);

							// accounts for the way back down making sure the cursor is below
							if (!CursorOverBottomBorder(true))
							{
								deltaWorldUnits.y = 0;
								deltaPixels.y = 0;
							}

							changeInPosition.y = deltaWorldUnits.y * (1 - RectTransform.pivot.y);
							changeInSize.y = -deltaPixels.y;
						}
					}

					rectPosition.y += changeInPosition.y;
					rectSize.y += changeInSize.y / CorrectedLossyScale.y;
				}
			}

			if (IsResizingOverRight || IsResizingOverLeft)
			{
				if (RectTransform.anchorMin.x == RectTransform.anchorMax.x)
				{
					if (IsResizingOverRight)
					{
						// accounts for going to far down
						if (rectSize.x + deltaPixels.x > Properties.MinSize.x  && rectSize.x + deltaPixels.x < Properties.MaxSize.x)
						{
							//deltaPixels.x = Properties.MinSize.x - size.x;

							// accounts for the way back up making sure the cursor is above
							if (!CursorOverRightBorder(true))
							{
								deltaWorldUnits.x = 0;
								deltaPixels.x = 0;
							}

							changeInPosition.x = deltaWorldUnits.x * RectTransform.pivot.x;
							changeInSize.x = deltaPixels.x;
						}
					}
					else if (IsResizingOverLeft)
					{
						// accounts for going to far down
						if (rectSize.x - deltaPixels.x > Properties.MinSize.x && rectSize.x - deltaPixels.x < Properties.MaxSize.x)
						{
							//deltaPixels.x = -(Properties.MinSize.x - size.x);

							// accounts for the way back down making sure the cursor is below
							if (!CursorOverLeftBorder(true))
							{
								deltaWorldUnits.x = 0;
								deltaPixels.x = 0;
							}

							changeInPosition.x = deltaWorldUnits.x * (1 - RectTransform.pivot.x);
							changeInSize.x = -deltaPixels.x;
						}
					}

					rectPosition.x += changeInPosition.x;
					rectSize.x += changeInSize.x / CorrectedLossyScale.x;
				}
				else
				{
					if (IsResizingOverRight)
					{
						// accounts for going to far down

						if (size.x + deltaPixels.x > Properties.MinSize.x && size.x + deltaPixels.x < Properties.MaxSize.x)
						{
							//deltaPixels.x = Properties.MinSize.x - size.x;

							// accounts for the way back up making sure the cursor is above
							if (!CursorOverRightBorder(true))
							{
								deltaWorldUnits.x = 0;
								deltaPixels.x = 0;
							}

							changeInPosition.x = deltaWorldUnits.x * RectTransform.pivot.x;
							changeInSize.x = deltaPixels.x;
						}
					}
					else if (IsResizingOverLeft)
					{
						// accounts for going to far down
						if (size.x - deltaPixels.x > Properties.MinSize.x && size.x - deltaPixels.x < Properties.MaxSize.x)
						{
							//deltaPixels.x = -(Properties.MinSize.x - size.x);

							// accounts for the way back down making sure the cursor is below
							if (!CursorOverLeftBorder(true))
							{
								deltaWorldUnits.x = 0;
								deltaPixels.x = 0;
							}

							changeInPosition.x = deltaWorldUnits.x * (1 - RectTransform.pivot.x);
							changeInSize.x = -deltaPixels.x;
						}
					}

					rectPosition.x += changeInPosition.x;
					rectSize.x += changeInSize.x / CorrectedLossyScale.x;
				}
			}

			RectTransform.sizeDelta = rectSize;
			RectTransform.position = rectPosition;
			RecalculatePositionCaches();
		}

		bool CursorOverTopBorder(bool canExceed = false)
		{
			if (canExceed)
				return LastScreenCursorPosition.y >= FrameInnerCorners[1].y;
			else
				return LastScreenCursorPosition.y >= FrameInnerCorners[1].y && LastScreenCursorPosition.y <= FrameOuterCorners[1].y && LastScreenCursorPosition.x >= FrameOuterCorners[0].x && LastScreenCursorPosition.x <= FrameOuterCorners[2].x;
		}

		bool CursorOverBottomBorder(bool canExceed = false)
		{
			if (canExceed)
				return LastScreenCursorPosition.y <= FrameInnerCorners[0].y;
			else
				return LastScreenCursorPosition.y <= FrameInnerCorners[0].y && LastScreenCursorPosition.y >= FrameOuterCorners[0].y && LastScreenCursorPosition.x >= FrameOuterCorners[0].x && LastScreenCursorPosition.x <= FrameOuterCorners[2].x;
		}

		bool CursorOverLeftBorder(bool canExceed = false)
		{
			if (canExceed)
				return LastScreenCursorPosition.x <= FrameInnerCorners[0].x;
			else
				return LastScreenCursorPosition.x <= FrameInnerCorners[0].x && LastScreenCursorPosition.x >= FrameOuterCorners[0].x && LastScreenCursorPosition.y >= FrameOuterCorners[0].y && LastScreenCursorPosition.y <= FrameOuterCorners[1].y;
		}

		bool CursorOverRightBorder(bool canExceed = false)
		{
			if (canExceed)
				return LastScreenCursorPosition.x >= FrameInnerCorners[3].x;
			else
				return LastScreenCursorPosition.x >= FrameInnerCorners[3].x && LastScreenCursorPosition.x <= FrameOuterCorners[3].x && LastScreenCursorPosition.y >= FrameOuterCorners[0].y && LastScreenCursorPosition.y <= FrameOuterCorners[1].y;
		}

		bool CursorOverCaption()
		{
			return LastScreenCursorPosition.x >= CaptionCorners[0].x && LastScreenCursorPosition.x <= CaptionCorners[3].x && LastScreenCursorPosition.y <= CaptionCorners[1].y && LastScreenCursorPosition.y >= CaptionCorners[0].y;
		}

		bool CursorOverWindow()
		{
			return LastScreenCursorPosition.x >= FrameOuterCorners[0].x && LastScreenCursorPosition.x <= FrameOuterCorners[3].x && LastScreenCursorPosition.y >= FrameOuterCorners[0].y && LastScreenCursorPosition.y <= FrameOuterCorners[1].y;
		}
	}
}
