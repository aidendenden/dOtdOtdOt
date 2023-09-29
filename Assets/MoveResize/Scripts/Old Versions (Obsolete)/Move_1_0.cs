using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Move_1_0 : MonoBehaviour {
	

	public bool includeChildren = true;			// Determines if the object's children will be included in the raycast return
	public bool ignoreTextBox = true;			// Determines if text box children will be included in the raycast return
	public bool disableMove = false;			// Determines if the ability to move is enabled or disabled
	public bool intervalBased = false;			// Determines if this object will move based on intervals
	bool startOnObject = false;					// Determines if the cursor is on the object when the mouse button is initially pressed
	public float intervalDistance = 50;			// Sets the size of intervals if the interval method is true
	public float topInset = 0;					// Sets the distance from the top edge of the object that the cursor must be in order to be considered on a valid moveable position
	public float bottomInset = 0;				// Sets the distance from the bottom edge of the object that the cursor must be in order to be considered on a valid moveable position
	public float rightInset = 0;				// Sets the distance from the right edge of the object that the cursor must be in order to be considered on a valid moveable position
	public float leftInset = 0;					// Sets the distance from the left edge of the object that the cursor must be in order to be considered on a valid moveable position
	float oldMouseX;							// Stores the cursor position on the x-axis for comparison with the newer "mouseX" variable
	float oldMouseY;							// Stores the cursor position on the y-axis for comparison with the newer "mouseY" variable
	float mouseX;								// Stores the current cursor position on the x-axis for comparison with the older "oldMouseX" variable
	float mouseY;								// Stores the current cursor position on the y-axis for comparison with the older "oldMouseY" variable
	float trackX;								// Used to track the distance the cursor moves over the x-axis during the entire time the mouse button is pressed. Interval method only
	float trackY;								// Used to track the distance the cursor moves over the y-axis during the entire time the mouse button is pressed. Interval method only
	
	
	void Update ()
	{
		if (disableMove == false)
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
					PerformRaycast ();								// Calls the function to perform a raycast
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
				
				
				MoveObject ();										// Calls the function to move the object
				
				
				oldMouseX = mouseX;									// Stores the current cursor position that will become the previous position in the next frame
				oldMouseY = mouseY;
			}
			
			
			if (Input.GetMouseButtonUp(0))							// When the mouse button is released, the "startOnObject" variable will be set to false, and the tracking variables will be reset to zero
			{
				startOnObject = false;
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
			transform.SetAsLastSibling();							// Sets this object to be the last sibling in the hierarchy, making it the forward-most object

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

					oldMouseX = Input.mousePosition.x;				// Stores the initial cursor position
					oldMouseY = Input.mousePosition.y;
				
					startOnObject = true;							// Sets the "startOnObject" variable to true
					return;
				}
			}
		}


		if(objectsHit[0].gameObject == this.gameObject) 			// This section runs only if this object is the front object where the cursor is
		{
			oldMouseX = Input.mousePosition.x;						// Stores the initial cursor position
			oldMouseY = Input.mousePosition.y;
				
			startOnObject = true;									// Sets the "startOnObject" variable to true
		}
	}
	
	
	public void MoveObject()										// This function is used to determine the amount of movement of the cursor, and then it moves the object
	{
		float movementX = mouseX - oldMouseX;						// Compares the current cursor position on the x-axis to the previous one, finding the amount of movement over the frame
		float movementY = mouseY - oldMouseY;						// Compares the current cursor position on the y-axis to the previous one, finding the amount of movement over the frame
		
		
		if (intervalBased == true)															// This section is only used if the interval method is selected
		{
			trackX = trackX + movementX;													// Tracks the amount of movement on the x-axis since the last interval
			
			
			if (trackX >= intervalDistance)													// Determines if the tracked POSITIVE movement on the x-axis is greater than the interval distance
			{
				int intervalsToMoveX = (Mathf.FloorToInt (trackX / intervalDistance));		// Determines how many intervals on the POSITIVE x-axis the object should be moved this frame
				movementX = intervalsToMoveX * intervalDistance;							// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else if (- trackX >= intervalDistance)											// Determines if the tracked NEGATIVE movement on the x-axis is greater than the interval distance
			{
				int intervalsToMoveX = (Mathf.FloorToInt (-trackX / intervalDistance));		// Determines how many intervals on the NEGATIVE x-axis the object should be moved this frame
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
				int intervalsToMoveY = (Mathf.FloorToInt (trackY / intervalDistance));		// Determines how many intervals on the POSITIVE y-axis the object should be moved this frame
				movementY = intervalsToMoveY * intervalDistance;							// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else if (- trackY >= intervalDistance)											// Determines if the tracked NEGATIVE movement on the y-axis is greater than the interval distance
			{
				int intervalsToMoveY = (Mathf.FloorToInt (-trackY / intervalDistance));		// Determines how many intervals on the NEGATIVE y-axis the object should be moved this frame
				movementY = - intervalsToMoveY * intervalDistance;							// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else
			{
				movementY = 0;																// If the tracked movement is not enough to move an interval, the movement of resize this frame is set to zero
			}				
			
			
			trackY = trackY % intervalDistance;												// Sets the tracking of movement on the y-axis to be the remainder of any extra movement if it is greater than the interval distance
		}
		
		
		float offsetX = mouseX - transform.position.x - movementX;							// Used to determine the distance between the cursor position and the object position on the x-axis			
		float offsetY = mouseY - transform.position.y - movementY;							// Used to determine the distance between the cursor position and the object position on the y-axis
		
		
		transform.position = new Vector2 (Input.mousePosition.x - offsetX, Input.mousePosition.y - offsetY);			// Moves the object
	}
	
	
	public void IntervalToggle ()		// This function toggles the interval method on and off
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
	
	
	public void MoveToggle ()			// This function toggles the ability to move the object on and off. Good for events that toggle
	{
		if(disableMove == true)
		{
			disableMove = false;
		}
		else
		{
			disableMove = true;
		}	
	}
	
	
	public void DisableMove ()			// This function disables the ability to move the object. Good for events that do not toggle
	{
		if (Input.GetMouseButton(0) && startOnObject == true)		// Assures that the move ability will not be disabled while already in progress. This is important when using events such as Pointer Enter and Exit
		{
			return;
		}
		else if(disableMove == false)
		{
			disableMove = true;
		}	
	}
	
	
	public void EnableMove ()			// This function enables the ability to move the object. Good for events that do not toggle
	{
		if(disableMove == true)
		{
			disableMove = false;
		}
	}
}