using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class Test : MonoBehaviour
{
    public List<KeyCode> targetKeys;

    private List<KeyCode> pressedKeys = new List<KeyCode>();

    void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode targetKey in targetKeys)
            {
                if (Input.GetKeyDown(targetKey))
                {
                    pressedKeys.Add(targetKey);
                }
            }
        }

        if (pressedKeys.Count == targetKeys.Count && targetKeys.Count != 0)
        {
           
            // 包含了所有目标按键位置
            for (int i = 0; i < pressedKeys.Count; i++)
            {
                string keyString = Enum.GetName(typeof(KeyCode), pressedKeys[i]);
                Debug.Log(keyString);
            }
            JudgmentButton();
            //Array.Clear(keylist, 0, keylist.Length); 
           
        }
    
    }

    public void JudgmentButton()
    {
        char[] myArray = pressedKeys.ConvertAll(c => (char)c).ToArray();
        char[] targetChars = targetKeys.ConvertAll(c => (char)c).ToArray();
        bool containsTargetChars = ContainsChars(myArray, targetChars);
        Debug.Log(containsTargetChars); // 输出 true
        if (containsTargetChars)
        {
            pressedKeys.Clear();
        }
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