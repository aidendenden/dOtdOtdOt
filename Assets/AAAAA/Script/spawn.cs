using UnityEngine;

public class spawn : MonoBehaviour
{
    public GameObject objectPrefab; // 用于生成的物体预制体

    public GameObject father;

    public Transform _Ttransform;

    public TypingSpeedCalculator _typingSpeedCalculator;


    void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(MyKeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    Vector2 a1 =
                        _typingSpeedCalculator.GetScreenCoordinates(
                            _typingSpeedCalculator.KeyCodeToV(_typingSpeedCalculator.inputNow));
                    Vector2 b1 = _Ttransform.position;
                    Vector2 c1 = b1 - a1;
                    Vector2 c2 = c1 * Random.Range(0.9f, 1.2f);
                    Vector2 d1 = a1 + c2 / 3;


                    // 在父对象的位置生成一个新的物体
                    GameObject newObject = Instantiate(objectPrefab, d1, Quaternion.identity);
                    // 将生成的物体设置为父对象的子对象

                    newObject.transform.SetParent(father.transform, false);
                }
            }
        }
    }
}