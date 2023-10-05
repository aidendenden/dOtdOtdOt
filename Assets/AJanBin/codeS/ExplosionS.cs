using UnityEngine;

public class ExplosionS : MonoBehaviour
{
    public float explosionForce = 10f; // ��ը����
    public float explosionRadius = 5f; // ��ը�뾶

    private void Start()
    {
        // ����������ʱ������ըЧ��
        Explode();
    }

    private void Explode()
    {
        // ��ȡ��ը��Χ�ڵ�������ײ��
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        // ����������ײ��
        foreach (Collider collider in colliders)
        {
            // ��ȡ��ײ���ϵĸ������
            Rigidbody rb = collider.GetComponent<Rigidbody>();

            // ������ڸ�������������ʩ�ӱ�ը��
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
    }
}