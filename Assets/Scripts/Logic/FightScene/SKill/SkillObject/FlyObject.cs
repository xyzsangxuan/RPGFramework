using System;
using UnityEngine;
//飞行道具
public class FlyObject:MonoBehaviour
{
    public Creature caster;
    public Creature target;
    public float speed =10;
    public float radius = 0.5f;
    public float duringtime = 10f;

    public bool isTrace = false;
    //命中目标回调
    public Action<FlyObject, Creature> OnHitTargetCallback;

    Rigidbody _rig;
    SphereCollider _collider;
    Timer timer;
    public void Init(Creature _caster,Creature _target,float _speed,float _radius, float _duringtime, bool _isTrace,Action<FlyObject, Creature> callback)
    {
        caster = _caster;
        target = _target;
        speed = _speed;
        radius = _radius;
        duringtime = _duringtime;
        isTrace = _isTrace;
        OnHitTargetCallback = callback;
        _rig = gameObject.AddComponent<Rigidbody>();
        _rig.useGravity = false;
        _collider = gameObject.AddComponent<SphereCollider>();
        _collider.radius = radius;
        _collider.isTrigger = true;

        timer =  TimerMgr.instance.CreateTimerAndStart(duringtime, 1, OnTimeEnd);
        if (!isTrace)
        {
            OnFly();
        }
        
    }

    private void Update()
    {
        //跟着target的飞行
        if (isTrace)
        {
            OnFly();
        }

    }
    //飞行
    private void OnFly()
    {
        if (target == null) { return; }
        _rig.velocity = (target.transform.position - transform.position).normalized * speed;
    }

    private void OnTimeEnd()
    {
        ResourcesManager.instance.Release(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if(target == null) { return; }
        
        var creature = other.GetComponent<Creature>();
        if(creature != target||!target.CanBeAttacked(caster))
        {
            return;
        }
        if(OnHitTargetCallback != null) {
            timer.Stop();
            OnHitTargetCallback(this, creature); }
    }

    
}

