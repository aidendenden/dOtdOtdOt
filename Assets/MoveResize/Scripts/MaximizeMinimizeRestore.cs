using UnityEngine;

public class MaximizeMinimizeRestore : MonoBehaviour {

	public bool disableMaximize = false;				// Determines if the ability to maximize is enabled or disabled
	public bool disableMinimize = false;				// Determines if the ability to minimize is enabled or disabled
	public bool disableRestore = false;					// Determines if the ability to restore is enabled or disabled
	public float minimizeOffsetMinX = -50;				// Sets the minimum offset of this object on the x-axis for use when this object is minimized
	public float minimizeOffsetMinY = -10;				// Sets the minimum offset of this object on the y-axis for use when this object is minimized
	public float minimizeOffsetMaxX = 50;				// Sets the maximum offset of this object on the x-axis for use when this object is minimized
	public float minimizeOffsetMaxY = 10;				// Sets the maximum offset of this object on the y-axis for use when this object is minimized
	public bool xLeftMinimizedPosition = false;			// Determines if this object will minimize to the left side of the parent
	public bool xCenterMinimizedPosition = false;		// Determines if this object will minimize to the center side of the parent on the x-axis
	public bool xRightMinimizedPosition = false;		// Determines if this object will minimize to the right side of the parent
	public bool xDefinedMinimizedPosition = false;		// Determines if the user will define the position on the x-axis for when the this object is minimized
	public float minimizePositionX = 0;					// Sets the position of this object on the x-axis for use when this object is minimized if the "xDefinedMinimizedPosition" option is selected
	public bool yTopMinimizedPosition = false;			// Determines if this object will minimize to the top of the parent
	public bool yCenterMinimizedPosition = false;		// Determines if this object will minimize to the center side of the parent on the y-axis
	public bool yBottomMinimizedPosition = false;		// Determines if this object will minimize to the bottom of the parent
	public bool yDefinedMinimizedPosition = false;		// Determines if the user will define the position on the y-axis for when the this object is minimized
	public float minimizePositionY = 0;					// Sets the position of this object on the y-axis for use when this object is minimized if the "yDefinedMinimizedPosition" option is selected
	bool maximized = false;								// Determines if this object is maximized
	bool minimized = false;								// Determines if this object is minimized
	float restorePositionX;								// Stores the position of this object on the x-axis for use when this object is restored
	float restorePositionY;								// Stores the position of this object on the y-axis for use when this object is restored
	float restoreOffsetMinX;							// Stores the minimum offset of this object on the x-axis for use when this object is restored
	float restoreOffsetMinY;							// Stores the minimum offset of this object on the y-axis for use when this object is restored
	float restoreOffsetMaxX;							// Stores the maximum offset of this object on the x-axis for use when this object is restored
	float restoreOffsetMaxY;							// Stores the maximum offset of this object on the y-axis for use when this object is restored
	float restoreAnchorMinX;							// Stores the minimum anchor of this object on the x-axis for use when this object is restored
	float restoreAnchorMinY;							// Stores the minimum anchor of this object on the y-axis for use when this object is restored
	float restoreAnchorMaxX;							// Stores the maximum anchor of this object on the x-axis for use when this object is restored
	float restoreAnchorMaxY;							// Stores the maximum anchor of this object on the y-axis for use when this object is restored

