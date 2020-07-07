using UnityEngine;

public class Setup : MonoBehaviour
{
    private void Awake()
    {
        //闪屏、更新、最后登录
        GameMgr.instance.Init();
    }
}
