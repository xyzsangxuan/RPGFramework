using System;
using UnityEngine;
using UnityEngine.UI;
//功能大厅
public class FunctionHall 
{
    GameObject _root;
    
    public FunctionHall()
    {
        _root = UIManager.instance.Add("UIPrefabs/FightUI/FunctionHall", UILayer.FightUI);

        var btnRoleAttr = _root.Find<Button>("BtnRole");

        btnRoleAttr.onClick.AddListener(OnRoleAttrClick);
    }

    private void OnRoleAttrClick()
    {
        //UIManager.instance.Add("UIPrefabs/System/RoleAttr/RoleAttrDlg");
        //new RoleAttrDlg();

        var roleAttr = DialogMgr.instance.Open<RoleAttrDlg>();
    }
}

