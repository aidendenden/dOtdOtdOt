using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableImage : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [Header("画框移动限制，坐标起始点屏幕中心")]
    public Vector2 MovementRestrictionsMin;
    public Vector2 MovementRestrictionsMax;
    
    private Vector2 localMousePos;
    private Vector3 planeLocalPos;
    private RectTransform targetObject;
    private RectTransform parentRectTransform;
    private RectTransform targetRectTransform;
    
    private void Start()
    {
        // 获取UI Image的RectTransform组件
        targetObject = this.transform.GetComponent<RectTransform>();
        targetRectTransform = targetObject;
        parentRectTransform = targetObject.parent as RectTransform;
        
        
        if (MovementRestrictionsMin == Vector2.zero)
            MovementRestrictionsMin = new Vector2(-Screen.width, -Screen.height);
        if (MovementRestrictionsMax == Vector2.zero)
            MovementRestrictionsMax = new Vector2(Screen.width , Screen.height);;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 将UI Image置于最前方
        planeLocalPos = targetRectTransform.localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, eventData.position, eventData.pressEventCamera, out localMousePos);
        targetObject.gameObject.transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPointerPosition;
        //屏幕点到矩形中的局部点
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, eventData.position, eventData.pressEventCamera, out localPointerPosition))
        {
            Vector3 offsetToOriginal = localPointerPosition - localMousePos;
            Vector3 pos= planeLocalPos + offsetToOriginal;
            
            pos.x = Mathf.Clamp(pos.x, MovementRestrictionsMin.x, MovementRestrictionsMax.x);
            pos.y = Mathf.Clamp(pos.y, MovementRestrictionsMin.y, MovementRestrictionsMax.y);


            targetObject.parent.parent.localPosition = pos;
        }
        
    }
}