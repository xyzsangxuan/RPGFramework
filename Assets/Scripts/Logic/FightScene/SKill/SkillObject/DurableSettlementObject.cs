using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

//持续性，范围伤害判定
public class DurableSettlementObject : MonoBehaviour
{
    private Creature _caster;
    private float _radius;
    private float _duringTime;
    private float _hitRate;
    CapsuleCollider _coller;
    private Timer _timerHitRate;

    private Action<DurableSettlementObject, List<Creature>> _hitCallback;

    private List<Creature> hitList = new List<Creature>();
    //打击到的目标列表
    /// <summary>
    /// 
    /// </summary>
    /// <param name="caster">施法者</param>
    /// <param name="radius">检测半径</param>
    /// <param name="duringTime">持续时间</param>
    /// <param name="hitRate">打击频率</param>
    /// <param name="hitCallback">打击的目标列表</param>
    public void Init(Creature caster, float radius,float duringTime,float hitRate, Action<DurableSettlementObject, List<Creature>> hitCallback)
    {
        _caster = caster;
        _radius = radius;
        _duringTime = duringTime;
        _hitRate = hitRate;
        _hitCallback = hitCallback;

        _coller = GetComponent<CapsuleCollider>();
        _coller.radius = radius;
        _coller.isTrigger = true;
        _coller.enabled = true;

        var rig = gameObject.AddComponent<Rigidbody>();
        rig.useGravity = false;

        TimerMgr.instance.CreateTimerAndStart(duringTime, 1, OnTimeEnd);
        _timerHitRate = TimerMgr.instance.CreateTimerAndStart(hitRate, -1, OnHitRate);
        
    }

    private void OnHitRate()
    {
        _coller.enabled = true;
        QuickCoroutine.instance.StartCorontine(OnWaitingHitEnd());
    }
    private IEnumerator OnWaitingHitEnd()
    {
        //必须保证触发器在创建和销毁之间又一个物理帧
        yield return new WaitForFixedUpdate();

        OnHitEnd();
    }
    private void OnHitEnd()
    {
        _coller.enabled = false;
        if (_hitCallback != null)
        {
            _hitCallback(this, hitList);
        }
        hitList.Clear();
    }
    private void OnTimeEnd()
    {
        _timerHitRate.Stop();
        ResourcesManager.instance.Release(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        var creature = other.GetComponent<Creature>();
        if (creature == null) { return; }
        if (creature == _caster) { return; }
        if (!creature.CanBeAttacked(_caster)) { return; }

        hitList.Add(creature);
        
    }
}
