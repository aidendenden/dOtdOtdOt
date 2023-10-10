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

public enum JudgmentsBased
{
    MoreThan,
    LessThan,
    Equal,
    MoreThanAndEqual,
    LessThanAndEqual
}

[Serializable]
public class DiceImage
{
    public SpriteRenderer Answer;
    public int AnswerNum;
}

public class CountDownSystem : MonoBehaviour
{
    //public CircularTimer[] circularTimers;

    public CircleTimerSprite[] circularTimers;

    public MangeManger gameManager;

    [Header("运算方法")] public OperationMath OperationMath = OperationMath.MixedOperations;

    [Header("判断方法")] public JudgmentsBased judgmentsBased1 = JudgmentsBased.Equal;
    public JudgmentsBased judgmentsBased2 = JudgmentsBased.MoreThan;

    public List<Sprite> newSprite;

    [Serialize] public List<DiceImage> DiceImages;

    [Header("间隔时间")] public float WaitTime = 1f;

    //private WaitForSeconds waitTime;

    public SpriteRenderer Dice1, Dice2, Dice3, Dice4;


    private void Awake()
    {
        //waitTime = new WaitForSeconds(WaitTime); 
    }

    private void Start()
    {
        TryGetComponent(out gameManager);
        //StartCoroutine(GameStart());
    }


    private void Update()
    {
        Dice1.sprite = newSprite[gameManager.numberOne];
        Dice3.sprite = newSprite[gameManager.numberOne];

        Dice2.sprite = newSprite[gameManager.numberTwo];
        Dice4.sprite = newSprite[gameManager.numberTwo];
    }

    public void TimerStart(int index)
    {
        var _ = UnityEngine.Random.Range(2, 12);
        Debug.Log(_ + "目标数");
        DiceImages[index].AnswerNum = _;
        DiceImages[index].Answer.sprite = newSprite[_];
        circularTimers[index].StartTimer();
    }


    // private IEnumerator EnumeratorGameStart()
    // {
    //     while (true)
    //     {
    //         // 调用您希望在每个时间间隔后执行的方法
    //         int index = 0;
    //         foreach (CircleTimerSprite timer in circularTimers)
    //         {
    //             int _int = UnityEngine.Random.Range(1, 2);
    //             if (_int==1)
    //             {
    //                 var _ = UnityEngine.Random.Range(2, 12);
    //                 Debug.Log(_+"目标数");
    //                 DiceImages[index].AnswerNum=_;
    //                 DiceImages[index].Answer.sprite = newSprite[_];
    //                 timer.StartTimer(); 
    //             }
    //             index++;
    //         }
    //
    //         yield return waitTime;
    //     }
    // }

    public void StartTimer()
    {
        foreach (CircleTimerSprite timer in circularTimers)
        {
            timer.StartTimer();
        }
    }

    public void PauseTimer()
    {
        foreach (CircleTimerSprite timer in circularTimers)
        {
            timer.PauseTimer();
        }
    }

    public void StopTimer()
    {
        foreach (CircleTimerSprite timer in circularTimers)
        {
            timer.StopTimer();
        }
    }

    public void ReStart()
    {
        foreach (CircleTimerSprite timer in circularTimers)
        {
            timer.StopTimer();
            timer.StartTimer();
        }
    }

