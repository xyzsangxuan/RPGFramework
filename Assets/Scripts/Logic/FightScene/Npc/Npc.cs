using System;
using UnityEngine;
//npc类型 和配置表里面的数据，一一对应
public enum eNpcType
{
    Function = 1,
    Monster = 2,
}

//Npc控制
public class Npc : Creature
{
    /*CreateSceneNpc _serverData;
    NpcDatabase _tableData;*/
    NpcAI _ai;
    public bool IsFuncNpc { get { return (tableData as NpcDatabase).NpcType == (int)eNpcType.Function; } }
    public override void Init(CreateSceneCreature createNpc, CreatureDatabase npcDatabase)
    {
        base.Init(createNpc, npcDatabase);

        transform.position = createNpc.pos;
        //只对怪物启用
        CheckAndInitAI();

    }

    private void CheckAndInitAI()
    {
        if (!IsFuncNpc)
        {
            _ai = new NpcAI();
            _ai.Init(this);
        }
    }

    public override bool CanBeAttacked(Creature attacker)
    {
        if (!base.CanBeAttacked(attacker)) { return false; }
        
        //攻击者必须是角色，才能打Npc
        var roleAttacker = attacker as Role;
        if(roleAttacker == null)
        {
            return false;
        }
        //怪物才能被打
        return !IsFuncNpc;
    }

    protected override void initMountPoint()
    {
        base.initMountPoint();
        CentPos = gameObject;
    }

    public override void Update()
    {
        base.Update();
        if(_ai != null)
        {
            _ai.Loop();
        }
    }

    protected override void OnDie()
    {
        base.OnDie();

        _ai = null;
    }

    protected override void OnRespawn()
    {
        base.OnRespawn();
        CheckAndInitAI();
    }
}

