using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManger : MonoBehaviour
{
    public GameObject BulletOne;

    public Transform TouZiOne;
    public Transform TouZiTwo;


    public GameObject BoooMPrefab; // 物体预制体
    

    public int bulletCount = 8; // 子弹数量

    public Transform T1;
    public Transform T2;

    private void Start()
    {
        GameEventManager.OnTrigger += Triggered;
    }

    void Triggered(string message, Transform _transform, Vector3 _vector3)
    {
        if (string.Equals(message, "CountDownAnswerIsTrue"))
        {
            Debug.Log($"结果成功：筛子一：{_vector3.x},筛子二：{_vector3.y}，目标数{_vector3.z}");
            FireOne(8);
            
        }
        
        if (string.Equals(message, "CountDownAnswerIsFalse"))
        {
            Debug.Log($"结果失败：筛子一：{_vector3.x},筛子二：{_vector3.y}，目标数{_vector3.z}");
            // BooomOne(12);

        }
        
    }

    public void FireOne(int B)
    {
        float angleInterval = 360f / bulletCount;

        for (int i = 0; i < B; i++)
        {
            float angle = i * angleInterval;
            Vector3 R = new Vector3(0f, 0f, angle);
            GameObject bullet = Instantiate(BulletOne, TouZiOne.position, Quaternion.identity);
            bullet.transform.rotation = Quaternion.Euler(90f, 0f, angle);
            bullet.GetComponent<Bullet>().speed = 7f;
            bullet.GetComponent<Bullet>().SetSpeed();
        }

        for (int i = 0; i < B; i++)
        {
            float angle = i * angleInterval;
            Vector3 R = new Vector3(0f, 0f, angle);
            GameObject bullet = Instantiate(BulletOne, TouZiTwo.position, Quaternion.identity);
            bullet.transform.rotation = Quaternion.Euler(90f, 0f, angle);
            bullet.GetComponent<Bullet>().speed = 5f;
            bullet.GetComponent<Bullet>().SetSpeed();
        }
    }


    public void BooomOne(int generationCount)
    {
        for (int i = 0; i < generationCount; i++)
        {
            GenerateObject();
        }
    }

    public void GenerateObject()
    {
           


        // 在指定范围内随机生成位置
        Vector3 position = new Vector3(UnityEngine.Random.Range(T1.position.x, T2.position.x),
                                       UnityEngine.Random.Range(T1.position.y, T2.position.y),
                                       UnityEngine.Random.Range(T1.position.z, T2.position.z));

        // 创建物体实例
        Instantiate(BoooMPrefab, position, Quaternion.identity);
    }

}