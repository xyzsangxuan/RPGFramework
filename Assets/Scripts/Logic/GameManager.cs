using UnityEngine.SceneManagement;

class GameManager : Singleton<GameManager>
{
    public void Init()
    {
        //启动游戏引擎
        //跳转第一个逻辑界面
        SceneManager.LoadScene("Login");
    }
}

