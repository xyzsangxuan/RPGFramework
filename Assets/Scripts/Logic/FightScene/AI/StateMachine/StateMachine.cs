using UnityEngine;
using System.Collections.Generic;

//状态机
public class StateMachine 
{
    //存储一个状态机所拥有的状态
    Dictionary<string, State> stateDic = new Dictionary<string, State>();
    //当前状态
    State currentState;
    //上一个状态
    State lastState;

    //通过构造函数初始化一些数据 
    public StateMachine()
    {

    }
    //注册状态
    //哪个子状态进行了注册，这个子状态的Machine这个字段就指向注册者（StateMachine的一个实例）
    public void RegisterState(string key,State value)
    {
        stateDic.Add(key, value);
        //value.machine = this;
    }
    //设置默认状态
    public void SetDefaultState(string key)
    {
        //找到字典的key所对应的状态
        if (stateDic.ContainsKey(key))
        {
            currentState = stateDic[key];
        }
        else
        {
            Debug.LogError("没有此状态无法设置，设置默认状态失败！");
        }
    }
    //切换状态
    public void ChangeState(string key)
    {
        //找到字典的key所对应的状态
        if (stateDic.ContainsKey(key))
        {
            currentState = stateDic[key];
        }
        else
        {
            Debug.LogError("没有此状态无法设置，切换状态失败！");
        }
    }
    //机器工作的方法
    public void DoWork()
    { 
        if(currentState != lastState)
        {
            currentState.EnterState();
            lastState = currentState;
        }

        currentState.UpdateState();
        if(currentState != lastState)
        {
            lastState.ExitState();
        }
    }
    
}
