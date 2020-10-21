using System.Collections.Generic;
using Unity.Mathematics;


//选人界面角色结构
public class SelectRoleInfo
{
    public string name;//角色名字
    public int modelId;//模型ID
}




//消息基类，用于定义消息
public class Cmd
{
    //当成结构体来使用，一定不要写函数
}
//登录消息 C-->S
public class LoginCmd : Cmd
{
    public string account;
    public string password;
}

//玩家的角色列表 S-->C
public class RoleListCmd : Cmd
{
    //List保存玩家的所有角色
    public List<SelectRoleInfo> allRole = new List<SelectRoleInfo>();
}
//选择角色C-->S
public class SelectRoleCmd : Cmd
{
    //角色索引
    public int index;
}

public class MainRoleThisIdCmd : Cmd
{
    public int thisId;
}

public class EnterMapCmd : Cmd
{
    public int mapId;
}

public class CreateSceneCreature : Cmd
{
    public int thisId;
    public string name;
    public int modelId;
    //血量、攻防
    public int hp;
    public int maxHp;
    public int attack;
    public int defence;

    public float3 pos;
    public float3 faceTo;
}

public class CreateSceneRole: CreateSceneCreature
{

}

public class CreateSceneNpc : CreateSceneCreature
{

}


public class JumpTo : Cmd
{
    public int id;
}
//允许跳入某个地图
public class JumpToMap : Cmd
{
    public int mapId;
}