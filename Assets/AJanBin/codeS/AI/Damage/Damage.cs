using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public bool isTouZi = false;
    public FSMTouZi fsmTouZi;
    public Animator animator;

    public float WuDiTime = 2f;
    public float WuDiTimeTOP = 2f;

    public string[] DamageType;
    public AudioSource audioSource;


    private void OnTriggerStay(Collider other)
    {
        if (IsStringExist(DamageType, other.tag))
        {
            

            if (isTouZi == true && WuDiTime<=0&& fsmTouZi.parameter.Hp>0)
            {
                audioSource.PlayOneShot(audioSource.clip);
                animator.SetTrigger("Hurt");
                fsmTouZi.parameter.Hp--;
                WuDiTime = WuDiTimeTOP;


            }



        }
    }

    private void Update()
    {
        if (WuDiTime > 0)
        {
            WuDiTime -= Time.deltaTime;
        }

    }


    private bool IsStringExist(string[] array, string target)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == target)
            {
                return true; // ���Ŀ���ַ����������е�ĳ���ַ�����ȣ��򷵻�true
            }
        }
        return false; // ����������������鶼û���ҵ���ȵ��ַ������򷵻�false
    }
}
