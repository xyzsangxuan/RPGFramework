using System.Collections.Generic;


public class CreatureDatabase : TableDatabase
{
    public string name;
    public string modelPath;
    public List<int> skillList;
    
    public int hp;
    public int attack;
    public int defence;
}

//Npc表内数据结构
public class NpcDatabase: CreatureDatabase
{
    public int NpcType;//1:功能NPC 2:怪物
}

//角色表 字典
public class NpcTable : ConfigTable<NpcDatabase,NpcTable>
{
    
}
