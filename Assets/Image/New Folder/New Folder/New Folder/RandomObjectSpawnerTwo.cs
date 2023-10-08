using System.Collections;
using UnityEngine;

public class RandomObjectSpawnerTwo : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    public float spawnDelay = 5f;
    public float checkDelay = 1f;

    private void Start()
    {
        InvokeRepeating("SpawnRandomObject", 0f, spawnDelay);
    }

    private void SpawnRandomObject()
    {
        int randomIndex = Random.Range(0, objectPrefabs.Length);
        Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        GameObject newObject = Instantiate(objectPrefabs[randomIndex], spawnPosition, Quaternion.identity);
       // StartCoroutine(CheckObjectExistence(newObject));
    }

    private IEnumerator CheckObjectExistence(GameObject obj)
    {
        while (obj != null)
        {
            yield return new WaitForSeconds(checkDelay);
        }
        yield return new WaitForSeconds(spawnDelay);
        SpawnRandomObject();
    }
}