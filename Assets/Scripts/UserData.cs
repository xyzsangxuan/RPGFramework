using System.Collections.Generic;

//选人界面角色结构
public  class SelectRoleInfo
{
    public string name;//角色名字
    public string modelResPath;//模型资源路径
}

//用户数据
public class UserData : Singleton<UserData>
{
    //假的角色数据
    public  List<SelectRoleInfo> allRole = new List<SelectRoleInfo>();

    public UserData()
    {
        allRole.Add(new SelectRoleInfo() { name = "第一个角色", modelResPath = "Prefabs/Player0" });
        allRole.Add(new SelectRoleInfo() { name = "第二个角色", modelResPath = "Prefabs/Player1" });
        allRole.Add(new SelectRoleInfo() { name = "第三个角色", modelResPath = "Prefabs/Player2" });
        allRole.Add(new SelectRoleInfo() { name = "第四个角色", modelResPath = "Prefabs/Player3" });
    }
}

