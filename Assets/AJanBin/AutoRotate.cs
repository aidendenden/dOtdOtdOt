using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0f, 0f, 30f);

    private void Update()
    {
        // 根据旋转速度进行自动旋转
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}