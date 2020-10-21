using System;
using System.Collections.Generic;

//管理所有的定时器
//有点复杂

public class TimerMgr : Singleton<TimerMgr>
{
    //
    public event Action<float> _timerLoopCallback;

    public List<Timer> _allTimer = new List<Timer>();

    //repeatTimes小于0表示无限循环
    public Timer CreateTimer(float deltaTime,int repeatTimes,Action callback)
    {
        var timer = new Timer();
        timer.DeltaTime = deltaTime;
        timer.RepeatTimes = repeatTimes;
        timer.Callback = callback;
        _allTimer.Add(timer);
        return timer;
    }
    //计时器驱动间隔
    public void Loop(float deltaTime)
    {
        if (_timerLoopCallback != null) { _timerLoopCallback(deltaTime); }
    }

    public Timer CreateTimerAndStart(float deltaTime, int repeatTimes, Action callback)
    {
        var timer = new Timer();
        timer.DeltaTime = deltaTime;
        timer.RepeatTimes = repeatTimes;
        timer.Callback = callback;
        _allTimer.Add(timer);
        timer.Start();
        return timer;
    }


    public void StopAll()
    {
        if(_allTimer == null) { return; }
        foreach(var timer in _allTimer)
        {
            timer.Stop();
        }
    }
}

public class Timer
{
    public float DeltaTime;
    public int RepeatTimes;
    public Action Callback;

    private float _duringTime; //计时的持续时间
    private int _repeatedTimes;//已经执行了多少次

    public bool isRunning = false;

    private void reset()
    {
        _duringTime = 0;
        _repeatedTimes = 0;

        Pause();
    }

    //开始、暂停、结束
    public void Start()
    {
        reset();
        isRunning = true;
        TimerMgr.instance._timerLoopCallback += Loop;
        
    }
    public void Pause()
    {
        isRunning = false;
        TimerMgr.instance._timerLoopCallback -= Loop;
    }

    public void Stop()
    {
        Pause();
        reset();
    }

    //Loop内计时
    public void Loop(float deltaTime)
    {
        _duringTime += deltaTime;
        //判断了时间间隔 浮点数不能用等号判断
        if(_duringTime > DeltaTime|| Util.FloatEqual(_duringTime,DeltaTime))
        {
            ++_repeatedTimes;
            _duringTime -= DeltaTime;
            if(Callback != null) { Callback(); }

            if( RepeatTimes > 0 && _repeatedTimes >= RepeatTimes)
            {
                Stop();
            }
        }
    }
}

