using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ResizeOther_1_0 : MonoBehaviour {
	

	public GameObject otherObject;						// Defines the object to be resized
	public bool includeChildren = true;					// Determines if the object's children will be included in the raycast return
	public bool ignoreTextBox = true;					// Determines if text box children will be included in the raycast return
	public bool disableResizeOtherObject = false;		// Determines if the ability to resize the other object is enabled or disabled
	public bool TopRight = false;						// Sets the object to be designated as the TopRight control for the other object
	public bool BottomRight = false;					// Sets the object to be designated as the BottomRight control for the other object
	public bool Right = false;							// Sets the object to be designated as the Right control for the other object
	public bool TopLeft = false;						// Sets the object to be designated as the TopLeft control for the other object
	public bool BottomLeft = false;						// Sets the object to be designated as the BottomLeft control for the other object
	public bool Left = false;							// Sets the object to be designated as the Left control for the other object
	public bool Top = false;							// Sets the object to be designated as the Top control for the other object
	public bool Bottom = false;							// Sets the object to be designated as the Bottom control for the other object
	public bool intervalBased = false;					// Determines if this other object will resize based on intervals
	bool startOnBorder = false;							// Determines if the cursor is on the object when the mouse button is initially pressed
	bool minimumWidthReached = false;					// Determines if the other object has reached the minimum width allowed
	bool minimumHeightReached = false;					// Determines if the other object has reached the minimum height allowed
	public float intervalDistance = 50;					// Sets the size of intervals if the interval method is true
	public float borderThickness = 10;					// Sets border thickness of the other object
	public float minimumWidth = 0;						// Sets the minimum width the other object may resize to. The minimum width shall be set to no less than twice the thickness of the border
	public float minimumHeight = 0;						// Sets the minimum height the other object may resize to. The minimum height shall be set to no less than twice the thickness of the border
	float oldMouseX;									// Stores the cursor position on the x-axis for comparison with the newer "mouseX" variable
	float oldMouseY;									// Stores the cursor position on the y-axis for comparison with the newer "mouseY" variable
	float mouseX;										// Stores the current cursor position on the x-axis for comparison with the older "oldMouseX" variable
	float mouseY;										// Stores the current cursor position on the y-axis for comparison with the older "oldMouseY" variable
	float trackX;										// Used to track the distance the cursor moves over the x-axis during the entire time the mouse button is pressed. Interval method only
	float trackY;										// Used to track the distance the cursor moves over the y-axis during the entire time the mouse button is pressed. Interval method only
	int sector = 0;										// Stores the sector that the cursor is on when the mouse button is initially pressed
	
	
	void Update ()
	{
		if(disableResizeOtherObject == false)						
		{
			if (minimumWidth < borderThickness)									// This section sets the minimum width and height to twice the thickness of the border if they are less. This is so the other object cannot be resized into nothingness unless the border is deliberately set to 0
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
				
				
				InitialResizeOtherObject ();									// Calls the initial function to determine if the cursor is on the object when the mouse button is intially pressed
				
				
				if(sector != 0)													// This if statement runs if the cursor is on the object when the mouse button is initally pressed
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

			
			if (Input.GetMouseButtonUp(0))										// When the mouse button is released, the "startOnBorder" variable will be set to false, and the sector and tracking variables will be reset to zero
			{
				startOnBorder = false;
				sector = 0;
				trackX = 0;
				trackY = 0;
			}
		}
	}


	void PerformRaycast ()				// This function performs a raycast to detect if this object or its child is in front of other objects if any overlap at the cursor's position when the mouse button is initially pressed. If so, the other object is set as the last sibling in the hierarchy
	{
		PointerEventData cursor = new PointerEventData(EventSystem.current);
		cursor.position = Input.mousePosition;
		List<RaycastResult> objectsHit = new List<RaycastResult> ();
		EventSystem.current.RaycastAll(cursor, objectsHit);
		int x = 0;

		if(objectsHit[0].gameObject == this.gameObject || objectsHit[0].gameObject.transform.IsChildOf (transform))				// This section runs only if this object or its child is the front object where the cursor is
		{
			transform.SetAsLastSibling();													// Sets this object to be the last sibling in the hierarchy, making it the forward-most object
			Transform otherObjectTransform = otherObject.GetComponent<Transform> ();
			otherObjectTransform.SetAsLastSibling();										// Sets the other object to be the last sibling in the hierarchy, making it the forward-most object

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
					
					startOnBorder = true;							// Sets the "startOnBorder" variable to true since the cursor is determined to be on the object
					return;
				}
			}
		}
		
		
		if(objectsHit[0].gameObject == this.gameObject) 			// This section runs only if this object is the front object where the cursor is
		{
			mouseX = Input.mousePosition.x;							// Stores the current cursor position
			mouseY = Input.mousePosition.y;
			
			startOnBorder = true;									// Sets the "startOnBorder" variable to true since the cursor is determined to be on the object
		}
	}

	
	void InitialResizeOtherObject ()			// This function is used to determine if the cursor is over the object when the mouse button it initially pressed. It is also used to determine which direction(s) the object will be assigned to resize the other object
	{
		RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();		// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
		float width = objectRectTransform.rect.width;
		float height = objectRectTransform.rect.height;
		float rightOuterBorder = (width * .5f);
		float leftOuterBorder = (width * -.5f);
		float topOuterBorder = (height * .5f);
		float bottomOuterBorder = (height * -.5f);
		
		
		// The following line determines if the cursor is on the object when the mouse button is initially pressed
		if(Input.mousePosition.x <= (transform.position.x + rightOuterBorder) && Input.mousePosition.x >= (transform.position.x + leftOuterBorder) && Input.mousePosition.y <= (transform.position.y + topOuterBorder) && Input.mousePosition.y >= (transform.position.y + bottomOuterBorder))
		{
			if (TopRight == true)						// This section determines which direction(s) the object will be assigned to resize the other object
			{
				sector = 1;
			}
			else if (BottomRight == true)
			{
				sector = 2;
			}
			else if (Right == true)
			{
				sector = 3;
			}
			else if (TopLeft == true)
			{
				sector = 4;
			}
			else if (BottomLeft == true)
			{
				sector = 5;
			}
			else if (Left == true)
			{
				sector = 6;
			}
			else if (Top == true)
			{
				sector = 7;
			}
			else if (Bottom == true)
			{
				sector = 8;
			}
			else
			{
				sector = 0;
			}
		}
	}
	
	
	void ResizeOtherObject ()			// This function is used to determine the amount of movement of the cursor, if the minimum limits have been reached, and then it resizes the other object
	{
		RectTransform otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();			// This section gets the RectTransform information from the other object. Height and width are stored in variables. The borders of the object are also defined
		float width = otherObjectRectTransform.rect.width;
		float height = otherObjectRectTransform.rect.height;
		float rightOuterBorder = (width * .5f);
		float leftOuterBorder = (width * -.5f);
		float topOuterBorder = (height * .5f);
		float bottomOuterBorder = (height * -.5f);
		
		float movementX = mouseX - oldMouseX;												// Compares the current cursor position on the x-axis to the previous one, finding the amount of movement over the frame
		float movementY = mouseY - oldMouseY;												// Compares the current cursor position on the y-axis to the previous one, finding the amount of movement over the frame
		
		float offsetX = mouseX - transform.position.x - movementX;							// Used to determine the distance between the cursor position and the object position on the x-axis. This is so the object does not jump to center around the cursor when it is moved
		float offsetY = mouseY - transform.position.y - movementY;							// Used to determine the distance between the cursor position and the object position on the y-axis. This is so the object does not jump to center around the cursor when it is moved
		
		
		if (intervalBased == true)															// This section is only used if the interval method is selected
		{
			trackX = trackX + movementX;													// Tracks the amount of movement on the x-axis since the last interval
			
			
			if (trackX >= intervalDistance)													// Determines if the tracked POSITIVE movement on the x-axis is greater than the interval distance
			{
				int intervalsToMoveX = (Mathf.FloorToInt (trackX / intervalDistance));		// Determines how many intervals on the POSITIVE x-axis the other object should be resized this frame
				movementX = intervalsToMoveX * intervalDistance;							// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else if (- trackX >= intervalDistance)											// Determines if the tracked NEGATIVE movement on the x-axis is greater than the interval distance
			{
				int intervalsToMoveX = (Mathf.FloorToInt (-trackX / intervalDistance));		// Determines how many intervals on the NEGATIVE x-axis the other object should be resized this frame
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
				int intervalsToMoveY = (Mathf.FloorToInt (trackY / intervalDistance));		// Determines how many intervals on the POSITIVE y-axis the other object should be resized this frame
				movementY = intervalsToMoveY * intervalDistance;							// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else if (- trackY >= intervalDistance)											// Determines if the tracked NEGATIVE movement on the y-axis is greater than the interval distance
			{
				int intervalsToMoveY = (Mathf.FloorToInt (-trackY / intervalDistance));		// Determines how many intervals on the NEGATIVE y-axis the other object should be resized this frame
				movementY = - intervalsToMoveY * intervalDistance;							// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else
			{
				movementY = 0;																// If the tracked movement is not enough to move an interval, the movement of resize this frame is set to zero
			}				
			
			
			trackY = trackY % intervalDistance;												// Sets the tracking of movement on the y-axis to be the remainder of any extra movement if it is greater than the interval distance
		}
		
		
		if(movementX > 0 && movementX > width - minimumWidth && sector > 3)					// Determines if the POSITIVE movement from a left sector is greater than the distance to reach the minimum width
		{
			movementX = width - minimumWidth;												// Sets the amount of POSITIVE movement to the distance it takes to reach the minimum width
			
			
			if (intervalBased == true)
			{
				minimumWidthReached = true;													// Sets the "minimumWidthReached" variable to true if the interval method is used
			}
		}
		else if (movementX < 0 && - movementX > width - minimumWidth && sector <= 3)		// Determines if the NEGATIVE movement from a right sector is greater than the distance to reach the minimum width
		{
			movementX = - ( width - minimumWidth);											// Sets the amount of NEGATIVE movement to the distance it takes to reach the minimum width
			
			
			if (intervalBased == true)
			{
				minimumWidthReached = true;													// Sets the "minimumWidthReached" variable to true if the interval method is used
			}
		}
		
		
		if(movementY > 0 && movementY > height - minimumHeight && (sector == 2 || sector == 5 || sector == 8))				// Determines if the POSITIVE movement from a bottom sector is greater than the distance to reach the minimum height
		{
			movementY = height - minimumHeight;																				// Sets the amount of POSITIVE movement to the distance it takes to reach the minimum height
			
			
			if (intervalBased == true)
			{
				minimumHeightReached = true;																				// Sets the "minimumHeightReached" variable to true if the interval method is used
			}
		}
		else if (movementY < 0 && - movementY > height - minimumHeight && (sector == 1 || sector == 4 || sector == 7))		// Determines if the NEGATIVE movement from a top sector is greater than the distance to reach the minimum height
		{
			movementY = - (height - minimumHeight);																			// Sets the amount of NEGATIVE movement to the distance it takes to reach the minimum height
			
			
			if (intervalBased == true)
			{
				minimumHeightReached = true;																				// Sets the "minimumHeightReached" variable to true if the interval method is used
			}
		}
		
		
		// The following section is broken down into each sector. Each sector determines the direction(s) that the other object will be resized. Each section has two purposes.
		// First, if the user continues to drag the cursor beyond the minimum limitations, the cursor will leave the draggable object.
		// While the other object will not minimize any further, if the cursor reverses direction, the other object would immediately enlarge even though the cursor may be nowhere near it.
		// These checks assure that the other object will no longer begin to enlarge until the cursor returns near the object on the axis of movement.
		// Secondly, the other object is resized based on the direction(s) assigned to the object. As for the offsets, the corresponding sides are as follows...
		// offsetMin.x : Left
		// offsetMin.y : Bottom
		// offsetMax.x : Right
		// offsetMax.y : Top
		// The first sector's comments apply to the rest.
		
		
		if(sector == 1)					// TopRight
		{
			if(TopRight == true)
			{
				if((offsetX < leftOuterBorder && intervalBased == false) || (offsetX < leftOuterBorder && intervalBased == true && minimumWidthReached == true))			// Determines if the cursor is beyond the minimum width
				{
					movementX = 0;																																			// Sets movement amount on the x-axis to zero
					trackX = 0;																																				// Sets tracked movement on the x-axis to zero
				}
				else
				{
					minimumWidthReached = false;																															// Sets "minimumWidthReached" variable to false
				}
				
				
				if((offsetY < bottomOuterBorder && intervalBased == false) || (offsetY < bottomOuterBorder && intervalBased == true && minimumHeightReached == true))		// Determines if the cursor is beyond the minimum height
				{
					movementY = 0;																																			// Sets movement amount on the y-axis to zero
					trackY = 0;																																				// Sets tracked movement on the y-axis to zero
				}
				else
				{
					minimumHeightReached = false;																															// Sets "minimumHeightReached" variable to false
				}
				
				
				otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMax.x + movementX, otherObjectRectTransform.offsetMax.y + movementY);						// Resizes the other object based on the appropriate offsets
			}
		}
		else if(sector == 2)			// BottomRight
		{
			if(BottomRight == true)
			{
				if((offsetX < leftOuterBorder && intervalBased == false) || (offsetX < leftOuterBorder && intervalBased == true && minimumWidthReached == true))
				{
					movementX = 0;
					trackX = 0;
				}
				else
				{
					minimumWidthReached = false;
				}
				
				
				if((offsetY > topOuterBorder && intervalBased == false) || (offsetY > topOuterBorder && intervalBased == true && minimumHeightReached == true))
				{
					movementY = 0;
					trackY = 0;
				}
				else
				{
					minimumHeightReached = false;
				}
				
				
				otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMin.x, otherObjectRectTransform.offsetMin.y + movementY);
				otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMax.x + movementX, otherObjectRectTransform.offsetMax.y);
			}
		}
		else if(sector == 3)			// Right
		{
			if(Right == true)
			{
				if((offsetX < leftOuterBorder && intervalBased == false) || (offsetX < leftOuterBorder && intervalBased == true && minimumWidthReached == true))
				{
					movementX = 0;
					trackX = 0;
				}
				else
				{
					minimumWidthReached = false;
				}
				
				
				otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMax.x + movementX, otherObjectRectTransform.offsetMax.y);
			}
		}
		else if(sector == 4)			// TopLeft
		{
			if(TopLeft == true)
			{
				if((offsetX > rightOuterBorder && intervalBased == false) || (offsetX > rightOuterBorder && intervalBased == true && minimumWidthReached == true))
				{
					movementX = 0;
					trackX = 0;
				}
				else
				{
					minimumWidthReached = false;
				}
				
				
				if((offsetY < bottomOuterBorder && intervalBased == false) || (offsetY < bottomOuterBorder && intervalBased == true && minimumHeightReached == true))
				{
					movementY = 0;
					trackY = 0;
				}
				else
				{
					minimumHeightReached = false;
				}
				
				
				otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMin.x + movementX, otherObjectRectTransform.offsetMin.y);
				otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMax.x, otherObjectRectTransform.offsetMax.y + movementY);
			}
		}
		else if(sector == 5)			// BottomLeft
		{
			if(BottomLeft == true)
			{
				if((offsetX > rightOuterBorder && intervalBased == false) || (offsetX > rightOuterBorder && intervalBased == true && minimumWidthReached == true))
				{
					movementX = 0;
					trackX = 0;
				}
				else
				{
					minimumWidthReached = false;
				}
				
				
				if((offsetY > topOuterBorder && intervalBased == false) || (offsetY > topOuterBorder && intervalBased == true && minimumHeightReached == true))
				{
					movementY = 0;
					trackY = 0;
				}
				else
				{
					minimumHeightReached = false;
				}
				
				
				otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMin.x + movementX, otherObjectRectTransform.offsetMin.y + movementY);
			}
		}
		else if(sector == 6)			// Left
		{
			if(Left == true)
			{
				if((offsetX > rightOuterBorder && intervalBased == false) || (offsetX > rightOuterBorder && intervalBased == true && minimumWidthReached == true))
				{
					movementX = 0;
					trackX = 0;
				}
				else
				{
					minimumWidthReached = false;
				}
				
				
				otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMin.x + movementX, otherObjectRectTransform.offsetMin.y);
			}
		}
		else if(sector == 7)			// Top
		{
			if(Top == true)
			{
				if((offsetY < bottomOuterBorder && intervalBased == false) || (offsetY < bottomOuterBorder && intervalBased == true && minimumHeightReached == true))
				{
					movementY = 0;
					trackY = 0;
				}
				else
				{
					minimumHeightReached = false;
				}
				
				
				otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMax.x, otherObjectRectTransform.offsetMax.y + movementY);
			}
		}
		else if(sector == 8)			// Bottom
		{
			if(Bottom == true)
			{
				if((offsetY > topOuterBorder && intervalBased == false) || (offsetY > topOuterBorder && intervalBased == true && minimumHeightReached == true))
				{
					movementY = 0;
					trackY = 0;
				}
				else
				{
					minimumHeightReached = false;
				}
				
				
				otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMin.x, otherObjectRectTransform.offsetMin.y + movementY);
			}
		}
	}
	
	
	public void IntervalToggle ()					// This function toggles the interval method on and off
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
	
	
	public void ResizeOtherObjectToggle ()			// This function toggles the ability to resize the other object on and off. Good for events that toggle
	{
		if(disableResizeOtherObject == true)
		{
			disableResizeOtherObject = false;
		}
		else
		{
			disableResizeOtherObject = true;
		}	
	}
	
	
	public void DisableResizeOtherObject ()			// This function disables this ability to resize the other object. Good for events that do not toggle
	{
		if (Input.GetMouseButton(0) && startOnBorder == true)		// Assures that the resize ability will not be disabled while already in progress. This is important when using events such as Pointer Enter and Exit
		{
			return;
		}
		else if(disableResizeOtherObject == false)
		{
			disableResizeOtherObject = true;
		}
	}
	
	
	public void EnableResizeOtherObject ()			// This function enables the ability to resize the other object. Good for events that do not toggle
	{
		if (disableResizeOtherObject == true)
		{
			disableResizeOtherObject = false;
		}
	}
	
	
	// The remaining functions serve to disable or enable the specific directional controls of this object. It is very important to set ONLY one to be true
	
	
	public void DisableTopRight ()								
	{
		if (TopRight == true)
		{
			TopRight = false;
		}
		else
		{
			TopRight = true;
		}	
	}
	
	
	public void DisableBottomRight ()
	{
		if (BottomRight == true)
		{
			BottomRight = false;
		}
		else
		{
			BottomRight = true;
		}	
	}
	
	
	public void DisableRight ()
	{
		if (Right == true)
		{
			Right = false;
		}
		else
		{
			Right = true;
		}	
	}
	
	
	public void DisableTopLeft ()
	{
		if (TopLeft == true)
		{
			TopLeft = false;
		}
		else
		{
			TopLeft = true;
		}	
	}
	
	
	public void DisableBottomLeft ()
	{
		if (BottomLeft == true)
		{
			BottomLeft = false;
		}
		else
		{
			BottomLeft = true;
		}
	}
	
	
	public void DisableLeft ()
	{
		if (Left == true)
		{
			Left = false;
		}
		else
		{
			Left = true;
		}	
	}
	
	
	public void DisableTop ()
	{
		if (Top == true)
		{
			Top = false;
		}
		else
		{
			Top = true;
		}	
	}
	
	
	public void DisableBottom ()
	{
		if (Bottom == true)
		{
			Bottom = false;
		}
		else
		{
			Bottom = true;
		}	
	}
}
