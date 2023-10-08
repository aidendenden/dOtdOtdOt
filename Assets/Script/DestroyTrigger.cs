using UnityEngine;

public class DestroyTrigger : MonoBehaviour
{
    public GameObject[] gameObjects;  // 要检测销毁的游戏物体
    public Animator animator;

    private bool a = true;
   

    private void Update()
    {
        // 检查每个游戏物体是否被销毁    
        bool allDestroyed = true;
        foreach (GameObject obj in gameObjects)
        {
            if (obj != null)
            {
                allDestroyed = false;
                break;
            }
        }

        if (allDestroyed&&a)
        {
            animator.SetTrigger("Change");
            a = false;
            
        }
    }
}