using UnityEngine;

public class ForceDirection : MonoBehaviour
{
    public float forceMagnitude = 10f; // ���Ĵ�С

    // ����˸�����ÿ�������Ӧһ������
    private Vector3[] directions = new Vector3[]
    {
        new Vector3(1f, 0f, 0f), // ��
        new Vector3(1f, 0f, 1f).normalized, // ����
        new Vector3(0f, 0f, 1f), // ��
        new Vector3(-1f, 0f, 1f).normalized, // ����
        new Vector3(-1f, 0f, 0f), // ��
        new Vector3(-1f, 0f, -1f).normalized, // ����
        new Vector3(0f, 0f, -1f), // ��
        new Vector3(1f, 0f, -1f).normalized // ����
    };

    public void ApplyForce(int directionIndex)
    {
        // ���ݷ���������ȡ��Ӧ������
        Vector3 direction = directions[directionIndex];

        // ������ʩ��һ����
        GetComponent<Rigidbody>().AddForce(direction * forceMagnitude, ForceMode.Impulse);
    }
}