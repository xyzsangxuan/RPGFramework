using UnityEngine;
using UnityEngine.SceneManagement;

class GameMgr : Singleton<GameMgr>
{
    GameObject _engineRoot;

    public void Init()
    {
        UIManager.instance.Init();
        //初始化一个驱动引擎
        if (_engineRoot == null)
        {
            _engineRoot = new GameObject("GameEngine");
            GameObject.DontDestroyOnLoad(_engineRoot);
            _engineRoot.AddComponent<GameEngine>();
        }
        //协程的初始化
        QuickCoroutine.instance.Init();

        //启动游戏引擎
        //跳转第一个逻辑界面
        SceneManager.LoadScene("Login");
        UIManager.instance.Repalce("UIPrefabs/Login/Login", UILayer.Normal);
        
    }
}

