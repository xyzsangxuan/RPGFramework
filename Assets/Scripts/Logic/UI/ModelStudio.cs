using UnityEngine;

public class ModelStudio{

    protected GameObject _modelStudio;//模型摄影棚
   
    public GameObject Root { get { return _modelStudio; } }

    protected GameObject _modelPositon;//模型放置节点

    public GameObject modelPlace { get { return _modelPositon; } }


    protected TouchEX _modelTouchRotate; //模型旋转节点

    protected GameObject _modelRoot; //放置的模型根节点
    public virtual void Init()
    {
        //处理模型摄影棚部分
        _modelStudio = ResourcesManager.instance.GetInstance("UIPrefabs/SelectRole/ModelStudio");
        _modelPositon = _modelStudio.Find<Transform>("ModelPostion").gameObject;
    }

    public void SetModel(GameObject modelRoot)
    {
        _modelRoot = modelRoot;
        modelRoot.transform.SetParent(_modelPositon.transform, false);
    }

    public void ClearModel()
    {
        _modelPositon.DestroyAllChildren();
        _modelPositon.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public void Destroy()
    {
        ResourcesManager.instance.Release(_modelStudio);
    }
}

