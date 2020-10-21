using System;
using UnityEngine;

public abstract class SkillLogicBase
{
    //public SkillTable tablebase;
    public SkillDatabase tablebase;
    protected Creature _caster;//施法者
    protected Creature _target;
    protected Action _skillEndCallback;//技能结束回调
    protected TimeLine _timeLine = new TimeLine();//技能时间线
    protected SkillCD _skillCD;

    public float leftCD{ get{ return _skillCD.leftCD; }}

    public bool IsInCD { get { return _skillCD.IsInCD; } }

    public float MaxCD { get { return _skillCD.maxCD; } }

    public void Init(Creature caster,SkillDatabase tabledata)
    {
        _caster = caster;
        tablebase = tabledata;
        InitTimeLine();
        _skillCD = new SkillCD();
        _skillCD.maxCD = tablebase.CD;
    }

    protected virtual void InitTimeLine()
    {
        Debug.LogError("子类没有初始化时间线！"+GetType().ToString());
    }
    //开始释放技能
    public void Start(Creature target, Action skillEndCallback)
    {
        _target = target;
        _skillEndCallback = skillEndCallback;
        _timeLine.Start();
        
    }



    public void Loop()
    {
        _timeLine.Loop(Time.deltaTime);
    }


    protected virtual void OnSkillEnd(int __null)
    {
        _target = null;
        //通知外界技能结束 
        if (_skillEndCallback != null)
        {
            _skillEndCallback();
        }
    }

    protected virtual void OnActionEnd(int actionId)
    {
        if (_caster.GetAim() == actionId)
        {
            _caster.SetAni(0);
        }
    }

    protected virtual void OnAction(int actionId)
    {
        _caster.SetAni(actionId);
    }

    protected virtual void OnSkillStart(int __null)
    {
        _skillCD.StartCD();
        if (_target != null)
        {
            //_caster.transform.LookAt(_target.transform);
            //2.5D的lookat
            _caster.LookAt(_target);
        }
    }
}