//通用的NPC状态机 
public class NpcAI
{
    private const string IDLE_NAME = "Idle";
    private const string ATTACK_NAME = "Attack";

    private Creature _owner;

    public StateMachine _machine;
    private StateIdle _stateIdle;
    private StateAttack _stateAttack;//自动追击
    //private _stateBack;

    public void Init(Creature owner)
    {
        _machine = new StateMachine();
        _owner = owner;
        _stateIdle = new StateIdle();
        _stateIdle.Init(_owner, _machine);
        _stateAttack = new StateAttack();
        _stateAttack.Init(_owner, _machine);
        
        _machine.RegisterState(IDLE_NAME, _stateIdle);
        _machine.RegisterState(ATTACK_NAME, _stateAttack);
        _machine.SetDefaultState(IDLE_NAME);
    }

    #region
    class StateIdle : State
    {
        public override void EnterState()
        {
            _owner.StopMove();
        }
        public override void UpdateState()
        {
            if(_owner.curTarget != null)
            {
                _machine.ChangeState(ATTACK_NAME);
            }
            
        }
        public override void ExitState()
        {
            
        }
    }
    class StateAttack : State
    {
        public override void EnterState()
        {
            _owner.CastSkill(1);
        }
        public override void UpdateState()
        {
            if (!_owner.IsAssistOrCastingSkill)
            {
                _machine.ChangeState(IDLE_NAME);
            }
        }
        public override void ExitState()
        {

        }
    }
    #endregion
    public void Loop()
    {
        SceneSystem();
        _machine.DoWork();
    }
    //感知系统 
    private void SceneSystem()
    {
        if(_owner.curTarget != null)
        {
            var dis = Util.Distance2_5D(_owner.curTarget.Position, _owner.Position);
            if (dis > GameSetting.MaxVisioDis*1.2)
            {
                _owner.curTarget = null;
            }

                return;
        }
        //找到最近的角色 攻击
        float minDis = float.MaxValue;
        Role minDisRole = null;
        //选中最近的可攻击的Npc
        foreach (var rolePair in RoleMgr.instance.allRole)
        {
            var role = rolePair.Value;
            bool temp = role.CanBeAttacked(_owner);
            if (!temp) { continue; }
            var dis = Util.Distance2_5D(role.Position, _owner.Position);
            if (dis > GameSetting.MaxVisioDis) { continue; }
            if (dis < minDis)
            {
                minDis = dis;
                minDisRole = role;
            }
        }
        _owner.curTarget = minDisRole;
    }
}
