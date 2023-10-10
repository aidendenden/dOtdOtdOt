using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManger : MonoBehaviour
{
    public GameObject BulletOne;

    public Transform TouZiOne;
    public Transform TouZiTwo;


    public GameObject HaHaHa;


    public GameObject BoooMPrefab; // 物体预制体
    

    public int bulletCount = 8; // 子弹数量

    public Transform T1;
    public Transform T2;

    public Transform E1;
    public Transform E2;
    public Animator Demo;
    public Animator Angle;

    public float intervalone = 15f;
    public float intervaltwo = 12f;
    public CountDownSystem countDownSystem;
    public Sprite[] FuHao;
    public SpriteRenderer FuHaoHao;
    public SpriteRenderer FuHaoHaoA;
    public GameObject EnemyOne;


    public float durationD = 5f; // 计时器持续时间
    private bool isTimerRunningD = false; // 计时器是否正在运行

    public float durationA = 5f; // 计时器持续时间
    private bool isTimerRunningA = false; // 计时器是否正在运行


    public void SpawnDemo()
    {

        if (1 == UnityEngine.Random.Range(0, 2))
        {
            countDownSystem.judgmentsBased1 = JudgmentsBased.MoreThan;
            FuHaoHao.sprite = FuHao[0]; 
        }
        else
        {
            countDownSystem.judgmentsBased1 = JudgmentsBased.LessThan;
            FuHaoHao.sprite = FuHao[1];
        }
       
        Demo.Play("真正的恶魔出现");
        countDownSystem.TimerStart(0);
        Debug.Log("恶魔");
    }

    public void SpawnAngle()
    {

        if (1 == UnityEngine.Random.Range(0, 2))
        {
            countDownSystem.judgmentsBased2 = JudgmentsBased.MoreThan;
            FuHaoHaoA.sprite = FuHao[1];
        }
        else
        {
            countDownSystem.judgmentsBased2 = JudgmentsBased.LessThan;
            FuHaoHaoA.sprite = FuHao[0];
        }
        Angle.Play("天使");
        countDownSystem.TimerStart(1);
        Debug.Log("天使");
        
    }

    private void Update()
    {
        
        if (isTimerRunningD)
        {
            durationD -= Time.deltaTime;
            if (durationD <= 0f)
            {
                TimerFinishedD();
                isTimerRunningD = false;
            }
        }


        if (isTimerRunningA)
        {
            durationA -= Time.deltaTime;
            if (durationA <= 0f)
            {
                TimerFinishedA();
                isTimerRunningA = false;
            }
        }
    }

    private void TimerFinishedD()
    {
        SpawnDemo();

    }
    private void TimerFinishedA()
    {
        SpawnAngle();

    }
    private void Start()
    {
        GameEventManager.OnTrigger += Triggered;
        SpawnDemo();
        SpawnAngle();
    }

    void Triggered(string message, Transform _transform, Vector3 _vector3)
    {
        if (message.Contains("CountDownAnswerIsTrue"))
        {
            int startIndex = message.IndexOf(":") + 1; // 获取冒号之后的索引
            string valueString = message.Substring(startIndex); // 获取冒号之后的字符串
            int value;

            if (int.TryParse(valueString, out value))
            {
                Debug.Log($"第{value+1}颗筛子,出结果了.结果成功：筛子一：{_vector3.x},筛子二：{_vector3.y}，目标数{_vector3.z}");
            }

            if (value == 0)
            {
                Demo.Play("恶魔走了");
                durationD = 3f;
                isTimerRunningD = true;
                EnemyOneSpawn(3);
            }
            else if (value == 1)
            {
                Angle.Play("天使走了");
                durationA = 3f;
                isTimerRunningA = true;
                FireOne(12);
            }
           
            HAHA();
           
        }
        
        if (message.Contains("CountDownAnswerIsFalse"))
        {
            int startIndex = message.IndexOf(":") + 1; // 获取冒号之后的索引
            string valueString = message.Substring(startIndex); // 获取冒号之后的字符串
            int value;

            if (int.TryParse(valueString, out value))
            {
                Debug.Log($"第{value+1}颗筛子,出结果了.结果失败：筛子一：{_vector3.x},筛子二：{_vector3.y}，目标数{_vector3.z}");
            }
            if (value == 0)
            {
                Demo.Play("恶魔走了");
                durationD = 3f;
                isTimerRunningD = true;
               

            }
            else if (value == 1)
            {
                Angle.Play("天使走了");
                durationA = 3f;
                isTimerRunningA = true;
            }
           // HAHA();


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
            bullet.GetComponent<Bullet>().speed = 7f;
            bullet.GetComponent<Bullet>().SetSpeed();
        }
    }


    public void BooomOne(int generationCount)
    {
        for (int i = 0; i < generationCount; i++)
        {
            GenerateObject(BoooMPrefab, T1, T2);
        }
    }

    public void EnemyOneSpawn(int generationCount)
    {
        for (int i = 0; i < generationCount; i++)
        {
            GenerateObject(EnemyOne,E1,E2);
        }
    }

    public void GenerateObject(GameObject a,Transform tt1,Transform tt2)
    {
           


        // 在指定范围内随机生成位置
        Vector3 position = new Vector3(UnityEngine.Random.Range(tt1.position.x, tt2.position.x),
                                       UnityEngine.Random.Range(tt1.position.y, tt2.position.y),
                                       UnityEngine.Random.Range(tt1.position.z, tt2.position.z));

        GameObject b = a;
        // 创建物体实例
        Instantiate(b, position, Quaternion.identity);
        b.SetActive(true);
    }

    public void HAHA()
    {
        HaHaHa.SetActive(true);
    }

}