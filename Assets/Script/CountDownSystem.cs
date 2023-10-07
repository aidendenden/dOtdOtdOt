using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
using System.IO;

public enum OperationMath
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
    MixedOperations
}

[Serializable]
public class DiceImage
{
    public Image Answer;
    public int AnswerNum;
}

public class CountDownSystem : MonoBehaviour
{
    public CircularTimer[] circularTimers;

    public MangeManger gameManager;

    [Header("运算方法")]
    public OperationMath OperationMath=OperationMath.MixedOperations;
    
    public List<Sprite> newSprite;
    
    [Serialize]
    public List<DiceImage> DiceImages;

    [Header("间隔时间")] 
    public float WaitTime = 1f;
    
    private WaitForSeconds waitTime;

    public Image Dice1, Dice2;


    private void Awake()
    {
        waitTime = new WaitForSeconds(WaitTime); 
    }

    private void Start()
    {
        TryGetComponent(out gameManager);
        StartCoroutine(GameStart());
    }


    private void Update()
    {
        Dice1.sprite = newSprite[gameManager.numberOne];
        
        Dice2.sprite = newSprite[gameManager.numberTwo];
        
    }

    
    
    private IEnumerator GameStart()
    {
        while (true)
        {
            // 调用您希望在每个时间间隔后执行的方法
            int index = 0;
            foreach (CircularTimer timer in circularTimers)
            {
                int _int = UnityEngine.Random.Range(1, 2);
                if (_int==1)
                {
                    var _ = UnityEngine.Random.Range(2, 12);
                    Debug.Log(_+"目标数");
                    DiceImages[index].AnswerNum=_;
                    DiceImages[index].Answer.sprite = newSprite[_];
                    timer.StartTimer(); 
                }
                index++;
            }

            yield return waitTime;
        }
    }
    
    public void StartTimer()
    {
        
        foreach (CircularTimer timer in circularTimers)
        {
            timer.StartTimer();
        }
    }

    public void PauseTimer()
    {
        foreach (CircularTimer timer in circularTimers)
        {
            timer.PauseTimer();
        }
    }

    public void StopTimer()
    {
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
    
    public void DidFinishedTimer(int index)
    {
        int num1 = gameManager.numberOne;
        int num2 = gameManager.numberTwo;

        bool b=false;
        
        switch (OperationMath)
        {
            case OperationMath.Addition:
                b=CanReachTargetNumberAdd(num1, num2,  DiceImages[index].AnswerNum);
                break;
            case OperationMath.Subtraction:
                b=CanReachTargetNumberSub(num1, num2,  DiceImages[index].AnswerNum);
                break;
            case OperationMath.Multiplication:
                b=CanReachTargetNumberMultiply(num1, num2,  DiceImages[index].AnswerNum);
                break;
            case OperationMath.Division:
                b=CanReachTargetNumberDivision(num1, num2,  DiceImages[index].AnswerNum);
                break;
            case OperationMath.MixedOperations:
                b=CanReachTargetNumberMix(num1, num2,  DiceImages[index].AnswerNum);
                break;
        }

        if (b)
        {
            GameEventManager.Instance.Triggered("CountDownAnswerIsTrue",transform,new Vector3(num1,num2, DiceImages[index].AnswerNum));
        }
        else
        {
            GameEventManager.Instance.Triggered("CountDownAnswerIsFalse",transform,new Vector3(num1,num2, DiceImages[index].AnswerNum)); 
        }
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