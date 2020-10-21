/// <summary>
/// 服务器的角色
/// </summary>
public class RoleServer
{
    static int _curThisId = 1;
    public static int getNewThisId()
    {
        return _curThisId++;
    }

    //对么？
    public int thisID;
    public string name;//角色名字
    public int modelId;//模型ID
    //攻防 血
    public int hp;
    public int attack;
    public int defence;
    //角色坐标
    //所在地图
    //道具信息

}
