using UnityEngine;
using System.Collections;
using System;
//负责技能的释放，流程部分
public class SkillCaster 
{
    Creature _owner;

    private Action OnSkillLogicEndCallback;

    public bool IsCasting { get { return _castingSkill != null; } }

    private SkillLogicBase _castingSkill;

    public void CastSkill(SkillLogicBase skillLogic, Creature target)
    {
        _castingSkill = skillLogic;
        skillLogic.Start(target,OnSkillLogicEnd);
    }

    private void OnSkillLogicEnd()
    {
        _castingSkill = null;
        //通知技能逻辑结束
        if(OnSkillLogicEndCallback != null) { OnSkillLogicEndCallback(); }
        //
    }

    public void Init(Creature owner,Action skillEndCallback)
    {
        _owner = owner;
        OnSkillLogicEndCallback = skillEndCallback;
    }

    internal void Loop()
    {
        //驱动正在释放的技能
        if(_castingSkill != null)
        {
            _castingSkill.Loop();
        }
    }
}
