using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BringToFront : MonoBehaviour{

	public bool stayAtFront = false;					// Determines if this objects will always be moved the the front of the UI
	public bool bringToFrontOnOver = false;				// Determines if this object will be set as the last sibling in the hierarchy when the cursor is over ths object
	public bool bringToFront = true;					// Determines if this object will be set as the last sibling in the hierarchy when the cursor is over this object and the mouse button is pressed
	public bool includeChildren = true;					// Determines if this object's children will be included in the raycast return
	public bool disableBringToFront = false;			// Determines if the ability to bring this object to the front of the UI is on or off

	public void Passive ()								// This function is called by the UIControl script when a raycast hits it and the mouse button is not pressed
	{
		if (disableBringToFront == false && bringToFrontOnOver == true)
		{
			transform.SetAsLastSibling();				// Sets this object to be the last sibling in the hierarchy, making it the forward-most object
		}
	}


	public void Active ()								// This function is called by the UIControl script when a raycast hits it and the mouse button is initially pressed
	{
		if (disableBringToFront == false && bringToFront == true)
		{
			transform.SetAsLastSibling();				// Sets this object to be the last sibling in the hierarchy, making it the forward-most object
		}
	}

	
	void LateUpdate ()									// This function sets this object as the last sibling every frame, making it the forward-most UI object
	{
		if (disableBringToFront == false && stayAtFront == true)
		{
			transform.SetAsLastSibling();				// Sets this object to be the last sibling in the hierarchy, making it the forward-most object
		}
	}


	public void StayAtFrontToggle ()					// This function toggles the ability to bring this object to the front every frame on and off. Good for events that toggle
	{
		stayAtFront = !stayAtFront;
	}


	public void StayAtFrontEnable ()					// This function enables the ability to bring this object to the front every frame. Good for events that do not toggle
	{
		stayAtFront = true;
	}


	public void StayAtFrontDisable ()					// This function disables the ability to bring this object to the front every frame. Good for events that do not toggle
	{
		stayAtFront = false;
	}


	public void BringToFrontToggle ()					// This function toggles the ability to bring this object to the front. Good for events that toggle
	{
		disableBringToFront = !disableBringToFront;
	}
	
	
	public void BringToFrontEnable ()					// This function enables the ability to bring this object to the front. Good for events that do not toggle
	{
		disableBringToFront = false;
	}


	public void BringToFrontDisable ()					// This function disables the ability to bring this object to the front. Good for events that do not toggle
	{
		disableBringToFront = true;
	}
}