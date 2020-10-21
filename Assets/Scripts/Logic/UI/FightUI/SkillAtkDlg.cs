using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//技能操作面板
public class SkillAtkDlg
{
    GameObject _root;

    List<SkillPanel> _allSkillPanel = new List<SkillPanel>();

    public Action<int> OnSkillBtnClick;
    //技能索引 CD百分比
    public Func<int, float> OnGetSkillCD;

    public SkillAtkDlg()
    {
        _root = UIManager.instance.Add("UIPrefabs/FightUI/SkillAtkDlg", UILayer.FightUI);

        for (int i = 0;i<GameSetting.MaxSkillNum; ++i)
        {
            var skillPanel = new SkillPanel();

            var index = (i+1);
            //var index = i;
            string panelName = "SkillPanel" + index.ToString();
            skillPanel.btn = _root.Find<Button>(panelName);
            skillPanel.countDown = _root.Find<Image>(panelName + "/CountDownImage");
            skillPanel.cDText = _root.Find<Text>(panelName + "/CountDownLabel");


            skillPanel.btn.onClick.AddListener(() => OnSkillClick(index));

            _allSkillPanel.Add(skillPanel);
        }
        TimerMgr.instance.CreateTimerAndStart(0.08f, -1, UpdatedCD);
    }

    private void UpdatedCD()
    {
        if (OnGetSkillCD == null) { return; }
        for (int i = 0; i < _allSkillPanel.Count; ++i)
        {
            SetSkillCD(_allSkillPanel[i], OnGetSkillCD(i+1));
        }
        
    }

    private void OnSkillClick(int index)
    {
        if(OnSkillBtnClick != null) { OnSkillBtnClick(index); }
    }

    private void SetSkillCD(SkillPanel skillPanel,float percent)
    {
        skillPanel.countDown.fillAmount = percent;
    }
}

//技能面板中，单个技能相关组件打包
public class SkillPanel
{
    public Button btn;
    public Image countDown;
    public Text cDText;
}
