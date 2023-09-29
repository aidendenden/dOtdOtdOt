using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ResizeOther_3_0 : MonoBehaviour {

	public GameObject otherObject;						// Defines the object to be resized
	public bool includeChildren = true;					// Determines if this object's children will be included in the raycast return
	public bool ignoreTextBox = true;					// Determines if any text box will be included in the raycast return
	public bool bringToFront = true;					// Determines if the other object will be set as the last sibling in the hierarchy
	public bool disableResizeOther = false;				// Determines if the ability to resize the other object is enabled or disabled
	public bool TopRight = false;						// Sets the object to be designated as the TopRight control for the other object
	public bool BottomRight = false;					// Sets the object to be designated as the BottomRight control for the other object
	public bool Right = false;							// Sets the object to be designated as the Right control for the other object
	public bool TopLeft = false;						// Sets the object to be designated as the TopLeft control for the other object
	public bool BottomLeft = false;						// Sets the object to be designated as the BottomLeft control for the other object
	public bool Left = false;							// Sets the object to be designated as the Left control for the other object
	public bool Top = false;							// Sets the object to be designated as the Top control for the other object
	public bool Bottom = false;							// Sets the object to be designated as the Bottom control for the other object
	public bool restrainToOtherParent = false;			// Determines if the other object will be restrained fully to its parent
	public float restrainRightInset = 0;				// Sets the distance from the right edge of its parent that the other object will be restrained to
	public float restrainLeftInset = 0;					// Sets the distance from the left edge of its parent that the other object will be restrained to
	public float restrainTopInset = 0;					// Sets the distance from the top edge of its parent that the other object will be restrained to
	public float restrainBottomInset = 0;				// Sets the distance from the bottom edge of its parent that the other object will be restrained to
	public bool intervalBased = false;					// Determines if this other object will resize based on intervals
	public float intervalDistance = 50;					// Sets the size of intervals if the interval method is true
	public float minimumWidth = 0;						// Sets the minimum width the other object may resize to. The minimum width shall be set to no less than twice the thickness of the border
	public float minimumHeight = 0;						// Sets the minimum height the other object may resize to. The minimum height shall be set to no less than twice the thickness of the border
	public bool customCursor = false;					// Determines if custom cursors are used
	public bool bringToFrontOnOver = false;				// Determines if the other object will be set as the last sibling in the hierarchy when the cursor is on this object
	public Texture2D normalCursor;						// The assigned normal custom cursor
	public int xHotspotNormal = 0;						// Sets the hotspot poisiton for the "normal" custom cursor on the x-axis
	public int yHotspotNormal = 0;						// Sets the hotspot poisiton for the "normal" custom cursor on the y-axis
	public Texture2D resizeLeftRightCursor;				// Defines the custom cursor for resizing from the left and right borders
	public int xHotspotLR = 0;							// Sets the hotspot poisiton for the "resizeLeftRight" custom cursor on the x-axis
	public int yHotspotLR = 0;							// Sets the hotspot poisiton for the "resizeLeftRight" custom cursor on the y-axis
	public Texture2D resizeTopBottomCursor;				// Defines the custom cursor for resizing from the top and bottom borders
	public int xHotspotTB = 0;							// Sets the hotspot poisiton for the "resizeTopBottom" custom cursor on the x-axis
	public int yHotspotTB = 0;							// Sets the hotspot poisiton for the "resizeTopBottom" custom cursor on the y-axis
	public Texture2D resizeTopLeftBottomRightCursor;	// Defines the custom cursor for resizing from the top left and bottom right borders
	public int xHotspotTLBR = 0;						// Sets the hotspot poisiton for the "resizeTopLeftBottomRight" custom cursor on the x-axis
	public int yHotspotTLBR = 0;						// Sets the hotspot poisiton for the "resizeTopLeftBottomRight" custom cursor on the y-axis
	public Texture2D resizeTopRightBottomLeftCursor;	// Defines the custom cursor for resizing from the top right and bottom left borders
	public int xHotspotTRBL = 0;						// Sets the hotspot poisiton for the "resizeTopRightBottomLeft" custom cursor on the x-axis
	public int yHotspotTRBL = 0;						// Sets the hotspot poisiton for the "resizeTopRightBottomLeft" custom cursor on the y-axis
	CursorMode cursorMode = CursorMode.Auto;			// The cursor mode
	bool onObject = false;								// Determines if the cursor is on this object when the mouse button is initially pressed
	bool startOnBorder = false;							// Determines if the cursor is on this object when the mouse button is initially pressed
	bool overObject = false;							// Determines if the cursor is on this object
	bool minimumWidthReached = false;					// Determines if the other object has reached the minimum width allowed
	bool minimumHeightReached = false;					// Determines if the other object has reached the minimum height allowed
	bool moveRight = false;								// Determines if the parent will resize from its right side
	bool moveLeft = false;								// Determines if the parent will resize from its left side
	bool moveTop = false;								// Determines if the parent will resize from its top
	bool moveBottom = false;							// Determines if the parent will resize from its bottom
	bool enteredObject = false;							// Determines if the cursor has entered or exited this object. This is important for changing back to a normal/null cursor only once after the cursor leaves a movable position 
	float oldMouseX;									// Stores the cursor position on the x-axis for comparison with the newer "mouseX" variable
	float oldMouseY;									// Stores the cursor position on the y-axis for comparison with the newer "mouseY" variable
	float mouseX;										// Stores the current cursor position on the x-axis for comparison with the older "oldMouseX" variable
	float mouseY;										// Stores the current cursor position on the y-axis for comparison with the older "oldMouseY" variable
	float trackX;										// Used to track the distance the cursor moves over the x-axis during the entire time the mouse button is pressed. Interval method only
	float trackY;										// Used to track the distance the cursor moves over the y-axis during the entire time the mouse button is pressed. Interval method only

	void Start ()
	{
		if (otherObject == null)
		{
			Debug.Log ("You have not assigned an object to resize to: " + transform.name);		// This only happens if there is no object assigned to "otherObject"
		}
		else
		{
			RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();		// This section gets the RectTransform information from this object. Height and width are stored in variables
			float width = objectRectTransform.rect.width;
			float height = objectRectTransform.rect.height;
			
			if (minimumWidth < width * 2)				// This section sets the minimum width to twice the width of this object if it is less. This is so an object cannot be resized into nothingness
			{
				minimumWidth = width * 2;
			}
			
			
			if (minimumHeight < height * 2)				// This section sets the minimum height to twice the height of this object if it is less. This is so an object cannot be resized into nothingness
			{
				minimumHeight = height * 2;
			}
			

			// The following section assigns the movement directions of this control based on selected options
			if (TopRight == true)						
			{
				moveRight = true;						// Top Right
				moveTop = true;
			}
			else if (BottomRight == true)
			{
				moveRight = true;						// Bottom Right
				moveBottom = true;
			}
			else if (Right == true)
			{
				moveRight = true;						// Right
			}
			else if (TopLeft == true)
			{
				moveLeft = true;						// Top Left
				moveTop = true;
			}
			else if (BottomLeft == true)
			{
				moveLeft = true;						// Bottom Left
				moveBottom = true;
			}
			else if (Left == true)
			{
				moveLeft = true;						// Left
			}
			else if (Top == true)
			{
				moveTop = true;							// Top
			}
			else if (Bottom == true)
			{
				moveBottom = true;						// Bottom
			}
		}
	}

	
	void Update ()
	{
		if(disableResizeOther == false && otherObject != null)						
		{
			if (Input.GetMouseButtonDown(0))
			{
				oldMouseX = Input.mousePosition.x;								// Stores the initial cursor position
				oldMouseY = Input.mousePosition.y;
				
				InitialResizeOther ();											// Calls the initial function to determine if the cursor is on the object when the mouse button is intially pressed
				
				if(onObject == true)											// This if statement runs if the cursor is on the object when the mouse button is initally pressed
				{
					PerformRaycast();											// Calls the function to perform a raycast
				}
			}
			else if (Input.GetMouseButton(0) && startOnBorder == true)			// Determines if the mouse button is currently pressed and if it was initially pressed when on the object
			{
				mouseX = Input.mousePosition.x;									// Stores the current cursor position
				mouseY = Input.mousePosition.y;
				
				ResizeOtherObject();											// Calls the function to resize the other object
				
				oldMouseX = mouseX;												// Stores the current cursor position that will become the previous position in the next frame
				oldMouseY = mouseY;
			}
			else if(customCursor == true && Input.GetMouseButton(0) == false)
			{
				SetCursor ();													// Calls the function to set the cursor to the proper cursor image
			}

			
			if (Input.GetMouseButtonUp(0))										// When the mouse button is released, the "startOnBorder" variable will be set to false, and the sector and tracking variables will be reset to zero
			{
				onObject = false;
				startOnBorder = false;
				trackX = 0;
				trackY = 0;
			}
		}
	}

	
	void SetCursor ()		// This function is used to set the cursor to the proper cursor image
	{
		RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();		// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
		float width = objectRectTransform.rect.width;
		float height = objectRectTransform.rect.height;
		float rightOuterBorder = (width * .5f);
		float leftOuterBorder = (width * -.5f);
		float topOuterBorder = (height * .5f);
		float bottomOuterBorder = (height * -.5f);

		Vector2 hotspot = new Vector2 (0 + xHotspotNormal, 0 + yHotspotNormal);					// This section defines the hotspots for the various custom cursors
		Vector2 horHotspot = new Vector2 (0 + xHotspotLR, 0 + yHotspotLR);
		Vector2 vertHotspot = new Vector2 (0 + xHotspotTB, 0 + yHotspotTB);
		Vector2 tlbrHotspot = new Vector2 (0 + xHotspotTLBR, 0 + yHotspotTLBR);
		Vector2 trblHotspot = new Vector2 (0 + xHotspotTRBL, 0 + yHotspotTRBL);
		
		// The following line determines if the cursor is on this object
		if(Input.mousePosition.x <= (transform.position.x + rightOuterBorder) && Input.mousePosition.x >= (transform.position.x + leftOuterBorder) && Input.mousePosition.y <= (transform.position.y + topOuterBorder) && Input.mousePosition.y >= (transform.position.y + bottomOuterBorder))
		{
			PerformConstantRaycast ();								// Calls the function to perform a raycast
			
			if (overObject == true)									// This is true if the raycast determines this object is selected
			{
				enteredObject = true;								// Sets the "enteredObject" variable to true. This is important for changing back to a normal/null cursor only once after the cursor leaves a movable position
				overObject = false;									// Sets the "overObject" variable to false
				
				
				if (Left == true || Right == true)
				{
					if (resizeLeftRightCursor != null)															// Left and Right
					{
						Cursor.SetCursor (resizeLeftRightCursor, horHotspot, cursorMode);						// Sets the cursor to use the "resizeLeftRight" custom cursor if assigned
					}
					else if (normalCursor != null)																// The above custom cursor is null
					{
						Cursor.SetCursor (normalCursor, hotspot, cursorMode);									// Sets the cursor to use the "normal" custom cursor if assigned
					}
					else																						// The above custom cursors are null
					{
						Cursor.SetCursor (null, Vector2.zero, cursorMode);										// Sets the cursor to use the null cursor
					}
				}
				else if (Top == true || Bottom == true)
				{
					if (resizeTopBottomCursor != null)															// Top and Bottom
					{
						Cursor.SetCursor (resizeTopBottomCursor, vertHotspot, cursorMode);						// Sets the cursor to use the "resizeTopBottom" custom cursor if assigned
					}
					else if (normalCursor != null)																// The above custom cursor is null
					{
						Cursor.SetCursor (normalCursor, hotspot, cursorMode);									// Sets the cursor to use the "normal" custom cursor if assigned
					}
					else																						// The above custom cursors are null
					{
						Cursor.SetCursor (null, Vector2.zero, cursorMode);										// Sets the cursor to use the null cursor
					}
				}
				else if (TopLeft == true || BottomRight == true)
				{
					if (resizeTopLeftBottomRightCursor != null)													// Top Left and Bottom Right
					{
						Cursor.SetCursor (resizeTopLeftBottomRightCursor, tlbrHotspot, cursorMode);				// Sets the cursor to use the "resizeTopLeftBottomRight" custom cursor if assigned
					}
					else if (normalCursor != null)																// The above custom cursor is null
					{
						Cursor.SetCursor (normalCursor, hotspot, cursorMode);									// Sets the cursor to use the "normal" custom cursor if assigned
					}
					else																						// The above custom cursors are null
					{
						Cursor.SetCursor (null, Vector2.zero, cursorMode);										// Sets the cursor to use the null cursor
					}
				}
				else if (TopRight == true || BottomLeft == true)
				{
					if (resizeTopRightBottomLeftCursor != null)													// Top Right and Bottom Left
					{
						Cursor.SetCursor (resizeTopRightBottomLeftCursor, trblHotspot, cursorMode);				// Sets the cursor to use the "resizeTopRightBottomLeft" custom cursor if assigned
					}
					else if (normalCursor != null)																// The above custom cursor is null
					{
						Cursor.SetCursor (normalCursor, hotspot, cursorMode);									// Sets the cursor to use the "normal" custom cursor if assigned
					}
					else																						// The above custom cursors are null
					{
						Cursor.SetCursor (null, Vector2.zero, cursorMode);										// Sets the cursor to use the null cursor
					}
				}
				else if (enteredObject == true)
				{
					enteredObject = false;																		// Sets the "enteredObject" variable to false. This is important for changing back to a normal/null cursor only once after the cursor leaves a movable position 
					
					if (normalCursor != null)																	
					{
						Cursor.SetCursor (normalCursor, hotspot, cursorMode);									// Sets the cursor to use the "normal" custom cursor if assigned
					}
					else																						// The above custom cursors are null
					{
						Cursor.SetCursor (null, Vector2.zero, cursorMode);										// Sets the cursor to use the null cursor
					}
				}
			}
			else if (enteredObject == true)
			{
				enteredObject = false;																			// Sets the "enteredObject" variable to false. This is important for changing back to a normal/null cursor only once after the cursor leaves a movable position 
				
				if (normalCursor != null)																	
				{
					Cursor.SetCursor (normalCursor, hotspot, cursorMode);										// Sets the cursor to use the "normal" custom cursor if assigned
				}
				else																							// The above custom cursors are null
				{
					Cursor.SetCursor (null, Vector2.zero, cursorMode);											// Sets the cursor to use the null cursor
				}
			}
		}
		else if (enteredObject == true)
		{
			enteredObject = false;																				// Sets the "enteredObject" variable to false. This is important for changing back to a normal/null cursor only once after the cursor leaves a movable position 
			
			if (normalCursor != null)																	
			{
				Cursor.SetCursor (normalCursor, hotspot, cursorMode);											// Sets the cursor to use the "normal" custom cursor if assigned
			}
			else																								// The above custom cursors are null
			{
				Cursor.SetCursor (null, Vector2.zero, cursorMode);												// Sets the cursor to use the null cursor
			}
		}
	}


	void PerformConstantRaycast ()				// This function performs a raycast to detect if this object or its child is in front of other objects if any overlap at the cursor's position when the mouse button is initially pressed. If so, the other object is set as the last sibling in the hierarchy
	{
		PointerEventData cursor = new PointerEventData(EventSystem.current);							// This section prepares a list for all objects hit with the raycast
		cursor.position = Input.mousePosition;
		List<RaycastResult> objectsHit = new List<RaycastResult> ();
		EventSystem.current.RaycastAll(cursor, objectsHit);
		int count = objectsHit.Count;
		int x = 0;
		
		if (count != 0)
		{
			if (objectsHit[x].gameObject.GetComponent("Text") == true && ignoreTextBox == true)			// This section runs if you wish to ignore text boxes. If an object with a "text" or "Ignore" script attache is the selected object, it will select the object behind it
			{
				while (objectsHit[x].gameObject.GetComponent("Text") == true || objectsHit[x].gameObject.GetComponent("Ignore") == true)
				{
					x++;
					
					if (x == count)
					{
						return;																			// Returns early from this function
					}
				}
			}
			else 													
			{
				while (objectsHit[x].gameObject.GetComponent("Ignore") == true)							// If an object with the "Ignore" script attached is the selected object, it will select the object behind it
				{
					x++;
					
					if (x == count)
					{
						return;																			// Returns early from this function
					}
				}
			}
			
			
			if(objectsHit[x].gameObject == this.gameObject || objectsHit[x].gameObject.transform.IsChildOf (transform))		// This section runs only if this object or its child is the front object where the cursor is
			{
				// This statement runs if the selected object has an "IgnoreThisChild" script attached is the selected object. It will continue to select the objects behind it until it finds one it should not ignore
				while (objectsHit[x].gameObject.GetComponent("IgnoreThisChild") == true || objectsHit[x].gameObject.GetComponent("Ignore") == true || (objectsHit[x].gameObject.GetComponent("Text") == true && ignoreTextBox == true))
				{
					x++;
					
					if (x == count)
					{
						return;																			// Returns early from this function
					}
				}


				if(objectsHit[x].gameObject == this.gameObject) 										// This section runs only if this object is the front object where the cursor is
				{
					overObject = true;																	// Sets the "overObject" variable to true
					
					if (bringToFrontOnOver == true)
					{
						transform.SetAsLastSibling();													// Sets this object to be the last sibling in the hierarchy, making it the forward-most object
						Transform otherObjectTransform = otherObject.GetComponent<Transform> ();
						otherObjectTransform.SetAsLastSibling();										// Sets the other object to be the last sibling in the hierarchy, making it the forward-most object
					}


					return;																				// Returns early from this function
				}

				
				if(includeChildren == true)								
				{
					// The following statement runs if the child object does not have a custom cursor script active or a DisableMoveReside script attached
					if(objectsHit[x].gameObject.GetComponent("DisableMoveResize") == false && (objectsHit[x].gameObject.GetComponent<Move>() == false || objectsHit[x].gameObject.GetComponent<Move>().customCursor == false) && (objectsHit[x].gameObject.GetComponent<MoveParent>() == false || objectsHit[x].gameObject.GetComponent<MoveParent>().customCursor == false) && (objectsHit[x].gameObject.GetComponent<MoveOther>() == false || objectsHit[x].gameObject.GetComponent<MoveOther>().customCursor == false) && (objectsHit[x].gameObject.GetComponent<Resize>() == false || objectsHit[x].gameObject.GetComponent<Resize>().customCursor == false) && (objectsHit[x].gameObject.GetComponent<ResizeParent>() == false || objectsHit[x].gameObject.GetComponent<ResizeParent>().customCursor == false) && (objectsHit[x].gameObject.GetComponent<ResizeOther>() == false || objectsHit[x].gameObject.GetComponent<ResizeOther>().customCursor == false) && (objectsHit[x].gameObject.GetComponent<ScaleHorizontal>() == false || objectsHit[x].gameObject.GetComponent<ScaleHorizontal>().customCursor == false) && (objectsHit[x].gameObject.GetComponent<ScaleVertical>() == false || objectsHit[x].gameObject.GetComponent<ScaleVertical>().customCursor == false))			// This statement runs only if the front object where the cursor is does not have the "DisableMoveResize" script attached
					{
						overObject = true;																// Sets the "overObject" variable to true			
					}
						

					if (bringToFrontOnOver == true)
					{
						transform.SetAsLastSibling();													// Sets this object to be the last sibling in the hierarchy, making it the forward-most object
						Transform otherObjectTransform = otherObject.GetComponent<Transform> ();
						otherObjectTransform.SetAsLastSibling();										// Sets the other object to be the last sibling in the hierarchy, making it the forward-most object
					}
				}
			}
		}
	}


	void InitialResizeOther ()			// This function is used to determine if the cursor is over the object when the mouse button it initially pressed. It is also used to determine which direction(s) the object will be assigned to resize the parent
	{
		RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();		// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
		float width = objectRectTransform.rect.width;
		float height = objectRectTransform.rect.height;
		float rightOuterBorder = (width * .5f);
		float leftOuterBorder = (width * -.5f);
		float topOuterBorder = (height * .5f);
		float bottomOuterBorder = (height * -.5f);
		
		// This section determines if the cursor is on the object when the mouse button is initially pressed
		if(Input.mousePosition.x <= (transform.position.x + rightOuterBorder) && Input.mousePosition.x >= (transform.position.x + leftOuterBorder) && Input.mousePosition.y <= (transform.position.y + topOuterBorder) && Input.mousePosition.y >= (transform.position.y + bottomOuterBorder))
		{
			onObject = true;
		}
		else
		{
			onObject = false;
		}
	}


	void PerformRaycast ()				// This function performs a raycast to detect if this object or its child is in front of other objects if any overlap at the cursor's position when the mouse button is initially pressed. If so, the other object is set as the last sibling in the hierarchy
	{
		PointerEventData cursor = new PointerEventData(EventSystem.current);						// This section prepares a list for all objects hit with the raycast
		cursor.position = Input.mousePosition;
		List<RaycastResult> objectsHit = new List<RaycastResult> ();
		EventSystem.current.RaycastAll(cursor, objectsHit);
		int count = objectsHit.Count;
		int x = 0;
		
		if (count != 0)
		{
			if (objectsHit[x].gameObject.GetComponent("Text") == true && ignoreTextBox == true)			// This section runs if you wish to ignore text boxes. If an object with a "text" or "Ignore" script attache is the selected object, it will select the object behind it
			{
				while (objectsHit[x].gameObject.GetComponent("Text") == true || objectsHit[x].gameObject.GetComponent("Ignore") == true)
				{
					x++;
					
					if (x == count)
					{
						return;																			// Returns early from this function
					}
				}
			}
			else 													
			{
				while (objectsHit[x].gameObject.GetComponent("Ignore") == true)							// If an object with the "Ignore" script attached is the selected object, it will select the object behind it
				{
					x++;
					
					if (x == count)
					{
						return;																			// Returns early from this function
					}
				}
			}


			if(objectsHit[x].gameObject == this.gameObject || objectsHit[x].gameObject.transform.IsChildOf (transform))		// This section runs only if this object or its child is the front object where the cursor is
			{
				// This statement runs if the selected object has an "IgnoreThisChild" script attached is the selected object. It will continue to select the objects behind it until it finds one it should not ignore
				while (objectsHit[x].gameObject.GetComponent("IgnoreThisChild") == true || objectsHit[x].gameObject.GetComponent("Ignore") == true || (objectsHit[x].gameObject.GetComponent("Text") == true && ignoreTextBox == true))
				{
					x++;
					
					if (x == count)
					{
						return;																			// Returns early from this function
					}
				}
		
			
				if(includeChildren == true)								
				{
					if(objectsHit[x].gameObject.GetComponent("DisableMoveResize") == false)				// This statement runs only if the front object where the cursor is does not have the "DisableMoveResize" script attached
					{
						mouseX = Input.mousePosition.x;													// Stores the current cursor position
						mouseY = Input.mousePosition.y;
						
						startOnBorder = true;															// Sets the "startOnBorder" variable to true since the cursor is determined to be on a valid moveable position

						if (bringToFront == true)
						{
							transform.SetAsLastSibling();														// Sets this object to be the last sibling in the hierarchy, making it the forward-most object
							Transform otherObjectTransform = otherObject.GetComponent<Transform> ();
							otherObjectTransform.SetAsLastSibling();											// Sets the other object to be the last sibling in the hierarchy, making it the forward-most object
						}


						return;																			// Returns early from this function
					}
				}
				
				
				if(objectsHit[x].gameObject == this.gameObject) 										// This section runs only if this object is the front object where the cursor is
				{
					mouseX = Input.mousePosition.x;														// Stores the current cursor position
					mouseY = Input.mousePosition.y;
					
					startOnBorder = true;																// Sets the "startOnBorder" variable to true since the cursor is determined to be on a valid moveable position
				
					if (bringToFront == true)
					{
						transform.SetAsLastSibling();														// Sets this object to be the last sibling in the hierarchy, making it the forward-most object
						Transform otherObjectTransform = otherObject.GetComponent<Transform> ();
						otherObjectTransform.SetAsLastSibling();											// Sets the other object to be the last sibling in the hierarchy, making it the forward-most object
					}
				}
			}
		}
	}


	void ResizeOtherObject ()			// This function is used to determine the amount of movement of the cursor, if the minimum limits have been reached, and then it resizes the other object
	{
		RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();										// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
		float width = objectRectTransform.rect.width;
		float height = objectRectTransform.rect.height;
		float rightOuterBorder = (width * .5f);
		float leftOuterBorder = (width * -.5f);
		float topOuterBorder = (height * .5f);
		float bottomOuterBorder = (height * -.5f);
		float leftEdge = transform.position.x + leftOuterBorder;
		float rightEdge = transform.position.x + rightOuterBorder;
		float bottomEdge = transform.position.y + bottomOuterBorder;
		float topEdge = transform.position.y + topOuterBorder;

		RectTransform otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();						// This section gets the RectTransform information from the other object. Height and width are stored in variables. The borders of the object are also defined
		float otherWidth = otherObjectRectTransform.rect.width;
		float otherHeight = otherObjectRectTransform.rect.height;
		float otherRightOuterBorder = (otherWidth * .5f);
		float otherLeftOuterBorder = (otherWidth * -.5f);
		float otherTopOuterBorder = (otherHeight * .5f);
		float otherBottomOuterBorder = (otherHeight * -.5f);
		Transform otherObjectTransform = otherObject.GetComponent<Transform> ();
		float otherLeftEdge = otherObjectTransform.position.x + otherLeftOuterBorder;
		float otherRightEdge = otherObjectTransform.position.x + otherRightOuterBorder;
		float otherBottomEdge = otherObjectTransform.position.y + otherBottomOuterBorder;
		float otherTopEdge = otherObjectTransform.position.y + otherTopOuterBorder;

		RectTransform parentRectTransform = otherObjectRectTransform.parent.gameObject.GetComponent<RectTransform> ();		// This section gets the RectTransform information from the parent. Height and width are stored in variables. The borders of the object are also defined
		float parentWidth = parentRectTransform.rect.width;
		float parentHeight = parentRectTransform.rect.height;
		float parentRightOuterBorder = (parentWidth * .5f) - restrainRightInset;
		float parentLeftOuterBorder = (parentWidth * -.5f) + restrainLeftInset;
		float parentTopOuterBorder = (parentHeight * .5f) - restrainTopInset;
		float parentBottomOuterBorder = (parentHeight * -.5f) + restrainBottomInset;
		float parentLeftEdge = otherObjectTransform.parent.position.x + parentLeftOuterBorder;
		float parentRightEdge = otherObjectTransform.parent.position.x + parentRightOuterBorder;
		float parentBottomEdge = otherObjectTransform.parent.position.y + parentBottomOuterBorder;
		float parentTopEdge = otherObjectTransform.parent.position.y + parentTopOuterBorder;

		float movementX = mouseX - oldMouseX;														// Compares the current cursor position on the x-axis to the previous one, finding the amount of movement over the frame
		float movementY = mouseY - oldMouseY;														// Compares the current cursor position on the y-axis to the previous one, finding the amount of movement over the frame
			
		if (intervalBased == true)																	// This section is only used if the interval method is selected
		{
			trackX = trackX + movementX;															// Tracks the amount of movement on the x-axis since the last interval
			
			if (trackX >= intervalDistance)															// Determines if the tracked POSITIVE movement on the x-axis is greater than the interval distance
			{
				int intervalsToMoveX = (Mathf.FloorToInt (trackX / intervalDistance));				// Determines how many intervals on the POSITIVE x-axis the parent should be resized this frame
				movementX = intervalsToMoveX * intervalDistance;									// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else if (- trackX >= intervalDistance)													// Determines if the tracked NEGATIVE movement on the x-axis is greater than the interval distance
			{
				int intervalsToMoveX = (Mathf.FloorToInt (-trackX / intervalDistance));				// Determines how many intervals on the NEGATIVE x-axis the parent should be resized this frame
				movementX = - intervalsToMoveX * intervalDistance;									// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else
			{
				movementX = 0;																		// If the tracked movement is not enough to move an interval, the movement of resize this frame is set to zero
			}				
			
			
			trackX = trackX % intervalDistance;														// Sets the tracking of movement on the x-axis to be the remainder of any extra movement if it is greater than the interval distance			
			trackY = trackY + movementY;															// Tracks the amount of movement on the y-axis since the last interval
			
			if (trackY >= intervalDistance)															// Determines if the tracked POSITIVE movement on the y-axis is greater than the interval distance
			{
				int intervalsToMoveY = (Mathf.FloorToInt (trackY / intervalDistance));				// Determines how many intervals on the POSITIVE y-axis the parent should be resized this frame
				movementY = intervalsToMoveY * intervalDistance;									// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else if (- trackY >= intervalDistance)													// Determines if the tracked NEGATIVE movement on the y-axis is greater than the interval distance
			{
				int intervalsToMoveY = (Mathf.FloorToInt (-trackY / intervalDistance));				// Determines how many intervals on the NEGATIVE y-axis the parent should be resized this frame
				movementY = - intervalsToMoveY * intervalDistance;									// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else
			{
				movementY = 0;																		// If the tracked movement is not enough to move an interval, the movement of resize this frame is set to zero
			}				
			
			
			trackY = trackY % intervalDistance;														// Sets the tracking of movement on the y-axis to be the remainder of any extra movement if it is greater than the interval distance
		}
		
		
		if(movementX > 0 && movementX > otherWidth - minimumWidth && moveLeft == true)				// Determines if the POSITIVE movement from a left sector is greater than the distance to reach the minimum width
		{
			movementX = otherWidth - minimumWidth;													// Sets the amount of POSITIVE movement to the distance it takes to reach the minimum width
			minimumWidthReached = true;	
		}
		else if (movementX < 0 && - movementX > otherWidth - minimumWidth && moveRight == true)		// Determines if the NEGATIVE movement from a right sector is greater than the distance to reach the minimum width
		{
			movementX = - ( otherWidth - minimumWidth);												// Sets the amount of NEGATIVE movement to the distance it takes to reach the minimum width
			minimumWidthReached = true;	
		}
		
		if (minimumWidthReached == true)			// This section determines if the cursor is inside of this object's inner borders if the minimum width has been reached. If so, movement and tracking are set to 0 on the x-axis
		{
			if ((moveRight == true && Input.mousePosition.x < transform.position.x + leftOuterBorder) || (moveLeft == true && Input.mousePosition.x > transform.position.x + rightOuterBorder))
			{
				movementX = 0;
				trackX = 0;
			}
			else
			{
				minimumWidthReached = false;	
			}
		}
		
		
		if(movementY > 0 && movementY > otherHeight - minimumHeight && moveBottom == true)			// Determines if the POSITIVE movement from a bottom sector is greater than the distance to reach the minimum height
		{
			movementY = otherHeight - minimumHeight;												// Sets the amount of POSITIVE movement to the distance it takes to reach the minimum height
			minimumHeightReached = true;
		}
		else if (movementY < 0 && - movementY > otherHeight - minimumHeight && moveTop == true)		// Determines if the NEGATIVE movement from a top sector is greater than the distance to reach the minimum height
		{
			movementY = - (otherHeight - minimumHeight);											// Sets the amount of NEGATIVE movement to the distance it takes to reach the minimum height
			minimumHeightReached = true;
		}
		
		
		if (minimumHeightReached == true)			// This section determines if the cursor is inside of this object's inner borders if the minimum height has been reached. If so, movement and tracking are set to 0 on the y-axis
		{
			if ((moveTop == true && Input.mousePosition.y < transform.position.y + bottomOuterBorder) || (moveBottom == true && Input.mousePosition.y > transform.position.y + topOuterBorder))
			{
				movementY = 0;
				trackY = 0;
			}
			else
			{
				minimumHeightReached = false;	
			}
		}
		
		
		if (restrainToOtherParent == true)			
		{
			if(movementX > 0 && movementX > parentRightEdge - otherRightEdge && moveRight == true)					// Determines if the POSITIVE movement from a right sector is greater than the distance to reach the parent's right edge
			{
				movementX = parentRightEdge - otherRightEdge;														// Sets the amount of POSITIVE movement to the distance it takes to reach the parent's right edge
			}
			else if (movementX < 0 && - movementX > otherLeftEdge - parentLeftEdge && moveLeft == true)				// Determines if the NEGATIVE movement from a left sector is greater than the distance to reach the parent's left edge
			{
				movementX = - ( otherLeftEdge - parentLeftEdge);													// Sets the amount of NEGATIVE movement to the distance it takes to reach the parent's leftt edge
			}
			
			
			if(movementY > 0 && movementY > parentTopEdge - otherTopEdge && moveTop == true)						// Determines if the POSITIVE movement from a top sector is greater than the distance to reach the parent's top edge
			{
				movementY = parentTopEdge - otherTopEdge;															// Sets the amount of POSITIVE movement to the distance it takes to reach the parent's top edge
			}
			else if (movementY < 0 && - movementY > otherBottomEdge - parentBottomEdge && moveBottom == true)		// Determines if the NEGATIVE movement from a bottom sector is greater than the distance to reach the parent's bottom edge
			{
				movementY = - ( otherBottomEdge - parentBottomEdge);												// Sets the amount of NEGATIVE movement to the distance it takes to reach the parent's bottom edge
			}
			
			
			// This section determines if the other object has reached its parents borders and the cursor is outside of the other object's outer borders. If so, the movement and tracking variables are set to 0
			if ((moveRight == true && otherRightEdge >= parentRightEdge && Input.mousePosition.x > rightEdge) || (moveLeft == true && otherLeftEdge <= parentLeftEdge && Input.mousePosition.x < leftEdge))
			{
				movementX = 0;
				trackX = 0;
			}
			
			
			if ((moveTop == true && otherTopEdge >= parentTopEdge && Input.mousePosition.y > topEdge) || (moveBottom == true && otherBottomEdge <= parentBottomEdge && Input.mousePosition.y < bottomEdge))
			{
				movementY = 0;
				trackY = 0;
			}
		}
		
		
		// The following section resizes the parent based on the assignment of the object
		if (TopRight == true)						
		{
			otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMax.x + movementX, otherObjectRectTransform.offsetMax.y + movementY);			// Top Right
		}
		else if (BottomRight == true)
		{
			otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMin.x, otherObjectRectTransform.offsetMin.y + movementY);						// Bottom Right
			otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMax.x + movementX, otherObjectRectTransform.offsetMax.y);
		}
		else if (Right == true)
		{
			otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMax.x + movementX, otherObjectRectTransform.offsetMax.y);						// Right
		}
		else if (TopLeft == true)
		{
			otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMin.x + movementX, otherObjectRectTransform.offsetMin.y);						// Top Left
			otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMax.x, otherObjectRectTransform.offsetMax.y + movementY);						
		}
		else if (BottomLeft == true)
		{
			otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMin.x + movementX, otherObjectRectTransform.offsetMin.y + movementY);			// Bottom Left
		}
		else if (Left == true)
		{
			otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMin.x + movementX, otherObjectRectTransform.offsetMin.y);						// Left					
		}
		else if (Top == true)
		{
			otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMax.x, otherObjectRectTransform.offsetMax.y + movementY);						// Top
		}
		else if (Bottom == true)
		{
			otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMin.x, otherObjectRectTransform.offsetMin.y + movementY);						// Bottom
		}
	}
	
	
	public void IntervalToggle ()									// This function toggles the interval method on and off
	{
		intervalBased = !intervalBased;
	}
	
	
	public void CustomCursorToggle ()								// This function toggles the use of custom cursors on and off
	{
		customCursor = !customCursor;
	}
	
	
	public void RestrainToggle ()									// This function toggles the ability to restrain the other object to its parent on and off
	{
		restrainToOtherParent = !restrainToOtherParent;
	}
	
	
	public void ResizeOtherToggle ()							// This function toggles the ability to resize the other object on and off. Good for events that toggle
	{
		disableResizeOther = !disableResizeOther;
	}
	
	
	public void DisableResizeOther ()							// This function disables this ability to resize the other object. Good for events that do not toggle
	{
		if (Input.GetMouseButton(0) && startOnBorder == true)		// Assures that the resize ability will not be disabled while already in progress. This is important when using events such as Pointer Enter and Exit
		{
			return;
		}
		else if(disableResizeOther == false)
		{
			disableResizeOther = true;
		}
	}
	
	
	public void EnableResizeOther ()							// This function enables the ability to resize the other object. Good for events that do not toggle
	{
		if (disableResizeOther == true)
		{
			disableResizeOther = false;
		}
	}
}