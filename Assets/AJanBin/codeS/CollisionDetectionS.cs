using System;
using UnityEngine;

public class CollisionDetectionS : MonoBehaviour
{
    public string targetTag = "Hand"; // 特定标签
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            GameEventManager.Instance.Triggered("to touch",collision.transform);
        }
    }

}