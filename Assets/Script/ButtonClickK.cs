using UnityEngine;

public class ButtonClickK : MonoBehaviour
{
    public GameObject prefab;  // Ԥ����

    public Transform transformOne;
    public Transform transformTwo;
    public float generationFrequency = 2f;  // ����Ƶ��

    private float timer = 0f;  // ��ʱ��


    private void Start()
    {
        // ��ʼ��Ԥ����λ��
        prefab.transform.position = GetRandomPosition();
    }

    public void OnButtonClick()
    {
        // �����ťʱ�����µ�Ԥ����
        GameObject newPrefab = Instantiate(prefab);
        newPrefab.transform.position = GetRandomPosition();
    }

    private Vector3 GetRandomPosition()
    {
        // �����䷶Χ���������λ��
        float x = Random.Range(transformOne.position.x, transformTwo.position.x);
        float y = Random.Range(transformOne.position.y, transformTwo.position.y);
      
        return new Vector2(x, y);
    }


    private void Update()
    {
        // ���¼�ʱ��
        timer += Time.deltaTime;

        // ����ʱ����������Ƶ��ʱ����Ԥ����
        if (timer >= generationFrequency)
        {
            GameObject newPrefab = Instantiate(prefab);
            newPrefab.transform.position = GetRandomPosition();
            timer = 0f;  // ���ü�ʱ��
        }
    }

}