	public void Maximize ()										// This function moves and resizes this object to the position and size of its parent			
	{
		if (maximized == false && disableMaximize == false)		// This section runs if this object is not currently maximized
		{
			RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();							// This section gets the RectTransform information from this object

			RectTransform parentRectTransform = transform.parent.gameObject.GetComponent<RectTransform> ();			// This section gets the RectTransform information from the parent
			float parentWidth = parentRectTransform.rect.width;
			float parentHeight = parentRectTransform.rect.height;
			float parentRightOuterBorder = (parentWidth * .5f);
			float parentLeftOuterBorder = (parentWidth * -.5f);
			float parentTopOuterBorder = (parentHeight * .5f);
			float parentBottomOuterBorder = (parentHeight * -.5f);
			float parentLeftEdge = transform.parent.position.x + parentLeftOuterBorder;
			float parentRightEdge = transform.parent.position.x + parentRightOuterBorder;
			float parentBottomEdge = transform.parent.position.y + parentBottomOuterBorder;
			float parentTopEdge = transform.parent.position.y + parentTopOuterBorder;

			float edgeOffsetMinX = parentLeftEdge - (parentWidth * objectRectTransform.anchorMin.x);				// This sets the offsets for the given anchors
			float edgeOffsetMaxX = parentRightEdge - (parentWidth * objectRectTransform.anchorMax.x);
			float edgeOffsetMinY = parentBottomEdge - (parentHeight * objectRectTransform.anchorMin.y);
			float edgeOffsetMaxY = parentTopEdge - (parentHeight * objectRectTransform.anchorMax.y);

			if (minimized == false)													// This section runs if this object is not currently minimized
			{
				restorePositionX = transform.position.x;							// Stores the position of this object on the x-axis for use when this object is restored
				restorePositionY = transform.position.y;							// Stores the position of this object on the y-axis for use when this object is restored
				restoreOffsetMinX = objectRectTransform.offsetMin.x;				// Stores the minimum offset of this object on the x-axis for use when this object is restored
				restoreOffsetMinY = objectRectTransform.offsetMin.y;				// Stores the minimum offset of this object on the y-axis for use when this object is restored
				restoreOffsetMaxX = objectRectTransform.offsetMax.x;				// Stores the maximum offset of this object on the x-axis for use when this object is restored
				restoreOffsetMaxY = objectRectTransform.offsetMax.y;				// Stores the maximum offset of this object on the y-axis for use when this object is restored
				restoreAnchorMinX = objectRectTransform.anchorMin.x;				// Stores the minimum anchor of this object on the x-axis for use when this object is restored
				restoreAnchorMinY = objectRectTransform.anchorMin.y;				// Stores the minimum anchor of this object on the y-axis for use when this object is restored
				restoreAnchorMaxX = objectRectTransform.anchorMax.x;				// Stores the maximum anchor of this object on the x-axis for use when this object is restored
				restoreAnchorMaxY = objectRectTransform.anchorMax.y;				// Stores the maximum anchor of this object on the y-axis for use when this object is restored
			}


			objectRectTransform.offsetMin = new Vector2 (edgeOffsetMinX, edgeOffsetMinY);							// This section moves and resizes this object to the position and size of its parent		
			objectRectTransform.offsetMax = new Vector2 (edgeOffsetMaxX, edgeOffsetMaxY);	
			transform.position = new Vector2 (transform.parent.position.x, transform.parent.position.y);			


			maximized = true;									// Sets the "maximized" variable to true		
			minimized = false;									// Sets the "minimized" variable to false	
		}
	}
	
	
	public void Minimize()										// This function minimizes this object based on minimum limits or user-defined values		
	{
		if (minimized == false && disableMinimize == false)		// This section runs if this object is not currently minimized
		{
			RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();							// This section gets the RectTransform information from this object

			RectTransform parentRectTransform = transform.parent.gameObject.GetComponent<RectTransform> ();			// This section gets the RectTransform information from the parent
			float parentWidth = parentRectTransform.rect.width;
			float parentHeight = parentRectTransform.rect.height;
			float parentRightOuterBorder = (parentWidth * .5f);
			float parentLeftOuterBorder = (parentWidth * -.5f);
			float parentTopOuterBorder = (parentHeight * .5f);
			float parentBottomOuterBorder = (parentHeight * -.5f);
			float parentLeftEdge = transform.parent.position.x + parentLeftOuterBorder;
			float parentRightEdge = transform.parent.position.x + parentRightOuterBorder;
			float parentBottomEdge = transform.parent.position.y + parentBottomOuterBorder;
			float parentTopEdge = transform.parent.position.y + parentTopOuterBorder;
						
			if (maximized == false)													// This section runs if this object is not currently maximized
			{
				restorePositionX = transform.position.x;							// Stores the position of this object on the x-axis for use when this object is restored
				restorePositionY = transform.position.y;							// Stores the position of this object on the y-axis for use when this object is restored
				restoreOffsetMinX = objectRectTransform.offsetMin.x;				// Stores the minimum offset of this object on the x-axis for use when this object is restored
				restoreOffsetMinY = objectRectTransform.offsetMin.y;				// Stores the minimum offset of this object on the y-axis for use when this object is restored
				restoreOffsetMaxX = objectRectTransform.offsetMax.x;				// Stores the maximum offset of this object on the x-axis for use when this object is restored
				restoreOffsetMaxY = objectRectTransform.offsetMax.y;				// Stores the maximum offset of this object on the y-axis for use when this object is restored
				restoreAnchorMinX = objectRectTransform.anchorMin.x;				// Stores the minimum anchor of this object on the x-axis for use when this object is restored
				restoreAnchorMinY = objectRectTransform.anchorMin.y;				// Stores the minimum anchor of this object on the y-axis for use when this object is restored
				restoreAnchorMaxX = objectRectTransform.anchorMax.x;				// Stores the maximum anchor of this object on the x-axis for use when this object is restored
				restoreAnchorMaxY = objectRectTransform.anchorMax.y;				// Stores the maximum anchor of this object on the y-axis for use when this object is restored
			}


			float x = 0;		// Stores the position this object will be moved to on the x-axis when minimized
			float y = 0;		// Stores the position this object will be moved to on the y-axis when minimized
			float ax = .5f;		// Stores the anchors this object will be set to on the x-axis when minimized
			float ay = .5f;		// Stores the anchors this object will be set to on the y-axis when minimized

			if (xDefinedMinimizedPosition == true)									// This section determines and sets the position this object will be moved to on the x-axis when minimized based on the selected option
			{
				x = minimizePositionX + transform.parent.position.x;
				ax = .5f;
			}
			else if (xLeftMinimizedPosition == true)
			{
				x = parentLeftEdge + ((minimizeOffsetMaxX - minimizeOffsetMinX) * .5f);
				ax = 0;
			}
			else if (xCenterMinimizedPosition == true)
			{
				x = transform.parent.position.x;
				ax = .5f;
			}
			else if (xRightMinimizedPosition == true)
			{
				x = parentRightEdge - ((minimizeOffsetMaxX - minimizeOffsetMinX) * .5f);
				ax = 1;
			}
			else
			{
				x = restorePositionX;
				ax = .5f;
			}


			if (yDefinedMinimizedPosition == true)									// This section determines and sets the position this object will be moved to on the y-axis when minimized based on the selected option
			{
				y = minimizePositionY+ transform.parent.position.y;
				ay = .5f;
			}
			else if (yTopMinimizedPosition == true)
			{
				y = parentTopEdge - ((minimizeOffsetMaxY - minimizeOffsetMinY) * .5f);
				ay = 1;
			}
			else if (yCenterMinimizedPosition == true)
			{
				y = transform.parent.position.y;
				ay = .5f;
			}
			else if (yBottomMinimizedPosition == true)
			{
				y = parentBottomEdge + ((minimizeOffsetMaxY - minimizeOffsetMinY) * .5f);
				ay = 0;
			}
			else
			{
				y = restorePositionY;
				ay = .5f;
			}


			objectRectTransform.anchorMin = new Vector2 (ax, ay);										// Sets the minimum anchors for this object when minimized
			objectRectTransform.anchorMax = new Vector2 (ax, ay);										// Sets the maximum anchors for this object when minimized
			objectRectTransform.offsetMin = new Vector2 (minimizeOffsetMinX, minimizeOffsetMinY);		// Sets the minimum offsets for this object when minimized
			objectRectTransform.offsetMax = new Vector2 (minimizeOffsetMaxX, minimizeOffsetMaxY);		// Sets the maximum offsets for this object when minimized
			transform.position = new Vector2 (x, y);													// Moves this object to this position when minimized

			minimized = true;			// Sets the "minimized" variable to true
			maximized = false;			// Sets the "maximized" variable to false
		}
	}
	
	
	public void Restore ()				// This function restores this object to its original position and size before it was minimized or maximized			
	{
		if (disableRestore == false && (maximized == true || minimized == true))						// This section runs only if this object is currently minimized or maximized
		{
			RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();				// This section gets the RectTransform information from this object

			objectRectTransform.anchorMin = new Vector2 (restoreAnchorMinX, restoreAnchorMinY);			// This section moves and resizes this object to its original position and size before being minimized or maximized
			objectRectTransform.anchorMax = new Vector2 (restoreAnchorMaxX, restoreAnchorMaxY);
			objectRectTransform.offsetMin = new Vector2 (restoreOffsetMinX, restoreOffsetMinY);
			objectRectTransform.offsetMax = new Vector2 (restoreOffsetMaxX, restoreOffsetMaxY);
			transform.position = new Vector2 (restorePositionX, restorePositionY);						

			maximized = false;			// Sets the "maximized" variable to false
			minimized = false;			// Sets the "minimized" variable to false
		}
	}
}