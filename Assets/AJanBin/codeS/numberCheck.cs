using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class numberCheck : MonoBehaviour
{
    public int TouZiNumber = 1;
    public bool oneOrTwo = true;

    private MangeManger mangermanger;
    // Start is called before the first frame update
    void Start()
    {
        mangermanger = GameObject.FindGameObjectWithTag("Manger").GetComponent<MangeManger>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
     if(other.CompareTag("PanZi")&&oneOrTwo) {
            mangermanger.numberOne = TouZiNumber;
        }

     if (other.CompareTag("PanZi") && !oneOrTwo)
        {
            mangermanger.numberTwo = TouZiNumber;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("PanZi") && oneOrTwo) {
            mangermanger.numberOne = 0;
        }

        if (other.CompareTag("PanZi") && !oneOrTwo)
        {
            mangermanger.numberTwo = 0;
        }
    }
}
