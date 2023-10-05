using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPTiao : MonoBehaviour
{
    public GameObject[] Tiao;
    public FSMTouZi fsmTouZi;
  
    void Update()
    {
        SetActiveObjects(Tiao, fsmTouZi.parameter.Hp);
    }


    private void SetActiveObjects(GameObject[] objects, int count)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (i < count)
            {
                objects[i].SetActive(true); // 将前count个物体设置为激活状态
            }
            else
            {
                objects[i].SetActive(false); // 将剩余的物体设置为关闭状态
            }
        }
    }

}
