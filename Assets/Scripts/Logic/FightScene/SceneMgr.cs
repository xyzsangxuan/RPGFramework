using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : Singleton<SceneMgr>
{
    public CameraController mainCameraControl;

    internal static void OnEnterMap(Cmd cmd)
    {
        if (!Net.CheckCmd(cmd, typeof(EnterMapCmd))) { return; }
        EnterMapCmd enterMapCmd = cmd as EnterMapCmd;

        // 跳转地图时，要重置RoleMgr、摇杆事件/摇杆状态、场景触摸事件、UI事件
        Reset();

        //enterMapCmd.mapId
        SceneMgr.instance.loadScene(enterMapCmd.mapId);
        
    }

    //重置场景
    private static void Reset()
    {
        //loading界面 Top层
        UIManager.instance.RemoveLayer(UILayer.Normal);
        RoleMgr.instance.Reset();
        NpcMgr.instance.Reset();
        FightUIMgr.instance.Reset();

        //
        TimerMgr.instance.StopAll();
        //不断加东西...
    }

    //加载场景
    private void loadScene(int mapId)
    {
        var mapDatabase = MapTable.instance[mapId];
        if(mapDatabase == null)
        {
            Debug.LogError("未找到地图: " + mapId);
            return;
        }

        UIManager.instance.UIEventSystemEnabled = false;
        
        //阻塞消息，暂时不处理
        Net.instance.Pause = true;

        //loading界面 UI的Top层

        var ao = SceneManager.LoadSceneAsync(mapDatabase.sceneName);
        QuickCoroutine.instance.StartCorontine(LoadEnd(ao));
        //SceneManager.LoadScene(mapDatabase.sceneName);
    }

    private IEnumerator LoadEnd(AsyncOperation ao)
    {
        while (!ao.isDone)
        {
            //进度条
            //Debug.Log(ao.progress);
            yield return new WaitForEndOfFrame();
        }
        
        OnLoadEnd();
    }

    private void OnLoadEnd()
    {
        InitCamera();
        FightUIMgr.instance.Init();
        //放开消息，进行分发
        Net.instance.Pause = false;
        UIManager.instance.UIEventSystemEnabled = true;
        //-------------场景加载完毕------------
        //销毁loading
    }

    private void InitCamera()
    {
        var camerObj = ResourcesManager.instance.GetInstance("SceneCamera");
        mainCameraControl = camerObj.GetComponent<CameraController>();
    }
}

