using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public float force = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // ���㵯������
            Vector3 direction = transform.position - collision.transform.position;
            direction.Normalize();

            // ʩ����ʹ��ҵ���
            rb.AddForce(direction * force, ForceMode.Impulse);
        }
    }
}