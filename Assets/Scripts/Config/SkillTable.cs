//技能表
public class SkillDatabase : TableDatabase
{
    //ID
    public string name;
    public string desc;
    public string scriptType;
    public string timeLine;
    //数值统一配置
    public float damage;
    public float castRange;
    public float preTime;
    public float CD;
    public float damageRange;
    public float duringTime;
    public float cost;
    
}

public class SkillTable : ConfigTable<SkillDatabase,SkillTable>
{
    
    
}
