using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableImage : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private RectTransform imageRectTransform;
    private Canvas canvas;

    [Header("画框移动限制，坐标起始点屏幕中心")]
    public Vector2 MovementRestrictionsMin;
    public Vector2 MovementRestrictionsMax;
    private void Start()
    {
        // 获取UI Image的RectTransform组件
        imageRectTransform = GetComponent<RectTransform>();

        // 获取Canvas组件
        canvas = GetComponentInParent<Canvas>();
        
        if (MovementRestrictionsMin == Vector2.zero)
            MovementRestrictionsMin = new Vector2(-Screen.width, -Screen.height);
        if (MovementRestrictionsMax == Vector2.zero)
            MovementRestrictionsMax = new Vector2(Screen.width , Screen.height);;

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
        position.x = Mathf.Clamp(position.x, MovementRestrictionsMin.x, MovementRestrictionsMax.x);
        position.y = Mathf.Clamp(position.y, MovementRestrictionsMin.y, MovementRestrictionsMax.y);
            
        imageRectTransform.parent.parent.localPosition = position;
    }
}