using UnityEngine.SceneManagement;

class GameMgr : Singleton<GameMgr>
{
    public void Init()
    {
        //协程的初始化
        QuickCoroutine.instance.Init();

        //启动游戏引擎
        //跳转第一个逻辑界面
        SceneManager.LoadScene("Login");
    }
}

