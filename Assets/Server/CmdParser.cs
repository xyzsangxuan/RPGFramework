using System;
using Unity.Mathematics;
using UnityEngine;
//未划分模块的消息解析
public static class CmdParser
{
    public static void OnLogin(Cmd cmd)
    {
        Debug.Log("分发LoginCmd成功！");
        //cmd的类型必须是LoginCmd
        if (!Net.CheckCmd(cmd, typeof(LoginCmd))) { return; }
        //验证账号密码

        //找到玩家的存档
        var playerData = Server.instance.DB.GetUserData(1);
        if(playerData == null)
        {
            playerData = new Player();
            //分配thisid；
            playerData.thisId = 1;
            Server.instance.DB.SavePlayerData(playerData);
        }
        Server.instance.curPlayer = playerData;
        var player = Server.instance.curPlayer;
        //向客户端发送玩家的已创建的角色列表
        RoleListCmd roleListCmd = new RoleListCmd();
        //roleListCmd.allRole = player.allRole.GetRange(0, player.allRole.Count);
        //深拷贝
        foreach(var role in player.allRole)
        {
            var roleInfo = new SelectRoleInfo() { name = role.name ,modelId =role.modelId};
            roleListCmd.allRole.Add(roleInfo);
        }
        Server.instance.SendCmd(roleListCmd);
    }

    internal static void OnJumpMap(Cmd cmd)
    {
        if (!Net.CheckCmd(cmd, typeof(JumpTo))) { return; }
        JumpTo jumpToCmd = cmd as JumpTo;

        //验证坐标信息
        //跳地图
        EnterMap(Server.instance.curPlayer.curRole,jumpToCmd.id);
    }

    internal static void OnSelectRole(Cmd cmd)
    {
        if (!Net.CheckCmd(cmd, typeof(SelectRoleCmd))) { return; }
        SelectRoleCmd selectRoleCmd = cmd as SelectRoleCmd;

        var curPlayer = Server.instance.curPlayer;

        var curRole = Server.instance.curPlayer.allRole[selectRoleCmd.index];
        curPlayer.curRole = curRole;
        var roleTableData = RoleTable.instance[curRole.modelId];
        curRole.name = roleTableData.name;
        curRole.hp = roleTableData.hp;
        curRole.attack = roleTableData.attack;
        curRole.defence = roleTableData.defence;

        //告诉客户端，场景编号
        //分配ThisId
        var thisId = RoleServer.getNewThisId();
        curPlayer.curRole.thisID = thisId;
        //告诉客户端主角的Thisid
        MainRoleThisIdCmd thisIdCmd = new MainRoleThisIdCmd() { thisId = thisId };

        Server.instance.SendCmd(thisIdCmd);

        //进入场景(原来的函数是没有MapId，或许这部分的代码应该另写)
        EnterMap(curRole,1);
    }

    private static void EnterMap(RoleServer curRole,int mapId)
    {
        var sceneId = mapId;
        EnterMapCmd enterMapCmd = new EnterMapCmd() { mapId = sceneId };

        //生成主角
        CreateSceneRole roleCmd = new CreateSceneRole();
        roleCmd.thisId = curRole.thisID;
        roleCmd.name = curRole.name;
        roleCmd.modelId = curRole.modelId;
        roleCmd.pos = new float3(0, 0, 0);
        roleCmd.faceTo = Vector3.forward;
        roleCmd.hp = curRole.hp;
        roleCmd.maxHp = curRole.hp;
        roleCmd.attack = curRole.attack;
        roleCmd.defence = curRole.defence;

        Server.instance.SendCmd(enterMapCmd);        
        Server.instance.SendCmd(roleCmd);

        //生成附近的配角（暂时不考虑）
        //生成附近的Npc
        //CreateSceneRole进场景

        CreateSmoeNpc();
    }

    private static void CreateSmoeNpc()
    {
        var npc1Cmd = new CreateSceneNpc();
        npc1Cmd.thisId = RoleServer.getNewThisId();
        npc1Cmd.modelId = 1;
        var npc1TableData = NpcTable.instance[npc1Cmd.modelId];
        npc1Cmd.name = npc1TableData.name;
        npc1Cmd.pos = new Vector3(0, 1, 5);
        npc1Cmd.hp = npc1TableData.hp;
        npc1Cmd.maxHp = npc1TableData.hp;
        npc1Cmd.attack = npc1TableData.attack;
        npc1Cmd.defence = npc1TableData.defence;


        var npc2Cmd = new CreateSceneNpc();
        npc2Cmd.thisId = RoleServer.getNewThisId();
        npc2Cmd.modelId = 2;
        var npc2TableData = NpcTable.instance[npc2Cmd.modelId];
        npc2Cmd.name = npc2TableData.name;
        npc2Cmd.pos = new Vector3(5, 1, 5);
        npc2Cmd.hp = npc2TableData.hp;
        npc2Cmd.maxHp = npc2TableData.hp;
        npc2Cmd.attack = npc2TableData.attack;
        npc2Cmd.defence = npc2TableData.defence;

        var npc3Cmd = new CreateSceneNpc();
        npc3Cmd.thisId = RoleServer.getNewThisId();
        npc3Cmd.modelId = 3;
        var npc3TableData = NpcTable.instance[npc3Cmd.modelId];
        npc3Cmd.name = npc3TableData.name;
        npc3Cmd.pos = new Vector3(5, 1, 0);
        npc3Cmd.hp = npc3TableData.hp;
        npc3Cmd.maxHp = npc3TableData.hp;
        npc3Cmd.attack = npc3TableData.attack;
        npc3Cmd.defence = npc3TableData.defence;

        Server.instance.SendCmd(npc1Cmd);
        Server.instance.SendCmd(npc2Cmd);
        Server.instance.SendCmd(npc3Cmd);
    }
}
