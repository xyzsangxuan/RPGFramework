//角色当前的目标（Target）
using System;
using UnityEngine;

public class SelectMgr
{
    Timer _timer;


    private Role _owner;

    public Creature curTarget;

    public void Init(Role owner)
    {
        //_timer = TimerMgr.instance.CreateTimerAndStart(0.3f, -1, CheckDistance);
        _owner = owner;

    }

    private void OnTargetHpChanged(int curHp, int maxHp)
    {
        FightUIMgr.instance.SetTargetInfo(curTarget.Name, curHp, maxHp);
    }

    //检查owner与目标之间的距离
    private void CheckDistance()
    {
        if(_owner == null || curTarget == null) { return; }
        var dis = Util.Distance2_5D(_owner.transform.position, curTarget.transform.position);
        if (dis > GameSetting.MaxVisioDis)
        {
            Select(null);
        }
        else
        {
            //仅仅做显示处理，数据已经在点击目标时获得
            FightUIMgr.instance.SetTargetActive(true);
            //FightUIMgr.instance.SetTargetInfo(curTarget.Name);
        }
    }
    //选中与非选中，统一入口
    internal void Select(Creature creature)
    {

        if(creature == curTarget) { return; }

        if(curTarget != null)
        {
            curTarget.HPChangedCallback -= OnTargetHpChanged;
        }

        curTarget = creature;

        _owner.curTarget = creature;

        if (creature == null)
        {
            //FightUIMgr.instance.HideTargetHead();
            FightUIMgr.instance.SetTargetActive(false);
            return;
        }
        /* //在视距外
         var dis = Util.Distance2_5D(_owner.transform.position, curTarget.transform.position);
         if (dis > GameSetting.MaxVisioDis)
         {
             return;
         }*/
        creature.HPChangedCallback += OnTargetHpChanged;
        FightUIMgr.instance.SetTargetInfo(creature.Name,creature.serverData.hp,creature.serverData.maxHp);
        
       
    }
}
