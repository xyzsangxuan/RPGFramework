/// <summary>
/// 状态结构 状态基类
/// </summary>
public class State
{
    public StateMachine _machine;
    public Creature _owner;
    public virtual void Init(Creature owner,StateMachine machine) 
    { 
        _owner = owner;
        _machine = machine;
    }

    //进入某个状态，选要做的准备或者初始化工作
    public virtual void EnterState() { }
    //进入某个状态中，需要持续更新的逻辑
    public virtual void UpdateState() { }
    //退出某个状态，需要重置的一些数据
    public virtual void ExitState() { }
}