using UnityEngine;
using System.Collections;
/// <summary>
/// Lua等初始化
/// </summary>
public class GameEngine : MonoBehaviour
{
    Timer timer1;

    private void Awake()
    {
        
    }
    private void Start()
    {
       
        timer1 = TimerMgr.instance.CreateTimer(0.1f, -1, () => { Debug.Log("游戏运行时间"); });
        //timer1.Start();
    }
    private void Update()
    {
        TimerMgr.instance.Loop(Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (timer1.isRunning) { timer1.Stop(); }
            else
            {
                timer1.Start();
            }
        }
    }

    private void FixedUpdate()
    {
        //TimerMgr.instance.FixedLoop();
    }
}
