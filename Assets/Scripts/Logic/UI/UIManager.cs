using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//UI管理
public class UIManager : Singleton<UIManager>
{
    //事件系统启用
    public bool UIEventSystemEnabled
    { 
        set { _eventSystem.enabled = value; }
        get { return _eventSystem.enabled; }
    }

    GameObject _uiRoot;

    EventSystem _eventSystem;

    //层次，根节点
    Dictionary<UILayer, GameObject> _uiLayerRoot = new Dictionary<UILayer, GameObject>();
    public void Init()
    {
        if(_uiRoot == null)
        {
            _uiRoot = ResourcesManager.instance.GetInstance("UIPrefabs/UISystem");
            GameObject.DontDestroyOnLoad(_uiRoot);
            _eventSystem = _uiRoot.Find<EventSystem>("EventSystem");
            
        }
        //ui 分5层
        var obj = _uiRoot.transform.Find("Canvas/Scene").gameObject;
        _uiLayerRoot.Add(UILayer.Scene, obj);
        _uiLayerRoot.Add(UILayer.Touch, _uiRoot.transform.Find("Canvas/Touch").gameObject);
        _uiLayerRoot.Add(UILayer.FightUI, _uiRoot.transform.Find("Canvas/FightUI").gameObject);
        _uiLayerRoot.Add(UILayer.Normal, _uiRoot.transform.Find("Canvas/Normal").gameObject);
        _uiLayerRoot.Add(UILayer.Top, _uiRoot.transform.Find("Canvas/Top").gameObject);

    }
    //删除某一层所有的UI
    internal void RemoveLayer(UILayer layer = UILayer.Normal)
    {
        _uiLayerRoot[layer].DestroyAllChildren();
    }

    public GameObject Repalce(string uiPath, UILayer layer = UILayer.Normal)
    {
        //Remove add
        RemoveLayer(layer);
        GameObject obj = Add(uiPath, layer);
        return obj;
    }

    //添加 删除

    public GameObject Add(string uiPath, UILayer layer = UILayer.Normal)
    {
        var root = ResourcesManager.instance.GetInstance(uiPath,_uiLayerRoot[layer].transform);
        return root;
    }
   
    public void Remove(GameObject ui)
    {
        ResourcesManager.instance.Release(ui);
    }
}
//ui层次
public enum UILayer { Scene, Touch,FightUI,Normal,Top}