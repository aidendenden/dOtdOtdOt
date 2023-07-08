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

    private float lastKeyPressTime; // 上一次按键事件的时间戳
    List<bool> inputList = new List<bool>();

    public delegate void KeyDownDelegate(string message);
    public static event KeyDownDelegate ContinuousStartDelegate;
    public static event KeyDownDelegate ContinuousUpdateDelegate;
    public static event KeyDownDelegate ContinuousEndDelegate;

    public void StartDelegate(string message) {
        Debug.Log("StartDelegate: " + message);
        if (ContinuousStartDelegate != null)
            ContinuousStartDelegate(message);
    }

    public void AddListenerStartDelegate(KeyDownDelegate listener) {
        ContinuousStartDelegate += listener;
    }

    public void RemoveListenerStartDelegate(KeyDownDelegate listener) {
        ContinuousStartDelegate -= listener;
    }
    
    public void UpdateDelegate(string message) {
        Debug.Log("UpdateDelegate: " + message);
        if (ContinuousUpdateDelegate != null)
            ContinuousUpdateDelegate(message);
    }

    public void AddListenerUpdateDelegate(KeyDownDelegate listener) {
        ContinuousUpdateDelegate += listener;
    }

    public void RemoveListenerUpdateDelegate(KeyDownDelegate listener) {
        ContinuousUpdateDelegate -= listener;
    }
    
    
    public void EndDelegate(string message) {
        Debug.Log("EndDelegate: " + message);
        if (ContinuousEndDelegate != null)
            ContinuousEndDelegate(message);
    }

    public void AddListenerEndDelegate(KeyDownDelegate listener) {
        ContinuousEndDelegate += listener;
    }

    public void RemoveListenerEndDelegate(KeyDownDelegate listener) {
        ContinuousEndDelegate -= listener;
    }
 

    private void Start()
    {
        inputList.Add(false);
        inputList.Add(false);
    }

    void Update()
    {
        NowKeyDown();
        Debug.Log(inputNow);
        IsInputFast();
        isKeyInputNoInput();
        //Debug.Log(inputList[0]+"|"+ inputList[1]);
        inputACheck();
    }


    public bool IsInputFast()
    {
        if (Input.anyKeyDown)
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
        }

        //inputList.RemoveAt(0);
        //inputList.Add(false);
        return false;
    }


    private float idleTime = 0f;
    private float idleThreshold = 0.5f; // 设置空闲阈值为5秒

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
            
            InputSt = KeyCodeToV(inputNow);
        }

        if (inputList[0] == true && inputList[1] == true)
        {
         AddListenerStartDelegate(delegate(string message) {  });
        }

        if (inputList[0] == true && inputList[1] == false)
        {
          
            InputEd = KeyCodeToV(inputNow);
        }

        Vector2 d = InputEd - InputSt;
        PanDuanFangXiang(d);
    }

    
    
    public Vector2 KeyCodeToV(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Alpha1:
                return new Vector2(0, 0);

            case KeyCode.Alpha2:
                return new Vector2(1, 0);

            case KeyCode.Alpha3:
                return new Vector2(2, 0);

            case KeyCode.Alpha4:
                return new Vector2(3, 0);

            case KeyCode.Alpha5:
                return new Vector2(4, 0);

            case KeyCode.Alpha6:
                return new Vector2(5, 0);

            case KeyCode.Alpha7:
                return new Vector2(6, 0);

            case KeyCode.Alpha8:
                return new Vector2(7, 0);

            case KeyCode.Alpha9:
                return new Vector2(8, 0);

            case KeyCode.Alpha0:
                return new Vector2(9, 0);

            case KeyCode.Q:
                return new Vector2(1, -1);

            case KeyCode.W:
                return new Vector2(2, -1);

            case KeyCode.E:
                return new Vector2(3, -1);

            case KeyCode.R:
                return new Vector2(4, -1);

            case KeyCode.T:
                return new Vector2(5, -1);

            case KeyCode.Y:
                return new Vector2(6, -1);

            case KeyCode.U:
                return new Vector2(7, -1);

            case KeyCode.I:
                return new Vector2(8, -1);

            case KeyCode.O:
                return new Vector2(9, -1);

            case KeyCode.P:
                return new Vector2(10, -1);

            case KeyCode.A:
                return new Vector2(1, -2);

            case KeyCode.S:
                return new Vector2(2, -2);

            case KeyCode.D:
                return new Vector2(3, -2);

            case KeyCode.F:
                return new Vector2(4, -2);

            case KeyCode.G:
                return new Vector2(5, -2);

            case KeyCode.H:
                return new Vector2(6, -2);

            case KeyCode.J:
                return new Vector2(7, -2);

            case KeyCode.K:
                return new Vector2(8, -2);

            case KeyCode.L:
                return new Vector2(9, -2);

            case KeyCode.Z:
                return new Vector2(1, -3);

            case KeyCode.X:
                return new Vector2(2, -3);

            case KeyCode.C:
                return new Vector2(3, -3);

            case KeyCode.V:
                return new Vector2(4, -3);
            case KeyCode.B:
                return new Vector2(5, -3);
            case KeyCode.N:
                return new Vector2(6, -3);

            case KeyCode.M:
                return new Vector2(7, -3);


            default: return new Vector2(0, 0);
        }
    }


    void test()
    {
        Vector2 a = new Vector2(0, 0);

        a = KeyCodeToV(KeyCode.Z) - KeyCodeToV(KeyCode.K);

        PanDuanFangXiang(a);
        Debug.Log(a);
    }

    void PanDuanFangXiang(Vector2 D)
    {
        if (D.x < 0)
        {
            Debug.Log("向左");
        }

        if (D.x > 0)
        {
            Debug.Log("向右");
        }

        if (D.x == 0)
        {
            Debug.Log("X为0");
        }


        if (D.y < 0)
        {
            Debug.Log("向下");
        }

        if (D.y > 0)
        {
            Debug.Log("向上");
        }

        if (D.y == 0)
        {
            Debug.Log("y为0");
        }
    }


    public void NowKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            inputNow = KeyCode.Alpha1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            inputNow = KeyCode.Alpha2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            inputNow = KeyCode.Alpha3;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            inputNow = KeyCode.Alpha4;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            inputNow = KeyCode.Alpha5;
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            inputNow = KeyCode.Alpha6;
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            inputNow = KeyCode.Alpha7;
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            inputNow = KeyCode.Alpha8;
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            inputNow = KeyCode.Alpha9;
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            inputNow = KeyCode.Alpha0;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            inputNow = KeyCode.Q;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            inputNow = KeyCode.W;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            inputNow = KeyCode.E;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            inputNow = KeyCode.R;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            inputNow = KeyCode.T;
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            inputNow = KeyCode.Y;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            inputNow = KeyCode.U;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            inputNow = KeyCode.I;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            inputNow = KeyCode.O;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            inputNow = KeyCode.P;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            inputNow = KeyCode.A;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            inputNow = KeyCode.S;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            inputNow = KeyCode.D;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            inputNow = KeyCode.F;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            inputNow = KeyCode.G;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            inputNow = KeyCode.H;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            inputNow = KeyCode.J;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            inputNow = KeyCode.K;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            inputNow = KeyCode.L;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            inputNow = KeyCode.Z;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            inputNow = KeyCode.X;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            inputNow = KeyCode.C;
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            inputNow = KeyCode.V;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            inputNow = KeyCode.B;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            inputNow = KeyCode.N;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            inputNow = KeyCode.M;
        }
    }
}