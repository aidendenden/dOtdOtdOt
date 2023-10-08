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
            // 计算弹开方向
            Vector3 direction = transform.position - collision.transform.position;
            direction.Normalize();

            // 施加力使玩家弹开
            rb.AddForce(direction * force, ForceMode.Impulse);
        }
    }
}