using UnityEngine;
using UnityEngine.AI;
using System;

public class Creature : MonoBehaviour
{
    //只读不写，只读变量，不加修改
    public CreateSceneCreature serverData;
    public CreatureDatabase tableData;

    //血量变化监听<当前血量，最大血量 >
    public event Action<int, int> HPChangedCallback;

    public Creature curTarget;//当前目标

    //移动管理
    protected NavMeshAgent _agent;
    //动作管理
    protected Animator _animator;
    //技能管理
    protected SkillMgr _skillMgr = new SkillMgr();
    //角色的短暂记忆，目标
    protected Purpose _purpose;
    //当前状态
    protected CreatureState CurState = CreatureState.Idle;

    public int ThisId { get { return serverData.thisId; } }
    public string Name { get { return serverData.name; } }
    public string modelpath { get { return tableData.modelPath; } }
    public GameObject CentPos { get; protected set; }
    public Vector3 Position { get { return transform.position; } }
    public bool IsCastingSkill { get { return _skillMgr.IsCastingSkill; } }
    public bool IsAssistOrCastingSkill { get { return _skillMgr.IsAssistOrCastingSkill; } }

    public bool IsAlive { get { return !(CreatureState.Die == CurState); } }
    
    
    //hp
    public int HP { 
        get { return serverData.hp; } 
        set {
            var newhp = Mathf.Max(0, value);
            var oldhp = serverData.hp;
            serverData.hp = newhp;
            //死亡，血量从非零变到零的过程
            if (serverData.hp <= 0 && oldhp>0)
            {
                //死亡
                OnDie();
                
            }
            //重生：0——>非零
            if(oldhp <=0 && serverData.hp > 0)
            {
                OnRespawn();
            }
            Debug.Log("当前血量:" + serverData.hp);
            //callback
            if(HPChangedCallback != null) { HPChangedCallback(value,serverData.maxHp); }

        }
    }
    //什么条件下可以移动
    public bool CanMove { get { return IsAlive && !IsCastingSkill; } }

    public bool CanCastSkill { get { return IsAlive && !IsCastingSkill; } }

    public Rigidbody _rig;

    protected virtual void OnRespawn()
    {
        Debug.Log("重生");
        SetState(CreatureState.Respawn);
        TimerMgr.instance.CreateTimerAndStart(1.2f, 1, () => { 
            if(GetState() == CreatureState.Respawn)
        {
            SetState(CreatureState.Idle);
        }
        });
    }

    protected virtual void OnDie()
    {
        Debug.Log("死亡");
        StopMove();
        SetState(CreatureState.Die);
        //  重生 

        TimerMgr.instance.CreateTimerAndStart(5, 1, () => HP = serverData.maxHp);
    }

    //获取技能CD百分比
    protected float GetSkillCDPercent(int index)
    {
        return _skillMgr.GetSkillCDPercent(index);
    }

    public virtual void Init(CreateSceneCreature serverData, CreatureDatabase tableData)
    {
        this.serverData = serverData;
        this.tableData = tableData;

        initMountPoint();

        _agent = gameObject.AddComponent<NavMeshAgent>();
        _agent.stoppingDistance = GameSetting.StopDistance;
        _agent.speed = 10f;
        //角速度
        _agent.angularSpeed = float.MaxValue;
        //加速度
        _agent.acceleration = float.MaxValue;
        //阻挡等级为不阻挡
        _agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        _rig = this.gameObject.AddComponent<Rigidbody>();
        _rig.useGravity = false;
        _rig.isKinematic = true;
        _animator = gameObject.GetComponent<Animator>();


        _skillMgr.Init(this);
    }

    protected virtual void initMountPoint()
    {
        
    }

    public void CastSkill(int index)
    {
        if (!CanCastSkill)
        {
            return;
        }
        _skillMgr.TryCastSkill(index);
    }

    //寻路到某个点
    public void PathTo(Vector3 target)
    {
        if (!CanMove)
        {
            return;
        }
        //_targetPoint = target;
        _agent.SetDestination(target);
        SetState(CreatureState.Move);
    }



    private void ResetPurpose()
    {
        _purpose = null;
    }

    public virtual bool CanBeAttacked(Creature attacker)
    {
        //活着的
        return IsAlive;
        //怪物
        //pk模式（此项目所有Role都可被攻击）
    }

    //带有目的性的移动
    public void PurposeTo(Vector3 target, float stopDistance = -1, Action arrivedCallback = null)
    {
        //判断技能直接Return
        if (!CanMove)
        {
            return;
        }


        if (stopDistance < 0) { stopDistance = GameSetting.StopDistance; }

        ResetPurpose();



        if (Util.Distance2_5D(transform.position, target) < stopDistance)
        {
            if (arrivedCallback != null)
            {
                arrivedCallback();
                return;
            }
        }



        _purpose = new Purpose();
        _purpose.targetPos = target;
        _purpose.stopDistance = stopDistance;
        _purpose.callback = arrivedCallback;

        PathTo(_purpose.targetPos);
    }

    //追踪到
    public void TraceTo(Creature target,float stopDistance = -1 ,Action arrivedCallback = null)
    {
        if (!CanMove)
        {
            return;
        }
        var purposTrace = new PurposeTrace();
        purposTrace.Owner = this;
        purposTrace.Init(target, stopDistance, arrivedCallback);
        _purpose = purposTrace;
        
        PathTo(_purpose.targetPos);
    }

    internal void LookAt(Creature target)
    {
        transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
    }

    public void StopMove()
    {
        _agent.isStopped = true;
        _agent.ResetPath();
        //SetAni(0);
        if(CurState == CreatureState.Move)
        {
            SetState(CreatureState.Idle);
        }
        ResetPurpose();
        //_targetPoint = null;

    }

    public void SetState(CreatureState newState)
    {
        CurState = newState;
        SetAni((int)CurState);
    }
    public CreatureState GetState( )
    {
        return CurState;
    }
    public void SetAni(int motionType)
    {
        _animator.SetInteger("MotionType", motionType);
    }
    public int GetAim()
    {
        return _animator.GetInteger("MotionType");
    }
    public virtual void Update()
    {
        RunLoop();

        if (_purpose == null) { return; }

        //判断到达目标点的距离
        var dis = Util.Distance2_5D(transform.position, _purpose.targetPos);

        if (dis < _purpose.stopDistance)
        {
            OnArrived();
            return;
        }
        //驱动目标 
        _purpose.Loop();

        
    }

    private void RunLoop()
    {
        _skillMgr.Loop();
    }

    private void OnArrived()
    {
        DoPurpos();

        StopMove();
    }

    private void DoPurpos()
    {
        if (_purpose != null)
        {
            if (_purpose.callback != null)
            {
                _purpose.callback();
            }
            ResetPurpose();
        }
    }
}

public enum CreatureState
{
    None = -1,
    Idle = 0,
    Move = 1,
    Die = 2,
    Respawn = 3,
    Attack = 10,
    //...
}
