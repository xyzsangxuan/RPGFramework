using System.Collections.Generic;


//Npc表内数据结构
public class NpcDatabase:TableDatabase
{
    public string name;
    public string modelPath;
    public List<int> TestIDList;
}

//角色表 字典
public class NpcTable : ConfigTable<NpcDatabase,NpcTable>
{
    public NpcTable()
    {
        Load("Config/NpcTable.csv");
    }
}
