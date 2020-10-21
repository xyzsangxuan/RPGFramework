using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIModel
{
    public UIModelStudio uIModelStudio;
    public UIModel(int width,int height,RawImage targetImage, GameObject modelRoot)
    {

        uIModelStudio = new UIModelStudio();
        uIModelStudio.Init();
        uIModelStudio.SetModel(modelRoot);

        var camera = uIModelStudio.Root.Find<Camera>("Camera");

        var texture = new RenderTexture(width, height, 24);
        camera.targetTexture = texture;

        //texture要画在图片上
        targetImage.texture = texture;
    }

    public void SetStudioPosition(ref Vector3 pos)
    {
        uIModelStudio.Root.transform.position = pos;
    }

    public void Destroy()
    {
        uIModelStudio.Destroy();
    }

}
//管理所有的UI模型，使其不干扰
public class UIModelMgr : Singleton<UIModelMgr>
{
    List<UIModel> _allUIModel = new List<UIModel>();

    private Vector3 _srcPos = new Vector3(-5000, -5000, -5000);

    private float _deltaY = -20;

    private Vector3 _curPos;

    public UIModelMgr()
    {
        _curPos = _srcPos;
    }

    public UIModel CreateUIModel(int width, int height, RawImage targetImage, GameObject modelRoot)
    {
        var uiModel = new UIModel(width,height,targetImage,modelRoot);
        uiModel.SetStudioPosition(ref _curPos);

        _curPos = new Vector3(_curPos.x, _curPos.y + _deltaY, _curPos.z);

        _allUIModel.Add(uiModel);

        return uiModel;
    }

    public void Release(UIModel uIModel)
    {
        uIModel.Destroy();
        _allUIModel.Remove(uIModel);
    }
}