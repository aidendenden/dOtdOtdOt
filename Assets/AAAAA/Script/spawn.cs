using UnityEngine;

public class spawn : MonoBehaviour
{
    public GameObject objectPrefab; // �������ɵ�����Ԥ����

    public GameObject father;

    public TypingSpeedCalculator _typingSpeedCalculator;

    void Update()
    {
        if (Input.anyKeyDown)
        {

            // �ڸ������λ������һ���µ�����
            GameObject newObject = Instantiate(objectPrefab, _typingSpeedCalculator.GetScreenCoordinates( _typingSpeedCalculator.KeyCodeToV(_typingSpeedCalculator.inputNow)), Quaternion.identity);
            // �����ɵ���������Ϊ��������Ӷ���
            newObject.transform.parent = father.transform;
        }
    }
}

