using System;
using UnityEngine;
using UnityEngine.EventSystems;


//用于接收触摸相关的事件
public class TouchEX : MonoBehaviour,IDragHandler,IPointerDownHandler,IPointerUpHandler
{
    //委托 观察者
    //拖动事件
    public Action<PointerEventData> DragCallback;
    //pointerdown
    public Action<PointerEventData> PointerDownCallback;
    //pointup
    public Action<PointerEventData> PointUpCallback;

    public void OnDrag(PointerEventData eventData)
    {
        if(DragCallback != null) { DragCallback(eventData); }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (PointerDownCallback != null) { PointerDownCallback(eventData); }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (PointUpCallback != null) { PointUpCallback(eventData); }
    }
}
