using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Resize_1_0 : MonoBehaviour {


	public bool includeChildren = true;			// Determines if the object's children will be included in the raycast return
	public bool ignoreTextBox = true;			// Determines if text box children will be included in the raycast return
	public bool disableResize = false;			// Determines if the ability to resize is enabled or disabled
	public bool disableTopRight = false;		// Determines if the ability to resize from the TopRight is enabled or disabled
	public bool disableBottomRight = false;		// Determines if the ability to resize from the BottomRight is enabled or disabled
	public bool disableRight = false;			// Determines if the ability to resize from the Right is enabled or disabled
	public bool disableTopLeft = false;			// Determines if the ability to resize from the TopLeft is enabled or disabled
	public bool disableBottomLeft = false;		// Determines if the ability to resize from the BottomLeft is enabled or disabled
	public bool disableLeft = false;			// Determines if the ability to resize from the Left is enabled or disabled
	public bool disableTop = false;				// Determines if the ability to resize from the Top is enabled or disabled
	public bool disableBottom = false;			// Determines if the ability to resize from the Bottom is enabled or disabled
	public bool intervalBased = false;			// Determines if this object will resize based on intervals
	bool startOnBorder = false;					// Determines if the cursor is on a border when the mouse button is initially pressed
	bool minimumWidthReached = false;			// Determines if the object has reached the minimum width allowed
	bool minimumHeightReached = false;			// Determines if the object has reached the minimum height allowed
	public float intervalDistance = 50;			// Sets the size of intervals if the interval method is true
	public float borderThickness = 10;			// Sets border thickness of the borders. This determines where the cursor must be in order for resizing to work
	public float minimumWidth = 0;				// Sets the minimum width the object may resize to. The minimum width shall be set to no less than twice the thickness of the border
	public float minimumHeight = 0;				// Sets the minimum height the object may resize to. The minimum height shall be set to no less than twice the thickness of the border
	float oldMouseX;							// Stores the cursor position on the x-axis for comparison with the newer "mouseX" variable
	float oldMouseY;							// Stores the cursor position on the y-axis for comparison with the newer "mouseY" variable
	float mouseX;								// Stores the current cursor position on the x-axis for comparison with the older "oldMouseX" variable
	float mouseY;								// Stores the current cursor position on the y-axis for comparison with the older "oldMouseY" variable
	float trackX;								// Used to track the distance the cursor moves over the x-axis during the entire time the mouse button is pressed. Interval method only
	float trackY;								// Used to track the distance the cursor moves over the y-axis during the entire time the mouse button is pressed. Interval method only
	int sector = 0;								// Stores the sector that the cursor is on when the mouse button is initially pressed

	
	void Update ()
	{
		if(disableResize == false)
		{
			if (minimumWidth < borderThickness)									// This section sets the minimum width and height to twice the thickness of the border if they are less. This is so an object cannot be resized into nothingness unless the border is deliberately set to 0
			{
				minimumWidth = borderThickness * 2;
			}
			
			
			if (minimumHeight < borderThickness)
			{
				minimumHeight = borderThickness * 2;
			}
			
			
			if (Input.GetMouseButtonDown(0))									
			{
				oldMouseX = Input.mousePosition.x;								// Stores the initial cursor position
				oldMouseY = Input.mousePosition.y;
				
				
				ResizeObject ();												// Calls the function to determine if the cursor is on a border when the mouse button is intially pressed
				
				
				if(sector != 0)													// This if statement runs if the cursor is on the object when the mouse button is initally pressed
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
			
			
			if (Input.GetMouseButtonUp(0))										// When the mouse button is released, the "startOnBorder" variable will be set to false, and the sector and tracking variables will be reset to zero
			{
				startOnBorder = false;
				sector = 0;
				trackX = 0;
				trackY = 0;
			}
		}
	}

	
	void PerformRaycast ()				// This function performs a raycast to detect if this object or its child is in front of other objects if any overlap at the cursor's position when the mouse button is initially pressed. If so, the object is set as the last sibling in the hierarchy
	{
		PointerEventData cursor = new PointerEventData(EventSystem.current);
		cursor.position = Input.mousePosition;
		List<RaycastResult> objectsHit = new List<RaycastResult> ();
		EventSystem.current.RaycastAll(cursor, objectsHit);
		int x = 0;

		if(objectsHit[0].gameObject == this.gameObject || objectsHit[0].gameObject.transform.IsChildOf (transform))				// This section runs only if this object or its child is the front object where the cursor is
		{
			transform.SetAsLastSibling();						// Sets this object to be the last sibling in the hierarchy, making it the forward-most object

			if(includeChildren == true)								
			{
				if (objectsHit[0].gameObject.GetComponent("Text") == true && ignoreTextBox == true)			// This section runs if you wish to ignore text boxes. If a text box is the front object, it will select the object behind it
				{
					while (objectsHit[x].gameObject.GetComponent("Text") == true)
					{
						x++;
					}
				}
				
				
				if(objectsHit[x].gameObject.GetComponent("DisableMoveResize") == false)						// This statement runs only if the front object where the cursor is does not have the "DisableMoveResize" script attached
				{
					
					mouseX = Input.mousePosition.x;					// Stores the current cursor position
					mouseY = Input.mousePosition.y;
					
					if(sector != 9)									// This if statement runs if the cursor is not on the center sector of the object
					{
						startOnBorder = true;						// Sets the "startOnBorder" variable to true since the cursor is determined to be on a valid moveable position
					}
					return;
				}
			}
		}


		if(objectsHit[0].gameObject == this.gameObject) 		// This section runs only if this object is the front object where the cursor is
		{
			mouseX = Input.mousePosition.x;						// Stores the current cursor position
			mouseY = Input.mousePosition.y;

			if(sector != 9)										// This if statement runs if the cursor is not on the center sector of the object
			{
				startOnBorder = true;							// Sets the "startOnBorder" variable to true since the cursor is determined to be on a valid moveable position
			}
		}
	}

	
	void ResizeObject ()			// This function is used to determine if the cursor is over a border when the mouse button it initially pressed and which border it is. It is also used to determine the amount of movement of the cursor, if the minimum limits have been reached, and then it resizes the object
	{
		RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();		// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
		float width = objectRectTransform.rect.width;
		float height = objectRectTransform.rect.height;
		float rightOuterBorder = (width * .5f);
		float leftOuterBorder = (width * -.5f);
		float topOuterBorder = (height * .5f);
		float bottomOuterBorder = (height * -.5f);
		float rightInnerBorder = rightOuterBorder - borderThickness;
		float leftInnerBorder = leftOuterBorder + borderThickness;
		float topInnerBorder = topOuterBorder - borderThickness;
		float bottomInnerBorder = bottomOuterBorder+ borderThickness;

		float movementX = mouseX - oldMouseX;												// Compares the current cursor position on the x-axis to the previous one, finding the amount of movement over the frame
		float movementY = mouseY - oldMouseY;												// Compares the current cursor position on the y-axis to the previous one, finding the amount of movement over the frame

		float offsetX = mouseX - transform.position.x - movementX;							// Used to determine the distance between the cursor position and the object position on the x-axis. This is so the object does not jump to center around the cursor when it is moved
		float offsetY = mouseY - transform.position.y - movementY;							// Used to determine the distance between the cursor position and the object position on the y-axis. This is so the object does not jump to center around the cursor when it is moved
		
		
		if (startOnBorder == false)															// The "startOnBorder" variable will be false when the mouse button is intially pressed
		{
			if(offsetX > rightInnerBorder && offsetX < rightOuterBorder && offsetY < topOuterBorder && offsetY > bottomOuterBorder)				// This section determines if the cursor is over a border when the mouse button it initially pressed and which border it is
			{
				if(offsetY > topInnerBorder)
				{
					sector = 1;
				}
				else if(offsetY < bottomInnerBorder)
				{
					sector = 2;
				}
				else
				{
					sector = 3;
				}
			}
			else if(offsetX < leftInnerBorder && offsetX > leftOuterBorder && offsetY < topOuterBorder && offsetY > bottomOuterBorder)
			{
				if(offsetY > topInnerBorder)
				{
					sector = 4;
				}
				else if(offsetY < bottomInnerBorder)
				{
					sector = 5;
				}
				else
				{
					sector = 6;
				}
			}
			else if(offsetY > topInnerBorder && offsetY < topOuterBorder && offsetX < rightOuterBorder && offsetX > leftOuterBorder)
			{
				sector = 7;
			}
			else if(offsetY < bottomInnerBorder && offsetY > bottomOuterBorder && offsetX < rightOuterBorder && offsetX > leftOuterBorder)
			{
				sector = 8;
			}
			else if(offsetY < topInnerBorder && offsetY > bottomInnerBorder && offsetX < rightInnerBorder && offsetX > leftInnerBorder)
			{
				sector = 9;
			}
			else
			{
				sector = 0;
			}
		}
		else    																					// The "startOnBorder" variable will be true after the mouse button is intially pressed when over a valid border
		{


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
			
			
			if(movementX > 0 && movementX > width - minimumWidth && sector > 3)						// Determines if the POSITIVE movement from a left sector is greater than the distance to reach the minimum width
			{
				movementX = width - minimumWidth;													// Sets the amount of POSITIVE movement to the distance it takes to reach the minimum width
				
				
				if (intervalBased == true)
				{
					minimumWidthReached = true;														// Sets the "minimumWidthReached" variable to true if the interval method is used
				}
			}
			else if (movementX < 0 && - movementX > width - minimumWidth && sector <= 3)			// Determines if the NEGATIVE movement from a right sector is greater than the distance to reach the minimum width
			{
				movementX = - ( width - minimumWidth);												// Sets the amount of NEGATIVE movement to the distance it takes to reach the minimum width
				
				
				if (intervalBased == true)
				{
					minimumWidthReached = true;														// Sets the "minimumWidthReached" variable to true if the interval method is used
				}
			}
			
			
			if(movementY > 0 && movementY > height - minimumHeight && (sector == 2 || sector == 5 || sector == 8))					// Determines if the POSITIVE movement from a bottom sector is greater than the distance to reach the minimum height
			{
				movementY = height - minimumHeight;																					// Sets the amount of POSITIVE movement to the distance it takes to reach the minimum height
				
				
				if (intervalBased == true)
				{
					minimumHeightReached = true;																					// Sets the "minimumHeightReached" variable to true if the interval method is used
				}
			}
			else if (movementY < 0 && - movementY > height - minimumHeight && (sector == 1 || sector == 4 || sector == 7))			// Determines if the NEGATIVE movement from a top sector is greater than the distance to reach the minimum height
			{
				movementY = - (height - minimumHeight);																				// Sets the amount of NEGATIVE movement to the distance it takes to reach the minimum height
				
				
				if (intervalBased == true)
				{
					minimumHeightReached = true;																					// Sets the "minimumHeightReached" variable to true if the interval method is used
				}
			}
			
			
			// The following section is broken down into each sector. Each sector determines the direction(s) that the object will be resized. Each section has two purposes.
			// First, if the user continues to drag the cursor beyond the minimum limitations, the cursor will leave the draggable border.
			// While the object will not minimize any further, if the cursor reverses direction, the object would immediately enlarge even though the cursor may be nowhere near the appropriate border.
			// These checks assure that the object will no longer begin to enlarge until the cursor returns near the appropriate border on the axis of movement.
			// Secondly, the object is resized based on the specific border. As for the offsets, the corresponding sides are as follows...
			// offsetMin.x : Left
			// offsetMin.y : Bottom
			// offsetMax.x : Right
			// offsetMax.y : Top
			// The first sector's comments apply to the rest.
			
			
			if(sector == 1)					// TopRight
			{
				if(disableTopRight == false)
				{
					if((offsetX < rightInnerBorder && intervalBased == false) || (offsetX < rightInnerBorder && intervalBased == true && minimumWidthReached == true))			// Determines if the cursor is beyond the minimum width
					{
						movementX = 0;																																			// Sets movement amount on the x-axis to zero
						trackX = 0;																																				// Sets tracked movement on the x-axis to zero
					}
					else
					{
						minimumWidthReached = false;																															// Sets "minimumWidthReached" variable to false
					}
					
					
					if((offsetY < topInnerBorder && intervalBased == false) || (offsetY < topInnerBorder && intervalBased == true && minimumHeightReached == true))				// Determines if the cursor is beyond the minimum height
					{
						movementY = 0;																																			// Sets movement amount on the y-axis to zero
						trackY = 0;																																				// Sets tracked movement on the y-axis to zero
					}
					else
					{
						minimumHeightReached = false;																															// Sets "minimumHeightReached" variable to false
					}
					
					
					objectRectTransform.offsetMax = new Vector2 (objectRectTransform.offsetMax.x + movementX, objectRectTransform.offsetMax.y + movementY);						// Resizes the object based on the appropriate offsets
				}
			}
			else if(sector == 2)			// BottomRight
			{
				if(disableBottomRight == false)
				{
					if((offsetX < rightInnerBorder && intervalBased == false) || (offsetX < rightInnerBorder && intervalBased == true && minimumWidthReached == true))
					{
						movementX = 0;
						trackX = 0;
					}
					else
					{
						minimumWidthReached = false;
					}
					
					
					if((offsetY > bottomInnerBorder && intervalBased == false) || (offsetY > bottomInnerBorder && intervalBased == true && minimumHeightReached == true))
					{
						movementY = 0;
						trackY = 0;
					}
					else
					{
						minimumHeightReached = false;
					}
					
					
					objectRectTransform.offsetMin = new Vector2 (objectRectTransform.offsetMin.x, objectRectTransform.offsetMin.y + movementY);
					objectRectTransform.offsetMax = new Vector2 (objectRectTransform.offsetMax.x + movementX, objectRectTransform.offsetMax.y);
				}
			}
			else if(sector == 3)			// Right
			{
				if(disableRight == false)
				{
					if((offsetX < rightInnerBorder && intervalBased == false) || (offsetX < rightInnerBorder && intervalBased == true && minimumWidthReached == true))
					{
						movementX = 0;
						trackX = 0;
					}
					else
					{
						minimumWidthReached = false;
					}
					
					
					objectRectTransform.offsetMax = new Vector2 (objectRectTransform.offsetMax.x + movementX, objectRectTransform.offsetMax.y);
				}
			}
			else if(sector == 4)			// TopLeft
			{
				if(disableTopLeft == false)
				{
					if((offsetX > leftInnerBorder && intervalBased == false) || (offsetX > leftInnerBorder && intervalBased == true && minimumWidthReached == true))
					{
						movementX = 0;
						trackX = 0;
					}
					else
					{
						minimumWidthReached = false;
					}
					
					
					if((offsetY < topInnerBorder && intervalBased == false) || (offsetY < topInnerBorder && intervalBased == true && minimumHeightReached == true))
					{
						movementY = 0;
						trackY = 0;
					}
					else
					{
						minimumHeightReached = false;
					}
					
					
					objectRectTransform.offsetMin = new Vector2 (objectRectTransform.offsetMin.x + movementX, objectRectTransform.offsetMin.y);
					objectRectTransform.offsetMax = new Vector2 (objectRectTransform.offsetMax.x, objectRectTransform.offsetMax.y + movementY);
				}
			}
			else if(sector == 5)			// BottomLeft
			{
				if(disableBottomLeft == false)
				{
					if((offsetX > leftInnerBorder && intervalBased == false) || (offsetX > leftInnerBorder && intervalBased == true && minimumWidthReached == true))
					{
						movementX = 0;
						trackX = 0;
					}
					else
					{
						minimumWidthReached = false;
					}
					
					
					if((offsetY > bottomInnerBorder && intervalBased == false) || (offsetY > bottomInnerBorder && intervalBased == true && minimumHeightReached == true))
					{
						movementY = 0;
						trackY = 0;
					}
					else
					{
						minimumHeightReached = false;
					}
					
					
					objectRectTransform.offsetMin = new Vector2 (objectRectTransform.offsetMin.x + movementX, objectRectTransform.offsetMin.y + movementY);
				}
			}
			else if(sector == 6)			// Left
			{
				if(disableLeft == false)
				{
					if((offsetX > leftInnerBorder && intervalBased == false) || (offsetX > leftInnerBorder && intervalBased == true && minimumWidthReached == true))
					{
						movementX = 0;
						trackX = 0;
					}
					else
					{
						minimumWidthReached = false;
					}
					
					
					objectRectTransform.offsetMin = new Vector2 (objectRectTransform.offsetMin.x + movementX, objectRectTransform.offsetMin.y);
				}
			}
			else if(sector == 7)			// Top
			{
				if(disableTop == false)
				{
					if((offsetY < topInnerBorder && intervalBased == false) || (offsetY < topInnerBorder && intervalBased == true && minimumHeightReached == true))
					{
						movementY = 0;
						trackY = 0;
					}
					else
					{
						minimumHeightReached = false;
					}
					
					
					objectRectTransform.offsetMax = new Vector2 (objectRectTransform.offsetMax.x, objectRectTransform.offsetMax.y + movementY);
				}
			}
			else if(sector == 8)			// Bottom
			{
				if(disableBottom == false)
				{
					if((offsetY > bottomInnerBorder && intervalBased == false) || (offsetY > bottomInnerBorder && intervalBased == true && minimumHeightReached == true))
					{
						movementY = 0;
						trackY = 0;
					}
					else
					{
						minimumHeightReached = false;
					}
					
					
					objectRectTransform.offsetMin = new Vector2 (objectRectTransform.offsetMin.x, objectRectTransform.offsetMin.y + movementY);
				}
			}
		}
	}


	public void IntervalToggle ()			// This function toggles the interval method on and off
	{
		if (intervalBased == false)
		{
			intervalBased = true;
		}
		else
		{
			intervalBased = false;
		}	
	}


	public void ResizeToggle ()				// This function toggles the ability to resize the object on and off. Good for events that toggle
	{
		if(disableResize == true)
		{
			disableResize = false;
		}
		else
		{
			disableResize = true;
		}	
	}


	public void DisableResizeObject ()		// This function disables this ability to resize the object. Good for events that do not toggle
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
	
	
	public void EnableResizeObject ()		// This function enables the ability to resize the object. Good for events that do not toggle
	{
		if (disableResize == true)
		{
			disableResize = false;
		}
	}
	

	// The remaining functions serve to disable or enable the specific borders' ability to resize the object
	
	
	public void DisableTopRight ()
	{
		if (disableTopRight == true)
		{
			disableTopRight = false;
		}
		else
		{
			disableTopRight = true;
		}	
	}
	
	
	public void DisableBottomRight ()
	{
		if (disableBottomRight == true)
		{
			disableBottomRight = false;
		}
		else
		{
			disableBottomRight = true;
		}	
	}
	
	
	public void DisableRight ()
	{
		if (disableRight == true)
		{
			disableRight = false;
		}
		else
		{
			disableRight = true;
		}	
	}
	
	
	public void DisableTopLeft ()
	{
		if (disableTopLeft == true)
		{
			disableTopLeft = false;
		}
		else
		{
			disableTopLeft = true;
		}	
	}
	
	
	public void DisableBottomLeft ()
	{
		if (disableBottomLeft == true)
		{
			disableBottomLeft = false;
		}
		else
		{
			disableBottomLeft = true;
		}
	}
	
	
	public void DisableLeft ()
	{
		if (disableLeft == true)
		{
			disableLeft = false;
		}
		else
		{
			disableLeft = true;
		}	
	}
	
	
	public void DisableTop ()
	{
		if (disableTop == true)
		{
			disableTop = false;
		}
		else
		{
			disableTop = true;
		}	
	}
	
	
	public void DisableBottom ()
	{
		if (disableBottom == true)
		{
			disableBottom = false;
		}
		else
		{
			disableBottom = true;
		}	
	}
}
