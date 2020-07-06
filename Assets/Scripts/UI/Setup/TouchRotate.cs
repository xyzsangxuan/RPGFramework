using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchRotate : MonoBehaviour,IDragHandler
{
    //委托 观察者
    //拖动事件
    public Action<PointerEventData> DragCallBack;


    public void OnDrag(PointerEventData eventData)
    {
        if(DragCallBack != null) { DragCallBack(eventData); }
    }
}
