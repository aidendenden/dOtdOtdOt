using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Transform target; // Ŀ������

    private void Update()
    {
        // ����Ŀ�������ڵ�ǰ��������ϵ�µ�λ��
        Vector3 targetPosition = transform.InverseTransformPoint(target.position);

        // ����Ŀ�������ڵ�ǰ��������ϵ�µ���ת�Ƕ�
        float angle = Mathf.Atan2(targetPosition.x, targetPosition.z) * Mathf.Rad2Deg;

        // �����������ת�Ƕȣ�ֻ�ı�Y����ת
        transform.rotation = Quaternion.Euler(57f, angle, 0);
    }
}