using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ScaleHorizontal_3_0 : MonoBehaviour {

	public GameObject leftObject;						// Defines the left object to be scaled
	public GameObject rightObject;						// Defines the right object to be scaled
	public bool includeChildren = true;					// Determines if this object's children will be included in the raycast return
	public bool ignoreTextBox = true;					// Determines if any text box will be included in the raycast return
	public bool disableScale = false;					// Determines if the ability to scale the other objects is enabled or disabled
	public bool restrainToParent = false;				// Determines if an object will be restrained fully to its parent
	public float restrainLeftInset = 0;					// Sets the distance from the left edge of its parent that the right object will be restrained to
	public float restrainRightInset = 0;				// Sets the distance from the right edge of its parent that the left object will be restrained to
	public bool intervalBased = false;					// Determines if the other objects will scale based on intervals
	public float intervalDistance = 50;					// Sets the size of intervals if the interval method is true
	public float lMinimumWidth = 10;					// Sets the minimum width the left object may scale to
	public float rMinimumWidth = 10;					// Sets the minimum width the right object may scale to
	public bool customCursor = false;					// Determines if custom cursors are used
	public Texture2D normalCursor;						// Defines the normal custom cursor
	public int xHotspotNormal = 0;						// Sets the hotspot poisiton for the "normal" custom cursor on the x-axis
	public int yHotspotNormal = 0;						// Sets the hotspot poisiton for the "normal" custom cursor on the y-axis
	public Texture2D moveHorizontalCursor;				// Defines the custom cursor for moving horizontally
	public int xHotspotHor = 0;							// Sets the hotspot poisiton for the "moveHorizontal" custom cursor on the x-axis
	public int yHotspotHor = 0;							// Sets the hotspot poisiton for the "moveHorizontal" custom cursor on the y-axis
	CursorMode cursorMode = CursorMode.Auto;			// The cursor mode
	bool enteredObject = false;							// Determines if the cursor has entered or exited this object. This is important for changing back to a normal/null cursor only once after the cursor leaves a movable position 
	bool startOnObject = false;							// Determines if the cursor is on this object when the mouse button is initially pressed
	bool lMinimumWidthReached = false;					// Determines if the left object has reached the minimum width allowed
	bool rMinimumWidthReached = false;					// Determines if the right object has reached the minimum width allowed
	bool lMaximumWidthReached = false;					// Determines if the left object has reached the minimum width allowed
	bool rMaximumWidthReached = false;					// Determines if the right object has reached the minimum width allowed
	bool scale = true;									// Determines if the conditions are true for the objects to be scaled
	float oldMouseX;									// Stores the cursor position on the x-axis for comparison with the newer "mouseX" variable
	float mouseX;										// Stores the current cursor position on the x-axis for comparison with the older "oldMouseX" variable
	float trackX;										// Used to track the distance the cursor moves over the x-axis during the entire time the mouse button is pressed. Interval method only
	
	void Start ()
	{
		if (leftObject == null && rightObject == null)
		{
			Debug.Log ("You have not assigned an object to scale to: " + transform.name);			// This only happens if neither "leftObject" or "rightObject" is assigned
		}
	}
	

	void Update ()
	{
		if (disableScale == false && (leftObject != null || rightObject != null))
		{
			if (Input.GetMouseButtonDown(0))									// Determines if the mouse button is initially pressed
			{
				RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();		// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
				float width = objectRectTransform.rect.width;
				float height = objectRectTransform.rect.height;
				float rightOuterBorder = (width * .5f);
				float leftOuterBorder = (width * -.5f);
				float topOuterBorder = (height * .5f);
				float bottomOuterBorder = (height * -.5f);
				
				oldMouseX = Input.mousePosition.x;								// Stores the initial cursor position

				// The following line determines if the cursor is on a valid movable position based on the object
				if(Input.mousePosition.x <= (transform.position.x + rightOuterBorder) && Input.mousePosition.x >= (transform.position.x + leftOuterBorder) && Input.mousePosition.y <= (transform.position.y + topOuterBorder) && Input.mousePosition.y >= (transform.position.y + bottomOuterBorder))
				{
					PerformRaycast ();											// Calls the function to perform a raycast
				}
			}
			else if (Input.GetMouseButton(0) && startOnObject == true)			// Determines if the mouse button is currently pressed and if it was initially pressed when on the object
			{
				mouseX = Input.mousePosition.x;									// Stores the current cursor position
				ScaleLeftRightObjects();										// Calls the function to scale the other objects
				oldMouseX = mouseX;												// Stores the current cursor position that will become the previous position in the next frame
			}
			else if(customCursor == true && Input.GetMouseButton(0) == false)	// Determines if the mouse button is not pressed and the "customCursor" variable is set to true
			{
				SetCursor ();													// Calls the function to set the cursor to the proper cursor image
			}
			
			
			if (Input.GetMouseButtonUp(0))										// When the mouse button is released, the "startOnObject" variable will be set to false, and the tracking variable will be reset to zero
			{
				startOnObject = false;
				trackX = 0;
			}
		}
	}


	void SetCursor ()		// This function is used to set the cursor to the proper cursor image
	{
		RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();			// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
		float width = objectRectTransform.rect.width;
		float height = objectRectTransform.rect.height;
		float rightOuterBorder = (width * .5f);
		float leftOuterBorder = (width * -.5f);
		float topOuterBorder = (height * .5f);
		float bottomOuterBorder = (height * -.5f);

		Vector2 hotspot = new Vector2 (0 + xHotspotNormal, 0 + yHotspotNormal);					// This section defines the hotspots for the various custom cursors
		Vector2 horHotspot = new Vector2 (0 + xHotspotHor, 0 + yHotspotHor);
		
		// The following line determines if the cursor is on this object
		if(Input.mousePosition.x <= (transform.position.x + rightOuterBorder) && Input.mousePosition.x >= (transform.position.x + leftOuterBorder) && Input.mousePosition.y <= (transform.position.y + topOuterBorder) && Input.mousePosition.y >= (transform.position.y + bottomOuterBorder))
		{
			PerformRaycast ();										// Calls the function to perform a raycast
			
			if (startOnObject == true)								// This is true if the raycast determines this object is selected
			{
				enteredObject = true;								// Sets the "enteredObject" variable to true. This is important for changing back to a normal/null cursor only once after the cursor leaves a movable position
				startOnObject = false;								// Sets the "startOnObject" variable to false so there will be no movement taking place since the mouse button is not pressed
				
				// The following line determines if the cursor is over a valid movable position
				if(Input.mousePosition.x <= (transform.position.x + rightOuterBorder) && Input.mousePosition.x >= (transform.position.x + leftOuterBorder) && Input.mousePosition.y <= (transform.position.y + topOuterBorder) && Input.mousePosition.y >= (transform.position.y + bottomOuterBorder))
				{
					if (moveHorizontalCursor != null)					// The default movement
					{
						Cursor.SetCursor (moveHorizontalCursor, horHotspot, cursorMode);		// Sets the cursor to use the "move horizontal" custom cursor if assigned
					}
					else if (normalCursor != null)												// The above custom cursor is null
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
	
	
	void PerformRaycast ()				// This function performs a raycast to detect if this object or its child is in front of other objects if any overlap at the cursor's position when the mouse button is initially pressed
	{
		PointerEventData cursor = new PointerEventData(EventSystem.current);								// This section prepares a list for all objects hit with the raycast
		cursor.position = Input.mousePosition;
		List<RaycastResult> objectsHit = new List<RaycastResult> ();
		EventSystem.current.RaycastAll(cursor, objectsHit);
		int count = objectsHit.Count;
		int x = 0;

		if (count != 0)
		{
			if (objectsHit[x].gameObject.GetComponent("Text") == true && ignoreTextBox == true)				// This section runs if you wish to ignore text boxes. If an object with a "text" or "Ignore" script attache is the selected object, it will select the object behind it
			{
				while (objectsHit[x].gameObject.GetComponent("Text") == true || objectsHit[x].gameObject.GetComponent("Ignore") == true)
				{
					x++;
					
					if (x == count)
					{
						return;																				// Returns early from this function
					}
				}
			}
			else 													
			{
				while (objectsHit[x].gameObject.GetComponent("Ignore") == true)								// If an object with the "Ignore" script attached is the selected object, it will select the object behind it
				{
					x++;
					
					if (x == count)
					{
						return;																				// Returns early from this function
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
						return;																				// Returns early from this function
					}
				}
			
			
				if(includeChildren == true)								
				{
					if(objectsHit[x].gameObject.GetComponent("DisableMoveResize") == false)					// This statement runs only if the front object where the cursor is does not have the "DisableMoveResize" script attached
					{
						mouseX = Input.mousePosition.x;														// Stores the current cursor position
						startOnObject = true;																// Sets the "startOnObject" variable to true since the cursor is determined to be on the object
						return;																				// Returns early from this function
					}
				}
			
			
				if(objectsHit[x].gameObject == this.gameObject) 											// This section runs only if this object is the front object where the cursor is
				{	
					mouseX = Input.mousePosition.x;															// Stores the current cursor position
					startOnObject = true;																	// Sets the "startOnObject" variable to true since the cursor is determined to be on the object
				}
			}
		}
	}
	

	void ScaleLeftRightObjects ()		// This function is used to determine the amount of movement of the cursor, if the minimum limits have been reached, and then it scales the other objects
	{
		RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();						// This section gets the RectTransform information from this object. Width is stored in a variable. The borders of the object are also defined
		float width = objectRectTransform.rect.width;
		float rightOuterBorder = (width * .5f);
		float leftOuterBorder = (width * -.5f);
		float rightEdge = transform.position.x + rightOuterBorder;
		float leftEdge = transform.position.x + leftOuterBorder;
		float rWidth;
		float lWidth;

		RectTransform parentRectTransform = transform.parent.gameObject.GetComponent<RectTransform> ();		// This section gets the RectTransform information from the parent. Width is stored in a variable. The borders of the object are also defined
		float parentWidth = parentRectTransform.rect.width;
		float parentRightOuterBorder = (parentWidth * .5f) - restrainRightInset;
		float parentLeftOuterBorder = (parentWidth * -.5f) + restrainLeftInset;;
		float parentRightEdge = transform.parent.position.x + parentRightOuterBorder;
		float parentLeftEdge = transform.parent.position.x + parentLeftOuterBorder;

		if (leftObject != null)																				// Determines if a left object is assigned
		{
			RectTransform leftObjectRectTransform = leftObject.gameObject.GetComponent<RectTransform> ();	// This section gets the RectTransform information from the left object. Width is stored in a variable
			lWidth = leftObjectRectTransform.rect.width;
		}
		else
		{
			lWidth = 0;																						// Sets the variables to 0 because there is no left object assianged
			lMinimumWidth = 0;
		}
		
		
		if (rightObject != null)																			// Determines if a right object is assigned
		{
			RectTransform rightObjectRectTransform = rightObject.gameObject.GetComponent<RectTransform> ();	// This section gets the RectTransform information from the right object. Width is stored in a variable
			rWidth = rightObjectRectTransform.rect.width;
		}
		else
		{
			rWidth = 0;																						// Sets the variables to 0 because there is no right object assianged
			rMinimumWidth = 0;
		}
		
		
		float movementX = mouseX - oldMouseX;																// Compares the current cursor position on the x-axis to the previous one, finding the amount of movement over the frame
		
		if (intervalBased == true)																			// This section is only used if the interval method is selected
		{
			trackX = trackX + movementX;																	// Tracks the amount of movement on the x-axis since the last interval
			
			if (trackX >= intervalDistance)																	// Determines if the tracked POSITIVE movement on the x-axis is greater than the interval distance
			{
				int intervalsToMoveX = (Mathf.FloorToInt (trackX / intervalDistance));						// Determines how many intervals on the POSITIVE x-axis the other objects should be scaled this frame
				movementX = intervalsToMoveX * intervalDistance;											// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else if (- trackX >= intervalDistance)															// Determines if the tracked NEGATIVE movement on the x-axis is greater than the interval distance
			{
				int intervalsToMoveX = (Mathf.FloorToInt (-trackX / intervalDistance));						// Determines how many intervals on the NEGATIVE x-axis the other objects should be scaled this frame
				movementX = - intervalsToMoveX * intervalDistance;											// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else
			{
				movementX = 0;																				// If the tracked movement is not enough to move an interval, the movement of resize this frame is set to zero
			}				
			
			
			trackX = trackX % intervalDistance;																// Sets the tracking of movement on the x-axis to be the remainder of any extra movement if it is greater than the interval distance			
		}
		
		
		if(movementX > 0 && movementX > rWidth - rMinimumWidth && rightObject != null)						// Determines if the POSITIVE movement is greater than the distance to reach the minimum width of the right object if assigned
		{
			movementX = rWidth - rMinimumWidth;																// Sets the amount of POSITIVE movement to the distance it takes to reach the minimum width of the right object
			rMinimumWidthReached = true;																	// Sets the "rMinimumWidthReached" variable to true
		}
		else if (movementX < 0 && - movementX > lWidth - lMinimumWidth && leftObject != null)				// Determines if the NEGATIVE movement is greater than the distance to reach the minimum width of the left object if assigned
		{
			movementX = - ( lWidth - lMinimumWidth);														// Sets the amount of NEGATIVE movement to the distance it takes to reach the minimum width of the left object
			lMinimumWidthReached = true;																	// Sets the "lMinimumWidthReached" variable to true
		}


		if (lWidth <= lMinimumWidth)																		// Determines if the left object is at the minimum width
		{
			lMinimumWidthReached = true;
		}
		else
		{
			lMinimumWidthReached = false;
		}
		
		
		if (rWidth <= rMinimumWidth)																		// Determines if the right object is at the minimum width
		{
			rMinimumWidthReached = true;
		}
		else
		{
			rMinimumWidthReached = false;
		}


		if (restrainToParent == true)
		{
			if(movementX > 0 && movementX > parentRightEdge - rightEdge)									// Determines if the POSITIVE movement is greater than the distance to reach the parent's right edge
			{
				movementX = parentRightEdge - rightEdge;													// Sets the amount of POSITIVE movement to the distance it takes to reach the parent's right edge
				lMaximumWidthReached = true;																// Sets the "lMaximumWidthReached" variable to true
			}
			else if (movementX < 0 && - movementX > leftEdge - parentLeftEdge)								// Determines if the NEGATIVE movement is greater than the distance to reach the parent's left edge
			{
				movementX = - ( leftEdge - parentLeftEdge);													// Sets the amount of NEGATIVE movement to the distance it takes to reach the parent's left edge
				rMaximumWidthReached = true;																// Sets the "rMaximumWidthReached" variable to true
			}


			if (leftEdge - parentLeftEdge <= restrainLeftInset)												// Determines if the right object is at the maximum width
			{
				rMaximumWidthReached = true;
			}
			else
			{
				rMaximumWidthReached = false;
			}
	

			if (parentRightEdge - rightEdge <= restrainRightInset)											// Determines if the left object is at the maximum width
			{
				lMaximumWidthReached = true;
			}
			else
			{
				lMaximumWidthReached = false;
			}
		}
		
		
		float offsetX = mouseX - transform.position.x - movementX;											// Used to determine the distance between the cursor position and the object position on the x-axis. This is so the object does not jump to center around the cursor when it is moved
		
		if (leftObject != null && rightObject != null)														// Determines if both objects are assigned
		{
			if((lMinimumWidthReached == true && offsetX < leftOuterBorder) || (rMinimumWidthReached == true && offsetX > rightOuterBorder))		// These section determines if a scale should be performed
			{
				scale = false;
			}
			else
			{
				scale = true;
			}
		}
		else if (leftObject != null)																		// Determines if the left object is assigned
		{
			if(lMinimumWidthReached == true && offsetX < leftOuterBorder)									// These section determines if a scale should be performed
			{
				scale = false;
			}
			else
			{
				scale = true;
			}
		}	
		else if (rightObject != null)																		// Determines if the right object is assigned
		{
			if(rMinimumWidthReached == true && offsetX > rightOuterBorder)									// These section determines if a scale should be performed
			{
				scale = false;
			}
			else
			{
				scale = true;
			}
		}


		if (restrainToParent == true)
		{
			if (lMaximumWidthReached == true && offsetX > rightOuterBorder)									// These section determines if a scale should be performed if only one object is assigned
			{
				scale = false;
			}
			else if (rMaximumWidthReached == true && offsetX < leftOuterBorder)
			{
				scale = false;
			}
		}
		
		
		if(scale == true)			// This statement runs if all factors allow scaling to take place
		{
			transform.position = new Vector2 (Input.mousePosition.x - offsetX, transform.position.y);															// Moves the object
			
			if (leftObject != null)																																// Determines if the left object is assigned
			{
				RectTransform leftObjectRectTransform = leftObject.gameObject.GetComponent<RectTransform> ();													// This section gets the RectTransform information from the left object
				leftObjectRectTransform.offsetMax = new Vector2 (leftObjectRectTransform.offsetMax.x + movementX, leftObjectRectTransform.offsetMax.y);			// Scales the left object
			}
			
			
			if (rightObject != null)																															// Determines if the right object is assigned
			{
				RectTransform rightObjectRectTransform = rightObject.gameObject.GetComponent<RectTransform> ();													// This section gets the RectTransform information from the right object
				rightObjectRectTransform.offsetMin = new Vector2 (rightObjectRectTransform.offsetMin.x + movementX, rightObjectRectTransform.offsetMin.y);		// Scales the right object
			}
		}		
	}
		
	
	public void IntervalToggle ()									// This function toggles the interval method on and off
	{
		intervalBased = !intervalBased;
	}
	
	
	public void ScaleOtherObjectToggles ()							// This function toggles the ability to scale the other objects on and off. Good for events that toggle
	{
		disableScale = !disableScale;
	}
	
	
	public void DisableScaleOtherObjects ()							// This function disables this ability to scale the other objects. Good for events that do not toggle
	{
		if (Input.GetMouseButton(0) && startOnObject == true)		// Assures that the scale ability will not be disabled while already in progress. This is important when using events such as Pointer Enter and Exit
		{
			return;
		}
		else if(disableScale == false)
		{
			disableScale = true;
		}
	}
	
	
	public void EnableScaleOtherObjects ()							// This function enables the ability to scale the other objects. Good for events that do not toggle
	{
		if (disableScale == true)
		{
			disableScale = false;
		}
	}
}