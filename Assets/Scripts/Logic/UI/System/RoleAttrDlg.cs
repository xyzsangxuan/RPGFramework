using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class RoleAttrDlg:Dialog
{
    //ModelStudio modelStudio;
    UIModel uIModel;
    public RoleAttrDlg()
    {
        Load("UIPrefabs/System/RoleAttr/RoleAttrDlg");

        var modelArea = _root.Find<RectTransform>("RoleModel");

        //加载模型
        var modelImage = _root.Find<RawImage>("RoleModel");
        var mainRole = RoleMgr.instance.MainRole;
        if (mainRole == null) { Debug.LogError("未找到主角"); return; }
        uIModel = UIModelMgr.instance.CreateUIModel(
            (int)modelArea.rect.width, 
            (int)modelArea.rect.height, 
            modelImage, 
            ResourcesManager.instance.GetInstance(mainRole.modelpath)
            );
    }

    protected override void OnCLose()
    {
        base.OnCLose();
        UIModelMgr.instance.Release(uIModel);
    }
}
