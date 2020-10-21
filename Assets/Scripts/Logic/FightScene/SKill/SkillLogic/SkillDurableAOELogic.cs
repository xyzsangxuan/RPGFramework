using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

public class SkillDurableAOELogic : SkillLogicBase
{
    protected override void InitTimeLine()
    {
        _timeLine.AddEvent(0, 0, OnSkillStart);//技能开始
        _timeLine.AddEvent(0, 30, OnAction);//播放动画
        _timeLine.AddEvent(0.1f, 2, OnParticle);//(在施法者身上)播放一个粒子特效
        _timeLine.AddEvent(0.1f, 1, OnSettlementObject);//结算
        //_timeLine.AddEvent(0.1f, 2, OnParticleFinish);// 注销该一个粒子特效
        _timeLine.AddEvent(1.33f, 30, OnActionEnd);//停止动画
        _timeLine.AddEvent(1.34f, 0, OnSkillEnd);//技能结束
    }


    //结算物
    private void OnSettlementObject(int settlementId)
    {
        var tableData = SettlementTable.instance[settlementId];
        if(tableData == null) { return; }


        var settlementObj = ResourcesManager.instance.GetInstance(tableData.resPath,_caster.CentPos.transform.position);

        

        var settlement = settlementObj.AddComponent<DurableSettlementObject>();

        settlement.Init(_caster,tableData.radius,tablebase.duringTime,0.2f ,OnhitSomething);
        // 跟着角色/不跟着角色 
        settlementObj.transform.SetParent(_caster.CentPos.transform);
    }

    private void OnhitSomething(DurableSettlementObject settlement,List<Creature> targetList)
    {
        if (targetList == null) { return; }
        //ResourcesManager.instance.Release(settlement.gameObject);
        foreach(var target in targetList)
        {
            DamageMgr.instance.Damage(_caster, target);
        }
    }

    private void OnParticle(int particleId)
    {
        
        var tableData = ParticleTable.instance[particleId];
        if(tableData == null) { return; }
        
        var particleObj = ResourcesManager.instance.GetInstance(tableData.resPath,_caster.CentPos.transform.position);
        particleObj.transform.SetParent(_caster.CentPos.transform);
        particleObj.name = tableData.resPath;
        var po = particleObj.AddComponent<ParticleObject>();
        po.Init();
    }
}
