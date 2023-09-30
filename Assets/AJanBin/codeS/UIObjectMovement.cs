using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UIObjectMovement : MonoBehaviour
{
    public string targetTag = "HandPoint"; // 目标UI物体的标签


   

    private GameObject targetObject; // 目标UI物体
    private MangeManger mangerManger;

    private void Start()
    {

        mangerManger = GameObject.FindGameObjectWithTag("Manger").GetComponent<MangeManger>();
        // 在场景中查找目标UI物体
        targetObject = GameObject.FindGameObjectWithTag(targetTag);
    }

    private void Update()
    {
        // 如果目标UI物体存在
        if (targetObject != null)
        {
            // 计算物体与目标UI物体之间的方向向量
            Vector2 direction = targetObject.transform.position - transform.position;

            // 移动物体
            transform.Translate(direction.normalized * mangerManger.moveSpeed * Time.deltaTime);
        }
       

    }
}