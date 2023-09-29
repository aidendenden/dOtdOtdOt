using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ScaleHorizontal_2_0 : MonoBehaviour {

	public GameObject leftObject;						// Defines the left object to be scaled
	public GameObject rightObject;						// Defines the right object to be scaled
	public bool includeChildren = true;					// Determines if the object's children will be included in the raycast return
	public bool ignoreTextBox = true;					// Determines if text box children will be included in the raycast return
	public bool disableScale = false;					// Determines if the ability to scale the other objects is enabled or disabled
	public bool intervalBased = false;					// Determines if this other objects will scale based on intervals
	bool startOnObject = false;							// Determines if the cursor is on the object when the mouse button is initially pressed
	bool lMinimumWidthReached = false;					// Determines if the left object has reached the minimum width allowed
	bool rMinimumWidthReached = false;					// Determines if the right object has reached the minimum width allowed
	bool scale = true;									// Determines if the conditions are true for the objects to be scaled
	public float intervalDistance = 50;					// Sets the size of intervals if the interval method is true
	public float lMinimumWidth = 10;					// Sets the minimum width the left object may scale to
	public float rMinimumWidth = 10;					// Sets the minimum width the right object may scale to
	float oldMouseX;									// Stores the cursor position on the x-axis for comparison with the newer "mouseX" variable
	float mouseX;										// Stores the current cursor position on the x-axis for comparison with the older "oldMouseX" variable
	float trackX;										// Used to track the distance the cursor moves over the x-axis during the entire time the mouse button is pressed. Interval method only
	
	
	void Update ()
	{
		if (disableScale == false)
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
			
			
			if (Input.GetMouseButtonUp(0))										// When the mouse button is released, the "startOnObject" variable will be set to false, and the tracking variable will be reset to zero
			{
				startOnObject = false;
				trackX = 0;
			}
		}
	}
	
	
	void PerformRaycast ()				// This function performs a raycast to detect if this object or its child is in front of other objects if any overlap at the cursor's position when the mouse button is initially pressed
	{
		PointerEventData cursor = new PointerEventData(EventSystem.current);
		cursor.position = Input.mousePosition;
		List<RaycastResult> objectsHit = new List<RaycastResult> ();
		EventSystem.current.RaycastAll(cursor, objectsHit);
		int x = 0;
		
		if(objectsHit[0].gameObject == this.gameObject || objectsHit[0].gameObject.transform.IsChildOf (transform))				// This section runs only if this object or its child is the front object where the cursor is
		{
			if(includeChildren == true)								
			{
				if (objectsHit[0].gameObject.GetComponent("Text") == true && ignoreTextBox == true)								// This section runs if you wish to ignore text boxes. If a text box is the front object, it will select the object behind it
				{
					while (objectsHit[x].gameObject.GetComponent("Text") == true)
					{
						x++;
					}
				}
				
				
				mouseX = Input.mousePosition.x;						// Stores the current cursor position
				startOnObject = true;								// Sets the "startOnObject" variable to true since the cursor is determined to be on the object
				return;
			}
		}
		
		
		if(objectsHit[0].gameObject == this.gameObject) 			// This section runs only if this object is the front object where the cursor is
		{
			mouseX = Input.mousePosition.x;							// Stores the current cursor position
			
			startOnObject = true;									// Sets the "startOnObject" variable to true since the cursor is determined to be on the object
		}
	}
	
	
	void ScaleLeftRightObjects ()									// This function is used to determine the amount of movement of the cursor, if the minimum limits have been reached, and then it scales the other objects
	{
		RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();							// This section gets the RectTransform information from this object. Width is stored in variables. The borders of the object are also defined
		float width = objectRectTransform.rect.width;
		float rightOuterBorder = (width * .5f);
		float leftOuterBorder = (width * -.5f);

		float lWidth;
		float rWidth;
		
		if (leftObject != null)																					// Determines if a left object is assigned
		{
			RectTransform leftObjectRectTransform = leftObject.gameObject.GetComponent<RectTransform> ();		// This section gets the RectTransform information from the left object. Width is stored in a variable
			lWidth = leftObjectRectTransform.rect.width;
		}
		else
		{
			lWidth = 0;																							// Sets the variables to 0 because there is no left object assianged
			lMinimumWidth = 0;
		}


		if (rightObject != null)																				// Determines if a right object is assigned
		{
			RectTransform rightObjectRectTransform = rightObject.gameObject.GetComponent<RectTransform> ();		// This section gets the RectTransform information from the right object. Width is stored in a variable
			rWidth = rightObjectRectTransform.rect.width;
		}
		else
		{
			rWidth = 0;																							// Sets the variables to 0 because there is no right object assianged
			rMinimumWidth = 0;
		}


		float movementX = mouseX - oldMouseX;													// Compares the current cursor position on the x-axis to the previous one, finding the amount of movement over the frame
		
		if (intervalBased == true)																// This section is only used if the interval method is selected
		{
			trackX = trackX + movementX;														// Tracks the amount of movement on the x-axis since the last interval
			
			if (trackX >= intervalDistance)														// Determines if the tracked POSITIVE movement on the x-axis is greater than the interval distance
			{
				int intervalsToMoveX = (Mathf.FloorToInt (trackX / intervalDistance));			// Determines how many intervals on the POSITIVE x-axis the other objects should be scaled this frame
				movementX = intervalsToMoveX * intervalDistance;								// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else if (- trackX >= intervalDistance)												// Determines if the tracked NEGATIVE movement on the x-axis is greater than the interval distance
			{
				int intervalsToMoveX = (Mathf.FloorToInt (-trackX / intervalDistance));			// Determines how many intervals on the NEGATIVE x-axis the other objects should be scaled this frame
				movementX = - intervalsToMoveX * intervalDistance;								// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else
			{
				movementX = 0;																	// If the tracked movement is not enough to move an interval, the movement of resize this frame is set to zero
			}				
			
			
			trackX = trackX % intervalDistance;													// Sets the tracking of movement on the x-axis to be the remainder of any extra movement if it is greater than the interval distance			
		}
		
		
		if(movementX > 0 && movementX > rWidth - rMinimumWidth && rightObject != null)			// Determines if the POSITIVE movement is greater than the distance to reach the minimum width of the right object if assigned
		{
			movementX = rWidth - rMinimumWidth;													// Sets the amount of POSITIVE movement to the distance it takes to reach the minimum width of the right object
			rMinimumWidthReached = true;														// Sets the "rMinimumWidthReached" variable to true
		}
		else if (movementX < 0 && - movementX > lWidth - lMinimumWidth && leftObject != null)	// Determines if the NEGATIVE movement is greater than the distance to reach the minimum width of the left object if assigned
		{
			movementX = - ( lWidth - lMinimumWidth);											// Sets the amount of NEGATIVE movement to the distance it takes to reach the minimum width of the left object
			lMinimumWidthReached = true;														// Sets the "lMinimumWidthReached" variable to true
		}
		
		
		if (lWidth <= lMinimumWidth)															// Determines if the left object is at the minimum width
		{
			lMinimumWidthReached = true;
		}
		else
		{
			lMinimumWidthReached = false;
		}
		
		
		if (rWidth <= rMinimumWidth)															// Determines if the right object is at the minimum width
		{
			rMinimumWidthReached = true;
		}
		else
		{
			rMinimumWidthReached = false;
		}
		
		
		float offsetX = mouseX - transform.position.x - movementX;								// Used to determine the distance between the cursor position and the object position on the x-axis. This is so the object does not jump to center around the cursor when it is moved

		if (leftObject != null && rightObject != null)											// Determines if both objects are assigned
		{
			if((lMinimumWidthReached == true && offsetX < leftOuterBorder) || (rMinimumWidthReached == true && offsetX > rightOuterBorder))				// These section determines if a scale should be performed
			{
				scale = false;
			}
			else
			{
				scale = true;
			}
		}
		else if (leftObject != null)															// Determines if the left object is assigned
		{
			if(lMinimumWidthReached == true && offsetX < leftOuterBorder)						// These section determines if a scale should be performed
			{
				scale = false;
			}
			else
			{
				scale = true;
			}
		}	
		else if (rightObject != null)															// Determines if the right object is assigned
		{
			if(rMinimumWidthReached == true && offsetX > rightOuterBorder)						// These section determines if a scale should be performed
			{
				scale = false;
			}
			else
			{
				scale = true;
			}
		}
		
		
		if(scale == true)
		{
			transform.position = new Vector2 (Input.mousePosition.x - offsetX, transform.position.y);														// Moves the object
			
			if (leftObject != null)																															// Determines if the left object is assigned
			{
				RectTransform leftObjectRectTransform = leftObject.gameObject.GetComponent<RectTransform> ();												// This section gets the RectTransform information from the left object
				leftObjectRectTransform.offsetMax = new Vector2 (leftObjectRectTransform.offsetMax.x + movementX, leftObjectRectTransform.offsetMax.y);		// Scales the left object
			}


			if (rightObject != null)																														// Determines if the right object is assigned
			{
				RectTransform rightObjectRectTransform = rightObject.gameObject.GetComponent<RectTransform> ();												// This section gets the RectTransform information from the right object
				rightObjectRectTransform.offsetMin = new Vector2 (rightObjectRectTransform.offsetMin.x + movementX, rightObjectRectTransform.offsetMin.y);	// Scales the right object
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