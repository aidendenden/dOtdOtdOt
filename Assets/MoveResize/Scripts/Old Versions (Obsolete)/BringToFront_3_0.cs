using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BringToFront_3_0 : MonoBehaviour{

	public bool stayAtFront = false;					// Determines if this objects will always be moved the the front of the UI
	public bool includeChildren = true;					// Determines if this object's children will be included in the raycast return
	public bool ignoreTextBox = true;					// Determines if any text box will be included in the raycast return


	void LateUpdate ()
	{
		if (stayAtFront == true)
		{
			transform.SetAsLastSibling();				// Sets this object to be the last sibling in the hierarchy, making it the forward-most object
		}
		else
		{
			if(Input.GetMouseButtonDown (0))
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
					PerformRaycast ();		// Calls the function to perform a raycast
				}
			}
		}
	}


	void PerformRaycast ()					// This function performs a raycast to detect if this object or its child is in front of other objects if any overlap at the cursor's position when the mouse button is initially pressed
	{
		PointerEventData cursor = new PointerEventData(EventSystem.current);						// This section prepares a list for all objects hit with the raycast
		cursor.position = Input.mousePosition;
		List<RaycastResult> objectsHit = new List<RaycastResult> ();
		EventSystem.current.RaycastAll(cursor, objectsHit);
		int count = objectsHit.Count;
		int x = 0;

		if (count != 0)
		{
			if (objectsHit[x].gameObject.GetComponent("Text") == true && ignoreTextBox == true)			// This section runs if you wish to ignore text boxes. If an object with a "text" or "Ignore" script attache is the selected object, it will select the object behind it
			{
				{
					x++;
					
					if (x == count)
					{
						return;																			// Returns early from this function
					}
				}
			}
			else 													
			{
				while (objectsHit[x].gameObject.GetComponent("Ignore") == true)							// If an object with the "Ignore" script attached is the selected object, it will select the object behind it
				{
					x++;
					
					if (x == count)
					{
						return;																			// Returns early from this function
					}
				}
			}


			if(objectsHit[x].gameObject == this.gameObject)												// This section runs only if this object is the front object where the cursor is
			{
				transform.SetAsLastSibling();															// Sets this object to be the last sibling in the hierarchy, making it the forward-most object
			}
			else if (objectsHit[x].gameObject.transform.IsChildOf (transform) && includeChildren == true)		// This section runs only if a child of this object is the front object where the cursor is
			{
				if(includeChildren == true)								
				{
					// This statement runs if the selected object has an "IgnoreThisChild" script attached is the selected object. It will continue to select the objects behind it until it finds one it should not ignore
					while (objectsHit[x].gameObject.GetComponent("IgnoreThisChild") == true || objectsHit[x].gameObject.GetComponent("Ignore") == true || (objectsHit[x].gameObject.GetComponent("Text") == true && ignoreTextBox == true))
					{
						x++;
						
						if (x == count)
						{
							return;																		// Returns early from this function
						}
					}

					transform.SetAsLastSibling();														// Sets this object to be the last sibling in the hierarchy, making it the forward-most object
				}
			}
		}
	}


	public void StayAtFrontToggle ()		// This function toggles the ability to bring this object to the front every frame on and off
	{
		stayAtFront = !stayAtFront;
	}
}