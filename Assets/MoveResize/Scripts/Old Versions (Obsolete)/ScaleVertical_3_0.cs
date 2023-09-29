using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ScaleVertical_3_0 : MonoBehaviour {

	public GameObject topObject;						// Defines the top object to be scaled
	public GameObject bottomObject;						// Defines the bottom object to be scaled
	public bool includeChildren = true;					// Determines if the object's children will be included in the raycast return
	public bool ignoreTextBox = true;					// Determines if text box children will be included in the raycast return
	public bool disableScale = false;					// Determines if the ability to scale the other objects is enabled or disabled
	public bool restrainToParent = false;				// Determines if a single object will be restrained fully to its parent
	public float restrainTopInset = 0;					// Sets the distance from the top edge of its parent that the bottom object will be restrained to
	public float restrainBottomInset = 0;				// Sets the distance from the bottom edge of its parent that the top object will be restrained to
	public bool intervalBased = false;					// Determines if this other objects will scale based on intervals
	public float intervalDistance = 50;					// Sets the size of intervals if the interval method is true
	public float tMinimumHeight = 10;					// Sets the minimum height the top object may scale to
	public float bMinimumHeight = 10;					// Sets the minimum height the bottom object may scale to
	public bool customCursor = false;					// Determines if custom cursors are used
	public Texture2D normalCursor;						// Defines the normal custom cursor
	public int xHotspotNormal = 0;						// Sets the hotspot poisiton for the "normal" custom cursor on the x-axis
	public int yHotspotNormal = 0;						// Sets the hotspot poisiton for the "normal" custom cursor on the y-axis
	public Texture2D moveVerticalCursor;				// Defines the custom cursor for moving vertically
	public int xHotspotVert = 0;						// Sets the hotspot poisiton for the "moveVeritcal" custom cursor on the x-axis
	public int yHotspotVert = 0;						// Sets the hotspot poisiton for the "moveVeritcal" custom cursor on the y-axis
	CursorMode cursorMode = CursorMode.Auto;			// The cursor mode
	bool enteredObject = false;							// Determines if the cursor has entered or exited this object. This is important for changing back to a normal/null cursor only once after the cursor leaves a movable position 
	bool startOnObject = false;							// Determines if the cursor is on the object when the mouse button is initially pressed
	bool tMinimumHeightReached = false;					// Determines if the top object has reached the minimum height allowed
	bool bMinimumHeightReached = false;					// Determines if the bottom object has reached the minimum height allowed
	bool tMaximumHeightReached = false;					// Determines if the top object has reached the minimum height allowed
	bool bMaximumHeightReached = false;					// Determines if the bottom object has reached the minimum height allowed
	bool scale = true;									// Determines if the conditions are true for the objects to be scaled
	float oldMouseY;									// Stores the cursor position on the y-axis for comparison with the newer "mouseY" variable
	float mouseY;										// Stores the current cursor position on the y-axis for comparison with the older "oldMouseY" variable
	float trackY;										// Used to track the distance the cursor moves over the y-axis during the entire time the mouse button is pressed. Interval method only

	void Start ()
	{
		if (topObject == null && bottomObject == null)
		{
			Debug.Log ("You have not assigned an object to scale to: " + transform.name);			// This only happens if neither "topObject" or "bottomObject" is assigned
		}
	}

	
	void Update ()
	{
		if (disableScale == false && (topObject != null || bottomObject != null))
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
				
				oldMouseY = Input.mousePosition.y;								// Stores the initial cursor position

				// The following line determines if the cursor is on a valid movable position based on the object
				if(Input.mousePosition.x <= (transform.position.x + rightOuterBorder) && Input.mousePosition.x >= (transform.position.x + leftOuterBorder) && Input.mousePosition.y <= (transform.position.y + topOuterBorder) && Input.mousePosition.y >= (transform.position.y + bottomOuterBorder))
				{
					PerformRaycast ();											// Calls the function to perform a raycast
				}
			}
			else if (Input.GetMouseButton(0) && startOnObject == true)			// Determines if the mouse button is currently pressed and if it was initially pressed when on the object
			{
				mouseY = Input.mousePosition.y;									// Stores the current cursor position
				ScaleTopBottomObjects();										// Calls the function to scale the other objects
				oldMouseY = mouseY;												// Stores the current cursor position that will become the previous position in the next frame
			}
			else if(customCursor == true && Input.GetMouseButton(0) == false)	// Determines if the mouse button is not pressed and the "customCursor" variable is set to true
			{
				SetCursor ();													// Calls the function to set the cursor to the proper cursor image
			}
			
			
			if (Input.GetMouseButtonUp(0))										// When the mouse button is released, the "startOnObject" variable will be set to false, and the tracking variable will be reset to zero
			{
				startOnObject = false;
				trackY = 0;
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
		Vector2 vertHotspot = new Vector2 (0 + xHotspotVert, 0 + yHotspotVert);
		
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
					if (moveVerticalCursor != null)					// The default movement
					{
						Cursor.SetCursor (moveVerticalCursor, vertHotspot, cursorMode);			// Sets the cursor to use the "move vertical" custom cursor if assigned
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
						mouseY = Input.mousePosition.y;														// Stores the current cursor position
						startOnObject = true;																// Sets the "startOnObject" variable to true since the cursor is determined to be on the object
						return;																				// Returns early from this function
					}
				}
				
				
				if(objectsHit[x].gameObject == this.gameObject) 											// This section runs only if this object is the front object where the cursor is
				{	
					mouseY = Input.mousePosition.y;															// Stores the current cursor position
					startOnObject = true;																	// Sets the "startOnObject" variable to true since the cursor is determined to be on the object
				}
			}
		}
	}
	
	
	void ScaleTopBottomObjects ()		// This function is used to determine the amount of movement of the cursor, if the minimum limits have been reached, and then it scales the other objects
	{
		RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();						// This section gets the RectTransform information from this object. Height is stored in a variable. The borders of the object are also defined
		float height = objectRectTransform.rect.height;
		float topOuterBorder = (height * .5f);
		float bottomOuterBorder = (height * -.5f);
		float topEdge = transform.position.y + topOuterBorder;
		float bottomEdge = transform.position.y + bottomOuterBorder;
		float tHeight;
		float bHeight;

		RectTransform parentRectTransform = transform.parent.gameObject.GetComponent<RectTransform> ();		// This section gets the RectTransform information from the parent. Height is stored in a variable. The borders of the object are also defined
		float parentHeight = parentRectTransform.rect.height;
		float parentTopOuterBorder = (parentHeight * .5f) - restrainTopInset;
		float parentBottomOuterBorder = (parentHeight * -.5f) + restrainBottomInset;
		float parentTopEdge = transform.parent.position.y + parentTopOuterBorder;
		float parentBottomEdge = transform.parent.position.y + parentBottomOuterBorder;

		if (topObject != null)																				// Determines if a top object is assigned
		{
			RectTransform topObjectRectTransform = topObject.gameObject.GetComponent<RectTransform> ();		// This section gets the RectTransform information from the top object. Height is stored in a variable
			tHeight = topObjectRectTransform.rect.height;
		}
		else
		{
			tHeight = 0;																					// Sets the variables to 0 because there is no top object assianged
			tMinimumHeight = 0;
		}


		if (bottomObject != null)																			// Determines if a bottom object is assigned
		{		
			RectTransform bottomObjectRectTransform = bottomObject.gameObject.GetComponent<RectTransform> ();	// This section gets the RectTransform information from the bottom object. Height is stored in a variable
			bHeight = bottomObjectRectTransform.rect.height;
		}
		else
		{
			bHeight = 0;																					// Sets the variables to 0 because there is no bottom object assianged
			bMinimumHeight = 0;
		}


		float movementY = mouseY - oldMouseY;																// Compares the current cursor position on the y-axis to the previous one, finding the amount of movement over the frame
		
		if (intervalBased == true)																			// This section is only used if the interval method is selected
		{
			trackY = trackY + movementY;																	// Tracks the amount of movement on the y-axis since the last interval
			
			if (trackY >= intervalDistance)																	// Determines if the tracked POSITIVE movement on the y-axis is greater than the interval distance
			{
				int intervalsToMoveX = (Mathf.FloorToInt (trackY / intervalDistance));						// Determines how many intervals on the POSITIVE y-axis the other objects should be scaled this frame
				movementY = intervalsToMoveX * intervalDistance;											// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else if (- trackY >= intervalDistance)															// Determines if the tracked NEGATIVE movement on the y-axis is greater than the interval distance
			{
				int intervalsToMoveX = (Mathf.FloorToInt (-trackY / intervalDistance));						// Determines how many intervals on the NEGATIVE y-axis the other objects should be scaled this frame
				movementY = - intervalsToMoveX * intervalDistance;											// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else
			{
				movementY = 0;																				// If the tracked movement is not enough to move an interval, the movement of scale this frame is set to zero
			}				
			
			
			trackY = trackY % intervalDistance;																// Sets the tracking of movement on the y-axis to be the remainder of any extra movement if it is greater than the interval distance			
		}
		
		
		if(movementY > 0 && movementY > tHeight - tMinimumHeight && topObject != null)						// Determines if the POSITIVE movement is greater than the distance to reach the minimum height of the top object if assigned
		{
			movementY = tHeight - tMinimumHeight;															// Sets the amount of POSITIVE movement to the distance it takes to reach the minimum height of the top object
			tMinimumHeightReached = true;																	// Sets the "bMinimumHeightReached" variable to true
		}
		else if (movementY < 0 && - movementY > bHeight - bMinimumHeight && bottomObject != null)			// Determines if the NEGATIVE movement is greater than the distance to reach the minimum height of the bottom object if assigned
		{
			movementY = - (bHeight - bMinimumHeight);														// Sets the amount of NEGATIVE movement to the distance it takes to reach the minimum height of the bottom object
			bMinimumHeightReached = true;																	// Sets the "tMinimumHeightReached" variable to true
		}

		
		if (tHeight <= tMinimumHeight)																		// Determines if the top object is at the minimum height
		{
			tMinimumHeightReached = true;
		}
		else
		{
			tMinimumHeightReached = false;
		}
		
		
		if (bHeight <= bMinimumHeight)																		// Determines if the bottom object is at the minimum height
		{
			bMinimumHeightReached = true;
		}
		else
		{
			bMinimumHeightReached = false;
		}


		if (restrainToParent == true)
		{
			if(movementY > 0 && movementY > parentTopEdge - topEdge)										// Determines if the POSITIVE movement is greater than the distance to reach the parent's top edge
			{
				movementY = parentTopEdge - topEdge;														// Sets the amount of POSITIVE movement to the distance it takes to reach the parent's top edge
				bMaximumHeightReached = true;																// Sets the "bMaximumHeightReached" variable to true
			}
			else if (movementY < 0 && - movementY > bottomEdge - parentBottomEdge)							// Determines if the NEGATIVE movement is greater than the distance to reach the parent's bottom edge
			{
				movementY = - ( bottomEdge - parentBottomEdge);												// Sets the amount of NEGATIVE movement to the distance it takes to reach the parent's bottom edge
				tMaximumHeightReached = true;																// Sets the "tMaximumHeightReached" variable to true
			}


			if (bottomEdge - parentBottomEdge <= restrainBottomInset)										// Determines if the top object is at the maximum height
			{
				tMaximumHeightReached = true;
			}
			else
			{
				tMaximumHeightReached = false;
			}
			
			
			if (parentTopEdge - topEdge <= restrainTopInset)												// Determines if the bottom object is at the maximum height
			{
				bMaximumHeightReached = true;
			}
			else
			{
				bMaximumHeightReached = false;
			}
		}
		
		
		float offsetY = mouseY - transform.position.y - movementY;											// Used to determine the distance between the cursor position and the object position on the x-axis. This is so the object does not jump to center around the cursor when it is moved

		if (topObject != null && bottomObject != null)														// Determines if both objects are assigned
		{
			if((tMinimumHeightReached == true && offsetY > bottomOuterBorder) || (bMinimumHeightReached == true && offsetY < topOuterBorder))	// These section determines if a scale should be performed
			{
				scale = false;
			}
			else
			{
				scale = true;
			}
		}
		else if (topObject != null)																			// Determines if the top object is assigned
		{	
			if(tMinimumHeightReached == true && offsetY > bottomOuterBorder)								// These section determines if a scale should be performed
			{
				scale = false;
			}
			else
			{
				scale = true;
			}
		}
		else if (bottomObject != null)																		// Determines if the bottom object is assigned
		{
			if(bMinimumHeightReached == true && offsetY < topOuterBorder)									// These section determines if a scale should be performed
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
			if (bMaximumHeightReached == true && offsetY > topOuterBorder)									// These section determines if a scale should be performed if only one object is assigned
			{
				scale = false;
			}
			else if (tMaximumHeightReached == true && offsetY < bottomOuterBorder)
			{
				scale = false;
			}
		}
		
		
		if(scale == true)			// This statement runs if all factors allow scaling to take place
		{
			transform.position = new Vector2 (transform.position.x, Input.mousePosition.y - offsetY);															// Moves the object
			
			if (topObject != null)																																// Determines if the top object is assigned
			{
				RectTransform topObjectRectTransform = topObject.gameObject.GetComponent<RectTransform> ();														// This section gets the RectTransform information from the top object
				topObjectRectTransform.offsetMin = new Vector2 (topObjectRectTransform.offsetMin.x, topObjectRectTransform.offsetMin.y + movementY);			// Scales the top object
			}


			if (bottomObject != null)																															// Determines if the bottom object is assigned
			{
				RectTransform bottomObjectRectTransform = bottomObject.gameObject.GetComponent<RectTransform> ();												// This section gets the RectTransform information from the bottom object
				bottomObjectRectTransform.offsetMax = new Vector2 (bottomObjectRectTransform.offsetMax.x, bottomObjectRectTransform.offsetMax.y + movementY);	// Scales the bottom object
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