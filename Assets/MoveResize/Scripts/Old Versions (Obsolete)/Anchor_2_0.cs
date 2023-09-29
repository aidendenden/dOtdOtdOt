using UnityEngine;

public class Anchor_2_0 : MonoBehaviour {

	public GameObject anchoredObject;			// Defines the object to be anchored	
	float lpx;									// Stores this object's local position on the x-axis
	float lpy;									// Stores this object's local position on the y-axis
	float w;									// Stores this object's width
	float h;									// Stores this object's height
	float aw;									// Stores the anchored object's width
	float ah;									// Stores the anchored object's height
	public bool getPosition = true;				// Determines if the anchored object should be anchored based on its initial position
	public bool disableAnchor = false;			// Determines if the ability to anchor the other object is enabled or disabled		
	public bool top = false;					// Determines if the top of the anchored object should be anchored to the top of this object
	public bool bottom = false;					// Determines if the bottom of the anchored object should be anchored to the bottom of this object
	public bool left = false;					// Determines if the left side of the anchored object should be anchored to the left side of this object
	public bool right = false;					// Determines if the right side of the anchored object should be anchored to the right side of this object
	public bool below = false;					// Determines if the anchored object should be anchored below this object
	public bool above = false;					// Determines if the anchored object should be anchored above this object
	public bool onRight = false;				// Determines if the anchored object should be anchored on the right side of this object
	public bool onLeft = false;					// Determines if the anchored object should be anchored on the left side of this object
	public float topAlignment = 0;				// Sets the offset from the top of this object to the top of the anchored object
	public float bottomAlignment = 0;			// Sets the offset from the bottom of this object to the bottom of the anchored object
	public float leftAlignment = 0;				// Sets the offset from the left side of this object to the left side of the anchored object
	public float rightAlignment = 0;			// Sets the offset from the right side of this object to the right side of the anchored object
	public float belowSpacing = 0;				// Sets the offset from the bottom of this object to the top of the anchored object
	public float aboveSpacing = 0;				// Sets the offset from the top of this object to the bottom of the anchored object
	public float onRightSpacing = 0;			// Sets the offset from the right side of this object to the left side of the anchored object
	public float onLeftSpacing = 0;				// Sets the offset from the left side of this object to the right side of the anchored object

	
	void Start ()
	{

		if (anchoredObject == null)
		{
			Debug.Log ("You have not assigned an anchored object to: " + transform.name);							// This only happens if there is no object assigned to "anchoredObject"
		}
		else
		{
			if (getPosition == true)
			{
				GetPosition ();						// Calls the function to get the position of the anchored object
			}


			PlaceAnchoredObject ();					// Calls the function to move and resize the anchored object
		}
	}


	void Update ()
	{
		if (disableAnchor == false && anchoredObject != null)
		{
			RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();							// This section gets the RectTransform information from this object
			RectTransform anchoredRectTransform = anchoredObject.gameObject.GetComponent<RectTransform> ();			// This section gets the RectTransform information from the anchored object

			// The following line determines if the objects have changed position or size since the last frame
			if (transform.localPosition.x != lpx || transform.localPosition.y != lpy || objectRectTransform.rect.width != w  || objectRectTransform.rect.height != h || anchoredRectTransform.rect.width != aw  || anchoredRectTransform.rect.height != ah)
			{
				PlaceAnchoredObject ();			// Calls the function to move and resize the anchored object
			}
		}
	}


	void GetPosition ()					// This function stores the differences in positioning of the two objects
	{
		RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();														// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
		float objectWidth = objectRectTransform.rect.width;
		float objectHeight = objectRectTransform.rect.height;
		float objectRightEdge = (objectWidth * .5f);
		float objectLeftEdge = (objectWidth * -.5f);
		float objectTopEdge = (objectHeight * .5f);
		float objectBottomEdge = (objectHeight * -.5f);

		RectTransform anchoredRectTransform = anchoredObject.gameObject.GetComponent<RectTransform> ();										// This section gets the RectTransform information from the anchored object. Height and width are stored in variables. The borders of the object are also defined
		float anchoredWidth = anchoredRectTransform.rect.width;
		float anchoredHeight = anchoredRectTransform.rect.height;
		float anchoredRightEdge = (anchoredWidth * .5f);
		float anchoredLeftEdge = (anchoredWidth * -.5f);
		float anchoredTopEdge = (anchoredHeight * .5f);
		float anchoredBottomEdge = (anchoredHeight * -.5f);

		topAlignment = anchoredRectTransform.offsetMax.y - objectRectTransform.offsetMax.y;													// Stores the offset from the top of this object to the top of the anchored object
		bottomAlignment = anchoredRectTransform.offsetMin.y - objectRectTransform.offsetMin.y;												// Stores the offset from the bottom of this object to the bottom of the anchored object
		leftAlignment = anchoredRectTransform.offsetMin.x - objectRectTransform.offsetMin.x;												// Stores the offset from the left side of this object to the left side of the anchored object
		rightAlignment = anchoredRectTransform.offsetMax.x - objectRectTransform.offsetMax.x;												// Stores the offset from the right side of this object to the right side of the anchored object
		belowSpacing = transform.localPosition.y - anchoredObject.transform.localPosition.y + objectBottomEdge - anchoredTopEdge;			// Stores the offset from the bottom of this object to the top of the anchored object
		aboveSpacing = anchoredObject.transform.localPosition.y - transform.localPosition.y - objectTopEdge + anchoredBottomEdge;			// Stores the offset from the top of this object to the bottom of the anchored object
		onRightSpacing = anchoredObject.transform.localPosition.x - transform.localPosition.x - objectRightEdge + anchoredLeftEdge;			// Stores the offset from the right side of this object to the left side of the anchored object
		onLeftSpacing = transform.localPosition.x - anchoredObject.transform.localPosition.x + objectLeftEdge - anchoredRightEdge;			// Stores the offset from the left side of this object to the right side of the anchored object
	}


