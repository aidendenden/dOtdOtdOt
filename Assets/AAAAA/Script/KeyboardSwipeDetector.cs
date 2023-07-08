using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class KeyboardSwipeDetector : MonoBehaviour
{
    public List<string> targetKeyList;

    private List<KeyCode> keyList = new List<KeyCode>(); // 用于存储按下的所有键

    void Update()
    {
        if(Input.anyKeyDown) // 检测是否有按键事件发生
        {
            foreach(KeyCode keyCode in System.Enum.GetValues(typeof(MyKeyCode))) // 遍历所有 KeyCode 枚举值
            {
                if(Input.GetKeyDown(keyCode)) // 检测按键是否被按下
                {
                    keyList.Add(keyCode); // 添加按下的键到键列表中
                }
            }

            for (int i = 0; i < keyList.Count; i++)
            {
                JudgmentButton(i);
            }
           
            Debug.Log(keyList.Count);
            if (keyList.Count>300)
            {
                keyList.RemoveRange(0, 200);
            }
        }
        
    }

  
    
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 30), "Keys pressed: " + string.Join(", ", keyList)); // 在屏幕上显示按下的键列表
    }
    
    
    public void JudgmentButton(int index)
    {
        char[] myArray = keyList.ConvertAll(c => (char)c).ToArray();
        char[] targetChars = targetKeyList[index].ToCharArray();
        bool containsTargetChars = ContainsChars(myArray, targetChars);
        Debug.Log(containsTargetChars); // 输出 true
    }

    static bool ContainsChars(char[] array, char[] targetChars)
    {
        foreach (char c in targetChars)
        {
            if (!array.Contains(c))
            {
                return false;
            }
        }

        return true;
    }
    
}

