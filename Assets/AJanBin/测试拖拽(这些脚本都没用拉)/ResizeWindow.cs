using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class ResizeWindow : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    public RectTransform[] childRectTransforms;
    public Vector2[] originalSizes;
    public Vector2 dragStartPosition;

    [Header("限制拉框大小的，不给值也会有默认大小")] public Vector2 minVector2;
    public Vector2 maxVector2;


    private void Start()
    {
        // 获取所有子对象的RectTransform组件
        childRectTransforms = GetComponentsInChildren<RectTransform>();

        // 记录所有子对象的初始大小
        originalSizes = new Vector2[childRectTransforms.Length];

        for (int i = 0; i < childRectTransforms.Length; i++)
        {
            originalSizes[i] = childRectTransforms[i].sizeDelta;
        }

        // 设置默认的最小和最大大小
        if (minVector2 == Vector2.zero)
            minVector2 = originalSizes[0];
        if (maxVector2 == Vector2.zero)
            maxVector2 = originalSizes[2];
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 将起始拖动位置记录下来
        dragStartPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 计算拖动的距离
        //Vector2 dragDelta = eventData.position - dragStartPosition;

      
        
        Vector2 dragDelta = Input.mousePosition-childRectTransforms[0].position;
       
        Debug.Log(dragDelta+"tttt");
        // 根据拖动距离更新所有子对象的大小

        // 计算拖动后的大小
        //Vector2 dragSize = originalSizes[0] + dragDelta;

        // 限制大小在最小值和最大值之间


        dragDelta = new Vector2(
            Mathf.Clamp(dragDelta.x, minVector2.x, maxVector2.x),
            Mathf.Clamp(dragDelta.y, minVector2.y, maxVector2.y)
        );


        childRectTransforms[0].sizeDelta = dragDelta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        for (int i = 0; i < childRectTransforms.Length; i++)
        {
            originalSizes[i] = childRectTransforms[i].sizeDelta;
        }
    }
}