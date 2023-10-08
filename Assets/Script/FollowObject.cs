using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform target;  // 要跟随的目标物体

    public Transform targetD;

    public float radius = 5f;  // 检测半径


    public bool isFollowing;  // 是否正在跟随

    private void Start()
    {
        isFollowing = false;
    }
    private void FixedUpdate()
    {

        CheckD();

        if (isFollowing)
        {
            // 将物体的位置设置为目标物体的位置
            transform.position = target.position;
        }

        if (target != null)
        {
            // 计算目标物体与当前物体之间的距离
            float distance = Vector3.Distance(transform.position, target.position);

            // 如果距离小于半径，则目标物体在半径周围
            if (distance < radius)
            {
                if (!isFollowing)
                {
                    // 当物体触碰到目标物体时，开始跟随目标物体
                    
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

         
            // 计算目标物体与当前物体之间的距离
            float distance = Vector3.Distance(transform.position, targetD.position);

            // 如果距离小于半径，则目标物体在半径周围
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