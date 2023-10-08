using UnityEngine;

public class ObjectSpawnerThree : MonoBehaviour
{
    public GameObject objectPrefab;
    public float moveSpeed = 5f;

    private void Start()
    {
        SpawnObject();
    }

    private void SpawnObject()
    {
        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(0f, Random.Range(0f, Screen.height), 0f));
        spawnPosition.z = 0f;
        GameObject newObject = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D rb = newObject.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(moveSpeed, 0f);
    }
}