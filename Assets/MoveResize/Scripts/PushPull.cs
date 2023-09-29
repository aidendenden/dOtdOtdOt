using UnityEngine;

public class PushPull : MonoBehaviour {

	public GameObject otherObject;						// Defines the object to be pushed or pulled
	public bool immediatePullLeft = false;				// Determines if the ability to pull will be enabled without an initial left push
	public bool immediatePullRight = false;				// Determines if the ability to pull will be enabled without an initial right push
	public bool immediatePullTop = false;				// Determines if the ability to pull will be enabled without an initial top push
	public bool immediatePullBottom = false;			// Determines if the ability to pull will be enabled without an initial bottom push
	public bool disablePush = false;					// Determines if the ability to push is enabled or disabled
	public bool disablePull = false;					// Determines if the ability to pull is enabled or disabled
	public bool leftPush = false;						// Determines if this object's left side will push the other object right
	public bool rightPush = false;						// Determines if this object's right side will push the other object left
	public bool topPush = false;						// Determines if this object's top side will push the other object down
	public bool bottomPush = false;						// Determines if this object's bottom side will push the other object up
	public bool leftPushLeft = false;					// Determines if this object's left side will push the other object's left's side right
	public bool rightPushRight = false;					// Determines if this object's right side will push the other object's right side left
	public bool topPushTop = false;						// Determines if this object's top side will push the other object's top side down
	public bool bottomPushBottom = false;				// Determines if this object's bottom side will push the other object's bottom side up
	public bool leftPushRight = false;					// Determines if this object's left side will push the other object's right side right
	public bool rightPushLeft = false;					// Determines if this object's right side will push the other object's left side left
	public bool topPushBottom = false;					// Determines if this object's top side will push the other object's bottom side down
	public bool bottomPushTop = false;					// Determines if this object's bottom side will push the other object's top side up
	public bool leftPull = false;						// Determines if this object's left side will pull the other object left
	public bool rightPull = false;						// Determines if this object's right side will pull the other object right
	public bool topPull = false;						// Determines if this object's top side will pull the other object up
	public bool bottomPull = false;						// Determines if this object's bottom side will pull the other object down
	public bool leftPullLeft = false;					// Determines if this object's left side will pull the other object's left's side left
	public bool rightPullRight = false;					// Determines if this object's right side will pull the other object's right side right
	public bool topPullTop = false;						// Determines if this object's top side will pull the other object's top side up
	public bool bottomPullBottom = false;				// Determines if this object's bottom side will pull the other object's bottom side down
	public bool leftPullRight = false;					// Determines if this object's left side will pull the other object's right side left
	public bool rightPullLeft = false;					// Determines if this object's right side will pull the other object's left side right
	public bool topPullBottom = false;					// Determines if this object's top side will pull the other object's bottom side up
	public bool bottomPullTop = false;					// Determines if this object's bottom side will pull the other object's top side down
	public float correctDistance = 0;					// Sets the distance from the specific side of this object the specific side of the other object at which the selected movements should begin to take place
	public float xMaximumPush = 0;						// Sets the maximum amount this object may push the other object on the x-axis
	public float yMaximumPush = 0;						// Sets the maximum amount this object may push the other object on the y-axis
	public float xMaximumPull = 0;						// Sets the maximum amount this object may pull the other object on the x-axis
	public float yMaximumPull = 0;						// Sets the maximum amount this object may pull the other object on the y-axis
	bool pushedLeft = false;							// Determines if this object has already pushed the other object from the left. This keep pulling from beginning until pushing has begun
	bool pushedRight = false;							// Determines if this object has already pushed the other object from the right. This keep pulling from beginning until pushing has begun
	bool pushedTop = false;								// Determines if this object has already pushed the other object from the top. This keep pulling from beginning until pushing has begun
	bool pushedBottom = false;							// Determines if this object has already pushed the other object from the bottom. This keep pulling from beginning until pushing has begun

	void Start ()
	{
		if (otherObject == null)
		{
			Debug.Log ("You have not assigned an object to push/pull to: " + transform.name);				// This only happens if there is no object assigned to "otherObject"
		}


		if (immediatePullLeft == true)
		{
			pushedLeft = true;			// This allows the other object to be pulled
		}


		if (immediatePullRight == true)
		{
			pushedRight = true;			// This allows the other object to be pulled
		}


		if (immediatePullTop == true)
		{
			pushedTop = true;			// This allows the other object to be pulled
		}


		if (immediatePullBottom == true)
		{
			pushedBottom = true;		// This allows the other object to be pulled
		}
	}


