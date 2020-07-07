using System;
using System.Collections.Generic;
/// <summary>
/// 服务器的角色
/// </summary>
public class RoleServer
{
    static int _curThisId = 1;
    public static int getNewThisId()
    {
        return _curThisId++;
    }
}
