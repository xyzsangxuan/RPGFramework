//角色表内数据结构
public class RoleDatabase:TableDatabase
{
    public string name;
    public string modelPath;
}

//角色表 字典
public class RoleTable: ConfigTable<RoleDatabase,RoleTable>
{
    public RoleTable()
    {
        Load("Config/RoleTable.csv");   
    }
}
