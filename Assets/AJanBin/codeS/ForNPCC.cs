using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForNPCC : MonoBehaviour
{


    public GameObject Qustion;
    public GameObject Eye;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnQustion()
    {
        Qustion.SetActive(true);
        Eye.SetActive(true);
    }
}
