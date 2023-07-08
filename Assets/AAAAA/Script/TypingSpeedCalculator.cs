using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypingSpeedCalculator : MonoBehaviour
{
    public float timeThreshold = 0.5f; // 两次按键之间的时间阈值（秒）

    public KeyCode inputNow;
    public Vector2 InputSt;
    public Vector2 InputEd;
    public bool isLianXueIng = false;
    public Vector2 HuaDongFangXiang;
    public DotCilck _dotCilck;
    public Vector2 nowScreenPosition;

    private float lastKeyPressTime; // 上一次按键事件的时间戳
    List<bool> inputList = new List<bool>();

    public int playerFraction;

    #region 事件

    public delegate void KeyDownDelegate(string message);

    public static event KeyDownDelegate ContinuousStartDelegate;
    public static event KeyDownDelegate ContinuousUpdateDelegate;
    public static event KeyDownDelegate ContinuousEndDelegate;

    public void StartDelegate(string message)
    {
        Debug.Log("StartDelegate: " + message);
        if (ContinuousStartDelegate != null)
            ContinuousStartDelegate(message);
    }

    public void AddListenerStartDelegate(KeyDownDelegate listener)
    {
        ContinuousStartDelegate += listener;
    }

    public void RemoveListenerStartDelegate(KeyDownDelegate listener)
    {
        ContinuousStartDelegate -= listener;
    }

    public void UpdateDelegate(string message)
    {
        Debug.Log("UpdateDelegate: " + message);
        if (ContinuousUpdateDelegate != null)
            ContinuousUpdateDelegate(message);
    }

    public void AddListenerUpdateDelegate(KeyDownDelegate listener)
    {
        ContinuousUpdateDelegate += listener;
    }

    public void RemoveListenerUpdateDelegate(KeyDownDelegate listener)
    {
        ContinuousUpdateDelegate -= listener;
    }


    public void EndDelegate(string message)
    {
        Debug.Log("EndDelegate: " + message);
        if (ContinuousEndDelegate != null)
            ContinuousEndDelegate(message);
    }

    public void AddListenerEndDelegate(KeyDownDelegate listener)
    {
        ContinuousEndDelegate += listener;
    }

    public void RemoveListenerEndDelegate(KeyDownDelegate listener)
    {
        ContinuousEndDelegate -= listener;
    }

    #endregion

    private void Start()
    {
        //_dotCilck = GameObject.FindGameObjectWithTag("Dot").GetComponent<DotCilck>();
        inputList.Add(false);
        inputList.Add(false);
        AddListenerEndDelegate(delegate(string message) { LeftHuaDot(); });
        AddListenerUpdateDelegate(delegate(string message) { scoreCalculation(); });

        //ContinuousEndDelegate += _dotCilck.HuaDongDot();
    }

    public void scoreCalculation()
    {
        playerFraction++;
        if (playerFraction >= 1000)
        {
            playerFraction = 0;
            _dotCilck.DianJiJinDu++;
            Debug.Log(_dotCilck.DianJiJinDu);
        }

        Debug.Log(playerFraction);
    }

    public void LeftHuaDot()
    {
        if (HuaDongFangXiang.x < 0)
        {
            _dotCilck.HuaDongDotL();
        }

        if (HuaDongFangXiang.x > 0)
        {
            _dotCilck.HuaDongDotR();
        }
    }

    void Update()
    {
        if (Input.anyKeyDown) // 检测是否有按键事件发生
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(MyKeyCode))) // 遍历所有 KeyCode 枚举值
            {
                if (Input.GetKeyDown(keyCode)) // 检测按键是否被按下
                {
                    inputNow = keyCode;
                    IsInputFast();
                    isKeyInputNoInput();
                    inputACheck();
                    Debug.Log(inputNow); // 添加按下的键到键列表中
                }
            }
        }
        //Debug.Log(inputList[0]+"|"+ inputList[1]);
    }


    public bool IsInputFast()
    {
        float currentKeyPressTime = Time.time; // 获取当前时间戳
        float timeSinceLastKeyPress = currentKeyPressTime - lastKeyPressTime; // 计算距离上一次按键事件的时间差

        if (timeSinceLastKeyPress <= timeThreshold)
        {
            // Debug.Log("连续按键间隔小于时间阈值：" + timeSinceLastKeyPress + " 秒");
            lastKeyPressTime = currentKeyPressTime; // 更新上一次按键事件的时间戳
            inputList.RemoveAt(0);
            inputList.Add(true);
            return true;
        }
        else
        {
            // Debug.Log("按键事件：" + currentKeyPressTime);
            lastKeyPressTime = currentKeyPressTime; // 更新上一次按键事件的时间戳
            inputList.RemoveAt(0);
            inputList.Add(false);
            return false;
        }
        //inputList.RemoveAt(0);
        //inputList.Add(false);
    }


    private float idleTime = 0f;
    private float idleThreshold = 0.2f; // 设置空闲阈值

    private void isKeyInputNoInput()
    {
        if (Input.anyKey)
        {
            idleTime = 0f; // 如果有键盘输入，重置空闲时间
        }

        else
        {
            idleTime += Time.deltaTime; // 如果没有键盘输入，增加空闲时间
        }

        if (idleTime >= idleThreshold)
        {
            // Debug.Log("No keyboard input for " + idleThreshold + " seconds.");
            inputList.RemoveAt(0);
            inputList.Add(false);
            //inputNow = KeyCode.Alpha1;
            // InputSt = new Vector2(0, 0);
            // InputEd = new Vector2(0, 0);
        }
    }

    private void inputACheck()
    {
        if (inputList[0] == false && inputList[1] == true)
        {
            Debug.Log("连续开始");

            //StartCoroutine("kaishi");

            InputSt = KeyCodeToV(inputNow);

            nowScreenPosition = GetScreenCoordinates(InputSt);
        }

        if (inputList[0] == true && inputList[1] == true)
        {
            Debug.Log("连续中");
            UpdateDelegate("chixuzho");
        }

        if (inputList[0] == true && inputList[1] == false)
        {
            if (!Input.anyKey)
            {
                Debug.Log("连续结束");


                isLianXueIng = false;
                InputEd = KeyCodeToV(inputNow);
                EndDelegate("aaa");
            }
        }

        Vector2 d = InputEd - InputSt;
        PanDuanFangXiang(d);
    }

    public Vector2 GetScreenCoordinates(Vector2 keyboardPosition)
    {
        Vector2 screenPixelPosition = default;
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;
        // float screenDpi = Screen.dpi;
        // ScreenOrientation screenOrientation = Screen.orientation;

        screenPixelPosition.x = keyboardPosition.x / 11 * screenWidth;
        screenPixelPosition.y = keyboardPosition.y / 4 * screenHeight;

        return screenPixelPosition;
    }

    public Vector2 KeyCodeToV(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Alpha1:
                return new Vector2(0, 3);
            case KeyCode.Alpha2:
                return new Vector2(1, 3);
            case KeyCode.Alpha3:
                return new Vector2(2, 3);
            case KeyCode.Alpha4:
                return new Vector2(3, 3);
            case KeyCode.Alpha5:
                return new Vector2(4, 3);
            case KeyCode.Alpha6:
                return new Vector2(5, 3);
            case KeyCode.Alpha7:
                return new Vector2(6, 3);
            case KeyCode.Alpha8:
                return new Vector2(7, 3);
            case KeyCode.Alpha9:
                return new Vector2(8, 3);
            case KeyCode.Alpha0:
                return new Vector2(10, 3);
            case KeyCode.Q:
                return new Vector2(0, 2);
            case KeyCode.W:
                return new Vector2(1, 2);
            case KeyCode.E:
                return new Vector2(2, 2);
            case KeyCode.R:
                return new Vector2(3, 2);
            case KeyCode.T:
                return new Vector2(4, 2);
            case KeyCode.Y:
                return new Vector2(5, 2);
            case KeyCode.U:
                return new Vector2(6, 2);
            case KeyCode.I:
                return new Vector2(7, 2);
            case KeyCode.O:
                return new Vector2(8, 2);
            case KeyCode.P:
                return new Vector2(10, 2);
            case KeyCode.A:
                return new Vector2(0, 1);
            case KeyCode.S:
                return new Vector2(1, 1);
            case KeyCode.D:
                return new Vector2(2, 1);
            case KeyCode.F:
                return new Vector2(3, 1);
            case KeyCode.G:
                return new Vector2(5, 1);
            case KeyCode.H:
                return new Vector2(7, 1);
            case KeyCode.J:
                return new Vector2(8, 1);
            case KeyCode.K:
                return new Vector2(9, 1);
            case KeyCode.L:
                return new Vector2(10, 1);
            case KeyCode.Z:
                return new Vector2(0, 0);
            case KeyCode.X:
                return new Vector2(1, 0);
            case KeyCode.C:
                return new Vector2(2, 0);
            case KeyCode.V:
                return new Vector2(5, 0);
            case KeyCode.B:
                return new Vector2(8, 0);
            case KeyCode.N:
                return new Vector2(9, 0);
            case KeyCode.M:
                return new Vector2(10, 0);

            default: return new Vector2(0, 0);
        }
    }


    void PanDuanFangXiang(Vector2 D)
    {
        if (D.x < 0)
        {
            HuaDongFangXiang.x = D.x;
            Debug.Log("向左");
        }

        if (D.x > 0)
        {
            HuaDongFangXiang.x = D.x;
            Debug.Log("向右");
        }

        if (D.x == 0)
        {
            HuaDongFangXiang.x = D.x;
            Debug.Log("X为0");
        }


        if (D.y < 0)
        {
            HuaDongFangXiang.y = D.y;
            Debug.Log("向下");
        }

        if (D.y > 0)
        {
            HuaDongFangXiang.y = D.y;
            Debug.Log("向上");
        }

        if (D.y == 0)
        {
            HuaDongFangXiang.y = D.y;
            Debug.Log("y为0");
        }
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