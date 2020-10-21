//主角
using System;
using UnityEngine;

public class MainRole:Role
{
    SelectMgr _selectMgr = new SelectMgr();

    public override void Init(CreateSceneCreature serverData, CreatureDatabase tableData)
    {
        base.Init(serverData, tableData);
        //主角设置一个单独的层
        gameObject.layer = LayerMask.NameToLayer("MainRole");

        BindingControlEvent();
        _selectMgr.Init(this);

        _skillMgr.AutoSelectCallback += SelectCreature;
    }

    private void BindingControlEvent()
    {

        //绑定控制事件-（隐含的要求：UI的创建要比主角早）
        FightUIMgr.instance.BindingJoystick(OnJoystickMove, OnJoystickMoveEnd);
        FightUIMgr.instance.BindingTouchScene(OnTouchSomething, LayerMask.NameToLayer("MainRole"));

        FightUIMgr.instance.BindingSkillBtn(OnSkill,UpdateSkillCDPercent);
    }

    private void OnSkill(int index)
    {
        CastSkill(index);

    }
    private float UpdateSkillCDPercent(int index)
    {
        return GetSkillCDPercent(index);
    }


    internal void OnJumpTo(int jumpToMapID)
    {
        Net.instance.SendCmd(new JumpTo() { id = jumpToMapID });
    }

    //点击场景
    private void OnTouchSomething(RaycastHit hitRet)
    {
        //判断技能直接Return
        if (_skillMgr.IsCastingSkill)
        {
            return;
        }

        SelectCreature(null);
        var hitObj = hitRet.transform.gameObject;
        if(hitObj.layer == LayerMask.NameToLayer("Ground"))
        {
            TouchGroundPathTo(hitRet.point);
            return;
        }

        var creature = hitObj.GetComponent<Creature>();
        if(creature != null)
        {
            //选中creature
            SelectCreature(creature);
            //打开目标角色面板
            var npc = creature as Npc;
            if(npc == null)//说明是角色
            {
                return;
            }

            if (!npc.IsFuncNpc)
            {
                return;
            }

            //先移动，再访问；先移动到目标附近，再触发一个事件
            PurposeTo(creature.transform.position, 2, () => { OnVisitNpc(npc); });

            return;
        }
    }

    private void SelectCreature(Creature creature)
    {
        _selectMgr.Select(creature);
    }

    private void OnVisitNpc(Npc npc)
    {
       
        Debug.Log("Visit:" + npc.Name);
    }

    //摇杆移动
    private void OnJoystickMoveEnd()
    {
        StopMove();
    }
    //摇杆移动
    private void OnJoystickMove(Vector2 dir)
    {
        //SelectNpc(null);
        var target = this.transform.position + new Vector3(dir.x, 0, dir.y)*10;//速度与目标点的校验值；与速度正相关，与方向更新频率负相关
        JoystickMove(target);
    }
}

