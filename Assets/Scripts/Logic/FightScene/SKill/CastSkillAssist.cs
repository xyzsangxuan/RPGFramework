using UnityEngine;
using System.Collections;
using System;
/// <summary>
/// 施法辅助
/// </summary>
public class CastSkillAssist
{
    Creature _owner;

    internal void Init(Creature owner)
    {
        _owner = owner;
    }

    public Creature SelectEnemy()
    {
        float minDis = float.MaxValue;
        Npc minDisNpc = null;
        //选中最近的可攻击的Npc
        foreach (var npcPair in NpcMgr.instance.allNpc)
        {
            var npc = npcPair.Value;
            if (!npc.CanBeAttacked(_owner)) { continue; }
            var dis = Util.Distance2_5D(npc.Position, _owner.Position);
            if(dis > GameSetting.MaxAutoSelectDis) { continue; }
            if(dis < minDis)
            {
                minDis = dis;
                minDisNpc = npc;
            }
        }
        return minDisNpc;
        //算距离，列表排序，取元素，太麻烦了
    }
}
