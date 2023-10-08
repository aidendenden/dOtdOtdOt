using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatBubble : MonoBehaviour
{
    [SerializeField] private SpriteRenderer bgSpriteRenderer;
    [SerializeField] private TextMeshPro textMeshPro;

    private string _bubbleText="";

    public string bubbleText
    {
        get { return _bubbleText; }
        set
        { 
            _bubbleText = value; 
            SetText(value);
        }
    }
    

    public void SetText(string text)
    {
        textMeshPro.SetText(text);
        textMeshPro.ForceMeshUpdate();
        var textSize = textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(1f, 0.5f);
        bgSpriteRenderer.size = textSize + padding;
        //bgSpriteRenderer.transform.localPosition = new Vector3(bgSpriteRenderer.size.x / 2f, bgSpriteRenderer.transform.localPosition.y);
    }
    
}