	void LateUpdate ()
	{
		if (disablePush == false && otherObject != null)
		{
			if (disablePush == false)
			{
				Push ();				// Calls the function to push the other object
			}
		}

		if (disablePull == false && otherObject != null)
		{
			if (disablePull == false)
			{
				Pull ();				// Calls the function to pull the other object
			}
		}
	}


	void Push ()		// This function pushes the other object
	{
		RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();							// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
		float width = objectRectTransform.rect.width;
		float height = objectRectTransform.rect.height;
		float rightOuterBorder = (width * .5f);
		float leftOuterBorder = (width * -.5f);
		float topOuterBorder = (height * .5f);
		float bottomOuterBorder = (height * -.5f);
		float leftEdge = transform.position.x + leftOuterBorder;
		float rightEdge = transform.position.x + rightOuterBorder;
		float bottomEdge = transform.position.y + bottomOuterBorder;
		float topEdge = transform.position.y + topOuterBorder;

		RectTransform otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();			// This section gets the RectTransform information from the other object. Height and width are stored in variables. The borders of the object are also defined
		float otherWidth = otherObjectRectTransform.rect.width;
		float otherHeight = otherObjectRectTransform.rect.height;
		float otherRightOuterBorder = (otherWidth * .5f);
		float otherLeftOuterBorder = (otherWidth * -.5f);
		float otherTopOuterBorder = (otherHeight * .5f);
		float otherBottomOuterBorder = (otherHeight * -.5f);
		Transform otherObjectTransform = otherObject.GetComponent<Transform> ();
		float otherLeftEdge = otherObjectTransform.position.x + otherLeftOuterBorder;
		float otherRightEdge = otherObjectTransform.position.x + otherRightOuterBorder;
		float otherBottomEdge = otherObjectTransform.position.y + otherBottomOuterBorder;
		float otherTopEdge = otherObjectTransform.position.y + otherTopOuterBorder;

		if (leftPush == true)
		{
			if (correctDistance > otherLeftEdge - leftEdge && rightEdge - otherRightEdge > xMaximumPush)
			{
				float movement = correctDistance - (otherLeftEdge - leftEdge);																				// Sets the amount of movement the other object should be pushed
				otherObjectTransform.position = new Vector2 (otherObjectTransform.position.x + movement, otherObjectTransform.position.y);					// Pushes the other object
				pushedLeft = true;																															// This allows the other object to be pulled

				objectRectTransform = gameObject.GetComponent<RectTransform> ();								// This section gets the RectTransform information from this object. Width is stored in a variable. The borders of the object are also defined	
				width = objectRectTransform.rect.width;
				rightOuterBorder = (width * .5f);
				rightEdge = transform.position.x + rightOuterBorder;
				
				otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();				// This section gets the RectTransform information from the other object. Width is stored in a variable. The borders of the object are also defined	
				otherWidth = otherObjectRectTransform.rect.width;
				otherRightOuterBorder = (otherWidth * .5f);
				otherObjectTransform = otherObject.GetComponent<Transform> ();
				otherRightEdge = otherObjectTransform.position.x + otherRightOuterBorder;
				
				if (rightEdge - otherRightEdge < xMaximumPush)
				{
					otherObjectTransform.position = new Vector2 (rightEdge - xMaximumPush - otherRightOuterBorder, otherObjectTransform.position.y);		// This corrects the placement of the other object if it was pushed too far
				}
			}
		}


		if (rightPush == true)
		{
			if (correctDistance > rightEdge - otherRightEdge && otherLeftEdge - leftEdge > xMaximumPush)
			{
				float movement = (rightEdge - otherRightEdge) - correctDistance;																			// Sets the amount of movement the other object should be pushed
				otherObjectTransform.position = new Vector2 (otherObjectTransform.position.x + movement, otherObjectTransform.position.y);					// Pushes the other object
				pushedRight = true;																															// This allows the other object to be pulled

				objectRectTransform = gameObject.GetComponent<RectTransform> ();								// This section gets the RectTransform information from this object. Width is stored in a variable. The borders of the object are also defined									
				width = objectRectTransform.rect.width;
				leftOuterBorder = (width * -.5f);
				leftEdge = transform.position.x + leftOuterBorder;
				
				otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();				// This section gets the RectTransform information from the other object. Width is stored in a variable. The borders of the object are also defined	
				otherWidth = otherObjectRectTransform.rect.width;
				otherLeftOuterBorder = (otherWidth * -.5f);
				otherObjectTransform = otherObject.GetComponent<Transform> ();
				otherLeftEdge = otherObjectTransform.position.x + otherLeftOuterBorder;
				
				if (otherLeftEdge - leftEdge < xMaximumPush)
				{
					otherObjectTransform.position = new Vector2 (leftEdge + xMaximumPush - otherLeftOuterBorder, otherObjectTransform.position.y);			// This corrects the placement of the other object if it was pushed too far
				}
			}
		}


		if (topPush == true)
		{
			if (correctDistance > topEdge - otherTopEdge && otherBottomEdge - bottomEdge > yMaximumPush)
			{
				float movement = (topEdge - otherTopEdge) - correctDistance;																				// Sets the amount of movement the other object should be pushed
				otherObjectTransform.position = new Vector2 (otherObjectTransform.position.x, otherObjectTransform.position.y + movement);					// Pushes the other object
				pushedTop = true;																															// This allows the other object to be pulled

				objectRectTransform = gameObject.GetComponent<RectTransform> ();								// This section gets the RectTransform information from this object. Height is stored in a variable. The borders of the object are also defined												
				height = objectRectTransform.rect.height;
				bottomOuterBorder = (height * -.5f);
				bottomEdge = transform.position.y + bottomOuterBorder;
				
				otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();				// This section gets the RectTransform information from the other object. Height is stored in a variable. The borders of the object are also defined								
				otherHeight = otherObjectRectTransform.rect.height;
				otherBottomOuterBorder = (otherHeight * -.5f);
				otherObjectTransform = otherObject.GetComponent<Transform> ();
				otherBottomEdge = otherObjectTransform.position.y + otherBottomOuterBorder;
				
				if (otherBottomEdge - bottomEdge < yMaximumPush)
				{
					otherObjectTransform.position = new Vector2 (otherObjectTransform.position.x, bottomEdge + yMaximumPush - otherBottomOuterBorder);		// This corrects the placement of the other object if it was pushed too far
				}
			}
		}


		if (bottomPush == true)
		{
			if (correctDistance > otherBottomEdge - bottomEdge && topEdge - otherTopEdge > yMaximumPush)
			{
				float movement = correctDistance - (otherBottomEdge - bottomEdge);																			// Sets the amount of movement the other object should be pushed
				otherObjectTransform.position = new Vector2 (otherObjectTransform.position.x, otherObjectTransform.position.y + movement);					// Pushes the other object
				pushedBottom = true;																														// This allows the other object to be pulled

				objectRectTransform = gameObject.GetComponent<RectTransform> ();								// This section gets the RectTransform information from this object. Height is stored in a variable. The borders of the object are also defined								
				height = objectRectTransform.rect.height;
				topOuterBorder = (height * .5f);
				topEdge = transform.position.y + topOuterBorder;
				
				otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();				// This section gets the RectTransform information from the other object. Height is stored in a variable. The borders of the object are also defined									
				otherHeight = otherObjectRectTransform.rect.height;
				otherTopOuterBorder = (otherHeight * .5f);
				otherObjectTransform = otherObject.GetComponent<Transform> ();
				otherTopEdge = otherObjectTransform.position.y + otherTopOuterBorder;
				
				if (topEdge - otherTopEdge < yMaximumPush)
				{
					otherObjectTransform.position = new Vector2 (otherObjectTransform.position.x, topEdge - yMaximumPush - otherTopOuterBorder);			// This corrects the placement of the other object if it was pushed too far
				}
			}
		}


		if (leftPushLeft == true)
		{
			if (correctDistance > otherLeftEdge - leftEdge && otherWidth > xMaximumPush)
			{
				float movement = correctDistance - (otherLeftEdge - leftEdge);																				// Sets the amount of movement the other object should be pushed
				otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMin.x + movement, otherObjectRectTransform.offsetMin.y);	// Pushes the other object
				pushedLeft = true;																															// This allows the other object to be pulled

				otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();				// Gets the RectTransform information from the other object
				
				if (otherObjectRectTransform.offsetMax.x - otherObjectRectTransform.offsetMin.x < xMaximumPush)
				{
					otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMax.x - xMaximumPush, otherObjectRectTransform.offsetMin.y);		// This corrects the placement of the other object if it was pushed too far
				}
			}
		}


		if (rightPushRight == true)
		{
			if (correctDistance > rightEdge - otherRightEdge && otherWidth > xMaximumPush)
			{
				float movement = (rightEdge - otherRightEdge) - correctDistance;																			// Sets the amount of movement the other object should be pushed
				otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMax.x + movement, otherObjectRectTransform.offsetMax.y);	// Pushes the other object
				pushedRight = true;																															// This allows the other object to be pulled

				otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();				// Gets the RectTransform information from the other object
				
				if (otherObjectRectTransform.offsetMax.x - otherObjectRectTransform.offsetMin.x < xMaximumPush)
				{
					otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMin.x + xMaximumPush, otherObjectRectTransform.offsetMax.y);		// This corrects the placement of the other object if it was pushed too far
				}
			}
		}


		if (topPushTop == true)
		{
			if (correctDistance > topEdge - otherTopEdge && otherHeight > yMaximumPush)
			{
				float movement = (topEdge - otherTopEdge) - correctDistance;																				// Sets the amount of movement the other object should be pushed
				otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMax.x, otherObjectRectTransform.offsetMax.y + movement);	// Pushes the other object
				pushedTop = true;																															// This allows the other object to be pulled
				
				otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();				// Gets the RectTransform information from the other object
				
				if (otherObjectRectTransform.offsetMax.y - otherObjectRectTransform.offsetMin.y < yMaximumPush)
				{
					otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMax.x, otherObjectRectTransform.offsetMin.y + yMaximumPush);
				}
			}
		}


		if (bottomPushBottom == true)
		{
			if (correctDistance > otherBottomEdge - bottomEdge && otherHeight > yMaximumPush)
			{
				float movement = correctDistance - (otherBottomEdge - bottomEdge);																			// Sets the amount of movement the other object should be pushed
				otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMin.x, otherObjectRectTransform.offsetMin.y + movement);	// Pushes the other object
				pushedBottom = true;																														// This allows the other object to be pulled
				
				otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();				// Gets the RectTransform information from the other object
				
				if (otherObjectRectTransform.offsetMax.y - otherObjectRectTransform.offsetMin.y < yMaximumPush)
				{
					otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMin.x, otherObjectRectTransform.offsetMax.y - yMaximumPush);		// This corrects the placement of the other object if it was pushed too far
				}
			}
		}


		if (leftPushRight == true)
		{
			if (correctDistance > otherRightEdge - leftEdge && rightEdge - otherRightEdge > xMaximumPush)
			{
				float movement = correctDistance - (otherRightEdge - leftEdge);																				// Sets the amount of movement the other object should be pushed
				otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMax.x + movement, otherObjectRectTransform.offsetMax.y);	// Pushes the other object
				pushedLeft = true;																															// This allows the other object to be pulled

				objectRectTransform = gameObject.GetComponent<RectTransform> ();								// This section gets the RectTransform information from this object. Width is stored in a variable. The borders of the object are also defined					
				width = objectRectTransform.rect.width;
				rightOuterBorder = (width * .5f);
				rightEdge = transform.position.x + rightOuterBorder;
				
				otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();				// This section gets the RectTransform information from the other object. Width is stored in a variable. The borders of the object are also defined	
				otherWidth = otherObjectRectTransform.rect.width;
				otherLeftOuterBorder = (otherWidth * -.5f);
				otherRightOuterBorder = (otherWidth * .5f);
				otherObjectTransform = otherObject.GetComponent<Transform> ();
				otherLeftEdge = otherObjectTransform.position.x + otherLeftOuterBorder;
				otherRightEdge = otherObjectTransform.position.x + otherRightOuterBorder;
				
				if (rightEdge - otherRightEdge < xMaximumPush)
				{
					otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMin.x + (rightEdge - otherLeftEdge) - xMaximumPush, otherObjectRectTransform.offsetMax.y);		// This corrects the placement of the other object if it was pushed too far
				}
			}
		}


		if (rightPushLeft == true)
		{
			if (correctDistance > rightEdge - otherLeftEdge && otherLeftEdge - leftEdge > xMaximumPush)
			{
				float movement = (rightEdge - otherLeftEdge) - correctDistance;																				// Sets the amount of movement the other object should be pushed	
				otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMin.x + movement, otherObjectRectTransform.offsetMin.y);	// Pushes the other object
				pushedRight = true;																															// This allows the other object to be pulled

				objectRectTransform = gameObject.GetComponent<RectTransform> ();								// This section gets the RectTransform information from this object. Width is stored in a variable. The borders of the object are also defined												
				width = objectRectTransform.rect.width;
				leftOuterBorder = (width * -.5f);
				leftEdge = transform.position.x + leftOuterBorder;
				
				otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();				// This section gets the RectTransform information from the other object. Width is stored in a variable. The borders of the object are also defined	
				otherWidth = otherObjectRectTransform.rect.width;
				otherLeftOuterBorder = (otherWidth * -.5f);
				otherRightOuterBorder = (otherWidth * .5f);
				otherObjectTransform = otherObject.GetComponent<Transform> ();
				otherLeftEdge = otherObjectTransform.position.x + otherLeftOuterBorder;
				otherRightEdge = otherObjectTransform.position.x + otherRightOuterBorder;
				
				if (otherLeftEdge - leftEdge < xMaximumPush)
				{
					otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMax.x - (otherRightEdge - leftEdge) + xMaximumPush, otherObjectRectTransform.offsetMin.y);		// This corrects the placement of the other object if it was pushed too far
				}
			}
		}


		if (topPushBottom == true)
		{
			if (correctDistance > topEdge - otherBottomEdge && otherBottomEdge - bottomEdge > yMaximumPush)
			{
				float movement = (topEdge - otherBottomEdge) - correctDistance;																				// Sets the amount of movement the other object should be pushed
				otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMin.x, otherObjectRectTransform.offsetMin.y+ movement);	// Pushes the other object
				pushedTop = true;																															// This allows the other object to be pulled

				objectRectTransform = gameObject.GetComponent<RectTransform> ();								// This section gets the RectTransform information from this object. Height is stored in a variable. The borders of the object are also defined								
				height = objectRectTransform.rect.height;
				bottomOuterBorder = (height * -.5f);
				bottomEdge = transform.position.y + bottomOuterBorder;

				otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();				// This section gets the RectTransform information from the other object. Height is stored in a variable. The borders of the object are also defined								
				otherHeight = otherObjectRectTransform.rect.height;
				otherTopOuterBorder = (otherHeight * .5f);
				otherBottomOuterBorder = (otherHeight * -.5f);
				otherObjectTransform = otherObject.GetComponent<Transform> ();
				otherTopEdge = otherObjectTransform.position.y + otherTopOuterBorder;
				otherBottomEdge = otherObjectTransform.position.y + otherBottomOuterBorder;

				if (otherBottomEdge - bottomEdge < yMaximumPush)
				{
					otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMin.x, otherObjectRectTransform.offsetMax.y - (otherTopEdge - bottomEdge) + yMaximumPush);		// This corrects the placement of the other object if it was pushed too far
				}
			}
		}


		if (bottomPushTop == true)
		{
			if (correctDistance > otherTopEdge - bottomEdge && topEdge - otherTopEdge > yMaximumPush)
			{
				float movement = correctDistance - (otherTopEdge - bottomEdge);																				// Sets the amount of movement the other object should be pushed
				otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMax.x, otherObjectRectTransform.offsetMax.y + movement);	// Pushes the other object
				pushedBottom = true;																														// This allows the other object to be pulled

				objectRectTransform = gameObject.GetComponent<RectTransform> ();								// This section gets the RectTransform information from this object. Height is stored in a variable. The borders of the object are also defined								
				height = objectRectTransform.rect.height;
				topOuterBorder = (height * .5f);
				topEdge = transform.position.y + topOuterBorder;
				
				otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();				// This section gets the RectTransform information from the other object. Height is stored in a variable. The borders of the object are also defined								
				otherHeight = otherObjectRectTransform.rect.height;
				otherTopOuterBorder = (otherHeight * .5f);
				otherBottomOuterBorder = (otherHeight * -.5f);
				otherObjectTransform = otherObject.GetComponent<Transform> ();
				otherTopEdge = otherObjectTransform.position.y + otherTopOuterBorder;
				otherBottomEdge = otherObjectTransform.position.y + otherBottomOuterBorder;

				if (topEdge - otherTopEdge < yMaximumPush)
				{
					otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMax.x, otherObjectRectTransform.offsetMin.y + (topEdge - otherBottomEdge) - yMaximumPush);		// This corrects the placement of the other object if it was pushed too far
				}
			}
		}
	}


	void Pull ()		// This function pulls the other object
	{
		RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();							// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
		float width = objectRectTransform.rect.width;
		float height = objectRectTransform.rect.height;
		float rightOuterBorder = (width * .5f);
		float leftOuterBorder = (width * -.5f);
		float topOuterBorder = (height * .5f);
		float bottomOuterBorder = (height * -.5f);
		float leftEdge = transform.position.x + leftOuterBorder;
		float rightEdge = transform.position.x + rightOuterBorder;
		float bottomEdge = transform.position.y + bottomOuterBorder;
		float topEdge = transform.position.y + topOuterBorder;
		
		RectTransform otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();			// This section gets the RectTransform information from the other object. Height and width are stored in variables. The borders of the object are also defined
		float otherWidth = otherObjectRectTransform.rect.width;
		float otherHeight = otherObjectRectTransform.rect.height;
		float otherRightOuterBorder = (otherWidth * .5f);
		float otherLeftOuterBorder = (otherWidth * -.5f);
		float otherTopOuterBorder = (otherHeight * .5f);
		float otherBottomOuterBorder = (otherHeight * -.5f);
		Transform otherObjectTransform = otherObject.GetComponent<Transform> ();
		float otherLeftEdge = otherObjectTransform.position.x + otherLeftOuterBorder;
		float otherRightEdge = otherObjectTransform.position.x + otherRightOuterBorder;
		float otherBottomEdge = otherObjectTransform.position.y + otherBottomOuterBorder;
		float otherTopEdge = otherObjectTransform.position.y + otherTopOuterBorder;

		if (leftPull == true && pushedLeft == true)
		{
			if (correctDistance < otherLeftEdge - leftEdge && rightEdge - otherRightEdge < xMaximumPull)
			{
				float movement = correctDistance - (otherLeftEdge - leftEdge);																					// Sets the amount of movement the other object should be pulled
				otherObjectTransform.position = new Vector2 (otherObjectTransform.position.x + movement, otherObjectTransform.position.y);

				objectRectTransform = gameObject.GetComponent<RectTransform> ();								// This section gets the RectTransform information from this object. Width is stored in a variable. The borders of the object are also defined
				width = objectRectTransform.rect.width;
				rightOuterBorder = (width * .5f);
				rightEdge = transform.position.x + rightOuterBorder;
							
				otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();				// This section gets the RectTransform information from the other object. Width is stored in a variable. The borders of the object are also defined	
				otherWidth = otherObjectRectTransform.rect.width;
				otherRightOuterBorder = (otherWidth * .5f);
				otherObjectTransform = otherObject.GetComponent<Transform> ();
				otherRightEdge = otherObjectTransform.position.x + otherRightOuterBorder;

				if (rightEdge - otherRightEdge > xMaximumPull)
				{
					otherObjectTransform.position = new Vector2 (rightEdge - xMaximumPull - otherRightOuterBorder, otherObjectTransform.position.y);			// This corrects the placement of the other object if it was pulled too far
					pushedLeft = false;
				}
			}
		}


		if (rightPull == true && pushedRight == true)
		{
			if (correctDistance < rightEdge - otherRightEdge && otherLeftEdge - leftEdge < xMaximumPull)
			{
				float movement = (rightEdge - otherRightEdge) - correctDistance;																				// Sets the amount of movement the other object should be pulled
				otherObjectTransform.position = new Vector2 (otherObjectTransform.position.x + movement, otherObjectTransform.position.y);						// Pulls the other object

				objectRectTransform = gameObject.GetComponent<RectTransform> ();								// This section gets the RectTransform information from this object. Width is stored in a variable. The borders of the object are also defined							
				width = objectRectTransform.rect.width;
				leftOuterBorder = (width * -.5f);
				leftEdge = transform.position.x + leftOuterBorder;

				otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();				// This section gets the RectTransform information from the other object. Width is stored in a variable. The borders of the object are also defined	
				otherWidth = otherObjectRectTransform.rect.width;
				otherLeftOuterBorder = (otherWidth * -.5f);
				otherObjectTransform = otherObject.GetComponent<Transform> ();
				otherLeftEdge = otherObjectTransform.position.x + otherLeftOuterBorder;

				if (otherLeftEdge - leftEdge > xMaximumPull)
				{
					otherObjectTransform.position = new Vector2 (leftEdge + xMaximumPull - otherLeftOuterBorder, otherObjectTransform.position.y);				// This corrects the placement of the other object if it was pulled too far
					pushedRight = false;
				}
			}
		}


		if (topPull == true && pushedTop == true) 																									
		{
			if (correctDistance < topEdge - otherTopEdge && otherBottomEdge - bottomEdge < yMaximumPull)
			{
				float movement = (topEdge - otherTopEdge) - correctDistance;																					// Sets the amount of movement the other object should be pulled
				otherObjectTransform.position = new Vector2 (otherObjectTransform.position.x, otherObjectTransform.position.y + movement);						// Pulls the other object

				objectRectTransform = gameObject.GetComponent<RectTransform> ();								// This section gets the RectTransform information from this object. Height is stored in a variable. The borders of the object are also defined							
				height = objectRectTransform.rect.height;
				bottomOuterBorder = (height * -.5f);
				bottomEdge = transform.position.y + bottomOuterBorder;
		
				otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();				// This section gets the RectTransform information from the other object. Height is stored in a variable. The borders of the object are also defined	
				otherHeight = otherObjectRectTransform.rect.height;
				otherBottomOuterBorder = (otherHeight * -.5f);
				otherObjectTransform = otherObject.GetComponent<Transform> ();
				otherBottomEdge = otherObjectTransform.position.y + otherBottomOuterBorder;

				if (otherBottomEdge - bottomEdge > yMaximumPull)
				{
					otherObjectTransform.position = new Vector2 (otherObjectTransform.position.x, bottomEdge + yMaximumPull - otherBottomOuterBorder);			// This corrects the placement of the other object if it was pulled too far
					pushedTop = false;
				}
			}
		}


		if (bottomPull == true && pushedBottom == true)
		{
			if (correctDistance < otherBottomEdge - bottomEdge && topEdge - otherTopEdge < yMaximumPull)
			{
				float movement = correctDistance - (otherBottomEdge - bottomEdge);																				// Sets the amount of movement the other object should be pulled
				otherObjectTransform.position = new Vector2 (otherObjectTransform.position.x, otherObjectTransform.position.y + movement);						// Pulls the other object

				objectRectTransform = gameObject.GetComponent<RectTransform> ();								// This section gets the RectTransform information from this object. Height is stored in a variable. The borders of the object are also defined								
				height = objectRectTransform.rect.height;
				topOuterBorder = (height * .5f);
				topEdge = transform.position.y + topOuterBorder;

				otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();				// This section gets the RectTransform information from the other object. Height is stored in a variable. The borders of the object are also defined
				otherHeight = otherObjectRectTransform.rect.height;
				otherTopOuterBorder = (otherHeight * .5f);
				otherObjectTransform = otherObject.GetComponent<Transform> ();
				otherTopEdge = otherObjectTransform.position.y + otherTopOuterBorder;

				if (topEdge - otherTopEdge > yMaximumPull)
				{
					otherObjectTransform.position = new Vector2 (otherObjectTransform.position.x, topEdge - yMaximumPull - otherTopOuterBorder);				// This corrects the placement of the other object if it was pulled too far
					pushedBottom = false;
				}
			}
		}
		
		
		if (leftPullLeft == true && pushedLeft == true)
		{
			if (correctDistance < otherLeftEdge - leftEdge && otherWidth < xMaximumPull)
			{
				float movement = correctDistance - (otherLeftEdge - leftEdge);																					// Sets the amount of movement the other object should be pulled
				otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMin.x + movement, otherObjectRectTransform.offsetMin.y);		// Pulls the other object

				otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();				// Gets the RectTransform information from the other object

				if (otherObjectRectTransform.offsetMax.x - otherObjectRectTransform.offsetMin.x > xMaximumPull)
				{
					otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMax.x - xMaximumPull, otherObjectRectTransform.offsetMin.y);			// This corrects the placement of the other object if it was pulled too far
					pushedLeft = false;
				}
			}
		}


		if (rightPullRight == true && pushedRight == true)
		{
			if (correctDistance < rightEdge - otherRightEdge && otherWidth < xMaximumPull)
			{
				float movement = (rightEdge - otherRightEdge) - correctDistance;																				// Sets the amount of movement the other object should be pulled
				otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMax.x + movement, otherObjectRectTransform.offsetMax.y);		// Pulls the other object

				otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();				// Gets the RectTransform information from the other object

				if (otherObjectRectTransform.offsetMax.x - otherObjectRectTransform.offsetMin.x > xMaximumPull)
				{
					otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMin.x + xMaximumPull, otherObjectRectTransform.offsetMax.y);			// This corrects the placement of the other object if it was pulled too far
					pushedRight = false;
				}
			}
		}


		if (topPullTop == true && pushedTop == true)
		{
			if (correctDistance < topEdge - otherTopEdge && otherHeight < yMaximumPull)
			{
				float movement = (topEdge - otherTopEdge) - correctDistance;																					// Sets the amount of movement the other object should be pulled
				otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMax.x, otherObjectRectTransform.offsetMax.y + movement);		// Pulls the other object

				otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();				// Gets the RectTransform information from the other object

				if (otherObjectRectTransform.offsetMax.y - otherObjectRectTransform.offsetMin.y > yMaximumPull)
				{
					otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMax.x, otherObjectRectTransform.offsetMin.y + yMaximumPull);			// This corrects the placement of the other object if it was pulled too far
					pushedTop = false;
				}
			}
		}


		if (bottomPullBottom == true && pushedBottom == true)
		{
			if (correctDistance < otherBottomEdge - bottomEdge && otherHeight < yMaximumPull)
			{
				float movement = correctDistance - (otherBottomEdge - bottomEdge);																				// Sets the amount of movement the other object should be pulled
				otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMin.x, otherObjectRectTransform.offsetMin.y + movement);		// Pulls the other object

				otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();				// Gets the RectTransform information from the other object

				if (otherObjectRectTransform.offsetMax.y - otherObjectRectTransform.offsetMin.y > yMaximumPull)
				{
					otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMin.x, otherObjectRectTransform.offsetMax.y - yMaximumPull);			// This corrects the placement of the other object if it was pulled too far
					pushedBottom = false;
				}
			}
		}
		
		
		if (leftPullRight == true && pushedLeft == true)
		{
			if (correctDistance < otherRightEdge - leftEdge && otherWidth > xMaximumPull)
			{
				float movement = correctDistance - (otherRightEdge - leftEdge);																					// Sets the amount of movement the other object should be pulled
				otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMax.x + movement, otherObjectRectTransform.offsetMax.y);		// Pulls the other object

				otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();				// Gets the RectTransform information from the other object

				if (otherObjectRectTransform.offsetMax.x - otherObjectRectTransform.offsetMin.x < xMaximumPull )
				{
					otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMin.x + xMaximumPull, otherObjectRectTransform.offsetMax.y);			// This corrects the placement of the other object if it was pulled too far
					pushedLeft = false;
				}
			}
		}


		if (rightPullLeft == true && pushedRight == true)
		{
			if (correctDistance < rightEdge - otherLeftEdge && otherWidth > xMaximumPull)
			{
				float movement = (rightEdge - otherLeftEdge) - correctDistance;																					// Sets the amount of movement the other object should be pulled
				otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMin.x + movement, otherObjectRectTransform.offsetMin.y);		// Pulls the other object

				otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();				// Gets the RectTransform information from the other object

				if (otherObjectRectTransform.offsetMax.x - otherObjectRectTransform.offsetMin.x < xMaximumPull )
				{
					otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMax.x - xMaximumPull, otherObjectRectTransform.offsetMin.y);			// This corrects the placement of the other object if it was pulled too far
					pushedRight = false;
				}
			}
		}


		if (topPullBottom == true && pushedTop == true)
		{
			if (correctDistance < topEdge - otherBottomEdge && otherHeight > yMaximumPull)
			{
				float movement = (topEdge - otherBottomEdge) - correctDistance;																					// Sets the amount of movement the other object should be pulled
				otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMin.x, otherObjectRectTransform.offsetMin.y+ movement);		// Pulls the other object

				otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();				// Gets the RectTransform information from the other object

				if (otherObjectRectTransform.offsetMax.y - otherObjectRectTransform.offsetMin.y < yMaximumPull)
				{
					otherObjectRectTransform.offsetMin = new Vector2 (otherObjectRectTransform.offsetMin.x, otherObjectRectTransform.offsetMax.y - yMaximumPull);			// This corrects the placement of the other object if it was pulled too far
					pushedTop = false;
				}
			}
		}


		if (bottomPullTop == true && pushedBottom == true)
		{
			if (correctDistance < otherTopEdge - bottomEdge && otherHeight > yMaximumPull)
			{
				float movement = correctDistance - (otherTopEdge - bottomEdge);																					// Sets the amount of movement the other object should be pulled
				otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMax.x, otherObjectRectTransform.offsetMax.y + movement);		// Pulls the other object

				otherObjectRectTransform = otherObject.gameObject.GetComponent<RectTransform> ();				// Gets the RectTransform information from the other object

				if (otherObjectRectTransform.offsetMax.y - otherObjectRectTransform.offsetMin.y < yMaximumPull)
				{
					otherObjectRectTransform.offsetMax = new Vector2 (otherObjectRectTransform.offsetMax.x, otherObjectRectTransform.offsetMin.y + yMaximumPull);			// This corrects the placement of the other object if it was pulled too far
					pushedBottom = false;
				}
			}
		}
	}


	public void PushToggle ()										// This function toggles the ability to push on and off. Good for events that toggle
	{
		disablePush = !disablePush;
	}


	public void PullToggle ()										// This function toggles the ability to pull on and off. Good for events that toggle
	{
		disablePull = !disablePull;
	}
	
	
	public void EnablePush ()										// This function enables the ability to push. Good for events that do not toggle
	{
		if(disablePush == true)
		{
			disablePush = false;
		}
	}


	public void EnablePull ()										// This function enables the ability to pull. Good for events that do not toggle
	{
		if(disablePull == true)
		{
			disablePull = false;
		}
	}
	
	
	public void DisablePush ()										// This function disables the ability to push. Good for events that do not toggle
	{
		if(disablePush == false)
		{
			disablePush = true;
		}
	}
	
	
	public void DisablePull ()										// This function disables the ability to pull. Good for events that do not toggle
	{
		if(disablePull == false)
		{
			disablePull = true;
		}
	}
}