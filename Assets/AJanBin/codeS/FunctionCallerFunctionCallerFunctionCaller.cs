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
        Demo.Play("真正的恶魔出现");
        countDownSystem.TimerStart(1);
        Debug.Log("恶魔");
    }
    private void CallFunctionTwo()
    {
        Angle.Play("天使");
        countDownSystem.TimerStart(0);
        Debug.Log("天使");
    }
}