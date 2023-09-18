using UnityEngine;

public class CollisionDetectionS : MonoBehaviour
{
    public GameObject objectToSpawn; // Ҫ���ɵ�����
    public string targetTag = "SpecialTag"; // �ض���ǩ
    public float spawnInterval = 1f; // ���ɼ��

    private float timer = 0f; // ��ʱ��

    private void Update()
    {
        timer += Time.deltaTime; // ÿ֡���¼�ʱ��
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (timer >= spawnInterval && collision.gameObject.CompareTag(targetTag))
        {
            Vector3 spawnPosition = collision.transform.position;
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            timer = 0f; // ���ü�ʱ��
        }
    }

}