using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class numberCheck : MonoBehaviour
{
    public int TouZiNumber = 1;
    public bool oneOrTwo = true;
    public FSMTouZi fsmTouZi;

    private MangeManger mangermanger;
    // Start is called before the first frame update
    void Start()
    {
        mangermanger = GameObject.FindGameObjectWithTag("Manger").GetComponent<MangeManger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (oneOrTwo&& fsmTouZi.parameter.Life==false)
        {
            mangermanger.numberOne = 0;
        }
        if (!oneOrTwo && fsmTouZi.parameter.Life == false)
        {
            mangermanger.numberTwo = 0;
        }
    }

    private void OnTriggerStay(Collider other)
    {
     if(other.CompareTag("PanZi")&&oneOrTwo&& fsmTouZi.parameter.Life ==true) {
            mangermanger.numberOne = TouZiNumber;
        }

     if (other.CompareTag("PanZi") && !oneOrTwo)
        {
            mangermanger.numberTwo = TouZiNumber;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("PanZi") && oneOrTwo&& fsmTouZi.parameter.Life == true) {
            mangermanger.numberOne = 0;
        }

        if (other.CompareTag("PanZi") && !oneOrTwo)
        {
            mangermanger.numberTwo = 0;
        }
    }
}
