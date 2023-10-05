using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float destroyDelay = 3f; // 延迟销毁的时间

    private void Start()
    {
        // 在指定的延迟时间后销毁游戏对象
        Destroy(gameObject, destroyDelay);
    }
}
