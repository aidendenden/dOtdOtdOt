using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CircleTimerSprite : MonoBehaviour
{
    [Header("倒计时时长")] public float duration = 10;

    public SpriteRenderer tailCapImage;

    public float CurrentTime { get; private set; }

    [Header("倒计时结束，事件绑定")] [Tooltip("如果倒计时结束去做什么")]
    public UnityEvent didFinishedTimerTime;

    public bool isPaused = true;

    public float _fillAmount = 0f;
    
    public float fillAmount
    {
        get { return _fillAmount; }
        set { _fillAmount = Mathf.Clamp01(value); }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
           // Debug.Log("www21212");
            CurrentTime += Time.deltaTime;

            if (CurrentTime >= duration)
            {
                isPaused = true;
                CurrentTime = duration;
                didFinishedTimerTime.Invoke();
            }

            fillAmount = (duration - CurrentTime) / duration;

            UpdateUI();
        }
    }

    void showCapImage(bool isShow)
    {
        tailCapImage.enabled = isShow;
    }

    public void UpdateUI()
    {
        showCapImage(true);

        if (fillAmount == 0f)
        {
            showCapImage(false);
        }
        else
        {
            showCapImage(true);
        }

        Vector3 capRotaionValue = Vector3.zero;
        capRotaionValue.z = 360 * (1 - fillAmount);
        tailCapImage.transform.localRotation = Quaternion.Euler(capRotaionValue);
    }

    public void PauseTimer()
    {
        isPaused = true;
    }

    public void StartTimer()
    {
       
        isPaused = false;
        
    }

    public void StopTimer()
    {
        CurrentTime = 0;
        //AfterImageTime = 0;
        isPaused = true;
        ResetTimer();
    }

    void ResetTimer()
    {
        fillAmount = 1;
    }
}