using UnityEngine;

public static class GameSetting
{
    static GameSetting()
    {
        MainRoleLayer = LayerMask.NameToLayer("MainRole");
    }

    public static int MainRoleLayer;

    //移动停止的最近距离
    public const float StopDistance = 1f;
    //最大的视觉距离（用于控制超过这个距离就认为看不到对象，用于TargetHead自动隐藏）
    public static float MaxVisioDis  = 15f;
    public static float MaxAutoSelectDis = 10f;
    //每个角色的最大技能数量
    public const int MaxSkillNum = 5;
}
