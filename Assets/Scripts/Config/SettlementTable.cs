public class SettlementTableData : TableDatabase
{
    public string name;
    public string resPath;
    public float radius;
}

public class SettlementTable : ConfigTable<SettlementTableData, SettlementTable>
{

}
