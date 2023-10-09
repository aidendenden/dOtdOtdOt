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
        //waitTimeOne = new WaitForSeconds(intervalone);
       // waitTimeTwo = new WaitForSeconds(intervaltwo); 
    }
    
    private void Start()
    {

        Demo.Play("真正的恶魔出现");
        countDownSystem.TimerStart(0);
        Debug.Log("恶魔");
        //StartCoroutine(EnumeratorGameStartOne());
        //StartCoroutine(EnumeratorGameStartTwo());
    }

    private IEnumerator EnumeratorGameStartOne()
    {
        while (true)
        {
            Demo.Play("真正的恶魔出现");
            countDownSystem.TimerStart(0);
            Debug.Log("恶魔");

            yield return waitTimeOne;
        }
    }


    private IEnumerator EnumeratorGameStartTwo()
    {
        while (true)
        {
            Angle.Play("天使");
            countDownSystem.TimerStart(1);
            Debug.Log("天使");
            yield return waitTimeTwo;
        }
    }
    
}