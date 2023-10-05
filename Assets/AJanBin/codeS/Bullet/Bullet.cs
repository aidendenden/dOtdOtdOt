using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;

    public GameObject explosionPrefab;

    new private Rigidbody rb;

    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetSpeed()
    {
        rb.velocity = gameObject.transform.up  * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag =="Enemy")
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
   
}
