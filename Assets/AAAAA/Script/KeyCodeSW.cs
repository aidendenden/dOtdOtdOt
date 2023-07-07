using System.Collections;
using System.Collections.Generic;
using UnityEngine;






public class KeyCodeSW : MonoBehaviour
{

    public Vector2 KeyCodeToV(KeyCode key)
    {
        
        switch (key)
        {
            case KeyCode.Alpha1:
                return  new Vector2(0, 0);
                break;
            case KeyCode.Alpha2:
                return new Vector2(1, 0);
                break;
            case KeyCode.Alpha3:
                return new Vector2(2, 0);
                break;
            case KeyCode.Alpha4:
                return new Vector2(3, 0);
                break;
            case KeyCode.Alpha5:
                return new Vector2(4, 0);
                break;
            case KeyCode.Alpha6:
                return new Vector2(5, 0);
                break;
            case KeyCode.Alpha7:
                return new Vector2(6, 0);
                break;
            case KeyCode.Alpha8:
                return new Vector2(7, 0);
                break;
            case KeyCode.Alpha9:
                return new Vector2(8, 0);
                break;
            case KeyCode.Alpha0:
                return new Vector2(9, 0);
                break;
            case KeyCode.Q:
                return new Vector2(1, -1);
                break;
            case KeyCode.W:
                return new Vector2(2, -1);
                break;
            case KeyCode.E:
                return new Vector2(3, -1);
                break;
            case KeyCode.R:
                return new Vector2(4, -1);
                break;
            case KeyCode.T:
                return new Vector2(5,-1);
                break;
            case KeyCode.Y:
                return new Vector2(6, -1);
                break;
            case KeyCode.U:
                return new Vector2(7,-1);
                break;
            case KeyCode.I:
                return new Vector2(8, -1);
                break;
            case KeyCode.O:
                return new Vector2(9,-1);
                break;
            case KeyCode.P:
                return new Vector2(10,-1);
                break;
            case KeyCode.A:
                return new Vector2(1, -2);
                break;
            case KeyCode.S:
                return new Vector2(2, -2);
                break;
            case KeyCode.D:
                return new Vector2(3, -2);
                break;
            case KeyCode.F:
                return new Vector2(4, -2);
                break;
            case KeyCode.G:
                return new Vector2(5, -2);
                break;
            case KeyCode.H:
                return new Vector2(6, -2);
                break;
            case KeyCode.J:
                return new Vector2(7, -2);
                break;
            case KeyCode.K:
                return new Vector2(8, -2);
                break;
            case KeyCode.L:
                return new Vector2(9, -2);
                break;
            case KeyCode.Z:
                return new Vector2(1, -3);
                break;
            case KeyCode.X:
                return new Vector2(2, -3);
                break;
            case KeyCode.C:
                return new Vector2(3, -3);
                break;
            case KeyCode.V:
                return new Vector2(4, -3);
                break;
            case KeyCode.B:
                return new Vector2(5, -3);
                break;
            case KeyCode.N:
                return new Vector2(6, -3);
                break;
            case KeyCode.M:
                return new Vector2(7, -3);
                break;






            default: return new Vector2(0,0);
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

    private void Start()
    {
        test();
    }
}
