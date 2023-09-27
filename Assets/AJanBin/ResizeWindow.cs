using UnityEngine;
using UnityEngine.EventSystems;

public class ResizeWindow : MonoBehaviour, IPointerDownHandler, IDragHandler,IEndDragHandler,IBeginDragHandler
{
    public RectTransform[] childRectTransforms;
    public Vector2[] originalSizes;
    public Vector2 dragStartPosition;

    [Header("限制拉框大小的，不给值也会有默认大小")]
    public Vector2 mixVector2 ;
    public Vector2 maxVector2 ;

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

        maxVector2 = originalSizes[2];
        mixVector2 = originalSizes[0];
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 将起始拖动位置记录下来
        dragStartPosition = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 在此处编写拖拽开始时的逻辑
        // 可以获取开始拖拽的物体及其信息，并根据需要进行处理
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        // 计算拖动的距离
        Vector2 dragDelta = eventData.position - dragStartPosition;

        // 根据拖动距离更新所有子对象的大小
        for (int i = 0; i < childRectTransforms.Length-1; i++)
        {
            var dragSize= originalSizes[i] + dragDelta;
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