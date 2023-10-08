using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform target;  // Ҫ�����Ŀ������

    public Transform targetD;

    public float radius = 5f;  // ���뾶


    public bool isFollowing;  // �Ƿ����ڸ���

    private void Start()
    {
        isFollowing = false;
    }
    private void FixedUpdate()
    {

        CheckD();

        if (isFollowing)
        {
            // �������λ������ΪĿ�������λ��
            transform.position = target.position;
        }

        if (target != null)
        {
            // ����Ŀ�������뵱ǰ����֮��ľ���
            float distance = Vector3.Distance(transform.position, target.position);

            // �������С�ڰ뾶����Ŀ�������ڰ뾶��Χ
            if (distance < radius)
            {
                if (!isFollowing)
                {
                    // �����崥����Ŀ������ʱ����ʼ����Ŀ������
                    
                        isFollowing = true;
                    
                }
            }
            else
            {
                isFollowing = false;
            }
        }
       



    }
  
    private void CheckD()
    {
        if (targetD != null)
        {

         
            // ����Ŀ�������뵱ǰ����֮��ľ���
            float distance = Vector3.Distance(transform.position, targetD.position);

            // �������С�ڰ뾶����Ŀ�������ڰ뾶��Χ
            if (distance < radius)
            {
                Debug.Log("aaaa");
                Destroy(gameObject);
            }
            else
            {

            }
        }
    }


 

}