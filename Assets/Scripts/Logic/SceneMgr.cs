using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : Singleton<SceneMgr>
{
    internal static void OnEnterMap(Cmd cmd)
    {
        if (!Net.CheckCmd(cmd, typeof(EnterMapCmd))) { return; }
        EnterMapCmd enterMapCmd = cmd as EnterMapCmd;

        //enterMapCmd.mapId
        SceneMgr.instance.loadScene(enterMapCmd.mapId);
        
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
        //阻塞消息，暂时不处理
        Net.instance.Pause = true;
        var ao = SceneManager.LoadSceneAsync(mapDatabase.sceneName);
        QuickCoroutine.instance.StartCorontine(LoadEnd(ao));
        //SceneManager.LoadScene(mapDatabase.sceneName);
    }

    private IEnumerator LoadEnd(AsyncOperation ao)
    {
        while (!ao.isDone)
        {
            //进度条
            Debug.Log(ao.progress);
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("场景加载完毕！");
        //放开消息，进行分发
        Net.instance.Pause = false;
    }
}

