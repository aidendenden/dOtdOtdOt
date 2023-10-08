using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public float timeToTrigger = 3f;  // 在区域内停留的时间
    public string functionName;  // 要调用的函数名称
    public Animator animator;

    private bool isPlayerInside = false;  // 玩家是否在区域内
    private float timer = 0f;  // 计时器

    private void Update()
    {
        if (isPlayerInside)
        {
            // 如果玩家在区域内，则开始计时
            timer += Time.deltaTime;
            Debug.Log(timer);
            if (timer >= timeToTrigger)
            {
                // 如果计时器达到设定时间，则调用指定的函数
                

                // 重置计时器和玩家状态
                timer = 0f;
                isPlayerInside = false;
                animator.SetTrigger("Change");
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("aaaa");
        if (collision.CompareTag("Player"))
        {
            // 如果玩家进入区域，则设置玩家状态为在区域内
            isPlayerInside = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("bbbb");
        if (collision.CompareTag("Player"))
        {
            // 如果玩家进入区域，则设置玩家状态为在区域内
            isPlayerInside = false;
        }
    }
}