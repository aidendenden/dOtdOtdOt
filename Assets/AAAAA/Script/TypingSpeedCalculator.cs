using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TypingSpeedCalculator : MonoBehaviour
{
    public float timeThreshold = 0.5f; // 两次按键之间的时间阈值（秒）
    
    private float lastKeyPressTime; // 上一次按键事件的时间戳

    void Update()
    {
        if (Input.anyKeyDown)
        {
            float currentKeyPressTime = Time.time; // 获取当前时间戳
            float timeSinceLastKeyPress = currentKeyPressTime - lastKeyPressTime; // 计算距离上一次按键事件的时间差

            if (timeSinceLastKeyPress <= timeThreshold)
            {
                Debug.Log("连续按键间隔小于时间阈值：" + timeSinceLastKeyPress + " 秒");
            }
            else
            {
                Debug.Log("按键事件：" + currentKeyPressTime);
            }

            lastKeyPressTime = currentKeyPressTime; // 更新上一次按键事件的时间戳
        }
    }
}
