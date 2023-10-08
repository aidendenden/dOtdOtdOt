using UnityEngine;

public class DestroyOnTrigger : MonoBehaviour
{
    public string targetTag = "Enemy"; // Ŀ�������tag

    private void OnTriggerEnter(Collider other)
    {
        // �����봥�����������Ƿ����ָ����tag
        if (other.CompareTag(targetTag))
        {
            // ���ٽ��봥����������
            Destroy(other.gameObject);
        }
    }
}