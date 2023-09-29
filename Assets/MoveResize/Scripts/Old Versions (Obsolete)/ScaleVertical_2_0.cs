using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ScaleVertical_2_0 : MonoBehaviour {

	public GameObject topObject;						// Defines the top object to be scaled
	public GameObject bottomObject;						// Defines the bottom object to be scaled
	public bool includeChildren = true;					// Determines if the object's children will be included in the raycast return
	public bool ignoreTextBox = true;					// Determines if text box children will be included in the raycast return
	public bool disableScale = false;					// Determines if the ability to scale the other objects is enabled or disabled
	public bool intervalBased = false;					// Determines if this other objects will scale based on intervals
	bool startOnObject = false;							// Determines if the cursor is on the object when the mouse button is initially pressed
	bool tMinimumHeightReached = false;					// Determines if the top object has reached the minimum height allowed
	bool bMinimumHeightReached = false;					// Determines if the bottom object has reached the minimum height allowed
	bool scale = true;									// Determines if the conditions are true for the objects to be scaled
	public float intervalDistance = 50;					// Sets the size of intervals if the interval method is true
	public float tMinimumHeight = 10;					// Sets the minimum height the top object may scale to
	public float bMinimumHeight = 10;					// Sets the minimum height the bottom object may scale to
	float oldMouseY;									// Stores the cursor position on the y-axis for comparison with the newer "mouseY" variable
	float mouseY;										// Stores the current cursor position on the y-axis for comparison with the older "oldMouseY" variable
	float trackY;										// Used to track the distance the cursor moves over the y-axis during the entire time the mouse button is pressed. Interval method only
	
	
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
			
			
			if (Input.GetMouseButtonUp(0))										// When the mouse button is released, the "startOnObject" variable will be set to false, and the tracking variable will be reset to zero
			{
				startOnObject = false;
				trackY = 0;
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
				
				
				mouseY = Input.mousePosition.y;						// Stores the current cursor position
				startOnObject = true;								// Sets the "startOnObject" variable to true since the cursor is determined to be on the object
				return;
			}
		}
		
		
		if(objectsHit[0].gameObject == this.gameObject) 			// This section runs only if this object is the front object where the cursor is
		{
			mouseY = Input.mousePosition.y;							// Stores the current cursor position
			
			startOnObject = true;									// Sets the "startOnObject" variable to true since the cursor is determined to be on the object
		}
	}
	
	
	void ScaleTopBottomObjects ()									// This function is used to determine the amount of movement of the cursor, if the minimum limits have been reached, and then it scales the other objects
	{
		RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();								// This section gets the RectTransform information from this object. Height is stored in variables. The borders of the object are also defined
		float height = objectRectTransform.rect.height;
		float bottomOuterBorder = (height * .5f);
		float topOuterBorder = (height * -.5f);
		float tHeight;
		float bHeight;
		
		if (topObject != null)																						// Determines if a top object is assigned
		{
			RectTransform topObjectRectTransform = topObject.gameObject.GetComponent<RectTransform> ();				// This section gets the RectTransform information from the top object. Height is stored in a variable
			tHeight = topObjectRectTransform.rect.height;
		}
		else
		{
			tHeight = 0;																							// Sets the variables to 0 because there is no top object assianged
			tMinimumHeight = 0;
		}


		if (bottomObject != null)																					// Determines if a bottom object is assigned
		{		
			RectTransform bottomObjectRectTransform = bottomObject.gameObject.GetComponent<RectTransform> ();		// This section gets the RectTransform information from the bottom object. Height is stored in a variable
			bHeight = bottomObjectRectTransform.rect.height;
		}
		else
		{
			bHeight = 0;																							// Sets the variables to 0 because there is no bottom object assianged
			bMinimumHeight = 0;
		}


		float movementY = mouseY - oldMouseY;														// Compares the current cursor position on the y-axis to the previous one, finding the amount of movement over the frame
		
		if (intervalBased == true)																	// This section is only used if the interval method is selected
		{
			trackY = trackY + movementY;															// Tracks the amount of movement on the y-axis since the last interval
			
			if (trackY >= intervalDistance)															// Determines if the tracked POSITIVE movement on the y-axis is greater than the interval distance
			{
				int intervalsToMoveX = (Mathf.FloorToInt (trackY / intervalDistance));				// Determines how many intervals on the POSITIVE y-axis the other objects should be scaled this frame
				movementY = intervalsToMoveX * intervalDistance;									// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else if (- trackY >= intervalDistance)													// Determines if the tracked NEGATIVE movement on the y-axis is greater than the interval distance
			{
				int intervalsToMoveX = (Mathf.FloorToInt (-trackY / intervalDistance));				// Determines how many intervals on the NEGATIVE y-axis the other objects should be scaled this frame
				movementY = - intervalsToMoveX * intervalDistance;									// Determines how much distance the movement will be based on how many intervals and the size of intervals
			}
			else
			{
				movementY = 0;																		// If the tracked movement is not enough to move an interval, the movement of scale this frame is set to zero
			}				
			
			
			trackY = trackY % intervalDistance;														// Sets the tracking of movement on the y-axis to be the remainder of any extra movement if it is greater than the interval distance			
		}
		
		
		if(movementY > 0 && movementY > tHeight - tMinimumHeight && topObject != null)				// Determines if the POSITIVE movement is greater than the distance to reach the minimum height of the top object if assigned
		{
			movementY = tHeight - tMinimumHeight;													// Sets the amount of POSITIVE movement to the distance it takes to reach the minimum height of the top object
			tMinimumHeightReached = true;															// Sets the "bMinimumHeightReached" variable to true
		}
		else if (movementY < 0 && - movementY > bHeight - bMinimumHeight && bottomObject != null)	// Determines if the NEGATIVE movement is greater than the distance to reach the minimum height of the bottom object if assigned
		{
			movementY = - (bHeight - bMinimumHeight);												// Sets the amount of NEGATIVE movement to the distance it takes to reach the minimum height of the bottom object
			bMinimumHeightReached = true;															// Sets the "tMinimumHeightReached" variable to true
		}
		
		
		if (tHeight <= tMinimumHeight)																// Determines if the top object is at the minimum height
		{
			tMinimumHeightReached = true;
		}
		else
		{
			tMinimumHeightReached = false;
		}
		
		
		if (bHeight <= bMinimumHeight)																// Determines if the bottom object is at the minimum height
		{
			bMinimumHeightReached = true;
		}
		else
		{
			bMinimumHeightReached = false;
		}
		
		
		float offsetY = mouseY - transform.position.y - movementY;									// Used to determine the distance between the cursor position and the object position on the x-axis. This is so the object does not jump to center around the cursor when it is moved

		if (topObject != null && bottomObject != null)												// Determines if both objects are assigned
		{
			if((tMinimumHeightReached == true && offsetY > bottomOuterBorder) || (bMinimumHeightReached == true && offsetY < topOuterBorder))				// These section determines if a scale should be performed
			{
				scale = false;
			}
			else
			{
				scale = true;
			}
		}
		else if (topObject != null)																	// Determines if the top object is assigned
		{
			if(tMinimumHeightReached == true && offsetY > bottomOuterBorder)						// These section determines if a scale should be performed
			{
				scale = false;
			}
			else
			{
				scale = true;
			}
		}
		else if (bottomObject != null)																// Determines if the bottom object is assigned
		{
			if(bMinimumHeightReached == true && offsetY < topOuterBorder)							// These section determines if a scale should be performed
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