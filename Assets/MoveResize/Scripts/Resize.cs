using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Resize : MonoBehaviour {

	public float movementMultiplier = 1;				// Defines the amount of multiplication for movement each frame
	public bool includeChildren = true;					// Determines if this object's children will be included in the raycast return
	public bool customCursor = true;					// Determines if custom cursors are used
	public bool bringToFrontOnOver = false;				// Determines if this object will be set as the last sibling in the hierarchy when the cursor is over this object
	public bool bringToFront = true;					// Determines if this object will be set as the last sibling in the hierarchy when the cursor is over this object and the mouse button is pressed
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
	public string cursor = "normal";					// Defines the cursor that should be used depending on the cursor location. This is a reference for the UIController script. Do not change
	bool TopRight = false;								// Determines if the TopRight section of this object's border is selected
	bool BottomRight = false;							// Determines if the BottomRight section of this object's border is selected
	bool Right = false;									// Determines if the Right section of this object's border is selected
	bool TopLeft = false;								// Determines if the TopLeft section of this object's border is selected
	bool BottomLeft = false;							// Determines if the BottomLeft section of this object's border is selected
	bool Left = false;									// Determines if the Left section of this object's border is selected
	bool Top = false;									// Determines if the Top section of this object's border is selected
	bool Bottom = false;								// Determines if the Bottom section of this object's border is selected
	bool startOnBorder = false;							// Determines if the cursor is on a border when the mouse button is initially pressed
	bool minimumWidthReached = false;					// Determines if this object has reached the minimum width allowed
	bool minimumHeightReached = false;					// Determines if this object has reached the minimum height allowed
	bool moveRight = false;								// Determines if this object will resize from its right side
	bool moveLeft = false;								// Determines if this object will resize from its left side
	bool moveTop = false;								// Determines if this object will resize from its top
	bool moveBottom = false;							// Determines if this object will resize from its bottom
	float oldMouseX;									// Stores the cursor position on the x-axis for comparison with the newer "mouseX" variable
	float oldMouseY;									// Stores the cursor position on the y-axis for comparison with the newer "mouseY" variable
	float mouseX;										// Stores the current cursor position on the x-axis for comparison with the older "oldMouseX" variable
	float mouseY;										// Stores the current cursor position on the y-axis for comparison with the older "oldMouseY" variable
	float trackX;										// Used to track the distance the cursor moves over the x-axis during the entire time the mouse button is pressed. Interval method only
	float trackY;										// Used to track the distance the cursor moves over the y-axis during the entire time the mouse button is pressed. Interval method only

	public void Passive ()								// This function is called by the UIControl script when a raycast hits it and the mouse button is not pressed
	{
		if (bringToFrontOnOver == true)
		{
			transform.SetAsLastSibling();				// Sets this object to be the last sibling in the hierarchy, making it the forward-most object
		}
		
		
		if (customCursor == true && disableResize == false)
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

			// The following line determines if the cursor is over a valid movable position
			if(Input.mousePosition.x > transform.position.x + rightInnerBorder && Input.mousePosition.x < transform.position.x + rightOuterBorder && Input.mousePosition.y < transform.position.y + topOuterBorder && Input.mousePosition.y > transform.position.y + bottomOuterBorder)
			{
				if(Input.mousePosition.y > transform.position.y + topInnerBorder)
				{
					if (disableTopRight == false)												// Top Right
					{		
						cursor = "topRightBottomLeft";											// Sets the cursor to use the "topRightBottomLeftl" custom cursor if assigned
					}
					else
					{
						cursor = "normal";														// Sets the cursor to use the "normal" custom cursor if assigned
					}
				}
				else if(Input.mousePosition.y < transform.position.y + bottomInnerBorder)
				{
					if (disableBottomRight == false)											// Bottom Right
					{
						cursor = "topLeftBottomRight";											// Sets the cursor to use the "topLeftBottomRight" custom cursor if assigned
					}
					else
					{
						cursor = "normal";														// Sets the cursor to use the "normal" custom cursor if assigned
					}
				}
				else
				{
					if (disableRight == false)													// Right
					{
						cursor = "horizontal";													// Sets the cursor to use the "horizontal" custom cursor if assigned
					}
					else
					{
						cursor = "normal";														// Sets the cursor to use the "normal" custom cursor if assigned
					}
				}
			}
			else if(Input.mousePosition.x < transform.position.x + leftInnerBorder && Input.mousePosition.x > transform.position.x + leftOuterBorder && Input.mousePosition.y < transform.position.y + topOuterBorder && Input.mousePosition.y > transform.position.y + bottomOuterBorder)
			{
				if(Input.mousePosition.y > transform.position.y + topInnerBorder)
				{
					if (disableTopLeft == false)												// Top Left
					{
						cursor = "topLeftBottomRight";											// Sets the cursor to use the "topLeftBottomRight" custom cursor if assigne
					}
					else
					{
						cursor = "normal";														// Sets the cursor to use the "normal" custom cursor if assigned
					}
				}
				else if(Input.mousePosition.y < transform.position.y + bottomInnerBorder)
				{
					if (disableBottomLeft == false)												// Bottom Left
					{
						cursor = "topRightBottomLeft";											// Sets the cursor to use the "topRightBottomLeftl" custom cursor if assigned
					}
					else
					{
						cursor = "normal";														// Sets the cursor to use the "normal" custom cursor if assigned
					}
				}
				else
				{
					if (disableLeft == false)													// Left
					{
						cursor = "horizontal";													// Sets the cursor to use the "horizontal" custom cursor if assigned
					}
					else
					{
						cursor = "normal";														// Sets the cursor to use the "normal" custom cursor if assigned
					}
				}
			}
			else if(Input.mousePosition.y > transform.position.y + topInnerBorder && Input.mousePosition.y < transform.position.y + topOuterBorder && Input.mousePosition.x < transform.position.x + rightOuterBorder && Input.mousePosition.x > transform.position.x + leftOuterBorder)
			{
				if (disableTop == false)														// Top
				{
					cursor = "vertical";														// Sets the cursor to use the "vertical" custom cursor if assigned
				}
				else
				{
					cursor = "normal";															// Sets the cursor to use the "normal" custom cursor if assigned
				}
			}
			else if(Input.mousePosition.y < transform.position.y + bottomInnerBorder && Input.mousePosition.y > transform.position.y + bottomOuterBorder && Input.mousePosition.x < transform.position.x + rightOuterBorder && Input.mousePosition.x > transform.position.x + leftOuterBorder)
			{
				if (disableBottom == false)														// Bottom
				{
					cursor = "vertical";														// Sets the cursor to use the "vertical" custom cursor if assigned
				}
				else
				{
					cursor = "normal";															// Sets the cursor to use the "normal" custom cursor if assigned
				}
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
		if (bringToFront == true)
		{
			transform.SetAsLastSibling();				// Sets this object to be the last sibling in the hierarchy, making it the forward-most object
		}

		if (disableResize == false)
		{
			if (minimumWidth < leftBorderThickness + rightBorderThickness)						// This section sets the minimum width to the thickness of the right and left borders if it is less. This is so an object cannot be resized into nothingness unless the borders are deliberately set to 0
			{
				minimumWidth = leftBorderThickness + rightBorderThickness;
			}
			
			
			if (minimumHeight < topBorderThickness + bottomBorderThickness)						// This section sets the minimum height to the thickness of the top and bottom borders if it is less. This is so an object cannot be resized into nothingness unless the borders are deliberately set to 0
			{
				minimumHeight = topBorderThickness + bottomBorderThickness;
			}
			

			oldMouseX = Input.mousePosition.x;													// Stores the initial cursor position
			oldMouseY = Input.mousePosition.y;
				
			InitialResizeObject ();																// Calls the function to determine if the cursor is on a border or on the object when the mouse button is intially pressed
		}
	}


	void InitialResizeObject ()							// This function is used to determine if the cursor is over the object when the mouse button it initially pressed. It is also used to determine which border section will be used to resize the object
	{
		RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();			// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
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
				TopRight = true;																// Top Right
				moveRight = true;		
				moveTop = true;
				startOnBorder  = true;
			}
			else if(oldMouseY < transform.position.y + bottomInnerBorder)
			{
				BottomRight = true;																// Bottom Right
				moveRight = true;		
				moveBottom = true;
				startOnBorder  = true;
			}
			else
			{
				Right = true;																	// Right
				moveRight = true;		
				startOnBorder  = true;
			}
		}
		else if(oldMouseX < transform.position.x + leftInnerBorder && oldMouseX > transform.position.x + leftOuterBorder && oldMouseY < transform.position.y + topOuterBorder && oldMouseY > transform.position.y + bottomOuterBorder)
		{
			if(oldMouseY > transform.position.y + topInnerBorder)
			{
				TopLeft = true;																	// Top Left
				moveLeft = true;		
				moveTop = true;
				startOnBorder  = true;
			}
			else if(oldMouseY < transform.position.y + bottomInnerBorder)
			{
				BottomLeft = true;																// Bottom Left
				moveLeft = true;		
				moveBottom = true;
				startOnBorder  = true;
			}
			else
			{
				Left = true;																	// Left
				moveLeft = true;		
				startOnBorder  = true;
			}
		}
		else if(oldMouseY > transform.position.y + topInnerBorder && oldMouseY < transform.position.y + topOuterBorder && oldMouseX < transform.position.x + rightOuterBorder && oldMouseX > transform.position.x + leftOuterBorder)
		{
			Top = true;																			// Top
			moveTop = true;				
			startOnBorder  = true;
		}
		else if(oldMouseY < transform.position.y + bottomInnerBorder && oldMouseY > transform.position.y + bottomOuterBorder && oldMouseX < transform.position.x + rightOuterBorder && oldMouseX > transform.position.x + leftOuterBorder)
		{
			Bottom = true;																		// Bottom
			moveBottom = true;			
			startOnBorder  = true;
		}
		else
		{
			startOnBorder  = false;																// Not on a border
		}
	}
	
	
	void Update ()
	{
		if (startOnBorder == true && (oldMouseX != Input.mousePosition.x || oldMouseY != Input.mousePosition.y))			// Determines if the mouse button is currently pressed, if it was initially pressed on a valid moveable position, and if the cursor position has moved since the last frame
		{
			mouseX = Input.mousePosition.x;														// Stores the current cursor position
			mouseY = Input.mousePosition.y;
			
			ResizeObject();																		// Calls the function to resize the object
			
			oldMouseX = mouseX;																	// Stores the current cursor position that will become the previous position in the next frame
			oldMouseY = mouseY;
		}
	}
	
	
	public void End ()									// This function is called by the UIControl script when the mouse button is initially released. Tracking and movement variables are reset to zero
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
		startOnBorder = false;
		trackX = 0;
		trackY = 0;
	}


	void ResizeObject ()								// This function is used to determine if the cursor is over a border when the mouse button it initially pressed and which border it is. It is also used to determine the amount of movement of the cursor, if the minimum limits have been reached, and then it resizes the object
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

		float movementX = (mouseX - oldMouseX) * movementMultiplier;										// Compares the current cursor position on the x-axis to the previous one, finding the amount of movement over the frame
		float movementY = (mouseY - oldMouseY) * movementMultiplier;										// Compares the current cursor position on the y-axis to the previous one, finding the amount of movement over the frame
	
		if (intervalBased == true)																			// This section is only used if the interval method is selected
		{
			trackX = trackX + movementX;																	// Tracks the amount of movement on the x-axis since the last interval

			if (trackX >= intervalDistance)																	// Determines if the tracked POSITIVE movement on the x-axis is greater than the interval distance
			{
				int intervalsToMoveX = (Mathf.FloorToInt (trackX / intervalDistance));						// Determines how many intervals on the POSITIVE x-axis the object should be resized this frame
				movementX = intervalsToMoveX * intervalDistance;											// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else if (- trackX >= intervalDistance)															// Determines if the tracked NEGATIVE movement on the x-axis is greater than the interval distance
			{
				int intervalsToMoveX = (Mathf.FloorToInt (-trackX / intervalDistance));						// Determines how many intervals on the NEGATIVE x-axis the object should be resized this frame
				movementX = - intervalsToMoveX * intervalDistance;											// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else
			{
				movementX = 0;																				// If the tracked movement is not enough to move an interval, the movement of resize this frame is set to zero
			}			
			
			
			trackX = trackX % intervalDistance;																// Sets the tracking of movement on the x-axis to be the remainder of any extra movement if it is greater than the interval distance	
			trackY = trackY + movementY;																	// Tracks the amount of movement on the y-axis since the last interval

			if (trackY >= intervalDistance)																	// Determines if the tracked POSITIVE movement on the y-axis is greater than the interval distance
			{
				int intervalsToMoveY = (Mathf.FloorToInt (trackY / intervalDistance));						// Determines how many intervals on the POSITIVE y-axis the object should be resized this frame
				movementY = intervalsToMoveY * intervalDistance;											// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else if (- trackY >= intervalDistance)															// Determines if the tracked NEGATIVE movement on the y-axis is greater than the interval distance
			{
				int intervalsToMoveY = (Mathf.FloorToInt (-trackY / intervalDistance));						// Determines how many intervals on the NEGATIVE y-axis the object should be resized this frame
				movementY = - intervalsToMoveY * intervalDistance;											// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else
			{
				movementY = 0;																				// If the tracked movement is not enough to move an interval, the movement of resize this frame is set to zero
			}				
			
			
			trackY = trackY % intervalDistance;																// Sets the tracking of movement on the y-axis to be the remainder of any extra movement if it is greater than the interval distance
		}

		
		if(movementX > 0 && movementX > width - minimumWidth && moveLeft == true)							// Determines if the POSITIVE movement from a left sector is greater than the distance to reach the minimum width
		{
			movementX = width - minimumWidth;																// Sets the amount of POSITIVE movement to the distance it takes to reach the minimum width
			minimumWidthReached = true;	
		}
		else if (movementX < 0 && - movementX > width - minimumWidth && moveRight == true)					// Determines if the NEGATIVE movement from a right sector is greater than the distance to reach the minimum width
		{
			movementX = - ( width - minimumWidth);															// Sets the amount of NEGATIVE movement to the distance it takes to reach the minimum width
			minimumWidthReached = true;	
		}


		if (minimumWidthReached == true)																	// This section determines if the cursor is inside of this object's inner borders if the minimum width has been reached. If so, movement and tracking are set to 0 on the x-axis
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
		
		
		if(movementY > 0 && movementY > height - minimumHeight && moveBottom == true)						// Determines if the POSITIVE movement from a bottom sector is greater than the distance to reach the minimum height
		{
			movementY = height - minimumHeight;																// Sets the amount of POSITIVE movement to the distance it takes to reach the minimum height
			minimumHeightReached = true;
		}
		else if (movementY < 0 && - movementY > height - minimumHeight && moveTop == true)					// Determines if the NEGATIVE movement from a top sector is greater than the distance to reach the minimum height
		{
			movementY = - (height - minimumHeight);															// Sets the amount of NEGATIVE movement to the distance it takes to reach the minimum height
			minimumHeightReached = true;
		}


		if (minimumHeightReached == true)																	// This section determines if the cursor is inside of this object's inner borders if the minimum height has been reached. If so, movement and tracking are set to 0 on the y-axis
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


	public void IntervalToggle ()						// This function toggles the interval method on and off
	{
		intervalBased = !intervalBased;
	}


	public void CustomCursorToggle ()					// This function toggles the use of custom cursors on and off
	{
		customCursor = !customCursor;
	}


	public void RestrainToggle ()						// This function toggles the ability to restrain this object to its parent on and off
	{
		restrainToParent = !restrainToParent;
	}


	public void ResizeToggle ()							// This function toggles the ability to resize the object on and off. Good for events that toggle
	{
		disableResize = !disableResize;
	}


	public void EnableResize ()							// This function enables the ability to resize the object. Good for events that do not toggle
	{
		disableResize = false;
	}


	public void DisableResize ()						// This function disables this ability to resize the object. Good for events that do not toggle
	{
		disableResize = true;
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