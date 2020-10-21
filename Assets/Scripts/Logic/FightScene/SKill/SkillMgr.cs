using System;
using System.Collections.Generic;
using UnityEngine;

//技能对象
public class SkillObject
{
    public SkillDatabase tableData;//技能静态数据
    public SkillLogicBase logic;//技能逻辑（动态数据）
}
//技能施法的接断
public enum CastSkillStage
{
    None = 0,//没有施法
    Assist = 1,//技能辅助阶段
    Casting = 2,//施法阶段
}


//技能数据（静态部分）
public class SkillMgr
{
    Creature _owner;
    List<SkillObject> _allSkill = new List<SkillObject>();
    SkillCaster _skillCaster = new SkillCaster();
    CastSkillAssist _castSkillAssist = new CastSkillAssist();

    CastSkillStage _castSkillStage = CastSkillStage.None;
    //自动选中某个目标的回调
    public Action<Creature> AutoSelectCallback;

    public bool IsCastingSkill { get { return _skillCaster.IsCasting; } }

    public bool IsAssistOrCastingSkill { get { return _castSkillStage == CastSkillStage.Assist || _castSkillStage == CastSkillStage.Casting; } }

    public void Init(Creature owner)
    {
        _owner = owner;

        _skillCaster.Init(_owner,OnSkillFinish);
        _castSkillAssist.Init(_owner);
        var skillIdList = _owner.tableData.skillList;
        //初始化技能
        for(int i = 0; i < GameSetting.MaxSkillNum; ++i)
        {
            var skillObj = new SkillObject();

            var skillId = skillIdList[i];
            skillObj.tableData = SkillTable.instance[skillId];
            
            if(skillObj.tableData == null)
            {
                Debug.LogError("Init技能初始化失败，未找到技能：id：" + skillId);

            }
            //暂时的，结果最终要通过读表,通过反射，将一个字符串反射成类型，不能通过New
            /*var skillType = Type.GetType(skillObj.tableData.scriptType);
            skillType.Assembly.CreateInstance(skillObj.tableData.scriptType);*/

            skillObj.logic = typeof(SkillLogicBase).Assembly.CreateInstance(skillObj.tableData.scriptType) as SkillLogicBase;
            if(skillObj.logic == null)
            {
                Debug.LogError("未找到技能逻辑："+ skillObj.tableData.scriptType);
                continue;
            }
            skillObj.logic.Init(_owner,skillObj.tableData);
            
            _allSkill.Add(skillObj);
        }
    }

    private void OnSkillFinish()
    {
        _castSkillStage = CastSkillStage.None;
    }

    internal void TryCastSkill(int index)
    {
        //技能为啥要index-1呢，不是很理解，整个流程需要走一遍发现问题
        //正在释放技能时，不能再放技能
        if (_skillCaster.IsCasting) { return; }
        //找到技能
        var skillObj = GetSkillObject(index);
        if(skillObj == null) {
            Debug.LogError("TryCastSkill未找到技能对象,索引号"+(index-1).ToString());
            return; }
        //CD
        if (skillObj.logic.IsInCD) { return; }


        // 如果这个技能需要指定目标，才需要检测目标有效性(需要目标&有目标&目标可被攻击)
        if(skillObj.tableData.castRange > 0)
        {
            //尝试自动选择技能目标
            if(_owner.curTarget == null)
            {
                var selectTarget = _castSkillAssist.SelectEnemy();
                if(selectTarget != null)
                {
                    Util.SafeCall(AutoSelectCallback, selectTarget);
                    /*if (AutoSelectCallback != null) { AutoSelectCallback(selectTarget); }*/
                }
                //_owner.curTarget = _castSkillAssist.SelectEnemy();
            }
            //真的木有目标
            if (_owner.curTarget == null) { return; }

            if (!_owner.curTarget.CanBeAttacked(_owner))
            {
                return;
            }

            //施法范围判断
            var dis = Util.Distance2_5D(_owner.transform.position, _owner.curTarget.transform.position);
            if (dis > skillObj.tableData.castRange)
            {
                Debug.Log("距离过远" + dis + "," + skillObj.tableData.castRange);
                
                //技能进入辅助阶段 
                _castSkillStage = CastSkillStage.Assist;
                _owner.TraceTo(_owner.curTarget , skillObj.tableData.castRange, ()=> OnAssistFinish(skillObj.logic));
                return;
            }
        }
        //技能类型（目标、位置）能不能放
        //开始放技能
        CastSkill(skillObj.logic);
        
    }

    private SkillObject GetSkillObject(int index)
    {
        return _allSkill[index - 1];
    }

    internal float GetSkillCDPercent(int index)
    {
        var skillobject = GetSkillObject(index);
        if(skillobject == null) { return 1; }
        return (skillobject.logic.leftCD/skillobject.logic.MaxCD);
    }

    //技能辅助执行完毕
    private void OnAssistFinish(SkillLogicBase logic)
    {
        _castSkillStage = CastSkillStage.Casting;
        CastSkill(logic);
    }

    private void CastSkill(SkillLogicBase skillLogic)
    {
        _owner.StopMove();
        _skillCaster.CastSkill(skillLogic, _owner.curTarget);
    }

    public void Loop()
    {
        _skillCaster.Loop();
        //skillLogic.Loop();
    }


}
