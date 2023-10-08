using UnityEngine;

public class DestroyOnTrigger : MonoBehaviour
{
    public string targetTag = "Enemy"; // 目标物体的tag

    private void OnTriggerEnter(Collider other)
    {
        // 检查进入触发器的物体是否具有指定的tag
        if (other.CompareTag(targetTag))
        {
            // 销毁进入触发器的物体
            Destroy(other.gameObject);
        }
    }
}