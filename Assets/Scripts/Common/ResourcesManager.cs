using UnityEngine;

///资源管理器。加载方式和使用逻辑分离
///以便于支持热更新、对象池
public class ResourcesManager : Singleton<ResourcesManager>
{
    public GameObject GetInstance(string respath,Transform parent)
    {
        return GameObject.Instantiate(GetResources<GameObject>(respath), parent);
    }
    public GameObject GetInstance(string respath)
    {
        return GameObject.Instantiate(GetResources<GameObject>(respath));
    }

    public T GetResources<T>(string respath)where T:UnityEngine.Object
    {
        return Resources.Load<T>(respath);
    }
}
