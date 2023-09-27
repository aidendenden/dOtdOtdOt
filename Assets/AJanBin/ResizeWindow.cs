using UnityEngine;
using UnityEngine.EventSystems;

public class ResizeWindow : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    public RectTransform[] childRectTransforms;
    public Vector2[] originalSizes;
    public Vector2 dragStartPosition;

    [Header("限制拉框大小的，不给值也会有默认大小")] public Vector2 minVector2;
    public Vector2 maxVector2;
    private Vector2 _difference;

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

        _difference = originalSizes[0] - originalSizes[1];
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 将起始拖动位置记录下来
        dragStartPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 计算拖动的距离
        Vector2 dragDelta = eventData.position - dragStartPosition;

        // 根据拖动距离更新所有子对象的大小
        for (int i = 0; i < childRectTransforms.Length - 1; i++)
        {
            // 计算拖动后的大小
            Vector2 dragSize = originalSizes[i] + dragDelta;

            // 限制大小在最小值和最大值之间

            if (i == 0)
            {
                dragSize = new Vector2(
                    Mathf.Clamp(dragSize.x, minVector2.x, maxVector2.x),
                    Mathf.Clamp(dragSize.y, minVector2.y, maxVector2.y)
                );
            }
            else if (i == 1)
            {
                dragSize = new Vector2(
                    Mathf.Clamp(dragSize.x, minVector2.x - _difference.y, maxVector2.x - _difference.x),
                    Mathf.Clamp(dragSize.y, minVector2.y - _difference.y, maxVector2.y - _difference.y)
                );
            }

            childRectTransforms[i].sizeDelta = dragSize;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        for (int i = 0; i < childRectTransforms.Length; i++)
        {
            originalSizes[i] = childRectTransforms[i].sizeDelta;
        }
    }
}