using System;
using UnityEngine;

public class FightUIMgr : Singleton<FightUIMgr> 
{
    //摇杆
    Joystick _joystick;

    //与场景的点击交互
    TouchScene _touchScene;
    //技能按钮
    SkillAtkDlg _skillAtkDlg;

    //人物头像
    //小地图
    //目标头像
    TargetHead _targetHead;
    //功能大厅
    FunctionHall _functionHall;

    public void Init()
    {
        if(_joystick == null)
        {
            _joystick = new Joystick();
        }

        if (_touchScene == null)
        {
            _touchScene = new TouchScene();
        }
        if (_functionHall == null)
        {
            _functionHall = new FunctionHall();
        }
        if(_targetHead == null)
        {
            _targetHead = new TargetHead();
            _targetHead.SetActive(false);
        }

        if (_skillAtkDlg == null)
        {
            _skillAtkDlg = new SkillAtkDlg();
        }
    }

    internal void SetTargetActive(bool bActive)
    {
        if (bActive)
        {
            _targetHead.Show();
        }
        else
        {
            _targetHead.Hide();
        }
    }

    public void BindingJoystick(Action<Vector2> onJoystickMove, Action onJoystickMoveEnd)
    {
        if(_joystick == null) { return; }
        _joystick.OnMoveDir = onJoystickMove;//
        _joystick.OnMoveEnd = onJoystickMoveEnd;//主角响应事件
    }

    public void BindingTouchScene(Action<RaycastHit> OntouchSth,int layer)
    {
        if (_touchScene == null) { return; }
        _touchScene.HitSthCallback = OntouchSth;
        _touchScene.unTouchLayer = layer;
    }

    public void BindingSkillBtn(Action<int> skillBtnCallback,Func<int,float> getSkillCDCallback)
    {
        if (_skillAtkDlg == null) { return; }
        _skillAtkDlg.OnSkillBtnClick = skillBtnCallback;
        _skillAtkDlg.OnGetSkillCD = getSkillCDCallback;
    }


    #region 目标头像
    public void SetTargetInfo(string name,int curHp,int maxHp, bool bActive = true)
    {
        SetTargetActive(true);

        _targetHead.SetInfo(name, curHp,maxHp,bActive);
    }

    #endregion


    public void ReleaseJoystick()
    {
        if (_joystick == null) { return; }
        _joystick.OnMoveDir = null;
        _joystick.OnMoveEnd = null;
    }

    public void ReleaseTouchScene()
    {
        if (_touchScene == null) { return; }
        _touchScene.HitSthCallback = null;
        
    }

    private void ResetJoystick()
    {
        if (_joystick == null) { return; }
        _joystick.Reset();
    }

    

    private void ResetSkillAtkDlg()
    {
        if (_skillAtkDlg == null) { return; }
        _skillAtkDlg.OnSkillBtnClick = null;
    }

    private void ResetTargetHead()
    {
        if (_targetHead == null) { return; }
        _targetHead.Hide();
    }




    internal void Reset()
    {
        ReleaseJoystick();
        ReleaseTouchScene();
        ResetJoystick();

        ResetTargetHead();
        ResetSkillAtkDlg();
    }
}
