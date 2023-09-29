using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableImage : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Vector2 localMousePos;
    private Vector3 planeLocalPos;
    private RectTransform targetObject;
    private RectTransform parentRectTransform;
    private RectTransform targetRectTransform;

    [Header("画框移动限制，坐标起始点屏幕中心")] public Vector2 MovementRestrictionsMin;
    public Vector2 MovementRestrictionsMax;


    void Start()
    {
        targetObject = this.transform.parent.parent.GetComponent<RectTransform>();
        targetRectTransform = targetObject;
        parentRectTransform = targetObject.parent as RectTransform;

        if (MovementRestrictionsMin == Vector2.zero)
            MovementRestrictionsMin = new Vector2(-Screen.width, -Screen.height);
        if (MovementRestrictionsMax == Vector2.zero)
            MovementRestrictionsMax = new Vector2(Screen.width, Screen.height);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        planeLocalPos = targetRectTransform.localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, eventData.position,
            eventData.pressEventCamera, out localMousePos);
        targetObject.gameObject.transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPointerPosition;
        //屏幕点到矩形中的局部点
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, eventData.position,
                eventData.pressEventCamera, out localPointerPosition))
        {
            Vector3 offsetToOriginal = localPointerPosition - localMousePos;
            var position = planeLocalPos + offsetToOriginal;

            position.x = Mathf.Clamp(position.x, MovementRestrictionsMin.x, MovementRestrictionsMax.x);
            position.y = Mathf.Clamp(position.y, MovementRestrictionsMin.y, MovementRestrictionsMax.y);

            targetObject.localPosition = position;
        }
    }
}