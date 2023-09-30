using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UIObjectMovement : MonoBehaviour
{
    public string targetTag = "HandPoint"; // Ŀ��UI����ı�ǩ


   

    private GameObject targetObject; // Ŀ��UI����
    private MangeManger mangerManger;

    private void Start()
    {

        mangerManger = GameObject.FindGameObjectWithTag("Manger").GetComponent<MangeManger>();
        // �ڳ����в���Ŀ��UI����
        targetObject = GameObject.FindGameObjectWithTag(targetTag);
    }

    private void Update()
    {
        // ���Ŀ��UI�������
        if (targetObject != null)
        {
            // ����������Ŀ��UI����֮��ķ�������
            Vector2 direction = targetObject.transform.position - transform.position;

            // �ƶ�����
            transform.Translate(direction.normalized * mangerManger.moveSpeed * Time.deltaTime);
        }
       

    }
}