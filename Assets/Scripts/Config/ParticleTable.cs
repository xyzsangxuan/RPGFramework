using UnityEngine;
using System.Collections;


public class ParticleDatabase : TableDatabase
{
    public string name;

    public string resPath;
}

public class ParticleTable : ConfigTable<ParticleDatabase,ParticleTable>
{
}
