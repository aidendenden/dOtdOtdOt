using UnityEngine;
using UnityEngine.EventSystems;

public class ResizeWindow : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private RectTransform imageRectTransform;
    private Vector2 dragStartPosition;
    private Vector2 originalSizeDelta;

    private void Start()
    {
        // 获取UI Image的RectTransform组件
        imageRectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 将初始大小和起始拖动位置记录下来
        originalSizeDelta = imageRectTransform.sizeDelta;
        dragStartPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 计算拖动的距离
        Vector2 dragDelta = eventData.position - dragStartPosition;

        // 根据拖动距离更新窗口大小
        imageRectTransform.sizeDelta = originalSizeDelta + dragDelta;
    }
}