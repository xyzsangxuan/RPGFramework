using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

//摇杆事件
public class Joystick 
{
    public Action<UnityEngine.Vector2> OnMoveDir;
    public Action OnMoveEnd;

    GameObject _root;
    RectTransform _innerBall;//内部小球
    RectTransform _outBall;//外部小球，摇杆激活区域

    float _radius = 0;

    UnityEngine.Vector2 _centerPos;

    UnityEngine.Vector2 _dir;//实际移动方向

    public Joystick()
    {
        _root = UIManager.instance.Add("UIPrefabs/FightUI/Joystick",UILayer.FightUI);

        _outBall = _root.Find<RectTransform>("bg");
        _innerBall = _root.Find<RectTransform>("bg/inner");

        //小球的移动半径
        _radius = _outBall.rect.width * 0.5f;
        //大球的中心点
        _centerPos = new float2(0, 0);

        var touchEx = _outBall.GetComponent<TouchEX>();
        touchEx.DragCallback = OnDrag;
        touchEx.PointerDownCallback = OnPointDown;
        touchEx.PointUpCallback = OnPointUp;


        TimerMgr.instance.CreateTimerAndStart(0.1f, -1, OnLoop);
    }

    private void OnLoop()
    {
        //小球在中心不旋转
        if (_dir == Vector2.zero) {  return; }

        if(OnMoveDir != null) { OnMoveDir(_dir); }
    }

    internal void Reset()
    {
        _dir = UnityEngine.Vector2.zero;
        _innerBall.localPosition = _centerPos;
    }

    private void OnPointUp(PointerEventData eventData)
    {
        Reset();
        if (OnMoveEnd != null) { OnMoveEnd(); }
    }

    private void OnPointDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    private void OnDrag(PointerEventData eventData)
    {
        UnityEngine.Vector2 targetPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_outBall,eventData.position,eventData.pressEventCamera,out targetPos);

        var dir = targetPos - _centerPos;
        _dir = dir.normalized;

        _innerBall.GetComponent<RectTransform>().localPosition = _dir * Mathf.Min(dir.magnitude, _radius);

        if(OnMoveDir != null) { OnMoveDir(_dir); }



    }
}

