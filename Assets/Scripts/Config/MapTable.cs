public class MapDatabase : TableDatabase
{
    //id
    public string name;
    public string sceneName;//场景名字
}


//地图表
public class MapTable : ConfigTable<MapDatabase, MapTable>
{
    public MapTable()
    {
        Load("Config/MapTable.csv");
    }
}

