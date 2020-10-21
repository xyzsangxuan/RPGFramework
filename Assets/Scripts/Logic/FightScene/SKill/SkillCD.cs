using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
//技能CD
public class SkillCD 
{
    public float maxCD;
    public float leftCD {
        get
        {
            return maxCD - (Time.time - _cdStartTime);
        }
    }

    public bool IsInCD { get { return leftCD > 0; } }
    //进入CD的时间戳
    private float _cdStartTime = float.MinValue;

    public void StartCD()
    {
        ReCD();
    }
    //CD回满，重新开始CD
    private void ReCD()
    {
        _cdStartTime = Time.time;
    }
    //清空CD
    public void ClearCD()
    {
        _cdStartTime = Time.time - 100000f;
    }
    
}
