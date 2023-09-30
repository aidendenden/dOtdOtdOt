using UnityEngine;
using UnityEngine.UI;

public class UIPatrol : MonoBehaviour
{
   
    public RectTransform[] patrolPoints; // �洢Ѳ�ߵ������
    public float moveSpeed = 3f; // ������ƶ��ٶ�
    private MangeManger mangerManger;

    private int currentPointIndex = 0; // ��ǰѲ�ߵ������


    private void Start()
    {
        mangerManger = GameObject.FindGameObjectWithTag("Manger").GetComponent<MangeManger>();
        
    }
    private void Update()
    {
        moveSpeed = mangerManger.moveSpeed * 1.1f;

        // ��ȡ��ǰѲ�ߵ�
        RectTransform currentPoint = patrolPoints[currentPointIndex];

        // ���������뵱ǰѲ�ߵ�֮��ķ�������
        Vector2 direction = currentPoint.position - transform.position;

        // �ƶ�����
        transform.Translate(direction.normalized * moveSpeed * Time.deltaTime);

        // �������ӽ���ǰѲ�ߵ㣬���л�����һ��Ѳ�ߵ�
        if (Vector2.Distance(transform.position, currentPoint.position) < 1f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }
}