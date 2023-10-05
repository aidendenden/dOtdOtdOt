using UnityEngine;

public class ExplosionS : MonoBehaviour
{
    public float explosionForce = 10f; // 爆炸力量
    public float explosionRadius = 5f; // 爆炸半径

    private void Start()
    {
        // 在物体生成时触发爆炸效果
        Explode();
    }

    private void Explode()
    {
        // 获取爆炸范围内的所有碰撞体
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        // 遍历所有碰撞体
        foreach (Collider collider in colliders)
        {
            // 获取碰撞体上的刚体组件
            Rigidbody rb = collider.GetComponent<Rigidbody>();

            // 如果存在刚体组件，则给它施加爆炸力
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
    }
}