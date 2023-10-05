using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManger : MonoBehaviour
{
    public GameObject BulletOne;

    public Transform TouZiOne;
    public Transform TouZiTwo;

    public int bulletCount = 8; // 子弹数量

    private void Start()
    {
        GameEventManager.OnTrigger += Triggered;
    }

    void Triggered(string message, Transform _transform, Vector3 _vector3)
    {
        if (string.Equals(message, "CountDownAnswerIsTrue"))
        {
            Debug.Log($"结果成功：筛子1：{_vector3.x},筛子2：{_vector3.y}，目标数{_vector3.z}");
            FireOne();
        }
        
        if (string.Equals(message, "CountDownAnswerIsFalse"))
        {
            Debug.Log($"结果失败：筛子1：{_vector3.x},筛子2：{_vector3.y}，目标数{_vector3.z}");
        }
        
    }

    public void FireOne()
    {
        float angleInterval = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleInterval;
            Vector3 R = new Vector3(0f, 0f, angle);
            GameObject bullet = Instantiate(BulletOne, TouZiOne.position, Quaternion.identity);
            bullet.transform.rotation = Quaternion.Euler(90f, 0f, angle);
            bullet.GetComponent<Bullet>().speed = 5f;
            bullet.GetComponent<Bullet>().SetSpeed();
        }

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleInterval;
            Vector3 R = new Vector3(0f, 0f, angle);
            GameObject bullet = Instantiate(BulletOne, TouZiTwo.position, Quaternion.identity);
            bullet.transform.rotation = Quaternion.Euler(90f, 0f, angle);
            bullet.GetComponent<Bullet>().speed = 5f;
            bullet.GetComponent<Bullet>().SetSpeed();
        }
    }
}