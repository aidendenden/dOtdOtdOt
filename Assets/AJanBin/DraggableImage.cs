using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableImage : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private RectTransform imageRectTransform;
    private Canvas canvas;

    private void Start()
    {
        // 获取UI Image的RectTransform组件
        imageRectTransform = GetComponent<RectTransform>();

        // 获取Canvas组件
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 将UI Image置于最前方
        imageRectTransform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 获取鼠标或触摸点的位置
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out position);

        // 设置UI Image的位置
        imageRectTransform.parent.localPosition = position;
    }
}