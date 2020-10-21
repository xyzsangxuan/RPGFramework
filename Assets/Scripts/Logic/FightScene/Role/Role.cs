using System;
using UnityEngine;
using UnityEngine.AI;
//角色
public class Role : Creature
{
    //只读不写，只读变量，不加修改
    CreateSceneRole _serverData;
    RoleDatabase _tableData;


    public override void Init(CreateSceneCreature serverData, CreatureDatabase tableData)
    {
        base.Init(serverData, tableData);
        _serverData = serverData as CreateSceneRole;
        _tableData = tableData as RoleDatabase;
    }

    protected override void initMountPoint()
    {
        CentPos = transform.Find("Slot_Center").gameObject;
    }

    public void JoystickMove(Vector3 target)
    {
        PurposeTo(target);
    }
    public void TouchGroundPathTo(Vector3 target)
    {
        PurposeTo(target);
    }
    public override bool CanBeAttacked(Creature attacker)
    {
        if (!base.CanBeAttacked(attacker)) { return false; }

        //除了自己，所有角色都能打
        return attacker != this;
    }
}

