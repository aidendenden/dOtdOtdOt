using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Transform target; // 目标物体

    private void Update()
    {
        // 计算目标物体在当前物体坐标系下的位置
        Vector3 targetPosition = transform.InverseTransformPoint(target.position);

        // 计算目标物体在当前物体坐标系下的旋转角度
        float angle = Mathf.Atan2(targetPosition.x, targetPosition.z) * Mathf.Rad2Deg;

        // 更新物体的旋转角度，只改变Y轴旋转
        transform.rotation = Quaternion.Euler(57f, angle, 0);
    }
}