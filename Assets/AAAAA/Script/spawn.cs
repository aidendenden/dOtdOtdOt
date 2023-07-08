using UnityEngine;

public class spawn : MonoBehaviour
{
    public GameObject objectPrefab; // 用于生成的物体预制体

    public GameObject father;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 在父对象的位置生成一个新的物体
            GameObject newObject = Instantiate(objectPrefab, transform.position, Quaternion.identity);
            // 将生成的物体设置为父对象的子对象
            newObject.transform.parent = father.transform;
        }
    }
}

