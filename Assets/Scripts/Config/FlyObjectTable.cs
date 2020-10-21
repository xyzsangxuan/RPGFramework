using UnityEngine;
using System.Collections;

public class FlyObjectDatabase : TableDatabase
{
    public string name;
    public string resPath;
    public float flySpeed;
    public float radius;
    public float duringTime;
    //是否追踪
    public bool trace;
}

//飞行道具表
public class FlyObjectTable : ConfigTable<FlyObjectDatabase,FlyObjectTable>
{
    
    
}
