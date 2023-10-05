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
                objects[i].SetActive(true); // ��ǰcount����������Ϊ����״̬
            }
            else
            {
                objects[i].SetActive(false); // ��ʣ�����������Ϊ�ر�״̬
            }
        }
    }

}
