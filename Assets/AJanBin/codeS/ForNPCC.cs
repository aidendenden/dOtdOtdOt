using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForNPCC : MonoBehaviour
{


    public GameObject Qustion;
    public GameObject Qustion2;
    public GameObject Eye;
    public GameObject stopP;

    public RectTransform npcP;
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
        Eye.transform.position = npcP.position;
        stopP.SetActive(false);
    }

    public void SpawnQustion2()
    {
        Qustion2.SetActive(true);
        Eye.SetActive(true);
        Eye.transform.position = npcP.position;
        stopP.SetActive(false);

    }
}
