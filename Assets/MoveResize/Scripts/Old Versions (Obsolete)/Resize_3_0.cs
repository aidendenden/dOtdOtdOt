using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Resize_3_0 : MonoBehaviour {
	
	public bool includeChildren = true;					// Determines if this object's children will be included in the raycast return
	public bool ignoreTextBox = true;					// Determines if any text box will be included in the raycast return
	public bool bringToFront = true;					// Determines if this object will be set as the last sibling in the hierarchy
	public bool disableResize = false;					// Determines if the ability to resize is enabled or disabled
	public bool disableTopRight = false;				// Determines if the ability to resize from the TopRight is enabled or disabled
	public bool disableBottomRight = false;				// Determines if the ability to resize from the BottomRight is enabled or disabled
	public bool disableRight = false;					// Determines if the ability to resize from the Right is enabled or disabled
	public bool disableTopLeft = false;					// Determines if the ability to resize from the TopLeft is enabled or disabled
	public bool disableBottomLeft = false;				// Determines if the ability to resize from the BottomLeft is enabled or disabled
	public bool disableLeft = false;					// Determines if the ability to resize from the Left is enabled or disabled
	public bool disableTop = false;						// Determines if the ability to resize from the Top is enabled or disabled
	public bool disableBottom = false;					// Determines if the ability to resize from the Bottom is enabled or disabled
	public bool restrainToParent = false;				// Determines if this object will be restrained fully to its parent
	public float restrainRightInset = 0;				// Sets the distance from the right edge of the parent that this object will be restrained to
	public float restrainLeftInset = 0;					// Sets the distance from the left edge of the parent that this object will be restrained to
	public float restrainTopInset = 0;					// Sets the distance from the top edge of the parent that this object will be restrained to
	public float restrainBottomInset = 0;				// Sets the distance from the bottom edge of the parent that this object will be restrained to
	public bool intervalBased = false;					// Determines if this object will resize based on intervals
	public float intervalDistance = 50;					// Sets the size of intervals if the interval method is true
	public float rightBorderThickness = 10;				// Sets the thickness of the right border. This determines where the cursor must be in order for resizing to work
	public float leftBorderThickness = 10;				// Sets the thickness of the left border. This determines where the cursor must be in order for resizing to work
	public float topBorderThickness = 10;				// Sets the thickness of the top border. This determines where the cursor must be in order for resizing to work
	public float bottomBorderThickness = 10;			// Sets the thickness of the bottom border. This determines where the cursor must be in order for resizing to work
	public float minimumWidth = 0;						// Sets the minimum width this object may resize to. The minimum width shall be set to no less than the combined width of right and left borders
	public float minimumHeight = 0;						// Sets the minimum height this object may resize to. The minimum height shall be set to no less less than the combined height of the top and bottom borders
	public bool customCursor = false;					// Determines if custom cursors are used
	public bool bringToFrontOnOver = false;				// Determines if this object will be set as the last sibling in the hierarchy when the cursor is on this object
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
	bool TopRight = false;								// Determines if the TopRight section of this object's border is selected
	bool BottomRight = false;							// Determines if the BottomRight section of this object's border is selected
	bool Right = false;									// Determines if the Right section of this object's border is selected
	bool TopLeft = false;								// Determines if the TopLeft section of this object's border is selected
	bool BottomLeft = false;							// Determines if the BottomLeft section of this object's border is selected
	bool Left = false;									// Determines if the Left section of this object's border is selected
	bool Top = false;									// Determines if the Top section of this object's border is selected
	bool Bottom = false;								// Determines if the Bottom section of this object's border is selected
	bool onObject = false;								// Determines if the cursor is on this object when the mouse button is initially pressed
	bool startOnBorder = false;							// Determines if the cursor is on a border when the mouse button is initially pressed
	bool overBorder = false;							// Determines if the cursor is on this object's border
	bool minimumWidthReached = false;					// Determines if this object has reached the minimum width allowed
	bool minimumHeightReached = false;					// Determines if this object has reached the minimum height allowed
	bool moveRight = false;								// Determines if this object will resize from its right side
	bool moveLeft = false;								// Determines if this object will resize from its left side
	bool moveTop = false;								// Determines if this object will resize from its top
	bool moveBottom = false;							// Determines if this object will resize from its bottom
	bool enteredObject = false;							// Determines if the cursor has entered or exited this object. This is important for changing back to a normal/null cursor only once after the cursor leaves a movable position 
	float oldMouseX;									// Stores the cursor position on the x-axis for comparison with the newer "mouseX" variable
	float oldMouseY;									// Stores the cursor position on the y-axis for comparison with the newer "mouseY" variable
	float mouseX;										// Stores the current cursor position on the x-axis for comparison with the older "oldMouseX" variable
	float mouseY;										// Stores the current cursor position on the y-axis for comparison with the older "oldMouseY" variable
	float trackX;										// Used to track the distance the cursor moves over the x-axis during the entire time the mouse button is pressed. Interval method only
	float trackY;										// Used to track the distance the cursor moves over the y-axis during the entire time the mouse button is pressed. Interval method only
	
	void Update ()
	{
		if(disableResize == false)
		{
			if (minimumWidth < leftBorderThickness + rightBorderThickness)		// This section sets the minimum width to the thickness of the right and left borders if it is less. This is so an object cannot be resized into nothingness unless the borders are deliberately set to 0
			{
				minimumWidth = leftBorderThickness + rightBorderThickness;
			}
			
			
			if (minimumHeight < topBorderThickness + bottomBorderThickness)		// This section sets the minimum height to the thickness of the top and bottom borders if it is less. This is so an object cannot be resized into nothingness unless the borders are deliberately set to 0
			{
				minimumHeight = topBorderThickness + bottomBorderThickness;
			}
			
			
			if (Input.GetMouseButtonDown(0))									
			{
				oldMouseX = Input.mousePosition.x;								// Stores the initial cursor position
				oldMouseY = Input.mousePosition.y;
								
				InitialResizeObject ();											// Calls the function to determine if the cursor is on a border or on the object when the mouse button is intially pressed
								
				if(onObject == true)											// This if statement runs if the cursor is on the object when the mouse button is initally pressed
				{
					PerformRaycast();											// Calls the function to perform a raycast
				}
			}
			else if (Input.GetMouseButton(0) && startOnBorder == true)			// Determines if the mouse button is currently pressed and if it was initially pressed when on a border
			{
				mouseX = Input.mousePosition.x;									// Stores the current cursor position
				mouseY = Input.mousePosition.y;
								
				ResizeObject();													// Calls the function to resize the object
								
				oldMouseX = mouseX;												// Stores the current cursor position that will become the previous position in the next frame
				oldMouseY = mouseY;
			}
			else if(customCursor == true && Input.GetMouseButton(0) == false)	// Determines if the mouse button is not pressed and the "customCursor" variable is set to true
			{
				SetCursor ();													// Calls the function to set the cursor to the proper cursor image
			}
			
			
			if (Input.GetMouseButtonUp(0))										// When the mouse button is released, the border and movement variables will be set to false, and the sector and tracking variables will be reset to zero
			{
				TopRight = false;
				BottomRight = false;
				Right = false;
				TopLeft = false;
				BottomLeft = false;
				Left = false;
				Top = false;
				Bottom = false;
				moveRight = false;
				moveLeft = false;
				moveTop = false;
				moveBottom = false;
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
		float rightInnerBorder = rightOuterBorder - rightBorderThickness;
		float leftInnerBorder = leftOuterBorder + leftBorderThickness;
		float topInnerBorder = topOuterBorder - topBorderThickness;
		float bottomInnerBorder = bottomOuterBorder + bottomBorderThickness;

		Vector2 hotspot = new Vector2 (0 + xHotspotNormal, 0 + yHotspotNormal);					// This section defines the hotspots for the various custom cursors
		Vector2 horHotspot = new Vector2 (0 + xHotspotLR, 0 + yHotspotLR);
		Vector2 vertHotspot = new Vector2 (0 + xHotspotTB, 0 + yHotspotTB);
		Vector2 tlbrHotspot = new Vector2 (0 + xHotspotTLBR, 0 + yHotspotTLBR);
		Vector2 trblHotspot = new Vector2 (0 + xHotspotTRBL, 0 + yHotspotTRBL);

		// The following line determines if the cursor is on the object
		if(Input.mousePosition.x <= (transform.position.x + rightOuterBorder) && Input.mousePosition.x >= (transform.position.x + leftOuterBorder) && Input.mousePosition.y <= (transform.position.y + topOuterBorder) && Input.mousePosition.y >= (transform.position.y + bottomOuterBorder))
		{
			PerformConstantRaycast ();			// Calls the function to perform a raycast
			
			if (overBorder == true)				// This is true if the raycast determines this object is selected
			{
				enteredObject = true;			// Sets the "enteredObject" variable to true. This is important for changing back to a normal/null cursor only once after the cursor leaves a movable position
				overBorder = false;				// Sets the "overBorder" variable to false
				
				// The following line determines if the cursor is over a valid movable position
				if(Input.mousePosition.x > transform.position.x + rightInnerBorder && Input.mousePosition.x < transform.position.x + rightOuterBorder && Input.mousePosition.y < transform.position.y + topOuterBorder && Input.mousePosition.y > transform.position.y + bottomOuterBorder)
				{
					if(Input.mousePosition.y > transform.position.y + topInnerBorder)
					{
						if (resizeTopRightBottomLeftCursor != null && disableTopRight == false)					// Top Right
						{
							Cursor.SetCursor (resizeTopRightBottomLeftCursor, trblHotspot, cursorMode);			// Sets the cursor to use the "resizeTopRightBottomLeft" custom cursor if assigned
						}
						else if (normalCursor != null)															// The above custom cursor is null
						{
							Cursor.SetCursor (normalCursor, hotspot, cursorMode);								// Sets the cursor to use the "normal" custom cursor if assigned
						}
						else																					// The above custom cursors are null
						{
							Cursor.SetCursor (null, Vector2.zero, cursorMode);									// Sets the cursor to use the null cursor
						}
					}
					else if(Input.mousePosition.y < transform.position.y + bottomInnerBorder)
					{
						if (resizeTopLeftBottomRightCursor != null && disableBottomRight == false)				// Bottom Right
						{
							Cursor.SetCursor (resizeTopLeftBottomRightCursor, tlbrHotspot, cursorMode);			// Sets the cursor to use the "resizeTopLeftBottomRight" custom cursor if assigned
						}
						else if (normalCursor != null)															// The above custom cursor is null
						{
							Cursor.SetCursor (normalCursor, hotspot, cursorMode);								// Sets the cursor to use the "normal" custom cursor if assigned
						}
						else																					// The above custom cursors are null
						{
							Cursor.SetCursor (null, Vector2.zero, cursorMode);									// Sets the cursor to use the null cursor
						}
					}
					else
					{
						if (resizeLeftRightCursor != null && disableRight == false)								// Right
						{
							Cursor.SetCursor (resizeLeftRightCursor, horHotspot, cursorMode);					// Sets the cursor to use the "resizeLeftRight" custom cursor if assigned
						}
						else if (normalCursor != null)															// The above custom cursor is null
						{
							Cursor.SetCursor (normalCursor, hotspot, cursorMode);								// Sets the cursor to use the "normal" custom cursor if assigned
						}
						else																					// The above custom cursors are null
						{
							Cursor.SetCursor (null, Vector2.zero, cursorMode);									// Sets the cursor to use the null cursor
						}
					}
				}
				else if(Input.mousePosition.x < transform.position.x + leftInnerBorder && Input.mousePosition.x > transform.position.x + leftOuterBorder && Input.mousePosition.y < transform.position.y + topOuterBorder && Input.mousePosition.y > transform.position.y + bottomOuterBorder)
				{
					if(Input.mousePosition.y > transform.position.y + topInnerBorder)
					{
						if (resizeTopLeftBottomRightCursor != null && disableTopLeft == false)					// Top Left
						{
							Cursor.SetCursor (resizeTopLeftBottomRightCursor, tlbrHotspot, cursorMode);			// Sets the cursor to use the "resizeTopLeftBottomRight" custom cursor if assigned
						}
						else if (normalCursor != null)															// The above custom cursor is null
						{
							Cursor.SetCursor (normalCursor, hotspot, cursorMode);								// Sets the cursor to use the "normal" custom cursor if assigned
						}
						else																					// The above custom cursors are null
						{
							Cursor.SetCursor (null, Vector2.zero, cursorMode);									// Sets the cursor to use the null cursor
						}
					}
					else if(Input.mousePosition.y < transform.position.y + bottomInnerBorder)
					{
						if (resizeTopRightBottomLeftCursor != null && disableBottomLeft == false)				// Bottom Left
						{
							Cursor.SetCursor (resizeTopRightBottomLeftCursor, trblHotspot, cursorMode);			// Sets the cursor to use the "resizeTopRightBottomLeft" custom cursor if assigned
						}
						else if (normalCursor != null)															// The above custom cursor is null
						{
							Cursor.SetCursor (normalCursor, hotspot, cursorMode);								// Sets the cursor to use the "normal" custom cursor if assigned
						}
						else																					// The above custom cursors are null
						{
							Cursor.SetCursor (null, Vector2.zero, cursorMode);									// Sets the cursor to use the null cursor
						}
					}
					else
					{
						if (resizeLeftRightCursor != null && disableLeft == false)								// Left
						{
							Cursor.SetCursor (resizeLeftRightCursor, horHotspot, cursorMode);					// Sets the cursor to use the "resizeLeftRight" custom cursor if assigned
						}
						else if (normalCursor != null)															// The above custom cursor is null
						{
							Cursor.SetCursor (normalCursor, hotspot, cursorMode);								// Sets the cursor to use the "normal" custom cursor if assigned
						}
						else																					// The above custom cursors are null
						{
							Cursor.SetCursor (null, Vector2.zero, cursorMode);									// Sets the cursor to use the null cursor
						}
					}
				}
				else if(Input.mousePosition.y > transform.position.y + topInnerBorder && Input.mousePosition.y < transform.position.y + topOuterBorder && Input.mousePosition.x < transform.position.x + rightOuterBorder && Input.mousePosition.x > transform.position.x + leftOuterBorder)
				{
					if (resizeTopBottomCursor != null && disableTop == false)									// Top
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
				else if(Input.mousePosition.y < transform.position.y + bottomInnerBorder && Input.mousePosition.y > transform.position.y + bottomOuterBorder && Input.mousePosition.x < transform.position.x + rightOuterBorder && Input.mousePosition.x > transform.position.x + leftOuterBorder)
				{
					if (resizeTopBottomCursor != null && disableBottom == false)								// Bottom
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
				else if(Input.mousePosition.y < transform.position.y + topInnerBorder && Input.mousePosition.y > transform.position.y + bottomInnerBorder && Input.mousePosition.x < transform.position.x + rightInnerBorder && Input.mousePosition.x > transform.position.x + leftInnerBorder)
				{
					if (normalCursor != null)																	// Center
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


	void PerformConstantRaycast ()				// This function performs a raycast to detect if this object or its child is in front of other objects if any overlap at the cursor's position when the mouse button is initially pressed. If so, the object can be set as the last sibling in the hierarchy
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
					overBorder = true;																	// Sets the "overBorder" variable to true
					
					if (bringToFrontOnOver == true)
					{
						transform.SetAsLastSibling();													// Sets this object to be the last sibling in the hierarchy, making it the forward-most object
					}


					return;																				// Returns early from this function
				}
				
				
				if(includeChildren == true)								
				{
					// The following statement runs if the child object does not have a custom cursor script active or a DisableMoveReside script attached
					if(objectsHit[x].gameObject.GetComponent("DisableMoveResize") == false && (objectsHit[x].gameObject.GetComponent<Move>() == false || objectsHit[x].gameObject.GetComponent<Move>().customCursor == false) && (objectsHit[x].gameObject.GetComponent<MoveParent>() == false || objectsHit[x].gameObject.GetComponent<MoveParent>().customCursor == false) && (objectsHit[x].gameObject.GetComponent<MoveOther>() == false || objectsHit[x].gameObject.GetComponent<MoveOther>().customCursor == false) && (objectsHit[x].gameObject.GetComponent<Resize>() == false || objectsHit[x].gameObject.GetComponent<Resize>().customCursor == false) && (objectsHit[x].gameObject.GetComponent<ResizeParent>() == false || objectsHit[x].gameObject.GetComponent<ResizeParent>().customCursor == false) && (objectsHit[x].gameObject.GetComponent<ResizeOther>() == false || objectsHit[x].gameObject.GetComponent<ResizeOther>().customCursor == false) && (objectsHit[x].gameObject.GetComponent<ScaleHorizontal>() == false || objectsHit[x].gameObject.GetComponent<ScaleHorizontal>().customCursor == false) && (objectsHit[x].gameObject.GetComponent<ScaleVertical>() == false || objectsHit[x].gameObject.GetComponent<ScaleVertical>().customCursor == false))			// This statement runs only if the front object where the cursor is does not have the "DisableMoveResize" script attached
					{
						overBorder = true;																// Sets the "overBorder" variable to true			
					}


					if (bringToFrontOnOver == true)
					{
						transform.SetAsLastSibling();													// Sets this object to be the last sibling in the hierarchy, making it the forward-most object
					}
					
					
					return;																				// Returns early from this function
				}
				
				

			}
		}
	}


	void InitialResizeObject ()			// This function is used to determine if the cursor is over the object when the mouse button it initially pressed. It is also used to determine which border section will be used to resize the object
	{
		RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();		// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
		float width = objectRectTransform.rect.width;
		float height = objectRectTransform.rect.height;
		float rightOuterBorder = (width * .5f);
		float leftOuterBorder = (width * -.5f);
		float topOuterBorder = (height * .5f);
		float bottomOuterBorder = (height * -.5f);
		float rightInnerBorder = rightOuterBorder - rightBorderThickness;
		float leftInnerBorder = leftOuterBorder + leftBorderThickness;
		float topInnerBorder = topOuterBorder - topBorderThickness;
		float bottomInnerBorder = bottomOuterBorder + bottomBorderThickness;

		// The following section determines where the cursor is positioned when the mouse button is initially pressed and if it is on the object. It then assigns movement controls based on which section of the border the curser is on
		if(oldMouseX > transform.position.x + rightInnerBorder && oldMouseX < transform.position.x + rightOuterBorder && oldMouseY < transform.position.y + topOuterBorder && oldMouseY > transform.position.y + bottomOuterBorder)				
		{
			if(oldMouseY > transform.position.y + topInnerBorder)
			{
				TopRight = true;		// Top Right
				moveRight = true;		
				moveTop = true;
				onObject = true;
			}
			else if(oldMouseY < transform.position.y + bottomInnerBorder)
			{
				BottomRight = true;		// Bottom Right
				moveRight = true;		
				moveBottom = true;
				onObject = true;
			}
			else
			{
				Right = true;			// Right
				moveRight = true;		
				onObject = true;
			}
		}
		else if(oldMouseX < transform.position.x + leftInnerBorder && oldMouseX > transform.position.x + leftOuterBorder && oldMouseY < transform.position.y + topOuterBorder && oldMouseY > transform.position.y + bottomOuterBorder)
		{
			if(oldMouseY > transform.position.y + topInnerBorder)
			{
				TopLeft = true;			// Top Left
				moveLeft = true;		
				moveTop = true;
				onObject = true;
			}
			else if(oldMouseY < transform.position.y + bottomInnerBorder)
			{
				BottomLeft = true;		// Bottom Left
				moveLeft = true;		
				moveBottom = true;
				onObject = true;
			}
			else
			{
				Left = true;			// Left
				moveLeft = true;		
				onObject = true;
			}
		}
		else if(oldMouseY > transform.position.y + topInnerBorder && oldMouseY < transform.position.y + topOuterBorder && oldMouseX < transform.position.x + rightOuterBorder && oldMouseX > transform.position.x + leftOuterBorder)
		{
			Top = true;					// Top
			moveTop = true;				
			onObject = true;
		}
		else if(oldMouseY < transform.position.y + bottomInnerBorder && oldMouseY > transform.position.y + bottomOuterBorder && oldMouseX < transform.position.x + rightOuterBorder && oldMouseX > transform.position.x + leftOuterBorder)
		{
			Bottom = true;				// Bottom
			moveBottom = true;			
			onObject = true;
		}
		else if(oldMouseY < transform.position.y + topInnerBorder && oldMouseY > transform.position.y + bottomInnerBorder && oldMouseX < transform.position.x + rightInnerBorder && oldMouseX > transform.position.x + leftInnerBorder)
		{
			onObject = true;			// Center of the object
		}
		else
		{
			onObject = false;			// Not on the object
		}
	}


	void PerformRaycast ()				// This function performs a raycast to detect if this object or its child is in front of other objects if any overlap at the cursor's position when the mouse button is initially pressed. If so, the object can be set as the last sibling in the hierarchy
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

						onObject = true;

						if(moveRight == true || moveLeft == true || moveTop == true || moveBottom == true)		// This if statement runs if the cursor is not on the center sector of the object
						{
							startOnBorder = true;														// Sets the "startOnBorder" variable to true since the cursor is determined to be on a valid moveable position
						}


						if (bringToFront == true)
						{
							transform.SetAsLastSibling();														// Sets this object to be the last sibling in the hierarchy, making it the forward-most object
						}


						return;																			// Returns early from this function
					}
				}


				if(objectsHit[x].gameObject == this.gameObject) 										// This section runs only if this object is the front object where the cursor is
				{
					mouseX = Input.mousePosition.x;														// Stores the current cursor position
					mouseY = Input.mousePosition.y;

					onObject = true;
					
					if(moveRight == true || moveLeft == true || moveTop == true || moveBottom == true)	// This if statement runs if the cursor is not on the center sector of the object
					{
						startOnBorder = true;															// Sets the "startOnBorder" variable to true since the cursor is determined to be on a valid moveable position
					}


					if (bringToFront == true)
					{
						transform.SetAsLastSibling();														// Sets this object to be the last sibling in the hierarchy, making it the forward-most object
					}
				}
			}
		}
	}


	void ResizeObject ()			// This function is used to determine if the cursor is over a border when the mouse button it initially pressed and which border it is. It is also used to determine the amount of movement of the cursor, if the minimum limits have been reached, and then it resizes the object
	{
		RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();						// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
		float width = objectRectTransform.rect.width;
		float height = objectRectTransform.rect.height;
		float rightOuterBorder = (width * .5f);
		float leftOuterBorder = (width * -.5f);
		float topOuterBorder = (height * .5f);
		float bottomOuterBorder = (height * -.5f);
		float rightInnerBorder = rightOuterBorder - rightBorderThickness;
		float leftInnerBorder = leftOuterBorder + leftBorderThickness;
		float topInnerBorder = topOuterBorder - topBorderThickness;
		float bottomInnerBorder = bottomOuterBorder + bottomBorderThickness;
		float leftEdge = transform.position.x + leftOuterBorder;
		float rightEdge = transform.position.x + rightOuterBorder;
		float bottomEdge = transform.position.y + bottomOuterBorder;
		float topEdge = transform.position.y + topOuterBorder;

		RectTransform parentRectTransform = transform.parent.gameObject.GetComponent<RectTransform> ();		// This section gets the RectTransform information from the parent. Height and width are stored in variables. The borders of the object are also defined
		float parentWidth = parentRectTransform.rect.width;
		float parentHeight = parentRectTransform.rect.height;
		float parentRightOuterBorder = (parentWidth * .5f) - restrainRightInset;
		float parentLeftOuterBorder = (parentWidth * -.5f) + restrainLeftInset;
		float parentTopOuterBorder = (parentHeight * .5f) - restrainTopInset;
		float parentBottomOuterBorder = (parentHeight * -.5f) + restrainBottomInset;
		float parentLeftEdge = transform.parent.position.x + parentLeftOuterBorder;
		float parentRightEdge = transform.parent.position.x + parentRightOuterBorder;
		float parentBottomEdge = transform.parent.position.y + parentBottomOuterBorder;
		float parentTopEdge = transform.parent.position.y + parentTopOuterBorder;

		float movementX = mouseX - oldMouseX;													// Compares the current cursor position on the x-axis to the previous one, finding the amount of movement over the frame
		float movementY = mouseY - oldMouseY;													// Compares the current cursor position on the y-axis to the previous one, finding the amount of movement over the frame
	
		if (intervalBased == true)																// This section is only used if the interval method is selected
		{
			trackX = trackX + movementX;														// Tracks the amount of movement on the x-axis since the last interval

			if (trackX >= intervalDistance)														// Determines if the tracked POSITIVE movement on the x-axis is greater than the interval distance
			{
				int intervalsToMoveX = (Mathf.FloorToInt (trackX / intervalDistance));			// Determines how many intervals on the POSITIVE x-axis the object should be resized this frame
				movementX = intervalsToMoveX * intervalDistance;								// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else if (- trackX >= intervalDistance)												// Determines if the tracked NEGATIVE movement on the x-axis is greater than the interval distance
			{
				int intervalsToMoveX = (Mathf.FloorToInt (-trackX / intervalDistance));			// Determines how many intervals on the NEGATIVE x-axis the object should be resized this frame
				movementX = - intervalsToMoveX * intervalDistance;								// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else
			{
				movementX = 0;																	// If the tracked movement is not enough to move an interval, the movement of resize this frame is set to zero
			}			
			
			
			trackX = trackX % intervalDistance;													// Sets the tracking of movement on the x-axis to be the remainder of any extra movement if it is greater than the interval distance	
			trackY = trackY + movementY;														// Tracks the amount of movement on the y-axis since the last interval

			if (trackY >= intervalDistance)														// Determines if the tracked POSITIVE movement on the y-axis is greater than the interval distance
			{
				int intervalsToMoveY = (Mathf.FloorToInt (trackY / intervalDistance));			// Determines how many intervals on the POSITIVE y-axis the object should be resized this frame
				movementY = intervalsToMoveY * intervalDistance;								// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else if (- trackY >= intervalDistance)												// Determines if the tracked NEGATIVE movement on the y-axis is greater than the interval distance
			{
				int intervalsToMoveY = (Mathf.FloorToInt (-trackY / intervalDistance));			// Determines how many intervals on the NEGATIVE y-axis the object should be resized this frame
				movementY = - intervalsToMoveY * intervalDistance;								// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else
			{
				movementY = 0;																	// If the tracked movement is not enough to move an interval, the movement of resize this frame is set to zero
			}				
			
			
			trackY = trackY % intervalDistance;													// Sets the tracking of movement on the y-axis to be the remainder of any extra movement if it is greater than the interval distance
		}

		
		if(movementX > 0 && movementX > width - minimumWidth && moveLeft == true)				// Determines if the POSITIVE movement from a left sector is greater than the distance to reach the minimum width
		{
			movementX = width - minimumWidth;													// Sets the amount of POSITIVE movement to the distance it takes to reach the minimum width
			minimumWidthReached = true;	
		}
		else if (movementX < 0 && - movementX > width - minimumWidth && moveRight == true)		// Determines if the NEGATIVE movement from a right sector is greater than the distance to reach the minimum width
		{
			movementX = - ( width - minimumWidth);												// Sets the amount of NEGATIVE movement to the distance it takes to reach the minimum width
			minimumWidthReached = true;	
		}


		if (minimumWidthReached == true)			// This section determines if the cursor is inside of this object's inner borders if the minimum width has been reached. If so, movement and tracking are set to 0 on the x-axis
		{
			if ((moveRight == true && Input.mousePosition.x < transform.position.x + rightInnerBorder) || (moveLeft == true && Input.mousePosition.x > transform.position.x + leftInnerBorder))
			{
				movementX = 0;
				trackX = 0;
			}
			else
			{
				minimumWidthReached = false;	
			}
		}
		
		
		if(movementY > 0 && movementY > height - minimumHeight && moveBottom == true)			// Determines if the POSITIVE movement from a bottom sector is greater than the distance to reach the minimum height
		{
			movementY = height - minimumHeight;													// Sets the amount of POSITIVE movement to the distance it takes to reach the minimum height
			minimumHeightReached = true;
		}
		else if (movementY < 0 && - movementY > height - minimumHeight && moveTop == true)		// Determines if the NEGATIVE movement from a top sector is greater than the distance to reach the minimum height
		{
			movementY = - (height - minimumHeight);												// Sets the amount of NEGATIVE movement to the distance it takes to reach the minimum height
			minimumHeightReached = true;
		}


		if (minimumHeightReached == true)			// This section determines if the cursor is inside of this object's inner borders if the minimum height has been reached. If so, movement and tracking are set to 0 on the y-axis
		{
			if ((moveTop == true && Input.mousePosition.y < transform.position.y + topInnerBorder) || (moveBottom == true && Input.mousePosition.y > transform.position.y + bottomInnerBorder))
			{
				movementY = 0;
				trackY = 0;
			}
			else
			{
				minimumHeightReached = false;	
			}
		}


		if (restrainToParent == true)				
		{
			if(movementX > 0 && movementX > parentRightEdge - rightEdge && moveRight == true)				// Determines if the POSITIVE movement from a right sector is greater than the distance to reach the parent's right edge
			{
				movementX = parentRightEdge - rightEdge;													// Sets the amount of POSITIVE movement to the distance it takes to reach the parent's right edge
			}
			else if (movementX < 0 && - movementX > leftEdge - parentLeftEdge && moveLeft == true)			// Determines if the NEGATIVE movement from a left sector is greater than the distance to reach the parent's left edge
			{
				movementX = - ( leftEdge - parentLeftEdge);													// Sets the amount of NEGATIVE movement to the distance it takes to reach the parent's leftt edge
			}


			if(movementY > 0 && movementY > parentTopEdge - topEdge && moveTop == true)						// Determines if the POSITIVE movement from a top sector is greater than the distance to reach the parent's top edge
			{
				movementY = parentTopEdge - topEdge;														// Sets the amount of POSITIVE movement to the distance it takes to reach the parent's top edge
			}
			else if (movementY < 0 && - movementY > bottomEdge - parentBottomEdge && moveBottom == true)	// Determines if the NEGATIVE movement from a bottom sector is greater than the distance to reach the parent's bottom edge
			{
				movementY = - ( bottomEdge - parentBottomEdge);												// Sets the amount of NEGATIVE movement to the distance it takes to reach the parent's bottom edge
			}


			// The following section determines if this object has reached its parents borders and the cursor is outside of the object's outer borders. If so, the movement and tracking variables are set to 0
			if ((moveRight == true && rightEdge >= parentRightEdge && Input.mousePosition.x > rightEdge) || (moveLeft == true && leftEdge <= parentLeftEdge && Input.mousePosition.x < leftEdge))
			{
				movementX = 0;
				trackX = 0;
			}


			if ((moveTop == true && topEdge >= parentTopEdge && Input.mousePosition.y > topEdge) || (moveBottom == true && bottomEdge <= parentBottomEdge && Input.mousePosition.y < bottomEdge))
			{
				movementY = 0;
				trackY = 0;
			}
		}


		// The following section resizes the object based on the section of the border being dragged
		if (TopRight == true && disableTopRight == false)						
		{
			objectRectTransform.offsetMax = new Vector2 (objectRectTransform.offsetMax.x + movementX, objectRectTransform.offsetMax.y + movementY);			// Top Right			
		}
		else if (BottomRight == true && disableBottomRight == false)
		{
			objectRectTransform.offsetMin = new Vector2 (objectRectTransform.offsetMin.x, objectRectTransform.offsetMin.y + movementY);						// Bottom Right
			objectRectTransform.offsetMax = new Vector2 (objectRectTransform.offsetMax.x + movementX, objectRectTransform.offsetMax.y);
		}
		else if (Right == true && disableRight == false)
		{
			objectRectTransform.offsetMax = new Vector2 (objectRectTransform.offsetMax.x + movementX, objectRectTransform.offsetMax.y);						// Right
		}
		else if (TopLeft == true && disableTopLeft == false)
		{
			objectRectTransform.offsetMin = new Vector2 (objectRectTransform.offsetMin.x + movementX, objectRectTransform.offsetMin.y);						// Top Left
			objectRectTransform.offsetMax = new Vector2 (objectRectTransform.offsetMax.x, objectRectTransform.offsetMax.y + movementY);			
		}
		else if (BottomLeft == true && disableBottomLeft == false)
		{
			objectRectTransform.offsetMin = new Vector2 (objectRectTransform.offsetMin.x + movementX, objectRectTransform.offsetMin.y + movementY);			// Bottom Left
		}
		else if (Left == true & disableLeft == false)
		{
			objectRectTransform.offsetMin = new Vector2 (objectRectTransform.offsetMin.x + movementX, objectRectTransform.offsetMin.y);						// Left
		}
		else if (Top == true && disableTop == false)
		{
			objectRectTransform.offsetMax = new Vector2 (objectRectTransform.offsetMax.x, objectRectTransform.offsetMax.y + movementY);						// Top
		}
		else if (Bottom == true && disableBottom == false)
		{
			objectRectTransform.offsetMin = new Vector2 (objectRectTransform.offsetMin.x, objectRectTransform.offsetMin.y + movementY);						// Bottom
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


	public void RestrainToggle ()									// This function toggles the ability to restrain this object to its parent on and off
	{
		restrainToParent = !restrainToParent;
	}


	public void ResizeToggle ()										// This function toggles the ability to resize the object on and off. Good for events that toggle
	{
		disableResize = !disableResize;
	}


	public void EnableResize ()										// This function enables the ability to resize the object. Good for events that do not toggle
	{
		if (disableResize == true)
		{
			disableResize = false;
		}
	}


	public void DisableResize ()									// This function disables this ability to resize the object. Good for events that do not toggle
	{
		if (Input.GetMouseButton(0) && startOnBorder == true)		// Assures that the resize ability will not be disabled while already in progress. This is important when using events such as Pointer Enter and Exit
		{
			return;
		}
		else if(disableResize == false)
		{
			disableResize = true;
		}
	}
	
	
	// The remaining functions serve to toggle the specific borders' ability to resize the object on and off
	public void DisableTopRight ()
	{
		disableTopRight = !disableTopRight;
	}
	
	
	public void DisableBottomRight ()
	{
		disableBottomRight = !disableBottomRight;
	}
	
	
	public void DisableRight ()
	{
		disableRight = !disableRight;
	}
	
	
	public void DisableTopLeft ()
	{
		disableTopLeft = !disableTopLeft;
	}
	
	
	public void DisableBottomLeft ()
	{
		disableBottomLeft = !disableBottomLeft;
	}
	
	
	public void DisableLeft ()
	{
		disableLeft = !disableLeft;
	}
	
	
	public void DisableTop ()
	{
		disableTop = !disableTop;
	}
	
	
	public void DisableBottom ()
	{
		disableBottom = !disableBottom;
	}
}