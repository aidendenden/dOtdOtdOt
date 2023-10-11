using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public GameObject ChangeS;
    public bool isTouZi = false;
    public bool isENE = false;
    public FSMTouZi fsmTouZi;
    public FSM fsmENE;
    public Animator animator;

    public float WuDiTime = 2f;
    public float WuDiTimeTOP = 2f;
    public float WuDiTimeTOPENE = 2f;



    public string[] DamageType;
    public AudioSource audioSource;


    private void OnTriggerStay(Collider other)
    {
        if (IsStringExist(DamageType, other.tag))
        {
            

            if (isTouZi == true && WuDiTime<=0&& fsmTouZi.parameter.Hp>0)
            {
                //audioSource.PlayOneShot(audioSource.clip);
                animator.SetTrigger("Hurt");
                if (other.tag == "HP")
                {
                    if (fsmTouZi.parameter.Hp < 3)
                    {
                        fsmTouZi.parameter.Hp++;
                    }
                    
                    WuDiTime = WuDiTimeTOP;
                }
                else
                {
                    if (fsmTouZi.parameter.Hp - 1 <= 0)
                    {
                        ChangeS.SetActive(true);
                    }
                    else
                    {
                        fsmTouZi.parameter.Hp--;
                    }
                    
                    WuDiTime = WuDiTimeTOP;
                }
                


            }

            if (isENE == true && WuDiTime <= 0 && fsmENE.parameter.Hp > 0)
            {
                //audioSource.PlayOneShot(audioSource.clip);
                animator.SetTrigger("Hurt");
                fsmENE.parameter.Hp--;
                WuDiTime = WuDiTimeTOPENE;


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
                return true; // 如果目标字符串与数组中的某个字符串相等，则返回true
            }
        }
        return false; // 如果遍历完整个数组都没有找到相等的字符串，则返回false
    }
}
