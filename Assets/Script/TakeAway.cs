using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeAway : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("inininin");
        Debug.Log(other.transform.tag);
        
        // if (other.transform.CompareTag("playerHand"))
        // {
        //     transform.SetParent(other.transform);
        // }
    }
}
