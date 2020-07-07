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
        
        Server.instance.curPlayer = new Player();
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

    internal static void OnSelectRole(Cmd cmd)
    {
        if (!Net.CheckCmd(cmd, typeof(SelectRoleCmd))) { return; }
        SelectRoleCmd selectRoleCmd = cmd as SelectRoleCmd;

        SelectRoleInfo curRole = Server.instance.curPlayer.allRole[selectRoleCmd.index];

        //进场景 告诉客户端，场景编号
        var sceneId = 1;
        EnterMapCmd enterMapCmd = new EnterMapCmd() { mapId = sceneId };
        //分配ThisId
        var thisId = RoleServer.getNewThisId();
        //告诉客户端主角的Thisid
        MainRoleThisIdCmd  thisIdCmd= new MainRoleThisIdCmd() { thisId = thisId };
        //生成主角
        CreateSceneRole roleCmd = new CreateSceneRole();
        roleCmd.thisId = thisId;
        roleCmd.name = curRole.name;
        roleCmd.modelId = curRole.modelId;
        roleCmd.pos = new float3(0, 0, 0);
        roleCmd.faceTo = Vector3.forward;

        Server.instance.SendCmd(enterMapCmd);
        Server.instance.SendCmd(thisIdCmd);
        Server.instance.SendCmd(roleCmd);

        //生成附近的配角（暂时不考虑）
        //生成附近的Npc
        //CreateSceneRole进场景
    }
}
