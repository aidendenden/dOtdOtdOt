﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MoveParent : MonoBehaviour {

	public float movementMultiplier = 1;				// Defines the amount of multiplication for movement each frame
	public bool includeChildren = true;					// Determines if this object's children will be included in the raycast return
	public bool customCursor = true;					// Determines if custom cursors are used
	public bool bringToFrontOnOver = false;				// Determines if the parent will be set as the last sibling in the hierarchy when the cursor is over this object
	public bool bringToFront = true;					// Determines if the parent will be set as the last sibling in the hierarchy when the cursor is over this object and the mouse button is pressed
	public bool disableMoveParent = false;				// Determines if the ability to move the parent is enabled or disabled
	public bool xOnly = false;							// Determines if the parent will move only on the x-axis
	public bool yOnly = false;							// Determines if the parent will move only on the y-axis
	public bool restrainToGrandparent = false;			// Determines if the parent will be restrained fully to its parent
	public float restrainRightInset = 0;				// Sets the distance from the right edge of its parent that the parent will be restrained to
	public float restrainLeftInset = 0;					// Sets the distance from the left edge of its parent that the parent will be restrained to
	public float restrainTopInset = 0;					// Sets the distance from the top edge of its parent that the parent will be restrained to
	public float restrainBottomInset = 0;				// Sets the distance from the bottom edge of its parent that the parent will be restrained to
	public bool intervalBased = false;					// Determines if the parent will move based on intervals
	public float intervalDistance = 50;					// Sets the size of intervals if the interval method is true
	public float rightInset = 0;						// Sets the distance from the right edge of this object that the cursor must be in order to be considered on a valid moveable position
	public float leftInset = 0;							// Sets the distance from the left edge of this object that the cursor must be in order to be considered on a valid moveable position
	public float topInset = 0;							// Sets the distance from the top edge of this object that the cursor must be in order to be considered on a valid moveable position
	public float bottomInset = 0;						// Sets the distance from the bottom edge of this object that the cursor must be in order to be considered on a valid moveable position
	public string cursor = "normal";					// Defines the cursor that should be used depending on the cursor location. This is a reference for the UIController script. Do not change
	bool worldSpace = false;							// Determines if the canvas' render mode is set to anything other than Screen Space - Overlay
	bool startOnObject = false;							// Determines if the cursor is on this object when the mouse button is initially pressed
	bool makeMoveHorizontal = true;						// Determines if the parent may move on the x-axis
	bool makeMoveVertical = true;						// Determines if the parent may move on the y-axis
	float oldMouseX;									// Stores the cursor position on the x-axis for comparison with the newer "mouseX" variable
	float oldMouseY;									// Stores the cursor position on the y-axis for comparison with the newer "mouseY" variable
	float mouseX;										// Stores the current cursor position on the x-axis for comparison with the older "oldMouseX" variable
	float mouseY;										// Stores the current cursor position on the y-axis for comparison with the older "oldMouseY" variable
	float trackX;										// Used to track the distance the cursor moves over the x-axis during the entire time the mouse button is pressed. Interval method only
	float trackY;										// Used to track the distance the cursor moves over the y-axis during the entire time the mouse button is pressed. Interval method only

	void Start ()
	{
		GameObject parent = this.gameObject.transform.parent.gameObject;												// Stores the parent of this object
		
		while (parent.gameObject.GetComponent<Canvas>() == false)														// Loops until the canvas is foound
		{
			parent = parent.transform.parent.gameObject;																// Stores the parent of the parent
		}
		
		
		if (parent.GetComponent<Canvas>().renderMode != RenderMode.ScreenSpaceOverlay)									// This section determines the render mode of the canvas
		{
			worldSpace = true;
		}
		else
		{
			worldSpace = false;
		}
	}
	
	
	public void Passive ()								// This function is called by the UIControl script when a raycast hits it and the mouse button is not pressed
	{
		if (bringToFrontOnOver == true)
		{
			transform.parent.SetAsLastSibling();		// Sets the parent to be the last sibling in the hierarchy, making it the forward-most object
		}
		
		
		if (customCursor == true && disableMoveParent == false)
		{
			if (worldSpace == false)
			{
				RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();							// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
				float width = objectRectTransform.rect.width;
				float height = objectRectTransform.rect.height;
				float rightOuterBorder = (width * .5f);
				float leftOuterBorder = (width * -.5f);
				float topOuterBorder = (height * .5f);
				float bottomOuterBorder = (height * -.5f);
				
				// The following line determines if the cursor is over a valid movable position
				if(Input.mousePosition.x <= (transform.position.x + rightOuterBorder - rightInset) && Input.mousePosition.x >= (transform.position.x + leftOuterBorder + leftInset) && Input.mousePosition.y <= (transform.position.y + topOuterBorder - topInset) && Input.mousePosition.y >= (transform.position.y + bottomOuterBorder + bottomInset))
				{
					if (xOnly == false && yOnly == false)	// The default movement
					{
						cursor = "move";					// Sets the cursor to use the "move" custom cursor if assigned
					}
					else if (xOnly == true)					// The "xOnly" variable is true
					{
						cursor = "horizontal";				// Sets the cursor to use the "moveHorizontal" custom cursor if assigned
					}
					else if (yOnly == true)					// The "yOnly" variable is true
					{
						cursor = "vertical";				// Sets the cursor to use the "moveVertical" custom cursor if assigned
					}
					else
					{
						cursor = "normal";					// Sets the cursor to use the "normal" custom cursor if assigned
					}
				}
				else 										// This section runs if the cursor is not over a valid movable position. It sets the cursor to use the "normal" custom cursor if assigned. If not, it sets it to the null cursor 
				{
					cursor = "normal";						// Sets the cursor to use the "normal" custom cursor if assigned
				}
			}
			else
			{
				if (xOnly == false && yOnly == false)	// The default movement
				{
					cursor = "move";					// Sets the cursor to use the "move" custom cursor if assigned
				}
				else if (xOnly == true)					// The "xOnly" variable is true
				{
					cursor = "horizontal";				// Sets the cursor to use the "moveHorizontal" custom cursor if assigned
				}
				else if (yOnly == true)					// The "yOnly" variable is true
				{
					cursor = "vertical";				// Sets the cursor to use the "moveVertical" custom cursor if assigned
				}
				else
				{
					cursor = "normal";					// Sets the cursor to use the "normal" custom cursor if assigned
				}
			}
		}
		else 										
		{
			cursor = "normal";							// Sets the cursor to use the "normal" custom cursor if assigned
		}
	}


	public void Active ()								// This function is called by the UIControl script when a raycast hits it and the mouse button is initially pressed
	{
		if (bringToFront == true)
		{
			transform.parent.SetAsLastSibling();		// Sets the parent to be the last sibling in the hierarchy, making it the forward-most object
		}


		if (disableMoveParent == false)
		{
			if (worldSpace == false)
			{
				RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();							// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
				float width = objectRectTransform.rect.width;
				float height = objectRectTransform.rect.height;
				float rightOuterBorder = (width * .5f);
				float leftOuterBorder = (width * -.5f);
				float topOuterBorder = (height * .5f);
				float bottomOuterBorder = (height * -.5f);

				// The following line determines if the cursor is on a valid movable position based on the object and the insets
				if(Input.mousePosition.x <= (transform.position.x + rightOuterBorder - rightInset) && Input.mousePosition.x >= (transform.position.x + leftOuterBorder + leftInset) && Input.mousePosition.y <= (transform.position.y + topOuterBorder - topInset) && Input.mousePosition.y >= (transform.position.y + bottomOuterBorder + bottomInset))
				{
					startOnObject = true;
					
					mouseX = Input.mousePosition.x;		// Stores the current cursor position
					mouseY = Input.mousePosition.y;
					
					oldMouseX = mouseX;					// Stores the current cursor position that will become the previous position in the next frame
					oldMouseY = mouseY;
				}
			}
			else
			{
				startOnObject = true;
				
				mouseX = Input.mousePosition.x;			// Stores the current cursor position
				mouseY = Input.mousePosition.y;
				
				oldMouseX = mouseX;						// Stores the current cursor position that will become the previous position in the next frame
				oldMouseY = mouseY;
			}
		}
	}


	void Update ()
	{
		if (startOnObject == true && (oldMouseX != Input.mousePosition.x || oldMouseY != Input.mousePosition.y))		// Determines if the mouse button is currently pressed, if it was initially pressed on a valid moveable position, and if the cursor position has moved since the last frame
		{
			mouseX = Input.mousePosition.x;				// Stores the current cursor position
			mouseY = Input.mousePosition.y;
			
			MoveParentObject ();						// Calls the function to move the object
			
			oldMouseX = mouseX;							// Stores the current cursor position that will become the previous position in the next frame
			oldMouseY = mouseY;
		}
	}


	public void End ()									// This function is called by the UIControl script when the mouse button is initially released. Tracking and movement variables are reset to zero
	{
		startOnObject = false;
		trackX = 0;
		trackY = 0;
	}

	
	void MoveParentObject()								// This function is used to determine the amount of movement of the cursor, and then it moves the parent
	{
		RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();									// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
		float width = objectRectTransform.rect.width;
		float height = objectRectTransform.rect.height;
		float rightOuterBorder = (width * .5f);
		float leftOuterBorder = (width * -.5f);
		float topOuterBorder = (height * .5f);
		float bottomOuterBorder = (height * -.5f);

		RectTransform parentRectTransform = transform.parent.gameObject.GetComponent<RectTransform> ();					// This section gets the RectTransform information from the parent. Height and width are stored in variables. The borders of the object are also defined
		float parentWidth = parentRectTransform.rect.width;
		float parentHeight = parentRectTransform.rect.height;
		float parentRightOuterBorder = (parentWidth * .5f);
		float parentLeftOuterBorder = (parentWidth * -.5f);
		float parentTopOuterBorder = (parentHeight * .5f);
		float parentBottomOuterBorder = (parentHeight * -.5f);
		
		RectTransform grandparentRectTransform = transform.parent.parent.gameObject.GetComponent<RectTransform> ();		// This section gets the RectTransform information from the grandparent. Height and width are stored in variables. The borders of the object are also defined
		float grandparentWidth = grandparentRectTransform.rect.width;
		float grandparentHeight = grandparentRectTransform.rect.height;
		float grandparentRightOuterBorder = (grandparentWidth * .5f) - restrainRightInset;
		float grandparentLeftOuterBorder = (grandparentWidth * -.5f) + restrainLeftInset;
		float grandparentTopOuterBorder = (grandparentHeight * .5f) - restrainTopInset;
		float grandparentBottomOuterBorder = (grandparentHeight * -.5f) + restrainBottomInset;

		float movementX = (mouseX - oldMouseX) * movementMultiplier;						// Compares the current cursor position on the x-axis to the previous one, finding the amount of movement over the frame
		float movementY = (mouseY - oldMouseY) * movementMultiplier;						// Compares the current cursor position on the y-axis to the previous one, finding the amount of movement over the frame

		if (intervalBased == true)															// This section is only used if the interval method is selected
		{
			trackX = trackX + movementX;													// Tracks the amount of movement on the x-axis since the last interval

			if (trackX >= intervalDistance)													// Determines if the tracked POSITIVE movement on the x-axis is greater than the interval distance
			{
				int intervalsToMoveX = (Mathf.FloorToInt (trackX / intervalDistance));		// Determines how many intervals on the POSITIVE x-axis the parent should be moved this frame
				movementX = intervalsToMoveX * intervalDistance;							// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else if (- trackX >= intervalDistance)											// Determines if the tracked NEGATIVE movement on the x-axis is greater than the interval distance
			{
				int intervalsToMoveX = (Mathf.FloorToInt (-trackX / intervalDistance));		// Determines how many intervals on the NEGATIVE x-axis the parent should be moved this frame
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
				int intervalsToMoveY = (Mathf.FloorToInt (trackY / intervalDistance));		// Determines how many intervals on the POSITIVE y-axis the parent should be moved this frame
				movementY = intervalsToMoveY * intervalDistance;							// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else if (- trackY >= intervalDistance)											// Determines if the tracked NEGATIVE movement on the y-axis is greater than the interval distance
			{
				int intervalsToMoveY = (Mathf.FloorToInt (-trackY / intervalDistance));		// Determines how many intervals on the NEGATIVE y-axis the parent should be moved this frame
				movementY = - intervalsToMoveY * intervalDistance;							// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else
			{
				movementY = 0;																// If the tracked movement is not enough to move an interval, the movement of resize this frame is set to zero
			}				
			
			
			trackY = trackY % intervalDistance;												// Sets the tracking of movement on the y-axis to be the remainder of any extra movement if it is greater than the interval distance
		}
		
		
		float offsetX = mouseX - transform.parent.position.x - movementX;					// Used to determine the distance between the cursor position and the parent position on the x-axis			
		float offsetY = mouseY - transform.parent.position.y - movementY;					// Used to determine the distance between the cursor position and the parent position on the y-axis

		if (restrainToGrandparent == true)
		{
			if (worldSpace == false)
			{
				// The following line determines if the parent is at the edge of its parent and if the cursor is beyond the allowable move boundary on the x-axis
				if ((grandparentRightOuterBorder <= (transform.parent.localPosition.x + parentRightOuterBorder) && (offsetX > transform.localPosition.x + rightOuterBorder)) || (grandparentLeftOuterBorder >= (transform.parent.localPosition.x + parentLeftOuterBorder) && (offsetX < transform.localPosition.x + leftOuterBorder)))															// Determines if the left object is at the minimum width
				{
					makeMoveHorizontal = false;			// Sets the permission to move horizonatally to false
				}
				else
				{
					makeMoveHorizontal = true;			// Sets the permission to move horizonatally to true
				}
				
				
				// The following line determines if the parent is at the edge of its parent and if the cursor is beyond the allowable move boundary on the y-axis
				if ((grandparentTopOuterBorder <= (transform.parent.localPosition.y + parentTopOuterBorder) && (offsetY > transform.localPosition.y + topOuterBorder)) || (grandparentBottomOuterBorder >= (transform.parent.localPosition.y + parentBottomOuterBorder) && (offsetY < transform.localPosition.y + bottomOuterBorder)))															// Determines if the left object is at the minimum width
				{
					makeMoveVertical = false;			// Sets the permission to move vertically to false
				}
				else
				{
					makeMoveVertical = true;			// Sets the permission to move vertically to true
				}
			}
		}
		else
		{
			makeMoveHorizontal = true;					// Sets the permission to move horizonatally to true
			makeMoveVertical = true;					// Sets the permission to move vertically to true
		}	


		if (xOnly == false && yOnly == false)
		{
			if (makeMoveHorizontal == true && makeMoveVertical == true)
			{
				transform.parent.position = new Vector2 (Input.mousePosition.x - offsetX, Input.mousePosition.y - offsetY);		// Moves the parent along the x-axis and y-axis
			}
			else if (makeMoveHorizontal == true && makeMoveVertical == false)
			{
				transform.parent.position = new Vector2 (Input.mousePosition.x - offsetX, transform.parent.position.y);			// Moves the parent along the x-axis
			}
			else if (makeMoveHorizontal == false && makeMoveVertical == true)
			{
				transform.parent.position = new Vector2 (transform.parent.position.x, Input.mousePosition.y - offsetY);			// Moves the parent along the y-axis
			}	
		}
		else if (xOnly == true && makeMoveHorizontal == true)
		{
			transform.parent.position = new Vector2 (Input.mousePosition.x - offsetX, transform.parent.position.y);				// Moves the parent along the x-axis
		}
		else if (yOnly == true && makeMoveVertical == true)
		{
			transform.parent.position = new Vector2 (transform.parent.position.x, Input.mousePosition.y - offsetY);				// Moves the parent along the y-axis
		}
		
		
		if (restrainToGrandparent == true)
		{
			parentRectTransform = transform.parent.gameObject.GetComponent<RectTransform> ();									// This section gets the RectTransform information from the parent. Height and width are stored in variables. The borders of the object are also defined
			parentWidth = parentRectTransform.rect.width;
			parentHeight = parentRectTransform.rect.height;
			parentRightOuterBorder = (parentWidth * .5f) - restrainRightInset;
			parentLeftOuterBorder = (parentWidth * -.5f) + restrainLeftInset;
			parentTopOuterBorder = (parentHeight * .5f) - restrainTopInset;
			parentBottomOuterBorder = (parentHeight * -.5f) + restrainBottomInset;

			grandparentRectTransform = transform.parent.parent.gameObject.GetComponent<RectTransform> ();						// This section gets the RectTransform information from the grandparent. Height and width are stored in variables. The borders of the object are also defined
			grandparentWidth = grandparentRectTransform.rect.width;
			grandparentHeight = grandparentRectTransform.rect.height;
			grandparentRightOuterBorder = (grandparentWidth * .5f) - restrainRightInset;
			grandparentLeftOuterBorder = (grandparentWidth * -.5f) + restrainLeftInset;
			grandparentTopOuterBorder = (grandparentHeight * .5f) - restrainTopInset;
			grandparentBottomOuterBorder = (grandparentHeight * -.5f) + restrainBottomInset;

			if (transform.parent.localPosition.x + parentLeftOuterBorder < grandparentLeftOuterBorder)
			{
				transform.parent.localPosition = new Vector2 (grandparentLeftOuterBorder - parentLeftOuterBorder, transform.parent.localPosition.y);			// Moves the parent to the left edge of its parent
			}
			else if (transform.parent.localPosition.x + parentRightOuterBorder > grandparentRightOuterBorder)
			{
				transform.parent.localPosition = new Vector2 (grandparentRightOuterBorder - parentRightOuterBorder, transform.parent.localPosition.y);			// Moves the parent to the right edge of its parent
			}


			if (transform.parent.localPosition.y + parentBottomOuterBorder < grandparentBottomOuterBorder)
			{
				transform.parent.localPosition = new Vector2 (transform.parent.localPosition.x, grandparentBottomOuterBorder - parentBottomOuterBorder);		// Moves the parent to the bottom edge of its parent
			}
			else if (transform.parent.localPosition.y + parentTopOuterBorder > grandparentTopOuterBorder)
			{
				transform.parent.localPosition = new Vector2 (transform.parent.localPosition.x, grandparentTopOuterBorder - parentTopOuterBorder);				// Moves the parent to the top edge of its parent
			}
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
	
	
	public void RestrainToggle ()						// This function toggles the ability to restrain the parent to its parent on and off
	{
		restrainToGrandparent = !restrainToGrandparent;
	}


	public void MoveParentToggle ()						// This function toggles the ability to move the parent on and off. Good for events that toggle
	{
		disableMoveParent = !disableMoveParent;
	}


	public void EnableMoveParent ()						// This function enables the ability to move the parent. Good for events that do not toggle
	{
		disableMoveParent = false;
	}


	public void DisableMoveParent ()					// This function disables this ability to move the parent. Good for events that do not toggle
	{
		disableMoveParent = true;
	}
}