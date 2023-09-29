using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ResizeOther : MonoBehaviour {

	public GameObject otherObject;						// Defines the object to be resized
	public float movementMultiplier = 1;				// Defines the amount of multiplication for movement each frame
	public bool includeChildren = true;					// Determines if this object's children will be included in the raycast return
	public bool customCursor = true;					// Determines if custom cursors are used
	public bool bringToFrontOnOver = false;				// Determines if the other object will be set as the last sibling in the hierarchy when the cursor is over this object
	public bool bringToFront = true;					// Determines if the other object will be set as the last sibling in the hierarchy when the cursor is over this object and the mouse button is pressed
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
	public string cursor = "normal";					// Defines the cursor that should be used depending on the cursor location. This is a reference for the UIController script. Do not change
	bool startOnBorder = false;							// Determines if the cursor is on this object when the mouse button is initially pressed
	bool minimumWidthReached = false;					// Determines if the other object has reached the minimum width allowed
	bool minimumHeightReached = false;					// Determines if the other object has reached the minimum height allowed
	bool moveRight = false;								// Determines if the parent will resize from its right side
	bool moveLeft = false;								// Determines if the parent will resize from its left side
	bool moveTop = false;								// Determines if the parent will resize from its top
	bool moveBottom = false;							// Determines if the parent will resize from its bottom
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
			Debug.Log ("You have not assigned an object to resize to: " + transform.name);			// This only happens if there is no object assigned to "otherObject"
		}
		else
		{
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


	public void Passive ()								// This function is called by the UIControl script when a raycast hits it and the mouse button is not pressed
	{
		if (bringToFrontOnOver == true)
		{
			transform.SetAsLastSibling();															// Sets this object to be the last sibling in the hierarchy, making it the forward-most object
			Transform otherObjectTransform = otherObject.GetComponent<Transform> ();
			otherObjectTransform.SetAsLastSibling();												// Sets the other object to be the last sibling in the hierarchy, making it the forward-most object
		}
		
		
		if (customCursor == true && disableResizeOther == false && otherObject != null)
		{
			if (Left == true || Right == true)
			{
				cursor = "horizontal";															// Sets the cursor to use the "horizontal" custom cursor if assigned
			}
			else if (Top == true || Bottom == true)
			{
				cursor = "vertical";															// Sets the cursor to use the "vertical" custom cursor if assigned
			}
			else if (TopLeft == true || BottomRight == true)
			{
				cursor = "topLeftBottomRight";													// Sets the cursor to use the "topLeftBottomRight" custom cursor if assigned
			}
			else if (TopRight == true || BottomLeft == true)
			{
				cursor = "topRightBottomLeft";													// Sets the cursor to use the "topRightBottomLeftl" custom cursor if assigned
			}
			else
			{
				cursor = "normal";																// Sets the cursor to use the "normal" custom cursor if assigned
			}
		}
		else
		{
			cursor = "normal";																	// Sets the cursor to use the "normal" custom cursor if assigned
		}
	}


	public void Active ()								// This function is called by the UIControl script when a raycast hits it and the mouse button is initially pressed
	{
		if (bringToFront == true && otherObject != null)
		{
			transform.SetAsLastSibling();															// Sets this object to be the last sibling in the hierarchy, making it the forward-most object
			Transform otherObjectTransform = otherObject.GetComponent<Transform> ();
			otherObjectTransform.SetAsLastSibling();												// Sets the other object to be the last sibling in the hierarchy, making it the forward-most object
		}


		if (disableResizeOther == false && otherObject != null)
		{
			RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();			// This section gets the RectTransform information from this object. Height and width are stored in variables
			float width = objectRectTransform.rect.width;
			float height = objectRectTransform.rect.height;
			
			if (minimumWidth < width * 2)															// This section sets the minimum width to twice the width of this object if it is less. This is so an object cannot be resized into nothingness
			{
				minimumWidth = width * 2;
			}
			
			
			if (minimumHeight < height * 2)															// This section sets the minimum height to twice the height of this object if it is less. This is so an object cannot be resized into nothingness
			{
				minimumHeight = height * 2;
			}
			
			
			startOnBorder = true;
			
			mouseX = Input.mousePosition.x;															// Stores the current cursor position
			mouseY = Input.mousePosition.y;
			
			oldMouseX = mouseX;																		// Stores the current cursor position that will become the previous position in the next frame
			oldMouseY = mouseY;
		}
	}


	void Update ()
	{
		if (startOnBorder == true && (oldMouseX != Input.mousePosition.x || oldMouseY != Input.mousePosition.y))			// Determines if the mouse button is currently pressed, if it was initially pressed on a valid moveable position, and if the cursor position has moved since the last frame
		{
			mouseX = Input.mousePosition.x;															// Stores the current cursor position
			mouseY = Input.mousePosition.y;
			
			ResizeOtherObject();																	// Calls the function to resize the parent
			
			oldMouseX = mouseX;																		// Stores the current cursor position that will become the previous position in the next frame
			oldMouseY = mouseY;
		}
	}


	public void End ()									// This function is called by the UIControl script when the mouse button is initially released. Tracking and movement variables are reset to zero
	{
		startOnBorder = false;
		trackX = 0;
		trackY = 0;
	}


	void ResizeOtherObject ()							// This function is used to determine the amount of movement of the cursor, if the minimum limits have been reached, and then it resizes the other object
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

		float movementX = (mouseX - oldMouseX) * movementMultiplier;								// Compares the current cursor position on the x-axis to the previous one, finding the amount of movement over the frame
		float movementY = (mouseY - oldMouseY) * movementMultiplier;								// Compares the current cursor position on the y-axis to the previous one, finding the amount of movement over the frame
			
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
		
		if (minimumWidthReached == true)															// This section determines if the cursor is inside of this object's inner borders if the minimum width has been reached. If so, movement and tracking are set to 0 on the x-axis
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
		
		
		if (minimumHeightReached == true)															// This section determines if the cursor is inside of this object's inner borders if the minimum height has been reached. If so, movement and tracking are set to 0 on the y-axis
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
	
	
	public void IntervalToggle ()						// This function toggles the interval method on and off
	{
		intervalBased = !intervalBased;
	}
	
	
	public void CustomCursorToggle ()					// This function toggles the use of custom cursors on and off
	{
		customCursor = !customCursor;
	}
	
	
	public void RestrainToggle ()						// This function toggles the ability to restrain the other object to its parent on and off
	{
		restrainToOtherParent = !restrainToOtherParent;
	}
	
	
	public void ResizeOtherToggle ()					// This function toggles the ability to resize the other object on and off. Good for events that toggle
	{
		disableResizeOther = !disableResizeOther;
	}
	

	public void EnableResizeOther ()					// This function enables the ability to resize the other object. Good for events that do not toggle
	{
		disableResizeOther = false;
	}


	public void DisableResizeOther ()					// This function disables this ability to resize the other object. Good for events that do not toggle
	{
		disableResizeOther = true;
	}
}