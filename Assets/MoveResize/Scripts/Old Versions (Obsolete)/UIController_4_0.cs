using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIController_4_0 : MonoBehaviour {

	public bool ignoreTextBox = true;					// Determines if raycasts ignore text boxes or objects with a "Text" script attached
	public bool everyFrame = false;						// Determines if this script will raycast every frame or only when the mouse button is initially pressed
	public bool customCursor = true;					// Determines if custom cursors will be used. In order for this to be active, the "everyFrame" variable must be true
	public Texture2D normalCursor;						// Defines the normal custom cursor
	public int xHotspotNormal = 0;						// Sets the hotspot poisiton for the "normal" custom cursor on the x-axis
	public int yHotspotNormal = 0;						// Sets the hotspot poisiton for the "normal" custom cursor on the y-axis
	public Texture2D moveCursor;						// Defines the custom cursor for moving
	public int xHotspotMove = 0;						// Sets the hotspot poisiton for the "move" custom cursor on the x-axis
	public int yHotspotMove = 0;						// Sets the hotspot poisiton for the "move" custom cursor on the y-axis
	public Texture2D horizontalCursor;					// Defines the custom cursor for moving when the "xOnly" variable is true
	public int xHotspotHor = 0;							// Sets the hotspot poisiton for the "horizontal" custom cursor on the x-axis
	public int yHotspotHor = 0;							// Sets the hotspot poisiton for the "horizontal" custom cursor on the y-axis
	public Texture2D verticalCursor;					// Defines the custom cursor for moving when the "yOnly" variable is true
	public int xHotspotVert = 0;						// Sets the hotspot poisiton for the "vertical" custom cursor on the x-axis
	public int yHotspotVert = 0;						// Sets the hotspot poisiton for the "vertical" custom cursor on the y-axis
	public Texture2D topLeftBottomRightCursor;			// Defines the custom cursor for resizing from the top left and bottom right borders
	public int xHotspotTLBR = 0;						// Sets the hotspot poisiton for the "topLeftBottomRight" custom cursor on the x-axis
	public int yHotspotTLBR = 0;						// Sets the hotspot poisiton for the "topLeftBottomRight" custom cursor on the y-axis
	public Texture2D topRightBottomLeftCursor;			// Defines the custom cursor for resizing from the top right and bottom left borders
	public int xHotspotTRBL = 0;						// Sets the hotspot poisiton for the "topRightBottomLeft" custom cursor on the x-axis
	public int yHotspotTRBL = 0;						// Sets the hotspot poisiton for the "topRightBottomLeft" custom cursor on the y-axis
	CursorMode cursorMode = CursorMode.Auto;			// Defines the cursor mode
	string cursorType = "normal";						// Defines the custom cursor that should be used currently
	bool buttonDown = false;							// Determines if the mouse button is currently pressed
	GameObject other;									// Defines the gameobject that the raycast has hit


	void Awake ()
	{
		SetCursor ();									// Calls the function to set the cursor
	}

	
	void Update ()
	{
		if (everyFrame == true && Input.GetMouseButton(0) == false && buttonDown == false)		// Runs every frame if the "everyFrame" variable is true, the mouse button is pressed, and the "buttonDown" variable is false
		{
			PerformRaycast ();							// Calls the function to perform a raycast
		}
		else
		{
			if (Input.GetMouseButtonDown(0))			// Runs when the mouse button is initially pressed
			{
				buttonDown = true;						
				
				PerformRaycast ();						// Calls the function to perform a raycast
			}


			if (Input.GetMouseButtonUp(0))				// Runs when the mouse button is initially released
			{
				buttonDown = false;

				if (other != null)						// Runs if there is no defined "other" gameobject
				{
					End ();								// Calls the function to end an action
				}
			}
		}
	}


	void PerformRaycast ()			// This function performs a raycast to detect if this object or its child is in front of other objects if any overlap at the cursor's position when the mouse button is initially pressed. If so, the object can be set as the last sibling in the hierarchy
	{
		PointerEventData cursor = new PointerEventData(EventSystem.current);										// This section prepares a list for all objects hit with the raycast
		cursor.position = Input.mousePosition;
		List<RaycastResult> objectsHit = new List<RaycastResult> ();
		EventSystem.current.RaycastAll(cursor, objectsHit);
		int count = objectsHit.Count;
		int x = 0;
		
		if (count != 0)																								// Runs if the raycast hit at least one object
		{
			if (objectsHit[x].gameObject.GetComponent<DisableMoveResize>() == true)									// Runs if the the hit object has a "DisableMoveResize" script attached
			{
				cursorType = "normal";																				// Sets the custom cursor that should be used to "normal"

				SetCursor ();																						// Calls the function to set the cursor
					
				return;																								// Returns early from this function
			}

			if ((objectsHit[x].gameObject.GetComponent("Text") == true && ignoreTextBox == true) || objectsHit[x].gameObject.GetComponent("Ignore") == true)				// Runs if the the hit object is a text box and the "ignoreTextBox" variable is true, or if it has an "Ignore" script attatched
			{
				while ((objectsHit[x].gameObject.GetComponent("Text") == true && ignoreTextBox == true) || objectsHit[x].gameObject.GetComponent("Ignore") == true)			// Runs while the the hit object is a text box and the "ignoreTextBox" variable is true, or if it has an "Ignore" script attatched
				{
					x++;																							// Selects the next object hit by the raycast
					
					if (x == count || objectsHit[x].gameObject.GetComponent<DisableMoveResize>() == true)			// Runs if there is no other object hit by the raycast or if the hit object has a "DisableMoveResize" script attached
					{
						cursorType = "normal";																		// Sets the custom cursor that should be used to "normal"
						
						SetCursor ();																				// Calls the function to set the cursor

						return;																						// Returns early from this function
					}
				}
			}


			other = objectsHit[x].gameObject;																		// Stores the selected object

			GetScriptedParent ();																					// Calls the function to get the parent of the selected object

			if (buttonDown == false)																				// Runs if the mouse button is not currently pressed
			{
				Pass ();																							// Calls the function for passive effects
			}
			else																									// Runs if the mouse button is currently pressed	
			{
				Activate ();																						// Calls the function for active effects
			}
		}
		else																										// Runs if the raycast did not hit at least one object	
		{
			cursorType = "normal";																					// Sets the custom cursor that should be used to "normal"

			SetCursor ();																							// Calls the function to set the cursor
		}
	}


	void GetScriptedParent ()		// This function gets the selected options of the scripted (grand)parent of the selected object and affects the selected object accordingly
	{
		// The following line determines if the selected object has a specific script. If not, the section continues to run
		if (other.gameObject.GetComponent<Move>() == false && other.gameObject.GetComponent<MoveParent>() == false && other.gameObject.GetComponent<MoveOther>() == false && other.gameObject.GetComponent<Resize>() == false && other.gameObject.GetComponent<ResizeParent>() == false && other.gameObject.GetComponent<ResizeOther>() == false && other.gameObject.GetComponent<BringToFront>() == false && other.gameObject.GetComponent<Canvas>() == false)
		{
			GameObject otherParent = other.gameObject.transform.parent.gameObject;									// Stores the parent of the selected object

			// The following line determines if the selected object's parent has a specific script attached. If not, it will check the (great) grandparents until it finds one or there is none remaining
			while (otherParent.gameObject.GetComponent<Move>() == false && otherParent.gameObject.GetComponent<MoveParent>() == false && otherParent.gameObject.GetComponent<MoveOther>() == false && otherParent.gameObject.GetComponent<Resize>() == false && otherParent.gameObject.GetComponent<ResizeParent>() == false && otherParent.gameObject.GetComponent<ResizeOther>() == false && otherParent.gameObject.GetComponent<BringToFront>() == false && otherParent.gameObject.GetComponent<Canvas>() == false)
			{
				otherParent = otherParent.transform.parent.gameObject;												// Stores the parent of the parent
			}


			bool includeChildren = false;																			// Determines if the scripted object's script's actions include its children
			bool bringToFront = false;																				// Determines if the affected object should be set as the last sibling on activation, setting it as the fore-most UI object
			bool bringToFrontOnOver = false;																		// Determines if the affected object should be set as the last sibling when the cursor is over the scripted object, setting it as the fore-most UI object
			string scriptType = "self";																				// Determines if the scripted object affects itself, its parent, or another object
			GameObject otherOther = null;																			// Defines the object the scripted object's script affects

			if (otherParent.gameObject.GetComponent<Canvas>() == false)												// Runs if the selected (grand)parent is not a canvas
			{
				// The following statement sections determine which specific script the selected (grand)parent has attached, then sets the variables to match the script's selected options
				if (otherParent.gameObject.GetComponent<Move>() == true)
				{
					includeChildren = otherParent.gameObject.GetComponent<Move>().includeChildren;
					bringToFront = otherParent.gameObject.GetComponent<Move>().bringToFront;
					bringToFrontOnOver = otherParent.gameObject.GetComponent<Move>().bringToFrontOnOver;
					scriptType = "self";
				}
				else if (otherParent.gameObject.GetComponent<MoveParent>() == true)
				{
					includeChildren = otherParent.gameObject.GetComponent<MoveParent>().includeChildren;
					bringToFront = otherParent.gameObject.GetComponent<MoveParent>().bringToFront;
					bringToFrontOnOver = otherParent.gameObject.GetComponent<MoveParent>().bringToFrontOnOver;
					scriptType = "parent";
				}
				else if (otherParent.gameObject.GetComponent<MoveOther>() == true)
				{
					includeChildren = otherParent.gameObject.GetComponent<MoveOther>().includeChildren;
					bringToFront = otherParent.gameObject.GetComponent<MoveOther>().bringToFront;
					bringToFrontOnOver = otherParent.gameObject.GetComponent<MoveOther>().bringToFrontOnOver;
					scriptType = "other";
					otherOther = otherParent.gameObject.GetComponent<MoveOther>().otherObject;
				}
				else if (otherParent.gameObject.GetComponent<Resize>() == true)
				{
					includeChildren = otherParent.gameObject.GetComponent<Resize>().includeChildren;
					bringToFront = otherParent.gameObject.GetComponent<Resize>().bringToFront;
					bringToFrontOnOver = otherParent.gameObject.GetComponent<Resize>().bringToFrontOnOver;
					scriptType = "self";
				}
				else if (otherParent.gameObject.GetComponent<ResizeParent>() == true)
				{
					includeChildren = otherParent.gameObject.GetComponent<ResizeParent>().includeChildren;
					bringToFront = otherParent.gameObject.GetComponent<ResizeParent>().bringToFront;
					bringToFrontOnOver = otherParent.gameObject.GetComponent<ResizeParent>().bringToFrontOnOver;
					scriptType = "parent";
				}
				else if (otherParent.gameObject.GetComponent<ResizeOther>() == true)
				{
					includeChildren = otherParent.gameObject.GetComponent<ResizeOther>().includeChildren;
					bringToFront = otherParent.gameObject.GetComponent<ResizeOther>().bringToFront;
					bringToFrontOnOver = otherParent.gameObject.GetComponent<ResizeOther>().bringToFrontOnOver;
					scriptType = "other";
					otherOther = otherParent.gameObject.GetComponent<ResizeOther>().otherObject;
				}
				else if (otherParent.gameObject.GetComponent<BringToFront>() == true)
				{
					includeChildren = otherParent.gameObject.GetComponent<BringToFront>().includeChildren;
					bringToFront = otherParent.gameObject.GetComponent<BringToFront>().bringToFront;
					bringToFrontOnOver = otherParent.gameObject.GetComponent<BringToFront>().bringToFrontOnOver;
					scriptType = "self";
				}
				else																								// Runs if there is none of the above scripts are attached
				{
					includeChildren = false;
					bringToFront = false;
					bringToFrontOnOver = false;
					scriptType = "self";
					otherOther = null;
				}


				if (includeChildren == true)																		// Runs if the scripted object's options should include its children
				{
					if (scriptType == "self")																		// Runs if the scripted object affects itself
					{
						if (bringToFrontOnOver == true && buttonDown == false)
						{
							otherParent.gameObject.transform.SetAsLastSibling ();									// Sets the scripted object as the last sibling when the cursor is over the object or its children, setting it as the fore-most UI object
						}
						
						
						if (bringToFront == true && buttonDown == true)
						{
							otherParent.gameObject.transform.SetAsLastSibling ();									// Sets the scripted object as the last sibling on activation, setting it as the fore-most UI object
						}
					}
					else if (scriptType == "parent")																// Runs if the scripted object affects its parent
					{
						if (bringToFrontOnOver == true && buttonDown == false)
						{
							otherParent.gameObject.transform.parent.SetAsLastSibling ();							// Sets the scripted object's parent as the last sibling when the cursor is over the scripted object or its children, setting it as the fore-most UI object
						}
						
						
						if (bringToFront == true && buttonDown == true)
						{
							otherParent.gameObject.transform.parent.SetAsLastSibling ();							// Sets the scripted object's parent as the last sibling on activation, setting it as the fore-most UI object
						}
					}
					else if (scriptType == "other")																	// Runs if the scripted object affects another object
					{
						if (bringToFrontOnOver == true && buttonDown == false)
						{
							otherOther.gameObject.transform.SetAsLastSibling ();									// Sets the defined other object as the last sibling when the cursor is over the scripted object or its children, setting it as the fore-most UI object
						}
						
						
						if (bringToFront == true && buttonDown == true)
						{
							otherOther.gameObject.transform.SetAsLastSibling ();									// Sets the defined other object as the last sibling on activation, setting it as the fore-most UI object
						}
					}


					other = otherParent;																			// Stores the selected (grand)parent as the selected object
				}
			}
		}
	}


	void Pass ()		// This function is used when the mouse button is not pressed. Mainly for determining which custom cursor should be used currently
	{
		// The following statments run depending on the specific script of the selected object, then enables the script, calls the script's Passive() function, stores the custom cursor that should be currently used, then disables the script. The exception is the "BringToFront" script in which it only calls the Passive() function
		if (other.gameObject.GetComponent<Move>() == true)
		{
			other.gameObject.GetComponent<Move>().enabled = true;
			other.gameObject.GetComponent<Move>().Passive();
			cursorType = other.gameObject.GetComponent<Move>().cursor;
			other.gameObject.GetComponent<Move>().enabled = false;
		}
		else if (other.gameObject.GetComponent<MoveParent>() == true)
		{
			other.gameObject.GetComponent<MoveParent>().enabled = true;
			other.gameObject.GetComponent<MoveParent>().Passive();
			cursorType = other.gameObject.GetComponent<MoveParent>().cursor;
			other.gameObject.GetComponent<MoveParent>().enabled = false;
		}
		else if (other.gameObject.GetComponent<MoveOther>() == true)
		{
			other.gameObject.GetComponent<MoveOther>().enabled = true;
			other.gameObject.GetComponent<MoveOther>().Passive();
			cursorType = other.gameObject.GetComponent<MoveOther>().cursor;
			other.gameObject.GetComponent<MoveOther>().enabled = false;
		}
		else if (other.gameObject.GetComponent<Resize>() == true)
		{
			other.gameObject.GetComponent<Resize>().enabled = true;
			other.gameObject.GetComponent<Resize>().Passive();
			cursorType = other.gameObject.GetComponent<Resize>().cursor;
			other.gameObject.GetComponent<Resize>().enabled = false;
		}
		else if (other.gameObject.GetComponent<ResizeParent>() == true)
		{
			other.gameObject.GetComponent<ResizeParent>().enabled = true;
			other.gameObject.GetComponent<ResizeParent>().Passive();
			cursorType = other.gameObject.GetComponent<ResizeParent>().cursor;
			other.gameObject.GetComponent<ResizeParent>().enabled = false;
		}
		else if (other.gameObject.GetComponent<ResizeOther>() == true)
		{
			other.gameObject.GetComponent<ResizeOther>().enabled = true;
			other.gameObject.GetComponent<ResizeOther>().Passive();
			cursorType = other.gameObject.GetComponent<ResizeOther>().cursor;
			other.gameObject.GetComponent<ResizeOther>().enabled = false;
		}
		else if (other.gameObject.GetComponent<ScaleHorizontal>() == true)
		{
			other.gameObject.GetComponent<ScaleHorizontal>().enabled = true;
			other.gameObject.GetComponent<ScaleHorizontal>().Passive();
			cursorType = other.gameObject.GetComponent<ScaleHorizontal>().cursor;
			other.gameObject.GetComponent<ScaleHorizontal>().enabled = false;
		}
		else if (other.gameObject.GetComponent<ScaleVertical>() == true)
		{
			other.gameObject.GetComponent<ScaleVertical>().enabled = true;
			other.gameObject.GetComponent<ScaleVertical>().Passive();
			cursorType = other.gameObject.GetComponent<ScaleVertical>().cursor;
			other.gameObject.GetComponent<ScaleVertical>().enabled = false;
		}
		else if (other.gameObject.GetComponent<BringToFront>() == true)
		{
			other.gameObject.GetComponent<BringToFront>().Passive();
		}
		else
		{
			cursorType = "normal";
		}

		SetCursor ();																					// Calls the function to set the cursor
	}


	void Activate ()		// This function is used when the mouse button is initially pressed when over a scripted object
	{
		// The following statments run depending on the specific script of the selected object, then enables the script, and calls the script's Active() function
		if (other.gameObject.GetComponent<Move>() == true)
		{
			other.gameObject.GetComponent<Move>().enabled = true;
			other.gameObject.GetComponent<Move>().Active();
		}
		else if (other.gameObject.GetComponent<MoveParent>() == true)
		{
			other.gameObject.GetComponent<MoveParent>().enabled = true;
			other.gameObject.GetComponent<MoveParent>().Active();
		}
		else if (other.gameObject.GetComponent<MoveOther>() == true)
		{
			other.gameObject.GetComponent<MoveOther>().enabled = true;
			other.gameObject.GetComponent<MoveOther>().Active();
		}
		else if (other.gameObject.GetComponent<Resize>() == true)
		{
			other.gameObject.GetComponent<Resize>().enabled = true;
			other.gameObject.GetComponent<Resize>().Active();
		}
		else if (other.gameObject.GetComponent<ResizeParent>() == true)
		{
			other.gameObject.GetComponent<ResizeParent>().enabled = true;
			other.gameObject.GetComponent<ResizeParent>().Active();
		}
		else if (other.gameObject.GetComponent<ResizeOther>() == true)
		{
			other.gameObject.GetComponent<ResizeOther>().enabled = true;
			other.gameObject.GetComponent<ResizeOther>().Active();
		}
		else if (other.gameObject.GetComponent<ScaleHorizontal>() == true)
		{
			other.gameObject.GetComponent<ScaleHorizontal>().enabled = true;
			other.gameObject.GetComponent<ScaleHorizontal>().Active();
		}
		else if (other.gameObject.GetComponent<ScaleVertical>() == true)
		{
			other.gameObject.GetComponent<ScaleVertical>().enabled = true;
			other.gameObject.GetComponent<ScaleVertical>().Active();
		}
		else if (other.gameObject.GetComponent<BringToFront>() == true)
		{
			other.gameObject.GetComponent<BringToFront>().Active();
		}
	}


	void End ()			// This function is used when the mouse button is initially released
	{
		// The following statments run depending on the specific script of the selected object, then calls the script's End() function, and disables the script
		if (other.gameObject.GetComponent<Move>() == true)
		{
			other.gameObject.GetComponent<Move>().End();
			other.gameObject.GetComponent<Move>().enabled = false;
		}
		else if (other.gameObject.GetComponent<MoveParent>() == true)
		{
			other.gameObject.GetComponent<MoveParent>().End();
			other.gameObject.GetComponent<MoveParent>().enabled = false;
		}
		else if (other.gameObject.GetComponent<MoveOther>() == true)
		{
			other.gameObject.GetComponent<MoveOther>().End();
			other.gameObject.GetComponent<MoveOther>().enabled = false;
		}
		else if (other.gameObject.GetComponent<Resize>() == true)
		{
			other.gameObject.GetComponent<Resize>().End();
			other.gameObject.GetComponent<Resize>().enabled = false;
		}
		else if (other.gameObject.GetComponent<ResizeParent>() == true)
		{
			other.gameObject.GetComponent<ResizeParent>().End();
			other.gameObject.GetComponent<ResizeParent>().enabled = false;
		}
		else if (other.gameObject.GetComponent<ResizeOther>() == true)
		{
			other.gameObject.GetComponent<ResizeOther>().End();
			other.gameObject.GetComponent<ResizeOther>().enabled = false;
		}
		else if (other.gameObject.GetComponent<ScaleHorizontal>() == true)
		{
			other.gameObject.GetComponent<ScaleHorizontal>().End();
			other.gameObject.GetComponent<ScaleHorizontal>().enabled = false;
		}
		else if (other.gameObject.GetComponent<ScaleVertical>() == true)
		{
			other.gameObject.GetComponent<ScaleVertical>().End();
			other.gameObject.GetComponent<ScaleVertical>().enabled = false;
		}


		other = null;																					// Sets the selected object to "null"
	}


	public void SetCursor ()		// This function sets the cursor to use the "normal" custom cursor
	{
		if (customCursor == true)																		// Runs if the "customCursor" variable is true
		{
			if (cursorType == "normal" && normalCursor != null)
			{
				Vector2 hotspot = new Vector2 (0 + xHotspotNormal, 0 + yHotspotNormal);					// Defines the hotspot for the custom cursor
				Cursor.SetCursor (normalCursor, hotspot, cursorMode);									// Sets the cursor to use the "normal" custom cursor if assigned
			}
			else if (cursorType == "move" && moveCursor != null)
			{
				Vector2 hotspot = new Vector2 (0 + xHotspotMove, 0 + yHotspotMove);						// Defines the hotspot for the custom cursor
				Cursor.SetCursor (moveCursor, hotspot, cursorMode);										// Sets the cursor to use the "move" custom cursor if assigned
			}
			else if (cursorType == "horizontal" && horizontalCursor != null)					
			{
				Vector2 hotspot = new Vector2 (0 + xHotspotHor, 0 + yHotspotHor);						// Defines the hotspot for the custom cursor
				Cursor.SetCursor (horizontalCursor, hotspot, cursorMode);								// Sets the cursor to use the "horizontalCursor" custom cursor if assigned
			}
			else if (cursorType == "vertical" && verticalCursor != null)						
			{
				Vector2 hotspot = new Vector2 (0 + xHotspotVert, 0 + yHotspotVert);						// Defines the hotspot for the custom cursor
				Cursor.SetCursor (verticalCursor, hotspot, cursorMode);									// Sets the cursor to use the "verticalCursor" custom cursor if assigned
			}
			else if (cursorType == "topLeftBottomRight" && topLeftBottomRightCursor != null)						
			{
				Vector2 hotspot = new Vector2 (0 + xHotspotTLBR, 0 + yHotspotTLBR);						// Defines the hotspot for the custom cursor
				Cursor.SetCursor (topLeftBottomRightCursor, hotspot, cursorMode);						// Sets the cursor to use the "topLeftBottomRightCursor" custom cursor if assigned
			}
			else if (cursorType == "topRightBottomLeft" && topRightBottomLeftCursor != null)						
			{
				Vector2 hotspot = new Vector2 (0 + xHotspotTRBL, 0 + yHotspotTRBL);						// Defines the hotspot for the custom cursor
				Cursor.SetCursor (topRightBottomLeftCursor, hotspot, cursorMode);						// Sets the cursor to use the "topRightBottomLeftCursor" custom cursor if assigned
			}
			else if (normalCursor != null)
			{
				Vector2 hotspot = new Vector2 (0 + xHotspotNormal, 0 + yHotspotNormal);					// Defines the hotspot for the custom cursor
				Cursor.SetCursor (normalCursor, hotspot, cursorMode);									// Sets the cursor to use the "normal" custom cursor if assigned
			}
			else
			{
				Cursor.SetCursor (null, Vector2.zero, cursorMode);										// Sets the cursor to use the null cursor
			}
		}
		else
		{
			Cursor.SetCursor (null, Vector2.zero, cursorMode);											// Sets the cursor to use the null cursor
		}
	}


	void SetCustomCursor ()			// This function sets the cursor to use the "normal" custom cursor
	{
		customCursor = true;
		SetCursor ();
	}

	
	void SetNullCursor ()			// This function sets the cursor to use the null cursor
	{
		customCursor = false;
		SetCursor ();
	}
	
	
	void ToggleCursor ()			// This function toggles the cursor between using the null cursor and the "normal" custom cursor
	{
		customCursor = !customCursor;
		SetCursor ();
	}
}