	void PlaceAnchoredObject ()			// This function moves and resizes the anchored object
	{
		RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();														// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
		float objectWidth = objectRectTransform.rect.width;
		float objectHeight = objectRectTransform.rect.height;
		float objectRightEdge = (objectWidth * .5f);
		float objectLeftEdge = (objectWidth * -.5f);
		float objectTopEdge = (objectHeight * .5f);
		float objectBottomEdge = (objectHeight * -.5f);

		RectTransform anchoredRectTransform = anchoredObject.gameObject.GetComponent<RectTransform> ();										// This section gets the RectTransform information from the anchored object. Height and width are stored in variables. The borders of the object are also defined
		float anchoredWidth = anchoredRectTransform.rect.width;
		float anchoredHeight = anchoredRectTransform.rect.height;
		float anchoredRightEdge = (anchoredWidth * .5f);
		float anchoredLeftEdge = (anchoredWidth * -.5f);
		float anchoredTopEdge = (anchoredHeight * .5f);
		float anchoredBottomEdge = (anchoredHeight * -.5f);

		if (top == true)
		{
			anchoredRectTransform.offsetMax = new Vector2 (anchoredRectTransform.offsetMax.x, objectRectTransform.offsetMax.y + topAlignment);			// Aligns the top of the anchored object to the top of this object based on offsets
		}


		if (bottom == true)
		{
			anchoredRectTransform.offsetMin = new Vector2 (anchoredRectTransform.offsetMin.x, objectRectTransform.offsetMin.y + bottomAlignment);		// Aligns the bottom of the anchored object to the bottom of this object based on offsets
		}


		if (left == true)
		{
			anchoredRectTransform.offsetMin = new Vector2 (objectRectTransform.offsetMin.x + leftAlignment, anchoredRectTransform.offsetMin.y);			// Aligns the left side of the anchored object to the left side of this object based on offsets
		}


		if (right == true)
		{
			anchoredRectTransform.offsetMax = new Vector2 (objectRectTransform.offsetMax.x + rightAlignment, anchoredRectTransform.offsetMax.y);		// Aligns the right side of the anchored object to the right of this object based on offsets
		}


		if (below == true)
		{
			anchoredObject.transform.localPosition = new Vector2 (anchoredObject.transform.localPosition.x, transform.localPosition.y + objectBottomEdge - anchoredTopEdge - belowSpacing);			// Aligns the top of the anchored object to the bottom of this object based on offsets
		}


		if (above == true)
		{
			anchoredObject.transform.localPosition = new Vector2 (anchoredObject.transform.localPosition.x, transform.localPosition.y + objectTopEdge - anchoredBottomEdge + aboveSpacing);			// Aligns the bottom of the anchored object to the top of this object based on offsets
		}


		if (onRight == true)
		{
			anchoredObject.transform.localPosition = new Vector2 (transform.localPosition.x + objectRightEdge - anchoredLeftEdge + onRightSpacing, anchoredObject.transform.localPosition.y);		// Aligns the left side of the anchored object to the right side of this object based on offsets
		}


		if (onLeft == true)
		{
			anchoredObject.transform.localPosition = new Vector2 (transform.localPosition.x + objectLeftEdge - anchoredRightEdge - onLeftSpacing, anchoredObject.transform.localPosition.y);		// Aligns the right side of the anchored object to the left side of this object based on offsets
		}
		
		
		lpx = transform.localPosition.x;				// Stores this object's current local position on the x-axis
		lpy = transform.localPosition.y;				// Stores this object's current local position on the y-axis
		w = objectWidth;								// Stores this object's current width
		h = objectHeight;								// Stores this object's current height
		aw = anchoredRectTransform.rect.width;			// Stores the anchored object's current width
		ah = anchoredRectTransform.rect.height;			// Stores the anchored object's current height
	}	


	void GetNewPosition ()				// This updates the offsets to use the current offsets
	{
		GetPosition ();					// Calls the function to get the position of the anchored object
		PlaceAnchoredObject ();			// Calls the function to move and resize the anchored object
	}
}