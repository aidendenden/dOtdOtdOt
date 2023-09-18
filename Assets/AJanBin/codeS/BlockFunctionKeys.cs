using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class BlockFunctionKeys : MonoBehaviour
{
    public KeyCode targetKey = KeyCode.Escape;
    public int targetCount = 3;
    public float timeLimit = 1f;

    private int keyCount = 0;
    private float timer = 0f;
    
    // Windows API 导入
    [DllImport("user32.dll")]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll")]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("user32.dll")]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    // 钩子回调函数委托
    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    // 钩子ID
    private const int WH_KEYBOARD_LL = 13;

    // 功能键的虚拟键码
    private const int VK_LWIN = 0x5B;
    private const int VK_RWIN = 0x5C;

    // 钩子句柄
    private IntPtr hookHandle = IntPtr.Zero;

    // 控制开关状态
    public bool blockFunctionKeys = true;

    // 键盘钩子回调函数
    private IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        // 屏蔽左右Win键
        if (blockFunctionKeys && nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
        {
            int vkCode = Marshal.ReadInt32(lParam);
            if (vkCode == VK_LWIN || vkCode == VK_RWIN)
            {
                return (IntPtr)1; // 返回非零值，表示拦截该按键
            }
        }

        return CallNextHookEx(hookHandle, nCode, wParam, lParam);
    }

    // Unity 游戏开始时调用
    private void Start()
    {
        // 安装键盘钩子
        hookHandle = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookCallback, GetModuleHandle(null), 0);
    }
    


    private void Update()
    {
        if (Input.GetKeyDown(targetKey))
        {
            if (timer <= 0f || keyCount >= targetCount || Time.time - timer > timeLimit)
            {
                // 重置计数器和计时器
                keyCount = 0;
                timer = Time.time;
            }

            keyCount++;

            if (keyCount >= targetCount)
            {
                // 执行您的操作
                UpdateBlockFunctionKeys(!blockFunctionKeys);
                Debug.Log("Key " + targetKey.ToString() + " pressed " + targetCount + " times within " + timeLimit + " seconds.");
                keyCount = 0; // 重置计数器
                timer = 0f; // 重置计时器
            }
        }
    }
    

    // Unity 游戏结束时调用
    private void OnApplicationQuit()
    {
        // 卸载键盘钩子
        if (hookHandle != IntPtr.Zero)
        {
            UnhookWindowsHookEx(hookHandle);
        }
    }

    // 更新屏蔽功能键的状态
    public void UpdateBlockFunctionKeys(bool block)
    {
        blockFunctionKeys = block;
    }

    // Windows消息常量
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_SYSKEYDOWN = 0x0104;
}