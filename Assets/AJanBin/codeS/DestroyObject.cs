using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float destroyDelay = 3f; // �ӳ����ٵ�ʱ��

    private void Start()
    {
        // ��ָ�����ӳ�ʱ���������Ϸ����
        Destroy(gameObject, destroyDelay);
    }
}
