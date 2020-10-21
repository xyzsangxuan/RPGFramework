using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//管理所有的系统界面
public class DialogMgr : Singleton<DialogMgr>{

    List<Dialog> _allDlg = new List<Dialog>();

   

    public T Open<T>() where T :Dialog,new()
    {
        var dlg = new T();

        _allDlg.Add(dlg);
        return dlg;
    }
    public void Close(Dialog dlg)
    {
        dlg.CloseSelf();
        _allDlg.Remove(dlg);
    }

    public void CloseAll()
    {
        foreach(var dlg in _allDlg)
        {
            dlg.CloseSelf();
        }
        _allDlg.Clear();
    }

}



//系统对话框的基类
public abstract class Dialog
{
    protected GameObject _root;
    public bool isAlive { get { return _root != null; } }
    protected void Load(string uiPath)
    {
        _root = UIManager.instance.Add(uiPath);

        var btnClose = _root.Find<Button>("BtnClose");

        if (btnClose != null)
        {
            btnClose.onClick.AddListener(OnCLose);
        }
        
    }
    public virtual void CloseSelf()
    {
        UIManager.instance.Remove(_root);
    }
    protected virtual void OnCLose()
    {
        CloseSelf();
    }
}