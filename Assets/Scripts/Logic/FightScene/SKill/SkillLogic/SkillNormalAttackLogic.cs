using UnityEngine;
using System.Collections;
using System;

//普通攻击（刀砍）
public class SkillNormalAttackLogic : SkillLogicBase
{
    protected override void InitTimeLine()
    {
        _timeLine.AddEvent(0, 0, OnSkillStart);//技能开始
        _timeLine.AddEvent(0, 10, OnAction);//播放动画
        _timeLine.AddEvent(0.4f, 1, OnNormalHit);//直接进行伤害结算
        _timeLine.AddEvent(1.33f, 10, OnActionEnd);//停止动画
        _timeLine.AddEvent(1.34f, 0, OnSkillEnd);//技能结束
    }

    private void OnNormalHit(int obj)
    {
        if(_target == null) { Debug.LogError("普通攻击未找到目标！"); return; }
        OnHitSomething(_target);
    }

    protected void OnHitSomething(Creature target)
    {
        //ResourcesManager.instance.Release(arg1.gameObject);
        DamageMgr.instance.Damage(_caster, target);
    }
}
