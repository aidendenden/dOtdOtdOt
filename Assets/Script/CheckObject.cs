using UnityEngine;

public class CheckObject : MonoBehaviour
{
    public string targetTag;  // Ҫ����Ŀ������ı�ǩ


    private Vector3 initialPosition;  // ��ʼλ��
    private bool isFollowing = false;  // �Ƿ����ڸ���
    private GameObject target;  // Ŀ������

    private void Start()
    {
        // ��¼��ʼλ��
        initialPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            // ���Ŀ�������Ƿ����
            target = GameObject.FindGameObjectWithTag(targetTag);
        }

        if (target != null)
        {
            // ���Ŀ��������ڣ��������λ������ΪĿ�������λ��
            transform.position = target.transform.position;
            isFollowing = true;
        }
        else if (isFollowing)
        {
            

            // ����Ѿ����ص���ʼλ�ã���ֹͣ����
            
                isFollowing = false;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
    }
}