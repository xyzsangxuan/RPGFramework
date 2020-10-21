using System;
using UnityEngine;

///资源管理器。加载方式和使用逻辑分离
///以便于支持热更新、对象池
public class ResourcesManager : Singleton<ResourcesManager>
{
    public GameObject GetInstance(string resPath,Transform parent)
    {
        var temp = GetResources<GameObject>(resPath);
        if(temp == null) { Debug.LogError("未找到资源"+resPath); return null; }

        return GameObject.Instantiate(temp,parent);
    }
    public GameObject GetInstance(string resPath)
    {
        return GameObject.Instantiate(GetResources<GameObject>(resPath));
    }

    public GameObject GetInstance(string resPath,Vector3 pos)
    {
        var temp = GetResources<GameObject>(resPath);
        if (temp == null) { Debug.LogError("未找到资源" + resPath); return null; }

        return GameObject.Instantiate(temp, pos,temp.transform.rotation);
    }


    public T GetResources<T>(string respath)where T:UnityEngine.Object
    {
        return Resources.Load<T>(respath);
    }

    internal void Release(GameObject ui)
    {
        GameObject.Destroy(ui);
    }
}
