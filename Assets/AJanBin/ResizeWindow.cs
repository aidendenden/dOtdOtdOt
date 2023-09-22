using UnityEngine;
using UnityEngine.EventSystems;

public class ResizeWindow : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private RectTransform[] childRectTransforms;
    private Vector2[] originalSizes;
    private Vector2 dragStartPosition;

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
        for (int i = 0; i < childRectTransforms.Length; i++)
        {
            childRectTransforms[i].sizeDelta = originalSizes[i] + dragDelta;
        }
    }
}