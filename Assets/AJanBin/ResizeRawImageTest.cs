using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResizeRawImage : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private RawImage rawImage;
    private Vector2 dragStartPosition;
    private Vector2 originalSizeDelta;

    private void Start()
    {
        // 获取RawImage组件
        rawImage = GetComponent<RawImage>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 将初始大小和起始拖动位置记录下来
        originalSizeDelta = rawImage.rectTransform.sizeDelta;
        dragStartPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 计算拖动的距离
        Vector2 dragDelta = eventData.position - dragStartPosition;

        // 根据拖动距离更新RawImage的大小
        rawImage.rectTransform.sizeDelta = originalSizeDelta + dragDelta;

        // 根据大小变化计算UV Rect的变化比例
        Vector2 uvScale = new Vector2(rawImage.rectTransform.sizeDelta.x / originalSizeDelta.x, rawImage.rectTransform.sizeDelta.y / originalSizeDelta.y);

        // 更新RawImage的UV Rect
        rawImage.uvRect = new Rect(rawImage.uvRect.position, uvScale);
    }
}