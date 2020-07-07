using System;
using UnityEngine;
//角色
public class Role : MonoBehaviour
{
    //只读不写，只读变量，不加修改
    CreateSceneRole _serverData;
    RoleDatabase _tableData;
    public void Init(CreateSceneRole serverData, RoleDatabase tableData)
    {
        _serverData = serverData;
        _tableData = tableData;
    }
}

