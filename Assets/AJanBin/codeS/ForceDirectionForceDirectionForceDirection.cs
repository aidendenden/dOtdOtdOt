using UnityEngine;

public class ForceDirection : MonoBehaviour
{
    public float forceMagnitude = 10f; // 力的大小

    // 定义八个方向，每个方向对应一个向量
    private Vector3[] directions = new Vector3[]
    {
        new Vector3(1f, 0f, 0f), // 右
        new Vector3(1f, 0f, 1f).normalized, // 右上
        new Vector3(0f, 0f, 1f), // 上
        new Vector3(-1f, 0f, 1f).normalized, // 左上
        new Vector3(-1f, 0f, 0f), // 左
        new Vector3(-1f, 0f, -1f).normalized, // 左下
        new Vector3(0f, 0f, -1f), // 下
        new Vector3(1f, 0f, -1f).normalized // 右下
    };

    public void ApplyForce(int directionIndex)
    {
        // 根据方向索引获取对应的向量
        Vector3 direction = directions[directionIndex];

        // 给物体施加一个力
        GetComponent<Rigidbody>().AddForce(direction * forceMagnitude, ForceMode.Impulse);
    }
}