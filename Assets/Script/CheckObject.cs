using UnityEngine;

public class CheckObject : MonoBehaviour
{
    public string targetTag;  // 要检测的目标物体的标签


    private Vector3 initialPosition;  // 初始位置
    private bool isFollowing = false;  // 是否正在跟随
    private GameObject target;  // 目标物体

    private void Start()
    {
        // 记录初始位置
        initialPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            // 检测目标物体是否存在
            target = GameObject.FindGameObjectWithTag(targetTag);
        }

        if (target != null)
        {
            // 如果目标物体存在，则将物体的位置设置为目标物体的位置
            transform.position = target.transform.position;
            isFollowing = true;
        }
        else if (isFollowing)
        {
            

            // 如果已经返回到初始位置，则停止跟随
            
                isFollowing = false;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
    }
}