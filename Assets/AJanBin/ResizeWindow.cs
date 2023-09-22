using UnityEngine;
using UnityEngine.EventSystems;

public class ResizeWindow : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public RectTransform[] childRectTransforms;
    public Vector2[] originalSizes;
    public Vector2 dragStartPosition;

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
        childRectTransforms[0].sizeDelta = originalSizes[0] + dragDelta;
        childRectTransforms[1].sizeDelta = originalSizes[1] + dragDelta;
        // for (int i = 0; i < childRectTransforms.Length; i++)
        // {
        //     childRectTransforms[i].sizeDelta = originalSizes[i] + dragDelta;
        // }
    }
    
 
    
    public void OnEndDrag(PointerEventData eventData)
    {
        for (int i = 0; i < childRectTransforms.Length; i++)
        {
            originalSizes[i] = childRectTransforms[i].sizeDelta;
        }
    }
}