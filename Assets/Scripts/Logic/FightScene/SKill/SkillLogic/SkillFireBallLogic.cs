using UnityEngine;
using System.Collections;
using System;

//火球的逻辑
public class SkillFireBallLogic:SkillLogicBase
{
    protected override void InitTimeLine()
    {
        _timeLine.AddEvent(0, 0, OnSkillStart);//技能开始
        _timeLine.AddEvent(0, 10, OnAction);//播放动画
        _timeLine.AddEvent(0.4f, 1, OnFlyObject);//生成飞行道具
        _timeLine.AddEvent(1.33f, 10, OnActionEnd);//停止动画
        _timeLine.AddEvent(1.34f, 0, OnSkillEnd);//技能结束
    }

    private void OnFlyObject(int flyObjId)
    {
        //FlyObjectTable.instance[0];为啥取不到第一排FlyObjectTable.instance[1];却能取到第一排
        FlyObjectDatabase flyDb = FlyObjectTable.instance[flyObjId];
        if(flyDb == null) { Debug.LogError("未找到飞行道具："+flyObjId);return; }

        //放一个火球
        var ball = ResourcesManager.instance.GetInstance(flyDb.resPath, _caster.CentPos.transform.position);

        _caster.transform.LookAt(_caster.curTarget.transform);

        var flyObject = ball.AddComponent<FlyObject>();
        flyObject.Init(_caster,_caster.curTarget, flyDb.flySpeed, flyDb.radius, flyDb.duringTime,flyDb.trace, OnHitSomething);
        //火球结束
    }

    protected void OnHitSomething(FlyObject flyObject, Creature target)
    {
        ResourcesManager.instance.Release(flyObject.gameObject);
        DamageMgr.instance.Damage(_caster, target);
    }
}
