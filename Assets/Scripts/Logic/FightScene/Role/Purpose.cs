using System;
using UnityEngine;
//角色身上的目标
//仅仅只走向一个点
public class Purpose
{
    public Vector3 targetPos;
    public float stopDistance;
    public Action callback;

    public virtual void Init(Creature target, float stopDis, Action _callback)
    {
        //目前仅给子类来用
    }
    //需要Update，作出反应A
    public virtual void Loop()
    {
        //需要Update，作出反应A
    }

}
//追踪性的目标
public class PurposeTrace : Purpose
{
    public Creature Target;//追踪目标
    public Creature Owner;//目标的持有者
    public override void Init(Creature target,float stopDis,Action _callback)
    {
        Target = target;
        stopDistance = stopDis;
        callback = _callback;
        targetPos = Target.Position;
    }
    //需要Update，作出反应B
    public override void Loop()
    {
        //做出反应B
        targetPos = Target.Position;
        Owner.PathTo(Target.Position);
    }
}
