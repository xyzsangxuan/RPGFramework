using System;
using System.Collections.Generic;
using UnityEngine;
//客户端
public interface IClient
{
    void SendCmd(Cmd cmd);
    void Receive(Cmd cmd);
}
//服务器
public interface IServer
{
    //连接服务器
    void Connect(IClient client);
    void SendCmd(Cmd cmd);
    void Receive(Cmd cmd);
}

//客户端访问服务器,实现接口
public class Net : Singleton<Net>, IClient
{
    IServer _server;
    //消息类型，消息解析函数
    Dictionary<Type, Action<Cmd>> _parser = new Dictionary<Type, Action<Cmd>>();

    //消息缓存
    List<Cmd> _cache = new List<Cmd>();

    private bool _pause;
    public bool Pause { 
        get { return _pause; } 
        set { 
            _pause = value;
            if(value == false)
            {
                Receive(null);
            }
        }
    }

    public Net()
    {
        _parser.Add(typeof(RoleListCmd),UserData.OnRoleList);
        _parser.Add(typeof(EnterMapCmd), SceneMgr.OnEnterMap); 
        _parser.Add(typeof(MainRoleThisIdCmd), RoleMgr.OnMainRoleThisid);
        _parser.Add(typeof(CreateSceneRole), RoleMgr.OnCreateSceneRole);
        _parser.Add(typeof(CreateSceneNpc), NpcMgr.OncreateSceneNpc);
    }

    public void ConnectServer(Action successCallback,Action failedCallback)
    {
        //给变量_server赋值
        _server = Server.instance;
        _server.Connect(this);


        if (true)
        {
            if(successCallback != null) { successCallback(); }
        }else{
            if(successCallback != null) { failedCallback(); }
        } 
    }

    public void Receive(Cmd cmd)
    {
        if(cmd != null)
        {
            //所有的消息进入缓存
            _cache.Add(cmd);
        }
        
        if (Pause) { return; }

        foreach(var cachaCmd in _cache)
        {
            //_server.send();
            Debug.Log("客户端收到消息: " + cachaCmd.GetType());
            //分发给静态函数
            Action<Cmd> func;
            if (_parser.TryGetValue(cachaCmd.GetType(), out func))
            {
                if (func != null)
                {
                    func(cachaCmd);
                }
            }
        }

        _cache.Clear();
    }

    public void SendCmd(Cmd cmd)
    {
        _server.Receive(cmd);
    }

    public static bool CheckCmd(Cmd cmd,Type targetType)
    {
        if(cmd.GetType() != targetType)
        {
            Debug.LogError(string.Format("需要{0}，但是收到了{1}" + targetType, cmd.GetType()));
            return false;
        }
        return true;
    }
}
