using System.Collections.Generic;
using UnityEngine;

//角色管理
public class RoleMgr : Singleton<RoleMgr>
{
    int _mainRoleThisId;
    //thisid role
    public Dictionary<int, Role> allRole = new Dictionary<int, Role>();

    internal static void OnCreateSceneRole(Cmd cmd)
    {
        if (!Net.CheckCmd(cmd, typeof(CreateSceneRole))) { return; }
        CreateSceneRole createSceneRole = cmd as CreateSceneRole;

        //
        RoleDatabase roleDatabase = RoleTable.instance[createSceneRole.modelId];

        if(roleDatabase == null)
        {
            Debug.LogError("未找到角色模型: "+createSceneRole.modelId);
        }
        //创建角色模型
        var roleObj = ResourcesManager.instance.GetInstance(roleDatabase.modelPath);
        Role role;
        //判断是否时主角
        if (RoleMgr.instance._mainRoleThisId == createSceneRole.thisId)
        {
            //主角
            role = roleObj.AddComponent<MainRole>();
        }
        else
        {
            role = roleObj.AddComponent<Role>();
        }
        
        
        role.Init(createSceneRole,roleDatabase);

        RoleMgr.instance.allRole[createSceneRole.thisId] = role;
    }

    internal static void OnMainRoleThisid(Cmd cmd)
    {
        if (!Net.CheckCmd(cmd, typeof(MainRoleThisIdCmd))) { return; }
        MainRoleThisIdCmd thisIdCmd = cmd as MainRoleThisIdCmd;

        RoleMgr.instance._mainRoleThisId = thisIdCmd.thisId;
    }
}