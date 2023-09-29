using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MoveOther_4_0 : MonoBehaviour {
	
	public GameObject otherObject;						// Defines the object to be moved
	public bool includeChildren = true;					// Determines if this object's children will be included in the raycast return
	public bool customCursor = true;					// Determines if custom cursors are used
	public bool bringToFrontOnOver = false;				// Determines if the other object will be set as the last sibling in the hierarchy when the cursor is over this object
	public bool bringToFront = true;					// Determines if the other object will be set as the last sibling in the hierarchy when the cursor is over this object and the mouse button is pressed
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
	public string cursor = "normal";					// Defines the cursor that should be used depending on the cursor location. This is a reference for the UIController script. Do not change
	bool startOnObject = false;							// Determines if the cursor is on this object when the mouse button is initially pressed
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
			Debug.Log ("You have not assigned an object to move to: " + transform.name);											// This only happens if there is no object assigned to "otherObject"
		}
	}


	public void Passive ()								// This function is called by the UIControl script when a raycast hits it and the mouse button is not pressed
	{
		if (bringToFrontOnOver == true)
		{
			Transform otherObjectTransform = otherObject.GetComponent<Transform> ();												// Gets the Transform information from the other object
			otherObjectTransform.SetAsLastSibling();																				// Sets the other object to be the last sibling in the hierarchy, making it the forward-most object
		}
		
		
		if (customCursor == true && disableMoveOther == false && otherObject != null)
		{
			RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();											// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
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
					cursor = "horizontal";				// Sets the cursor to use the "horizontal" custom cursor if assigned
				}
				else if (yOnly == true)					// The "yOnly" variable is true
				{
					cursor = "vertical";				// Sets the cursor to use the "vertical" custom cursor if assigned
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
			cursor = "normal";							// Sets the cursor to use the "normal" custom cursor if assigned
		}
	}
	
	
	public void Active ()								// This function is called by the UIControl script when a raycast hits it and the mouse button is initially pressed
	{
		if (bringToFront == true && otherObject != null)
		{
			Transform otherObjectTransform = otherObject.GetComponent<Transform> ();												// Gets the Transform information from the other object
			otherObjectTransform.SetAsLastSibling();																				// Sets the other object to be the last sibling in the hierarchy, making it the forward-most object
		}


		if (disableMoveOther == false && otherObject != null)
		{
			RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();											// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
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
				
				mouseX = Input.mousePosition.x;			// Stores the current cursor position
				mouseY = Input.mousePosition.y;
				
				oldMouseX = mouseX;						// Stores the current cursor position that will become the previous position in the next frame
				oldMouseY = mouseY;
			}
		}
	}
	
	
	void Update ()
	{
		if (startOnObject == true && (oldMouseX != Input.mousePosition.x || oldMouseY != Input.mousePosition.y))					// Determines if the mouse button is currently pressed, if it was initially pressed on a valid moveable position, and if the cursor position has moved since the last frame
		{
			mouseX = Input.mousePosition.x;				// Stores the current cursor position
			mouseY = Input.mousePosition.y;
			
			MoveOtherObject ();							// Calls the function to move the object
			
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


	void MoveOtherObject()								// This function is used to determine the amount of movement of the cursor, and then it moves the object
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
				makeMoveHorizontal = false;				// Sets the permission to move horizonatally to false
			}
			else
			{
				makeMoveHorizontal = true;				// Sets the permission to move horizonatally to true
			}

			
			// The following line determines if the other object is at the edge of its parent and if the cursor is beyond the allowable move boundary on the y-axis
			if ((parentTopOuterBorder <= (otherObjectTransform.localPosition.y + otherTopOuterBorder) && (mouseY > transform.position.y + topOuterBorder)) || (parentBottomOuterBorder >= (otherObjectTransform.localPosition.y + otherBottomOuterBorder) && (mouseY < transform.position.y + bottomOuterBorder)))															// Determines if the left object is at the minimum width
			{
				makeMoveVertical = false;				// Sets the permission to move vertically to false
			}
			else
			{
				makeMoveVertical = true;				// Sets the permission to move vertically to true
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
				otherObjectTransform.position = new Vector2 (Input.mousePosition.x - offsetX, Input.mousePosition.y - offsetY);		// Moves the other object along the x-axis and y-axis
			}
			else if (makeMoveHorizontal == true && makeMoveVertical == false)
			{
				otherObjectTransform.position = new Vector2 (Input.mousePosition.x - offsetX, otherObjectTransform.position.y);		// Moves the other object along the x-axis
			}
			else if (makeMoveHorizontal == false && makeMoveVertical == true)
			{
				otherObjectTransform.position = new Vector2 (otherObjectTransform.position.x, Input.mousePosition.y - offsetY);		// Moves the other object along the y-axis
			}	
		}
		else if (xOnly == true && makeMoveHorizontal == true)
		{
			otherObjectTransform.position = new Vector2 (Input.mousePosition.x - offsetX, otherObjectTransform.position.y);			// Moves the other object along the x-axis
		}
		else if (yOnly == true && makeMoveVertical == true)
		{
			otherObjectTransform.position = new Vector2 (otherObjectTransform.position.x, Input.mousePosition.y - offsetY);			// Moves the other object along the y-axis
		}


		if (restrainToOtherParent == true)
		{
			otherObjectRectTransform = otherObject.GetComponent<RectTransform> ();													// This section gets the RectTransform information from the other object. Height and width are stored in variables. The borders of the object are also defined
			otherWidth = otherObjectRectTransform.rect.width;
			otherHeight = otherObjectRectTransform.rect.height;
			otherRightOuterBorder = (otherWidth * .5f);
			otherLeftOuterBorder = (otherWidth * -.5f);
			otherTopOuterBorder = (otherHeight * .5f);
			otherBottomOuterBorder = (otherHeight * -.5f);
			
			parentRectTransform = otherObjectRectTransform.parent.gameObject.GetComponent<RectTransform> ();						// This section gets the RectTransform information from the parent. Height and width are stored in variables. The borders of the object are also defined
			parentWidth = parentRectTransform.rect.width;
			parentHeight = parentRectTransform.rect.height;
			parentRightOuterBorder = (parentWidth * .5f) - restrainRightInset;
			parentLeftOuterBorder = (parentWidth * -.5f) + restrainLeftInset;
			parentTopOuterBorder = (parentHeight * .5f) - restrainTopInset;
			parentBottomOuterBorder = (parentHeight * -.5f) + restrainBottomInset;
			
			otherObjectTransform = otherObject.GetComponent<Transform> ();															// Gets the Transform information from the other object

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
	
	
	public void MoveOtherToggle ()						// This function toggles the ability to move the other object on and off. Good for events that toggle
	{
		disableMoveOther = !disableMoveOther;
	}


	public void EnableMoveOther ()						// This function enables the ability to move the other object. Good for events that do not toggle
	{
		disableMoveOther = false;
	}

	
	public void DisableMoveOther ()						// This function disables this ability to move the other object. Good for events that do not toggle
	{
		disableMoveOther = true;
	}
}