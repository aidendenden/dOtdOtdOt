using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManger : MonoBehaviour
{
    public GameObject BulletOne;

    public Transform TouZiOne;
    public Transform TouZiTwo;

    public int bulletCount = 8; // ×Óµ¯ÊýÁ¿


    public void FireOne()
    {
        float angleInterval = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleInterval;
            Vector3 R = new Vector3 (0f, 0f, angle);
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
