using UnityEngine;

public class DelayedFunctionCaller : MonoBehaviour
{
    public Animator animator;
    public float delayTime = 2f;

    private void Start()
    {
        Invoke("CallFunction", delayTime);
    }

    private void CallFunction()
    {
        // 在这里调用你想要执行的函数
        animator.SetTrigger("Change");
    }
}