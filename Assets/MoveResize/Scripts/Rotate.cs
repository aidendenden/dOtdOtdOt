using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Rotate : MonoBehaviour {

	public float movementMultiplier = 1;				// Defines the amount of multiplication for movement each frame
	public bool includeChildren = true;					// Determines if this object's children will be included in the raycast return
	public bool customCursor = true;					// Determines if custom cursors are used
	public bool bringToFrontOnOver = false;				// Determines if this object will be set as the last sibling in the hierarchy when the cursor is over ths object
	public bool bringToFront = true;					// Determines if this object will be set as the last sibling in the hierarchy when the cursor is over this object and the mouse button is pressed
	public bool disableRotate = false;					// Determines if the ability to rotate the object is enabled or disabled
	public string cursor = "normal";					// Defines the cursor that should be used depending on the cursor location. This is a reference for the UIController script. Do not change
	bool startOnObject = false;							// Determines if the cursor is on this object when the mouse button is initially pressed
	bool rotate = true;									// Determines if the conditions are true for the objects to be rotated
	float oldMouseX;									// Stores the cursor position on the x-axis for comparison with the newer "mouseX" variable
	float mouseX;										// Stores the current cursor position on the x-axis for comparison with the older "oldMouseX" variable


	public void Passive ()								// This function is called by the UIControl script when a raycast hits it and the mouse button is not pressed
	{
		if (customCursor == true && disableRotate == false)
		{
			RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();					// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
			float width = objectRectTransform.rect.width;
			float height = objectRectTransform.rect.height;
			float rightOuterBorder = (width * .5f);
			float leftOuterBorder = (width * -.5f);
			float topOuterBorder = (height * .5f);
			float bottomOuterBorder = (height * -.5f);
			
			// The following line determines if the cursor is over a valid movable position
			if(Input.mousePosition.x <= (transform.position.x + rightOuterBorder) && Input.mousePosition.x >= (transform.position.x + leftOuterBorder) && Input.mousePosition.y <= (transform.position.y + topOuterBorder) && Input.mousePosition.y >= (transform.position.y + bottomOuterBorder))
			{
				cursor = "horizontal";					// Sets the cursor to use the "horizontal" custom cursor if assigned
			}
			else
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
		if (disableRotate == false)
		{
			RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();					// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
			float width = objectRectTransform.rect.width;
			float height = objectRectTransform.rect.height;
			float rightOuterBorder = (width * .5f);
			float leftOuterBorder = (width * -.5f);
			float topOuterBorder = (height * .5f);
			float bottomOuterBorder = (height * -.5f);
			
			// The following line determines if the cursor is on a valid movable position based on the object and the insets
			if(Input.mousePosition.x <= (transform.position.x + rightOuterBorder) && Input.mousePosition.x >= (transform.position.x + leftOuterBorder) && Input.mousePosition.y <= (transform.position.y + topOuterBorder) && Input.mousePosition.y >= (transform.position.y + bottomOuterBorder))
			{
				startOnObject = true;
				oldMouseX = Input.mousePosition.x;		// Stores the initial cursor position
			}
		}
	}
	
	
	void Update ()
	{
		if (startOnObject == true && oldMouseX != Input.mousePosition.x)									// Determines if the mouse button is currently pressed and if it was initially pressed when on the object
		{
			mouseX = Input.mousePosition.x;				// Stores the current cursor position
			RotateObject();								// Calls the function to rotate the object
			oldMouseX = mouseX;							// Stores the current cursor position that will become the previous position in the next frame
		}
	}
	
	
	public void End ()									// This function is called by the UIControl script when the mouse button is initially released. Tracking and movement variables are reset to zero
	{
		startOnObject = false;
	}
	
	
	void RotateObject ()								// This function is used to determine the amount of movement of the cursor, if the minimum limits have been reached, and then it rotates the object
	{
		float movementX = (mouseX - oldMouseX) * movementMultiplier;										// Compares the current cursor position on the x-axis to the previous one, finding the amount of movement over the frame
				
		
		if(rotate == true)								// This statement runs if all factors allow rotating to take place
		{
			this.gameObject.transform.Rotate (0, 0, -movementX);
		}		
	}

	
	public void RotateObjectToggle ()				// This function toggles the ability to rotate the object on and off. Good for events that toggle
	{
		disableRotate = !disableRotate;
	}
	
	
	public void EnableRotateObject ()				// This function enables the ability to rotate the object. Good for events that do not toggle
	{
		disableRotate = false;
	}
	
	
	public void DisableRotateObject ()				// This function disables this ability to rotate the object. Good for events that do not toggle
	{
		disableRotate = true;
	}
}