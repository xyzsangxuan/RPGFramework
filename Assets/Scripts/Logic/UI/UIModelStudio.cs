using UnityEngine;

public class UIModelStudio :ModelStudio{
    public override void Init()
    {
        //处理模型摄影棚部分
        _modelStudio = ResourcesManager.instance.GetInstance("UIPrefabs/System/UIModelStudio");
        _modelPositon = _modelStudio.Find<Transform>("ModelPostion").gameObject;
    }
}


