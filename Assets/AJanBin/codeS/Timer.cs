using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public GameObject objectToSpawn; // 要生成的物体
    public float spawnInterval = 0.5f;
    [HideInInspector]
    public float timer;

    private MangeManger mmanger;

    // Update is called once per frames
    private void Start()
    {
        GameEventManager.OnTrigger += HandleTrigger;
        mmanger = GameObject.FindGameObjectWithTag("Manger").GetComponent<MangeManger>();
        
    }

    void HandleTrigger(string message, Transform _transform,Vector3 v)
    {
        if (String.Equals(message,"to touch")&& timer >= spawnInterval)
        {
            Vector3 spawnPosition = _transform.position - v*mmanger.Hard;
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            timer = 0f; 
        }
       
    }


    void Update()
    {
        timer += Time.deltaTime; // 每帧更新计时器
    }
}