    public void DidFinishedTimer(int index)
    {
        int num1 = gameManager.numberOne;
        int num2 = gameManager.numberTwo;

        bool b = false;

        JudgmentsBased _judgments = JudgmentsBased.Equal;

        if (index == 0)
        {
            _judgments = judgmentsBased1;
        }
        else if (index == 1)
        {
            _judgments = judgmentsBased2;
        }

        switch (OperationMath)
        {
            case OperationMath.Addition:
                b = CanReachTargetNumberAdd(num1, num2, DiceImages[index].AnswerNum,_judgments);
                break;
            case OperationMath.Subtraction:
                b = CanReachTargetNumberSub(num1, num2, DiceImages[index].AnswerNum,_judgments);
                break;
            case OperationMath.Multiplication:
                b = CanReachTargetNumberMultiply(num1, num2, DiceImages[index].AnswerNum,_judgments);
                break;
            case OperationMath.Division:
                b = CanReachTargetNumberDivision(num1, num2, DiceImages[index].AnswerNum,_judgments);
                break;
            case OperationMath.MixedOperations:
                b = CanReachTargetNumberMix(num1, num2, DiceImages[index].AnswerNum,_judgments);
                break;
        }

        if (b)
        {
            GameEventManager.Instance.Triggered($"CountDownAnswerIsTrue:{index}", transform,
                new Vector3(num1, num2, DiceImages[index].AnswerNum));
        }
        else
        {
            GameEventManager.Instance.Triggered($"CountDownAnswerIsFalse:{index}", transform,
                new Vector3(num1, num2, DiceImages[index].AnswerNum));
        }
    }

    #region 处理方法
    public bool CanReachTargetNumberAdd(int number1, int number2, int target)
    {
        if (number1 + number2 == target)
        {
            return true;
        }

        return false;
    }

