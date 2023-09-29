using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MoveOther_3_0 : MonoBehaviour {
	
	public GameObject otherObject;						// Defines the object to be moved
	public bool includeChildren = true;					// Determines if this object's children will be included in the raycast return
	public bool ignoreTextBox = true;					// Determines if any text box will be included in the raycast return
	public bool bringToFront = true;					// Determines if the other object will be set as the last sibling in the hierarchy
	public bool disableMoveOther = false;				// Determines if the ability to move the other object is enabled or disabled
	public bool xOnly = false;							// Determines if the other object will move only on the x-axis
	public bool yOnly = false;							// Determines if the other object will move only on the y-axis
	public bool restrainToOtherParent = false;			// Determines if the other object will be restrained fully to its parent
	public float restrainRightInset = 0;				// Sets the distance from the right edge of its parent that the other object will be restrained to
	public float restrainLeftInset = 0;					// Sets the distance from the left edge of its parent that the other object will be restrained to
	public float restrainTopInset = 0;					// Sets the distance from the top edge of its parent that the other object will be restrained to
	public float restrainBottomInset = 0;				// Sets the distance from the bottom edge of its parent that the other object will be restrained to
	public bool intervalBased = false;					// Determines if the other object will move based on intervals
	public float intervalDistance = 50;					// Sets the size of intervals if the interval method is true
	public float rightInset = 0;						// Sets the distance from the right edge of this object that the cursor must be in order to be considered on a valid moveable position
	public float leftInset = 0;							// Sets the distance from the left edge of this object that the cursor must be in order to be considered on a valid moveable position
	public float topInset = 0;							// Sets the distance from the top edge of this object that the cursor must be in order to be considered on a valid moveable position
	public float bottomInset = 0;						// Sets the distance from the bottom edge of this object that the cursor must be in order to be considered on a valid moveable position
	public bool customCursor = false;					// Determines if custom cursors are used
	public bool bringToFrontOnOver = false;				// Determines if the other object will be set as the last sibling in the hierarchy when the cursor is on this object
	public Texture2D normalCursor;						// Defines the normal custom cursor
	public int xHotspotNormal = 0;						// Sets the hotspot poisiton for the "normal" custom cursor on the x-axis
	public int yHotspotNormal = 0;						// Sets the hotspot poisiton for the "normal" custom cursor on the y-axis
	public Texture2D moveCursor;						// Defines the custom cursor for moving
	public int xHotspotMove = 0;						// Sets the hotspot poisiton for the "move" custom cursor on the x-axis
	public int yHotspotMove = 0;						// Sets the hotspot poisiton for the "move" custom cursor on the y-axis
	public Texture2D moveHorizontalCursor;				// Defines the custom cursor for moving when the "xOnly" variable is true
	public int xHotspotHor = 0;							// Sets the hotspot poisiton for the "moveHorizontal" custom cursor on the x-axis
	public int yHotspotHor = 0;							// Sets the hotspot poisiton for the "moveHorizontal" custom cursor on the y-axis
	public Texture2D moveVerticalCursor;				// Defines the custom cursor for moving when the "yOnly" variable is true
	public int xHotspotVert = 0;						// Sets the hotspot poisiton for the "moveVeritcal" custom cursor on the x-axis
	public int yHotspotVert = 0;						// Sets the hotspot poisiton for the "moveVeritcal" custom cursor on the y-axis
	CursorMode cursorMode = CursorMode.Auto;			// Sets the cursor mode
	bool enteredObject = false;							// Determines if the cursor has entered or exited this object. This is important for changing back to a normal/null cursor only once after the cursor leaves a movable position 
	bool startOnObject = false;							// Determines if the cursor is on this object when the mouse button is initially pressed
	bool overObject = false;							// Determines if the cursor is on this object
	bool makeMoveHorizontal = true;						// Determines if the other object may move on the x-axis
	bool makeMoveVertical = true;						// Determines if the other object may move on the y-axis
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
			Debug.Log ("You have not assigned an object to move to: " + transform.name);					// This only happens if there is no object assigned to "otherObject"
		}
	}


	void Update ()
	{
		if (disableMoveOther == false && otherObject != null)
		{
			if (Input.GetMouseButtonDown(0))
			{
				RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();				// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
				float width = objectRectTransform.rect.width;
				float height = objectRectTransform.rect.height;
				float rightOuterBorder = (width * .5f);
				float leftOuterBorder = (width * -.5f);
				float topOuterBorder = (height * .5f);
				float bottomOuterBorder = (height * -.5f);

				// The following line determines if the cursor is on a valid movable position based on the object and the insets
				if(Input.mousePosition.x <= (transform.position.x + rightOuterBorder - rightInset) && Input.mousePosition.x >= (transform.position.x + leftOuterBorder + leftInset) && Input.mousePosition.y <= (transform.position.y + topOuterBorder - topInset) && Input.mousePosition.y >= (transform.position.y + bottomOuterBorder + bottomInset))
				{
						PerformRaycast ();							// Calls the function to perform a raycast
				}
				// The following line determines if the cursor is on the object even if not on a valid movable position based on the insets
				else if(Input.mousePosition.x <= (transform.position.x + rightOuterBorder) && Input.mousePosition.x >= (transform.position.x + leftOuterBorder) && Input.mousePosition.y <= (transform.position.y + topOuterBorder) && Input.mousePosition.y >= (transform.position.y + bottomOuterBorder))
				{
					PerformRaycast ();								// Calls the function to perform a raycast
					startOnObject = false;							// Sets the "startOnObject" variable to false since the cursor is not on a valid moveable position
				}
			}
			else if (Input.GetMouseButton(0) && startOnObject == true && (oldMouseX != Input.mousePosition.x || oldMouseY != Input.mousePosition.y))			// Determines if the mouse button is currently pressed, if it was initially pressed on a valid moveable position, and if the cursor position has moved since the last frame
			{
				mouseX = Input.mousePosition.x;						// Stores the current cursor position
				mouseY = Input.mousePosition.y;

				MoveOtherObject ();									// Calls the function to move the object

				oldMouseX = mouseX;									// Stores the current cursor position that will become the previous position in the next frame
				oldMouseY = mouseY;
			}
			else if(customCursor == true && Input.GetMouseButton(0) == false)
			{
				SetCursor ();										// Calls the function to set the cursor to the proper cursor image
			}
			
			
			if (Input.GetMouseButtonUp(0))							// When the mouse button is released, the "startOnObject" variable will be set to false, and the tracking variables will be reset to zero
			{
				startOnObject = false;
				trackX = 0;
				trackY = 0;
			}
		}
	}


	void SetCursor ()												// This function is used to set the cursor to the proper cursor image
	{
		RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();			// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
		float width = objectRectTransform.rect.width;
		float height = objectRectTransform.rect.height;
		float rightOuterBorder = (width * .5f);
		float leftOuterBorder = (width * -.5f);
		float topOuterBorder = (height * .5f);
		float bottomOuterBorder = (height * -.5f);

		Vector2 hotspot = new Vector2 (0 + xHotspotNormal, 0 + yHotspotNormal);					// This section defines the hotspots for the various custom cursors
		Vector2 moveHotspot = new Vector2 (0 + xHotspotMove, 0 + yHotspotMove);
		Vector2 horHotspot = new Vector2 (0 + xHotspotHor, 0 + yHotspotHor);
		Vector2 vertHotspot = new Vector2 (0 + xHotspotVert, 0 + yHotspotVert);
		
		// The following line determines if the cursor is on this object
		if(Input.mousePosition.x <= (transform.position.x + rightOuterBorder) && Input.mousePosition.x >= (transform.position.x + leftOuterBorder) && Input.mousePosition.y <= (transform.position.y + topOuterBorder) && Input.mousePosition.y >= (transform.position.y + bottomOuterBorder))
		{
			PerformConstantRaycast ();								// Calls the function to perform a raycast
			
			if (overObject == true)									// This is true if the raycast determines this object is selected
			{
				enteredObject = true;								// Sets the "enteredObject" variable to true. This is important for changing back to a normal/null cursor only once after the cursor leaves a movable position
				overObject = false;									// Sets the "overObject" variable to false for the next frame
				
				// The following line determines if the cursor is over a valid movable position
				if(Input.mousePosition.x <= (transform.position.x + rightOuterBorder - rightInset) && Input.mousePosition.x >= (transform.position.x + leftOuterBorder + leftInset) && Input.mousePosition.y <= (transform.position.y + topOuterBorder - topInset) && Input.mousePosition.y >= (transform.position.y + bottomOuterBorder + bottomInset))
				{
					if (moveCursor != null && xOnly == false && yOnly == false)					// The default movement
					{
						Cursor.SetCursor (moveCursor, moveHotspot, cursorMode);					// Sets the cursor to use the "move" custom cursor if assigned
					}
					else if (moveHorizontalCursor != null && xOnly == true)						// The "xOnly" variable is true
					{
						Cursor.SetCursor (moveHorizontalCursor, horHotspot, cursorMode);		// Sets the cursor to use the "moveHorizontal" custom cursor if assigned
					}
					else if (moveVerticalCursor != null && yOnly == true)						// The "yOnly" variable is true
					{
						Cursor.SetCursor (moveVerticalCursor, vertHotspot, cursorMode);			// Sets the cursor to use the "moveVertical" custom cursor if assigned
					}
					else if (normalCursor != null)												// The above custom cursors are null
					{
						Cursor.SetCursor (normalCursor, hotspot, cursorMode);					// Sets the cursor to use the "normal" custom cursor if assigned
					}
					else																		// The above custom cursors are null
					{
						Cursor.SetCursor (null, Vector2.zero, cursorMode);						// Sets the cursor to use the null cursor
					}
				}
				else 		// This section runs if the cursor is not over a valid movable position. It sets the cursor to use the "normal" custom cursor if assigned. If not, it sets it to the null cursor 
				{
					if (normalCursor != null)													
					{
						Cursor.SetCursor (normalCursor, hotspot, cursorMode);					// Sets the cursor to use the "normal" custom cursor if assigned
					}
					else																		// The above custom cursor is null
					{
						Cursor.SetCursor (null, Vector2.zero, cursorMode);						// Sets the cursor to use the null cursor
					}
				}
			}
			else if (enteredObject == true)
			{
				enteredObject = false;															// Sets the "enteredObject" variable to false. This is important for changing back to a normal/null cursor only once after the cursor leaves a movable position 
				
				if (normalCursor != null)													
				{
					Cursor.SetCursor (normalCursor, hotspot, cursorMode);						// Sets the cursor to use the "normal" custom cursor if assigned
				}
				else																			// The above custom cursor is null
				{
					Cursor.SetCursor (null, Vector2.zero, cursorMode);							// Sets the cursor to use the null cursor
				}
			}
		}
		else if (enteredObject == true)
		{
			enteredObject = false;																// Sets the "enteredObject" variable to false. This is important for changing back to a normal/null cursor only once after the cursor leaves a movable position 
			
			if (normalCursor != null)													
			{
				Cursor.SetCursor (normalCursor, hotspot, cursorMode);							// Sets the cursor to use the "normal" custom cursor if assigned
			}	
			else																				// The above custom cursor is null
			{
				Cursor.SetCursor (null, Vector2.zero, cursorMode);								// Sets the cursor to use the null cursor
			}
		}
	}


	void PerformConstantRaycast ()				// This function performs a raycast to detect if this object or its child is in front of other objects if any overlap at the cursor's position when the mouse button is initially pressed. If so, the other object can be set as the last sibling in the hierarchy
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
						Transform otherObjectTransform = otherObject.GetComponent<Transform> ();		// Gets the Transform information from the other object
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
						Transform otherObjectTransform = otherObject.GetComponent<Transform> ();		// Gets the Transform information from the other object
						otherObjectTransform.SetAsLastSibling();										// Sets the other object to be the last sibling in the hierarchy, making it the forward-most object
					}
				}
			}
		}
	}


	void PerformRaycast ()				// This function performs a raycast to detect if this object or its child is in front of other objects if any overlap at the cursor's position when the mouse button is initially pressed. If so, the other object can be set as the last sibling in the hierarchy
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
		
			
				if(includeChildren == true)								
				{
					if(objectsHit[x].gameObject.GetComponent("DisableMoveResize") == false)				// This statement runs only if the front object where the cursor is does not have the "DisableMoveResize" script attached
					{
						oldMouseX = Input.mousePosition.x;												// Stores the initial cursor position
						oldMouseY = Input.mousePosition.y;
						
						startOnObject = true;															// Sets the "startOnObject" variable to true

						if (bringToFront == true)
						{
							Transform otherObjectTransform = otherObject.GetComponent<Transform> ();	// Gets the Transform information from the other object
							otherObjectTransform.SetAsLastSibling();									// Sets the other object to be the last sibling in the hierarchy, making it the forward-most object
						}


						return;																			// Returns early from this function
					}
				}
				
				
				if(objectsHit[x].gameObject == this.gameObject) 										// This section runs only if this object is the front object where the cursor is
				{	
					oldMouseX = Input.mousePosition.x;													// Stores the initial cursor position
					oldMouseY = Input.mousePosition.y;
					
					startOnObject = true;																// Sets the "startOnObject" variable to true

					if (bringToFront == true)
					{
						Transform otherObjectTransform = otherObject.GetComponent<Transform> ();		// Gets the Transform information from the other object
						otherObjectTransform.SetAsLastSibling();										// Sets the other object to be the last sibling in the hierarchy, making it the forward-most object
					}
				}
			}
		}
	}
	
	
	void MoveOtherObject()			// This function is used to determine the amount of movement of the cursor, and then it moves the object
	{
		RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();												// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
		float width = objectRectTransform.rect.width;
		float height = objectRectTransform.rect.height;
		float rightOuterBorder = (width * .5f);
		float leftOuterBorder = (width * -.5f);
		float topOuterBorder = (height * .5f);
		float bottomOuterBorder = (height * -.5f);

		RectTransform otherObjectRectTransform = otherObject.GetComponent<RectTransform> ();										// This section gets the RectTransform information from the other object. Height and width are stored in variables. The borders of the object are also defined
		float otherWidth = otherObjectRectTransform.rect.width;
		float otherHeight = otherObjectRectTransform.rect.height;
		float otherRightOuterBorder = (otherWidth * .5f);
		float otherLeftOuterBorder = (otherWidth * -.5f);
		float otherTopOuterBorder = (otherHeight * .5f);
		float otherBottomOuterBorder = (otherHeight * -.5f);

		RectTransform parentRectTransform = otherObjectRectTransform.parent.gameObject.GetComponent<RectTransform> ();				// This section gets the RectTransform information from the other object's parent. Height and width are stored in variables. The borders of the object are also defined
		float parentWidth = parentRectTransform.rect.width;
		float parentHeight = parentRectTransform.rect.height;
		float parentRightOuterBorder = (parentWidth * .5f) - restrainRightInset;
		float parentLeftOuterBorder = (parentWidth * -.5f) + restrainLeftInset;
		float parentTopOuterBorder = (parentHeight * .5f) - restrainTopInset;
		float parentBottomOuterBorder = (parentHeight * -.5f) + restrainBottomInset;

		Transform otherObjectTransform = otherObject.GetComponent<Transform> ();													// Gets the Transform information from the other object

		float movementX = mouseX - oldMouseX;												// Compares the current cursor position on the x-axis to the previous one, finding the amount of movement over the frame
		float movementY = mouseY - oldMouseY;												// Compares the current cursor position on the y-axis to the previous one, finding the amount of movement over the frame
		
		
		if (intervalBased == true)															// This section is only used if the interval method is selected
		{
			trackX = trackX + movementX;													// Tracks the amount of movement on the x-axis since the last interval
			
			
			if (trackX >= intervalDistance)													// Determines if the tracked POSITIVE movement on the x-axis is greater than the interval distance
			{
				int intervalsToMoveX = (Mathf.FloorToInt (trackX / intervalDistance));		// Determines how many intervals on the POSITIVE x-axis the other object should be moved this frame
				movementX = intervalsToMoveX * intervalDistance;							// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else if (- trackX >= intervalDistance)											// Determines if the tracked NEGATIVE movement on the x-axis is greater than the interval distance
			{
				int intervalsToMoveX = (Mathf.FloorToInt (-trackX / intervalDistance));		// Determines how many intervals on the NEGATIVE x-axis the other object should be moved this frame
				movementX = - intervalsToMoveX * intervalDistance;							// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else
			{
				movementX = 0;																// If the tracked movement is not enough to move an interval, the movement of resize this frame is set to zero
			}				
			
			
			trackX = trackX % intervalDistance;												// Sets the tracking of movement on the x-axis to be the remainder of any extra movement if it is greater than the interval distance
			trackY = trackY + movementY;													// Tracks the amount of movement on the y-axis since the last interval
			
			
			if (trackY >= intervalDistance)													// Determines if the tracked POSITIVE movement on the y-axis is greater than the interval distance
			{
				int intervalsToMoveY = (Mathf.FloorToInt (trackY / intervalDistance));		// Determines how many intervals on the POSITIVE y-axis the other object should be moved this frame
				movementY = intervalsToMoveY * intervalDistance;							// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else if (- trackY >= intervalDistance)											// Determines if the tracked NEGATIVE movement on the y-axis is greater than the interval distance
			{
				int intervalsToMoveY = (Mathf.FloorToInt (-trackY / intervalDistance));		// Determines how many intervals on the NEGATIVE y-axis the other object should be moved this frame
				movementY = - intervalsToMoveY * intervalDistance;							// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else
			{
				movementY = 0;																// If the tracked movement is not enough to move an interval, the movement of resize this frame is set to zero
			}				
			
			
			trackY = trackY % intervalDistance;												// Sets the tracking of movement on the y-axis to be the remainder of any extra movement if it is greater than the interval distance
		}


		float offsetX = mouseX - otherObjectTransform.position.x - movementX;				// Used to determine the distance between the cursor position and the other object position on the x-axis			
		float offsetY = mouseY - otherObjectTransform.position.y - movementY;				// Used to determine the distance between the cursor position and the other object position on the y-axis

		if (restrainToOtherParent == true)
		{
			// The following line determines if the other object is at the edge of its parent and if the cursor is beyond the allowable move boundary on the x-axis
			if ((parentRightOuterBorder <= (otherObjectTransform.localPosition.x + otherRightOuterBorder) && (mouseX > transform.position.x + rightOuterBorder)) || (parentLeftOuterBorder >= (otherObjectTransform.localPosition.x + otherLeftOuterBorder) && (mouseX < transform.position.x + leftOuterBorder)))															// Determines if the left object is at the minimum width
			{
				makeMoveHorizontal = false;		// Sets the permission to move horizonatally to false
			}
			else
			{
				makeMoveHorizontal = true;		// Sets the permission to move horizonatally to true
			}

			
			// The following line determines if the other object is at the edge of its parent and if the cursor is beyond the allowable move boundary on the y-axis
			if ((parentTopOuterBorder <= (otherObjectTransform.localPosition.y + otherTopOuterBorder) && (mouseY > transform.position.y + topOuterBorder)) || (parentBottomOuterBorder >= (otherObjectTransform.localPosition.y + otherBottomOuterBorder) && (mouseY < transform.position.y + bottomOuterBorder)))															// Determines if the left object is at the minimum width
			{
				makeMoveVertical = false;		// Sets the permission to move vertically to false
			}
			else
			{
				makeMoveVertical = true;		// Sets the permission to move vertically to true
			}
		}
		else
		{
			makeMoveHorizontal = true;			// Sets the permission to move horizonatally to true
			makeMoveVertical = true;			// Sets the permission to move vertically to true
		}	


		if (xOnly == false && yOnly == false)
		{
			if (makeMoveHorizontal == true && makeMoveVertical == true)
			{
				otherObjectTransform.position = new Vector2 (Input.mousePosition.x - offsetX, Input.mousePosition.y - offsetY);									// Moves the other object along the x-axis and y-axis
			}
			else if (makeMoveHorizontal == true && makeMoveVertical == false)
			{
				otherObjectTransform.position = new Vector2 (Input.mousePosition.x - offsetX, otherObjectTransform.position.y);									// Moves the other object along the x-axis
			}
			else if (makeMoveHorizontal == false && makeMoveVertical == true)
			{
				otherObjectTransform.position = new Vector2 (otherObjectTransform.position.x, Input.mousePosition.y - offsetY);									// Moves the other object along the y-axis
			}	
		}
		else if (xOnly == true && makeMoveHorizontal == true)
		{
			otherObjectTransform.position = new Vector2 (Input.mousePosition.x - offsetX, otherObjectTransform.position.y);										// Moves the other object along the x-axis
		}
		else if (yOnly == true && makeMoveVertical == true)
		{
			otherObjectTransform.position = new Vector2 (otherObjectTransform.position.x, Input.mousePosition.y - offsetY);										// Moves the other object along the y-axis
		}


		if (restrainToOtherParent == true)
		{
			otherObjectRectTransform = otherObject.GetComponent<RectTransform> ();																				// This section gets the RectTransform information from the other object. Height and width are stored in variables. The borders of the object are also defined
			otherWidth = otherObjectRectTransform.rect.width;
			otherHeight = otherObjectRectTransform.rect.height;
			otherRightOuterBorder = (otherWidth * .5f);
			otherLeftOuterBorder = (otherWidth * -.5f);
			otherTopOuterBorder = (otherHeight * .5f);
			otherBottomOuterBorder = (otherHeight * -.5f);
			
			parentRectTransform = otherObjectRectTransform.parent.gameObject.GetComponent<RectTransform> ();													// This section gets the RectTransform information from the parent. Height and width are stored in variables. The borders of the object are also defined
			parentWidth = parentRectTransform.rect.width;
			parentHeight = parentRectTransform.rect.height;
			parentRightOuterBorder = (parentWidth * .5f) - restrainRightInset;
			parentLeftOuterBorder = (parentWidth * -.5f) + restrainLeftInset;
			parentTopOuterBorder = (parentHeight * .5f) - restrainTopInset;
			parentBottomOuterBorder = (parentHeight * -.5f) + restrainBottomInset;
			
			otherObjectTransform = otherObject.GetComponent<Transform> ();																						// Gets the Transform information from the other object

			if (otherObjectTransform.localPosition.x + otherLeftOuterBorder < parentLeftOuterBorder)
			{
				otherObjectTransform.localPosition = new Vector2 (parentLeftOuterBorder - otherLeftOuterBorder, otherObjectTransform.localPosition.y);			// Moves the other object to the left edge of its parent
			}
			else if (otherObjectTransform.localPosition.x + otherRightOuterBorder > parentRightOuterBorder)
			{
				otherObjectTransform.localPosition = new Vector2 (parentRightOuterBorder - otherRightOuterBorder, otherObjectTransform.localPosition.y);		// Moves the other object to the right edge of its parent
			}
			
			
			if (otherObjectTransform.localPosition.y + otherBottomOuterBorder < parentBottomOuterBorder)
			{
				otherObjectTransform.localPosition = new Vector2 (otherObjectTransform.localPosition.x, parentBottomOuterBorder - otherBottomOuterBorder);		// Moves the other object to the bottom edge of its parent
			}
			else if (otherObjectTransform.localPosition.y + otherTopOuterBorder > parentTopOuterBorder)
			{
				otherObjectTransform.localPosition = new Vector2 (otherObjectTransform.localPosition.x, parentTopOuterBorder - otherTopOuterBorder);			// Moves the other object to the top edge of its parent
			}
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
	
	
	public void MoveOtherToggle ()									// This function toggles the ability to move the other object on and off. Good for events that toggle
	{
		disableMoveOther = !disableMoveOther;
	}
	
	
	public void DisableMoveOther ()									// This function disables this ability to move the other object. Good for events that do not toggle
	{
		if (Input.GetMouseButton(0) && startOnObject == true)		// Assures that the move ability will not be disabled while already in progress. This is important when using events such as Pointer Enter and Exit
		{
			return;
		}
		else if(disableMoveOther == false)
		{
			disableMoveOther = true;
		}
	}
	
	
	public void EnableMoveOther ()									// This function enables the ability to move the other object. Good for events that do not toggle
	{
		if (disableMoveOther == true)
		{
			disableMoveOther = false;
		}
	}
}