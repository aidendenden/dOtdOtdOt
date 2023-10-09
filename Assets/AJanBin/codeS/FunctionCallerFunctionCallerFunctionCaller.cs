using System.Collections;
using UnityEngine;

public class FunctionCaller : MonoBehaviour
{
    public float intervalone = 15f;
    public float intervaltwo = 12f;
    public CountDownSystem countDownSystem;

    public Animator Demo;
    public Animator Angle;

    private WaitForSeconds waitTimeOne;
    private WaitForSeconds waitTimeTwo;
    
    private void Awake()
    {
        waitTimeOne = new WaitForSeconds(intervalone);
        waitTimeTwo = new WaitForSeconds(intervaltwo); 
    }
    
    private void Start()
    {
        StartCoroutine(EnumeratorGameStartOne());
        StartCoroutine(EnumeratorGameStartTwo());
    }

    private IEnumerator EnumeratorGameStartOne()
    {
        while (true)
        {
            Demo.Play("�����Ķ�ħ����");
            countDownSystem.TimerStart(0);
            Debug.Log("��ħ");

            yield return waitTimeOne;
        }
    }


    private IEnumerator EnumeratorGameStartTwo()
    {
        while (true)
        {
            Angle.Play("��ʹ");
            countDownSystem.TimerStart(1);
            Debug.Log("��ʹ");
            yield return waitTimeTwo;
        }
    }
    
}