using System;
using System.Collections.Generic;
using UnityEngine;
//服务器
//服务器目前只支持一个客户端
public class Server : Singleton<Server>, IServer
{
    IClient _client;
    IDataBase _db;
    public IDataBase DB { get { return _db; } }
    //消息类型，消息解析函数
    Dictionary<Type, Action<Cmd>> _parser = new Dictionary<Type, Action<Cmd>>();

    //暂时只保留一个玩家
    public Player curPlayer;


    public Server()
    {
        _db = SQLiteMgr.instance;
        _db.Init();

        _parser.Add(typeof(LoginCmd), CmdParser.OnLogin);
        _parser.Add(typeof(SelectRoleCmd), CmdParser.OnSelectRole);
        _parser.Add(typeof(JumpTo), CmdParser.OnJumpMap);
    }

    public void Connect(IClient client)
    {
        _client = client;
    }

    public void Receive(Cmd cmd)
    {
        //_client.send();
        Debug.Log("服务器收到消息: "+cmd.GetType());


        //分发给静态函数
        Action<Cmd> func;
        if(_parser.TryGetValue(cmd.GetType(), out func)){
            if(func != null)
            {
                func(cmd);
            }
        }
    }

    public void SendCmd(Cmd cmd)
    {
        _client.Receive(cmd);
    }
}
