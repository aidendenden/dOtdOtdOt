using UnityEngine;

public class spawn : MonoBehaviour
{
    public GameObject objectPrefab; // 用于生成的物体预制体

    public GameObject father;

    public TypingSpeedCalculator _typingSpeedCalculator;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(MyKeyCode)))
            {
                // 在父对象的位置生成一个新的物体
                GameObject newObject = Instantiate(objectPrefab, _typingSpeedCalculator.GetScreenCoordinates( _typingSpeedCalculator.KeyCodeToV(_typingSpeedCalculator.inputNow)), Quaternion.identity);
                // 将生成的物体设置为父对象的子对象

                newObject.transform.SetParent(father.transform, false);
            }
        }
    }
}

