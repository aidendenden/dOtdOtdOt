using UnityEngine;
using UnityEngine.UI;

public class UIPatrol : MonoBehaviour
{
   
    public RectTransform[] patrolPoints; // �洢Ѳ�ߵ������
    public float moveSpeed = 3f; // ������ƶ��ٶ�
    private MangeManger mangerManger;
    

    private int currentPointIndex = 0; // ��ǰѲ�ߵ������
    private Animator animatorQust;
    private Animator animatorNpc;

    private void Start()
    {
        mangerManger = GameObject.FindGameObjectWithTag("Manger").GetComponent<MangeManger>();
        animatorNpc = GameObject.FindGameObjectWithTag("NPC").GetComponent<Animator>();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TouZi"))
        {
            Debug.Log("PENG!");
            animatorQust = GameObject.FindGameObjectWithTag("QustNow").GetComponent<Animator>();
            GameObject.FindGameObjectWithTag("QustNow").tag = "QustP";
            animatorQust.SetTrigger("OK");
            mangerManger.Stopp();

        }
    }

    public void eyeStop()
    {
        gameObject.SetActive(false);
    }
   
    public void NpcT()
    {
        animatorNpc.SetTrigger("Talk1");
    }
    
}