using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FollowMouse : MonoBehaviour
{
    public Camera _camera;
    public GameObject needMoveImage;
    
    void Start()
    {
      
    }
    
    void Update()
    {
        // 获取鼠标在屏幕上的位置
        Vector3 mousePosition = Input.mousePosition;

        // 将鼠标位置转换为世界坐标
        Vector3 worldPosition = _camera.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0f; // 确保Z轴位置为0，以保持2D平面

        // 将Sprite位置设置为鼠标位置
        needMoveImage.transform.position = worldPosition;
    }
}
