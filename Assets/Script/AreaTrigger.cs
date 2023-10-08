using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public float timeToTrigger = 3f;  // ��������ͣ����ʱ��
    public string functionName;  // Ҫ���õĺ�������
    public Animator animator;

    private bool isPlayerInside = false;  // ����Ƿ���������
    private float timer = 0f;  // ��ʱ��

    private void Update()
    {
        if (isPlayerInside)
        {
            // �������������ڣ���ʼ��ʱ
            timer += Time.deltaTime;
            Debug.Log(timer);
            if (timer >= timeToTrigger)
            {
                // �����ʱ���ﵽ�趨ʱ�䣬�����ָ���ĺ���
                

                // ���ü�ʱ�������״̬
                timer = 0f;
                isPlayerInside = false;
                animator.SetTrigger("Change");
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("aaaa");
        if (collision.CompareTag("Player"))
        {
            // �����ҽ����������������״̬Ϊ��������
            isPlayerInside = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("bbbb");
        if (collision.CompareTag("Player"))
        {
            // �����ҽ����������������״̬Ϊ��������
            isPlayerInside = false;
        }
    }
}