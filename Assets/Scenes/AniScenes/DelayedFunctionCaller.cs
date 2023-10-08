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
        // �������������Ҫִ�еĺ���
        animator.SetTrigger("Change");
    }
}