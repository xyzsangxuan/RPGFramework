using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
// 范围结算
public class SettlementObject : MonoBehaviour
{
    private Creature _caster;
    private float _radius;
    private Action<SettlementObject,List<Creature>> _hitCallback;

    private List<Creature> hitList = new List<Creature>();
    //打击到的目标列表
    public void Init(Creature caster,float radius,Action<SettlementObject,List<Creature>> hitCallback)
    {
        _caster = caster;
        _radius = radius;
        _hitCallback = hitCallback;

        var coller = GetComponent<CapsuleCollider>();
        coller.radius = radius;
        coller.isTrigger = true;
        coller.enabled = true;

        var rig = gameObject.AddComponent<Rigidbody>();
        rig.useGravity = false;

        QuickCoroutine.instance.StartCorontine(OnWaitingHitEnd());
    }

    private IEnumerator OnWaitingHitEnd()
    {
        //必须保证触发器在创建和销毁之间又一个物理帧
        yield return new WaitForFixedUpdate();

        OnHitEnd();
    }
    //表示结算结束
    private void OnHitEnd()
    {
        if (_hitCallback != null)
        {
            _hitCallback(this,hitList);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var creature = other.GetComponent<Creature>();
        if(creature == null) { return; }
        if (creature == _caster) { return; }
        if (!creature.CanBeAttacked(_caster)) { return; }

        hitList.Add(creature);
    }

}
