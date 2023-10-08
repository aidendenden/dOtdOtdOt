using UnityEngine;

public class ButtonClickK : MonoBehaviour
{
    public GameObject prefab;  // 预制体

    public Transform transformOne;
    public Transform transformTwo;
    public float generationFrequency = 2f;  // 生成频率

    private float timer = 0f;  // 计时器


    private void Start()
    {
        // 初始化预制体位置
        prefab.transform.position = GetRandomPosition();
    }

    public void OnButtonClick()
    {
        // 点击按钮时生成新的预制体
        GameObject newPrefab = Instantiate(prefab);
        newPrefab.transform.position = GetRandomPosition();
    }

    private Vector3 GetRandomPosition()
    {
        // 在区间范围内生成随机位置
        float x = Random.Range(transformOne.position.x, transformTwo.position.x);
        float y = Random.Range(transformOne.position.y, transformTwo.position.y);
      
        return new Vector2(x, y);
    }


    private void Update()
    {
        // 更新计时器
        timer += Time.deltaTime;

        // 当计时器超过生成频率时生成预制体
        if (timer >= generationFrequency)
        {
            GameObject newPrefab = Instantiate(prefab);
            newPrefab.transform.position = GetRandomPosition();
            timer = 0f;  // 重置计时器
        }
    }

}