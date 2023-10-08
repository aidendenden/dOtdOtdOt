using UnityEngine;

public class FunctionCaller : MonoBehaviour
{
    public float intervalone = 15f;
    public float intervaltwo = 12f;
    public CountDownSystem countDownSystem;

    public Animator Demo;
    public Animator Angle;

    private void Start()
    {
        InvokeRepeating("CallFunctionOne", 0f, intervalone);
        InvokeRepeating("CallFunctionTwo", 0f, intervaltwo);
    }

    private void CallFunctionOne()
    {
        Demo.Play("�����Ķ�ħ����");
        countDownSystem.TimerStart(1);
        Debug.Log("��ħ");
    }
    private void CallFunctionTwo()
    {
        Angle.Play("��ʹ");
        countDownSystem.TimerStart(0);
        Debug.Log("��ʹ");
    }
}