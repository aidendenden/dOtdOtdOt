using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BringToFront_1_0 : MonoBehaviour{


	void Update ()
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
				PerformRaycast ();			// Calls the function to perform a raycast
			}
		}
	}


	void PerformRaycast ()					// This function performs a raycast to detect if this object or its child is in front of other objects if any overlap at the cursor's position when the mouse button is initially pressed
	{
		PointerEventData cursor = new PointerEventData(EventSystem.current);
		cursor.position = Input.mousePosition;
		List<RaycastResult> objectsHit = new List<RaycastResult> ();
		EventSystem.current.RaycastAll(cursor, objectsHit);

		if(objectsHit[0].gameObject == this.gameObject || objectsHit[0].gameObject.transform.IsChildOf (transform))			// This section runs only if this object or its child is the front object where the cursor is
		{
			transform.SetAsLastSibling();					// Sets this object to be the last sibling in the hierarchy, making it the forward-most object
		}
	}
}
