using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{ 
    // public GameObject objectToSpawn; // 要生成的物体
    // public string targetTag = "Hand"; // 特定标签
    // public float spawnInterval = 1f;
    public float timer;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; // 每帧更新计时器
    }
}
