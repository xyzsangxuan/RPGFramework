using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEditor.SceneManagement;

public class EasyEditor : Editor
{
    [MenuItem("Custom/GotoSetup")]
    public static void GotoSetup()
    {
        EditorSceneManager.OpenScene(Application.dataPath + "/Scenes/Setup.unity");
    }

    [MenuItem("Custom/GotoUIEditor")]
    public static void GotoUIEditor()
    {
        EditorSceneManager.OpenScene(Application.dataPath + "/Scenes/UIEditor.unity");
    }

    //把配置文件放入Resources目录下
    [MenuItem("Custom/ConfigToResources")]
    public static void ConfigToResources()
    {
        //找到目标路径和源路径

        var srcPath = Application.dataPath + "/../Config/";
        var dstPath = Application.dataPath + "/Resources/Config/";
        //递归清空目录
        if (Directory.Exists(dstPath))
        {
            Directory.Delete(dstPath, true);
            Directory.CreateDirectory(dstPath);
        }
        else
        {
            Directory.CreateDirectory(dstPath);
        }
        
        
        //把源路径内的所有文件，复制到目标路径，并添加扩展名
        foreach(var filePath in Directory.GetFiles(srcPath))
        {
            var fileName = filePath.Substring(filePath.LastIndexOf('/') + 1);
            File.Copy(filePath, dstPath+fileName + ".bytes",true);
        }
        //强制刷新引擎客户端
        AssetDatabase.Refresh();

        Debug.Log("配置文件复制完毕！");
    }
}
