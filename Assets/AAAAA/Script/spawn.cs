using UnityEngine;

public class spawn : MonoBehaviour
{
    public GameObject objectPrefab; // �������ɵ�����Ԥ����

    public GameObject father;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // �ڸ������λ������һ���µ�����
            GameObject newObject = Instantiate(objectPrefab, transform.position, Quaternion.identity);
            // �����ɵ���������Ϊ��������Ӷ���
            newObject.transform.parent = father.transform;
        }
    }
}

