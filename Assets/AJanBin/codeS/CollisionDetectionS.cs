using UnityEngine;

public class CollisionDetectionS : MonoBehaviour
{
    public GameObject objectToSpawn; // 要生成的物体
    public string targetTag = "Hand"; // 特定标签
    public float spawnInterval = 1f; // 生成间隔
    public Timer _timer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_timer.timer >= spawnInterval && collision.gameObject.CompareTag(targetTag))
        {
            Vector3 spawnPosition = collision.transform.position;
            GameObject dough =Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            dough.GetComponent<CollisionDetectionS>()._timer = _timer;
            dough.GetComponent<CollisionDetectionS>().objectToSpawn = objectToSpawn;
            _timer.timer = 0f; // 重置计时器
        }
    }

}