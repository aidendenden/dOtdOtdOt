using UnityEngine;
using System.Collections;

public class CustomNormalCursor_3_0 : MonoBehaviour {

	public bool customCursor = true;					// Determines if the custom cursor is to be used
	public Texture2D normalCursor;						// Defines the normal custom cursor
	public int xHotspotNormal = 0;						// Sets the hotspot poisiton for the "normal" custom cursor on the x-axis
	public int yHotspotNormal = 0;						// Sets the hotspot poisiton for the "normal" custom cursor on the y-axis
	CursorMode cursorMode = CursorMode.Auto;			// The cursor mode
	bool isCustom = false;								// Determines if custom cursor is being used

	void Awake ()
	{
		if (customCursor == true)
		{
			SetCustomCursor ();				// Calls the function to set the cursor
		}
	}


	void SetCustomCursor ()					// This function sets the cursor to use the "normal" custom cursor
	{
		if (normalCursor != null)
		{
			Vector2 hotspot = new Vector2 (0 + xHotspotNormal, 0 + yHotspotNormal);					// Defines the hotspot for the custom cursor
			Cursor.SetCursor (normalCursor, hotspot, cursorMode);									// Sets the cursor to use the "normal" custom cursor if assigned
			isCustom = true;
		}
	}


	void SetNullCursor ()					// This function sets the cursor to use the null cursor
	{
		Cursor.SetCursor (null, Vector2.zero, cursorMode);											// Sets the cursor to use the null cursor
		isCustom = false;
	}


	void ToggleCursor ()					// This function toggles the cursor between using the null cursor and the "normal" custom cursor
	{
		if (isCustom == false && normalCursor != null)
		{
			Vector2 hotspot = new Vector2 (0 + xHotspotNormal, 0 + yHotspotNormal);					// Defines the hotspot for the custom cursor
			Cursor.SetCursor (normalCursor, hotspot, cursorMode);									// Sets the cursor to use the "normal" custom cursor if assigned
			isCustom = true;
		}
		else
		{
			Cursor.SetCursor (null, Vector2.zero, cursorMode);										// Sets the cursor to use the null cursor
			isCustom = false;
		}
	}
}
