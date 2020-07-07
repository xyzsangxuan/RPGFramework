# RPGFramework
这是一个简单的RPG游戏模板
## 目前程序结构：

| 脚本结构 |  |  |  |  |
| ---- | ---- | ---- | ---- | ---- |
| Scripts  | Commmon | QuickCoroutine.cs |||
|  |  | ResourcesManager.cs |||
|  |  | Singleton.cs |||
|  | Config | ConfigTable.cs | ConfigTable.cs |ConfigTable|
|  |  |  |  | TableDatabase  |
|  |  |  | MapTable.cs | MapTable |
|  |  |  |  | MapDatabase |
|  |  |  | NpcTable.cs | NpcTable |
|  |  |  |  | NpcDatabase |
|  |  |  | RoleTable.cs | RoleTable |
|  |  |  |  | RoleDatabase |
|  | Extern | UnityExtern.cs |||
|  | Logic | GameMgr.cs |||
|  |  | MainRole.cs |||
|  |  | Role.cs |||
|  |  | RoleMgr.cs |||
|  |  | SceneMgr.cs |||
|  |  | UtilsMonoBehaviour.cs |||
|  | Net | Cmd.cs |Cmd||
|  |  ||SelectRoleInfo||
|  |  ||LoginCmd||
|  |  ||RoleListCmd||
|  |  ||SelectRoleCmd||
|  |  ||MainRoleThisIdCmd||
|  |  ||EnterMapCmd||
|  |  ||CreateSceneRole||
|  |  | Net.cs |Net||
|  |  ||IServer||
|  |  ||IClient||
|  | Server | CmdParser.cs |||
|  |  | DB.cs |||
|  |  ||SQLiteMgr||
|  |  ||IDataBase||
|  |  ||QueryDefine||
|  |  | Player.cs |||
|  |  | RoleServer.cs |||
|  |  | Server.cs |||
|  | UI | Setup | Login.cs ||
|  |  || SelectRole.cs ||
|  |  || TouchRotate.cs ||
|  | Setup ||||
|  | UserData ||||
|  |  ||||