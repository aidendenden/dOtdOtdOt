using System;
using Unity.Mathematics;
using UnityEngine;

public class CollisionDetectionS : MonoBehaviour
{
    public string targetTag = "Hand"; // 特定标签
    public float touchNumber = 0;

    private float touchMax = 5;
    private Transform _transform;
    private MangeManger mmanger;


    private void Start()
    {
        _transform = gameObject.GetComponent<Transform>();
        mmanger = GameObject.FindGameObjectWithTag("Manger").GetComponent<MangeManger>();
        touchMax = touchMax/mmanger.Hard;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag(targetTag))
        {
            Vector3 p = collision.transform.position - _transform.position;

            if (touchNumber > touchMax)
            {

                
             
                GameEventManager.Instance.Triggered("to touch", collision.transform,p);

            }
            else {
                if(p.x < 0 || p.y < 0)
                {
                    _transform.localScale -= p * 0.01f*mmanger.Hard;
                }
                if (p.x > 0 || p.y > 0)
                {
                    _transform.localScale += p * 0.01f*mmanger.Hard;
                }

                touchNumber++; 
            }
        }

        
    }

}