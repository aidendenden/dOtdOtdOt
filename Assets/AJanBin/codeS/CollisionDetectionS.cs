using UnityEngine;

public class CollisionDetectionS : MonoBehaviour
{
    public GameObject objectToSpawn; // 要生成的物体
    public string targetTag = "SpecialTag"; // 特定标签
    public float spawnInterval = 1f; // 生成间隔

    private float timer = 0f; // 计时器

    private void Update()
    {
        timer += Time.deltaTime; // 每帧更新计时器
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (timer >= spawnInterval && collision.gameObject.CompareTag(targetTag))
        {
            Vector3 spawnPosition = collision.transform.position;
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            timer = 0f; // 重置计时器
        }
    }

}