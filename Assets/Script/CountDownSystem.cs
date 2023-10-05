using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public enum OperationMath
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
    MixedOperations
}

public class CountDownSystem : MonoBehaviour
{
    public CircularTimer[] circularTimers;

    public MangeManger gameManager;

    [Header("运算方法")]
    public OperationMath OperationMath=OperationMath.MixedOperations;
    
    //public Text text;

    private void Start()
    {
        TryGetComponent(out gameManager);
        StartTimer();
    }

    public void StartTimer()
    {
        //text.text = "Start";
        foreach (CircularTimer timer in circularTimers)
        {
            timer.StartTimer();
        }
    }

    public void PauseTimer()
    {
        //text.text = "Pause";
        foreach (CircularTimer timer in circularTimers)
        {
            timer.PauseTimer();
        }
    }

    public void StopTimer()
    {
        //text.text = "Stop";
        foreach (CircularTimer timer in circularTimers)
        {
            timer.StopTimer();
        }
    }

    public void ReStart()
    {
        foreach (CircularTimer timer in circularTimers)
        {
            timer.StopTimer();
            timer.StartTimer();
        }
    }

    public void Scoring()
    {
        int targetNum = UnityEngine.Random.Range(2, 12);
        int num1 = gameManager.numberOne;
        int num2 = gameManager.numberTwo;

        bool b=false;
        
        switch (OperationMath)
        {
            case OperationMath.Addition:
                b=CanReachTargetNumberAdd(num1, num2, targetNum);
                break;
            case OperationMath.Subtraction:
                b=CanReachTargetNumberSub(num1, num2, targetNum);
                break;
            case OperationMath.Multiplication:
                b=CanReachTargetNumberMultiply(num1, num2, targetNum);
                break;
            case OperationMath.Division:
                b=CanReachTargetNumberDivision(num1, num2, targetNum);
                break;
            case OperationMath.MixedOperations:
                b=CanReachTargetNumberMix(num1, num2, targetNum);
                break;
        }

        if (b)
        {
            GameEventManager.Instance.Triggered("CountDownAnswerIsTrue",transform,new Vector3(num1,num2,targetNum));
        }
        else
        {
            GameEventManager.Instance.Triggered("CountDownAnswerIsFalse",transform,new Vector3(num1,num2,targetNum)); 
        }
        
    }

    public void DidFinishedTimer()
    {
        //text.text = "Finished";
    }

    public bool CanReachTargetNumberAdd(int number1, int number2, int target)
    {
        if (number1 + number2 == target)
        {
            return true;
        }
        
        return false;
    }
    
    public bool CanReachTargetNumberSub(int number1, int number2, int target)
    {
        if (number1 - number2 == target || number2 - number1 == target)
        {
            return true;
        }
        
        return false;
    }
    
    public bool CanReachTargetNumberMultiply(int number1, int number2, int target)
    {
        if (number1 * number2 == target)
        {
            return true;
        }
        
        return false;
    }
    
    public bool CanReachTargetNumberDivision(int number1, int number2, int target)
    {
        if (number2 != 0 && (number1 / number2 == target || number2 / number1 == target))
        {
            return true;
        }
        
        return false;
    }

    public bool CanReachTargetNumberMix(int number1, int number2, int target)
    {
        // 判断两个数相加是否等于目标数
        if (number1 + number2 == target)
        {
            return true;
        }

        // 判断两个数相减是否等于目标数
        if (number1 - number2 == target || number2 - number1 == target)
        {
            return true;
        }

        // 判断两个数相乘是否等于目标数
        if (number1 * number2 == target)
        {
            return true;
        }

        // 判断两个数相除是否等于目标数，并确保没有除以零的情况
        if (number2 != 0 && (number1 / number2 == target || number2 / number1 == target))
        {
            return true;
        }

        // 如果以上条件都不满足，则无法通过加、减、乘、除得到目标数
        return false;
    }
}