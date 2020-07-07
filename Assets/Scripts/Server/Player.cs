using System;
using System.Collections.Generic;

//表示玩家
public class Player
{
    //假的角色数据
    public List<SelectRoleInfo> allRole = new List<SelectRoleInfo>();

    public Player()
    {
        allRole.Add(new SelectRoleInfo() { name = "第一个角色", modelId = 1 });
        allRole.Add(new SelectRoleInfo() { name = "第二个角色", modelId = 2 });
        allRole.Add(new SelectRoleInfo() { name = "第三个角色", modelId = 3 });
        allRole.Add(new SelectRoleInfo() { name = "第四个角色", modelId = 4 });
    }
}
