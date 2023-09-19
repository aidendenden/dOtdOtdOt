using System;
using Unity.Mathematics;
using UnityEngine;

public class CollisionDetectionS : MonoBehaviour
{
    public string targetTag = "Hand"; // 特定标签
    public int touchNumber = 0;

    private Transform _transform;
    private int touchMax = 20;


    private void Start()
    {
        _transform = gameObject.GetComponent<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag(targetTag))
        {
            if (touchNumber > touchMax)
            {

                Vector3 p = collision.transform.position - _transform.position;
             
                GameEventManager.Instance.Triggered("to touch", collision.transform,p);

            }
            else {
                _transform.localScale += new Vector3(0.01f, 0.01f, 0);
                touchNumber++; 
            }
        }

        
    }

}