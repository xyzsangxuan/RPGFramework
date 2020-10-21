/*
/// <summary>
/// 状态结构
/// </summary>
public class State
{
    public Action EnterCallback;
    public Action LoopCallback;
    public Action ExitCallback;
}*/

/// <summary>
/// 状态机驱动
/// </summary>
/*public class StateMachineEngine
{
    State _curState;

    public void ChangeState(State newState)
    {
        //前一个状态的Exit
        if(_curState != null)
        {
            //Utils.SafeActionCallback(_curState.ExitCallback);
            _curState.ExitCallback();
        }
        //切换状态
        _curState = newState;
        //新状态的Enter
        if (_curState != null)
        {
            //Utils.SafeActionCallback(_curState.EnterCallback);
            _curState.EnterCallback();
        }

    }
    /// <summary>
    /// 驱动，能量来源
    /// </summary>
    public void Loop()
    {
        if (_curState == null) { return; }
        //Utils.SafeActionCallback(_curState.LoopCallback);
        _curState.LoopCallback();
}
}*/

