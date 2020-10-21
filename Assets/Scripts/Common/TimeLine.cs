using System;

public class TimeLine
{
    public float TimeSpeed { get; set; }

    //是否开始
    private bool m_isStart;
    //是否暂停
    private bool m_isPause;
    //当前计时
    private float m_curTime;
    //重置事件
    private Action m_Reset;
    //每帧回调
    private Action<float> m_update;

    public TimeLine()
    {
        TimeSpeed = 1;

        Reset();
    }
    /// <summary>
    /// 添加事件
    /// </summary>
    public void AddEvent(float delay,int id,Action<int> method)
    {
        LineEvent param = new LineEvent(delay, id, method);
        m_update += param.Invoke;
        m_Reset += param.Reset;
    }
    public void Start()
    {
        Reset();

        m_isStart = true;
        m_isPause = false;
    }
    public void Pause()
    {
        m_isPause = true;
    }
    public void Resume()
    {
        m_isPause = false;
    }
    public void Reset()
    {
        m_curTime = 0;
        m_isStart = false;
        m_isPause = false;

        if(null != m_Reset)
        {
            m_Reset();
        }
    }
    public void Loop(float deltaTime)
    {
        //当（isStart && ！isPause）时执行时间线
        if(!m_isStart || m_isPause)
        {
            return;
        }
        m_curTime += deltaTime;
        if(null != m_update)
        {
            m_update(m_curTime);
        }
    }
    /// <summary>
    /// 时间线事件
    /// </summary>
    private class LineEvent
    {
        public float Delay { get; protected set; }
        public int Id { get; protected set; }
        public Action<int> Method { get; protected set; }

        private bool m_isInvoke = false;
        public LineEvent(float delay, int id,Action<int> method)
        {
            Delay = delay;
            Id = id;
            Method = method;

            Reset();
        }
        //每帧执行，time是从时间线开始，到目前为止经过的时间
        internal void Invoke(float time)
        {
            if(time < Delay)
            {
                return;
            }
            //执行时间，已经超过了Event的延迟时间，（记住，float不要用相等判断）
            if(!m_isInvoke && null != Method)
            {
                m_isInvoke = true;
                Method(Id);
            }
        }
        internal void Reset()
        {
            m_isInvoke = false;
        }
    }
}
