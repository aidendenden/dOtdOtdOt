using UnityEngine;

public class DestroyTrigger : MonoBehaviour
{
    public GameObject[] gameObjects;  // Ҫ������ٵ���Ϸ����
    public Animator animator;

    private bool a = true;
   

    private void Update()
    {
        // ���ÿ����Ϸ�����Ƿ�����    
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