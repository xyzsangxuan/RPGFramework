using System.Collections.Generic;
using UnityEngine.SceneManagement;

//用户数据
public class UserData : Singleton<UserData>
{
    public List<SelectRoleInfo> allRole = new List<SelectRoleInfo>();
    internal static void OnRoleList(Cmd cmd)
    {
        //cmd的类型必须是RoleListCmd
        if (!Net.CheckCmd(cmd, typeof(RoleListCmd))) { return; }
        RoleListCmd roleListCmd = cmd as RoleListCmd;
       

        UserData.instance.allRole = roleListCmd.allRole;

        if (roleListCmd.allRole.Count > 0)
        {
            //选人界面
            SceneManager.LoadScene("SelectRole");
        }
        else
        {
            //创建角色界面
            SceneManager.LoadScene("CreateRole");
        }
    }
}

