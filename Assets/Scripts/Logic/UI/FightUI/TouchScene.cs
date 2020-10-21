using System;
using UnityEngine;
using UnityEngine.EventSystems;

//UI战斗场景点击交互
public class TouchScene 
{
    public Action<RaycastHit> HitSthCallback;

    public int unTouchLayer;//不应该不检测的层

    GameObject _root;

    public TouchScene()
    {
        _root = UIManager.instance.Add("UIPrefabs/FightUI/TouchScene", UILayer.Touch);

        var touchEx = _root.GetComponent<TouchEX>();
        //touchEx.PointerDownCallback =
        touchEx.PointUpCallback = OnTouchScene;
    }

    private void OnTouchScene(PointerEventData eventData)
    {
        //eventData.position
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);

        RaycastHit ret;
        if(Physics.Raycast(ray,out ret,Mathf.Infinity,~(1<<unTouchLayer)))
        {
            if (HitSthCallback != null) { HitSthCallback(ret); }
        }
    }
}