    public bool CanReachTargetNumberAdd(int number1, int number2, int target, JudgmentsBased judgmentsBased)
    {
        switch (judgmentsBased)
        {
            case JudgmentsBased.MoreThan:
                if (number1 + number2 > target)
                {
                    return true;
                }
                
                break;
            case JudgmentsBased.LessThan:
                if (number1 + number2 < target)
                {
                    return true;
                }
                break;
            case JudgmentsBased.Equal:
                if (number1 + number2 == target)
                {
                    return true;
                }
                break;
            case JudgmentsBased.MoreThanAndEqual:
                if (number1 + number2 >= target)
                {
                    return true;
                }
                break;
            case JudgmentsBased.LessThanAndEqual:
                if (number1 + number2 <= target)
                {
                    return true;
                }
                break;
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

    public bool CanReachTargetNumberSub(int number1, int number2, int target, JudgmentsBased judgmentsBased)
    {
        switch (judgmentsBased)
        {
            case JudgmentsBased.MoreThan:
                if (number1 - number2 > target || number2 - number1 > target)
                {
                    return true;
                }
                break;
            case JudgmentsBased.LessThan:
                if (number1 - number2 < target || number2 - number1 < target)
                {
                    return true;
                }
                break;
            case JudgmentsBased.Equal:
                if (number1 - number2 == target || number2 - number1 == target)
                {
                    return true;
                }
                break;
            case JudgmentsBased.MoreThanAndEqual:
                if (number1 - number2 >= target || number2 - number1 >= target)
                {
                    return true;
                }
                break;
            case JudgmentsBased.LessThanAndEqual:
                if (number1 - number2 <= target || number2 - number1 <= target)
                {
                    return true;
                }
                break;
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

    public bool CanReachTargetNumberMultiply(int number1, int number2, int target, JudgmentsBased judgmentsBased)
    {
        switch (judgmentsBased)
        {
            case JudgmentsBased.MoreThan:
                if (number1 * number2 > target)
                {
                    return true;
                }
                break;
            case JudgmentsBased.LessThan:
                if (number1 * number2 < target)
                {
                    return true;
                }
                break;
            case JudgmentsBased.Equal:
                if (number1 * number2 == target)
                {
                    return true;
                }
                break;
            case JudgmentsBased.MoreThanAndEqual:
                if (number1 * number2 >= target)
                {
                    return true;
                }
                break;
            case JudgmentsBased.LessThanAndEqual:
                if (number1 * number2 <= target)
                {
                    return true;
                }
                break;
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

    public bool CanReachTargetNumberDivision(int number1, int number2, int target, JudgmentsBased judgmentsBased)
    {
        switch (judgmentsBased)
        {
            case JudgmentsBased.MoreThan:
                if (number2 != 0 && (number1 / number2 > target || number2 / number1 > target))
                {
                    return true;
                }
                break;
            case JudgmentsBased.LessThan:
                if (number2 != 0 && (number1 / number2 < target || number2 / number1 < target))
                {
                    return true;
                }
                break;
            case JudgmentsBased.Equal:
                if (number2 != 0 && (number1 / number2 == target || number2 / number1 == target))
                {
                    return true;
                }
                break;
            case JudgmentsBased.MoreThanAndEqual:
                if (number2 != 0 && (number1 / number2 >= target || number2 / number1 >= target))
                {
                    return true;
                }
                break;
            case JudgmentsBased.LessThanAndEqual:
                if (number2 != 0 && (number1 / number2 <= target || number2 / number1 <= target))
                {
                    return true;
                }
                break;
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

    public bool CanReachTargetNumberMix(int number1, int number2, int target, JudgmentsBased judgmentsBased)
    {
        
        switch (judgmentsBased)
        {
            case JudgmentsBased.MoreThan:
                // 判断两个数相加是否等于目标数
                if (number1 + number2 > target)
                {
                    return true;
                }

                // 判断两个数相减是否等于目标数
                if (number1 - number2 > target || number2 - number1 > target)
                {
                    return true;
                }

                // 判断两个数相乘是否等于目标数
                if (number1 * number2 > target)
                {
                    return true;
                }

                // 判断两个数相除是否等于目标数，并确保没有除以零的情况
                if (number2 != 0 && (number1 / number2 > target || number2 / number1 > target))
                {
                    return true;
                }

                // 如果以上条件都不满足，则无法通过加、减、乘、除得到目标数
                break;
            case JudgmentsBased.LessThan:
                // 判断两个数相加是否等于目标数
                if (number1 + number2 < target)
                {
                    return true;
                }

                // 判断两个数相减是否等于目标数
                if (number1 - number2 < target || number2 - number1 < target)
                {
                    return true;
                }

                // 判断两个数相乘是否等于目标数
                if (number1 * number2 < target)
                {
                    return true;
                }

                // 判断两个数相除是否等于目标数，并确保没有除以零的情况
                if (number2 != 0 && (number1 / number2 < target || number2 / number1 < target))
                {
                    return true;
                }

                // 如果以上条件都不满足，则无法通过加、减、乘、除得到目标数
                break;
            case JudgmentsBased.Equal:
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
                
                break;
            case JudgmentsBased.MoreThanAndEqual:
                // 判断两个数相加是否等于目标数
                if (number1 + number2 >= target)
                {
                    return true;
                }

                // 判断两个数相减是否等于目标数
                if (number1 - number2 >= target || number2 - number1 >= target)
                {
                    return true;
                }

                // 判断两个数相乘是否等于目标数
                if (number1 * number2 >= target)
                {
                    return true;
                }

                // 判断两个数相除是否等于目标数，并确保没有除以零的情况
                if (number2 != 0 && (number1 / number2 >= target || number2 / number1 >= target))
                {
                    return true;
                }

                // 如果以上条件都不满足，则无法通过加、减、乘、除得到目标数
                break;
            case JudgmentsBased.LessThanAndEqual:
                // 判断两个数相加是否等于目标数
                if (number1 + number2 <= target)
                {
                    return true;
                }

                // 判断两个数相减是否等于目标数
                if (number1 - number2 <= target || number2 - number1 <= target)
                {
                    return true;
                }

                // 判断两个数相乘是否等于目标数
                if (number1 * number2 <= target)
                {
                    return true;
                }

                // 判断两个数相除是否等于目标数，并确保没有除以零的情况
                if (number2 != 0 && (number1 / number2 <= target || number2 / number1 <= target))
                {
                    return true;
                }

                // 如果以上条件都不满足，则无法通过加、减、乘、除得到目标数
                break;
        }
        
        return false;
    }
    
    #endregion
}