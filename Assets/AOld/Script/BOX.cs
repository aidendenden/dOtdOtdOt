using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOX : MonoBehaviour
{
    public bool isCon = false;

    public float timeLimit = 0.2f; // ��ʱ����ʱ������
    private float timer; // ��ǰ��ʱ����ֵ

    private int listC;

    private void Start()
    {
        timer = 0f; // ��ʼ����ʱ��
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (listC + 1 >= 3)
            {
                listC = 3;
            }
            else
            {
                listC++;
            }
        }

        Timing();
        isConAtive();
    }

   

    private void isConAtive()
    {
        if (listC >= 2)
        {
            isCon = true;
           
        }
        else
        {
            isCon = false;
            
        }
    }

    private void Timing()
    {
        timer += Time.deltaTime; // ���¼�ʱ����ֵ

        if (timer >= timeLimit)
        {
            
            if (listC-1 <= 0)
            {
                listC = 0;
            }
            else
            {
                
                listC--;
            }
            timer = 0;
        }
    }




}
