using UnityEngine;
using System.Collections;
using System.Configuration;
//伤害管理
public class DamageMgr : Singleton<DamageMgr>
{
    //造成伤害
    public void Damage(Creature caster,Creature target)
    {
        //mathf.max(1,攻-防)
        var damage = Mathf.Max(1, (caster.serverData.attack-target.serverData.defence));

        target.HP = target.HP - damage;

    }
    
}
