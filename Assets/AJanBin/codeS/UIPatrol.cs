using UnityEngine;
using UnityEngine.UI;

public class UIPatrol : MonoBehaviour
{
   
    public RectTransform[] patrolPoints; // 存储巡逻点的数组
    public float moveSpeed = 3f; // 物体的移动速度
    private MangeManger mangerManger;

    private int currentPointIndex = 0; // 当前巡逻点的索引


    private void Start()
    {
        mangerManger = GameObject.FindGameObjectWithTag("Manger").GetComponent<MangeManger>();
        
    }
    private void Update()
    {
        moveSpeed = mangerManger.moveSpeed * 1.1f;

        // 获取当前巡逻点
        RectTransform currentPoint = patrolPoints[currentPointIndex];

        // 计算物体与当前巡逻点之间的方向向量
        Vector2 direction = currentPoint.position - transform.position;

        // 移动物体
        transform.Translate(direction.normalized * moveSpeed * Time.deltaTime);

        // 如果物体接近当前巡逻点，则切换到下一个巡逻点
        if (Vector2.Distance(transform.position, currentPoint.position) < 1f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }
}