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
            Debug.Log(keyList.Count);
        }
        
    }

    public void JudgmentButton()
    {
        char[] myArray = keyList.ConvertAll(c => (char)c).ToArray();
        char[] targetChars = targetKeyList[0].ToCharArray();
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
    
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 30), "Keys pressed: " + string.Join(", ", keyList)); // 在屏幕上显示按下的键列表
    }
}

 public enum MyKeyCode
  {
    /// <summary>
    ///   <para>Not assigned (never returned as the result of a keystroke).</para>
    /// </summary>
    None = 0,
    
    Alpha0 = 48, // 0x00000030
    /// <summary>
    ///   <para>The '1' key on the top of the alphanumeric keyboard.</para>
    /// </summary>
    Alpha1 = 49, // 0x00000031
    /// <summary>
    ///   <para>The '2' key on the top of the alphanumeric keyboard.</para>
    /// </summary>
    Alpha2 = 50, // 0x00000032
    /// <summary>
    ///   <para>The '3' key on the top of the alphanumeric keyboard.</para>
    /// </summary>
    Alpha3 = 51, // 0x00000033
    /// <summary>
    ///   <para>The '4' key on the top of the alphanumeric keyboard.</para>
    /// </summary>
    Alpha4 = 52, // 0x00000034
    /// <summary>
    ///   <para>The '5' key on the top of the alphanumeric keyboard.</para>
    /// </summary>
    Alpha5 = 53, // 0x00000035
    /// <summary>
    ///   <para>The '6' key on the top of the alphanumeric keyboard.</para>
    /// </summary>
    Alpha6 = 54, // 0x00000036
    /// <summary>
    ///   <para>The '7' key on the top of the alphanumeric keyboard.</para>
    /// </summary>
    Alpha7 = 55, // 0x00000037
    /// <summary>
    ///   <para>The '8' key on the top of the alphanumeric keyboard.</para>
    /// </summary>
    Alpha8 = 56, // 0x00000038
    /// <summary>
    ///   <para>The '9' key on the top of the alphanumeric keyboard.</para>
    /// </summary>
    Alpha9 = 57, // 0x00000039
    A = 97, // 0x00000061
    /// <summary>
    ///   <para>'b' key.</para>
    /// </summary>
    B = 98, // 0x00000062
    /// <summary>
    ///   <para>'c' key.</para>
    /// </summary>
    C = 99, // 0x00000063
    /// <summary>
    ///   <para>'d' key.</para>
    /// </summary>
    D = 100, // 0x00000064
    /// <summary>
    ///   <para>'e' key.</para>
    /// </summary>
    E = 101, // 0x00000065
    /// <summary>
    ///   <para>'f' key.</para>
    /// </summary>
    F = 102, // 0x00000066
    /// <summary>
    ///   <para>'g' key.</para>
    /// </summary>
    G = 103, // 0x00000067
    /// <summary>
    ///   <para>'h' key.</para>
    /// </summary>
    H = 104, // 0x00000068
    /// <summary>
    ///   <para>'i' key.</para>
    /// </summary>
    I = 105, // 0x00000069
    /// <summary>
    ///   <para>'j' key.</para>
    /// </summary>
    J = 106, // 0x0000006A
    /// <summary>
    ///   <para>'k' key.</para>
    /// </summary>
    K = 107, // 0x0000006B
    /// <summary>
    ///   <para>'l' key.</para>
    /// </summary>
    L = 108, // 0x0000006C
    /// <summary>
    ///   <para>'m' key.</para>
    /// </summary>
    M = 109, // 0x0000006D
    /// <summary>
    ///   <para>'n' key.</para>
    /// </summary>
    N = 110, // 0x0000006E
    /// <summary>
    ///   <para>'o' key.</para>
    /// </summary>
    O = 111, // 0x0000006F
    /// <summary>
    ///   <para>'p' key.</para>
    /// </summary>
    P = 112, // 0x00000070
    /// <summary>
    ///   <para>'q' key.</para>
    /// </summary>
    Q = 113, // 0x00000071
    /// <summary>
    ///   <para>'r' key.</para>
    /// </summary>
    R = 114, // 0x00000072
    /// <summary>
    ///   <para>'s' key.</para>
    /// </summary>
    S = 115, // 0x00000073
    /// <summary>
    ///   <para>'t' key.</para>
    /// </summary>
    T = 116, // 0x00000074
    /// <summary>
    ///   <para>'u' key.</para>
    /// </summary>
    U = 117, // 0x00000075
    /// <summary>
    ///   <para>'v' key.</para>
    /// </summary>
    V = 118, // 0x00000076
    /// <summary>
    ///   <para>'w' key.</para>
    /// </summary>
    W = 119, // 0x00000077
    /// <summary>
    ///   <para>'x' key.</para>
    /// </summary>
    X = 120, // 0x00000078
    /// <summary>
    ///   <para>'y' key.</para>
    /// </summary>
    Y = 121, // 0x00000079
    /// <summary>
    ///   <para>'z' key.</para>
    /// </summary>
    Z = 122, // 0x0000007A
  }
