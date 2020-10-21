using LitJson;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;

public interface IDataBase
{
    void Init();
    Player GetUserData(int id);
    void SavePlayerData(Player playerData);
    void Close();
}

public class SQLiteMgr : Singleton<SQLiteMgr>, IDataBase
{
    private SqliteConnection conn;
    private SqliteCommand cmd;

    const string PlayerTableName = "PlayerData";

    //初始化
    public void Init()
    {
        //new一个数据库连接，后面时数据库文件的路径，成功后会在当前项目根目录下生成一个文件ServerDB.db
        //conn = new SqliteConnection("Data Source=C:/ServerDB.db");
        conn = new SqliteConnection("Data Source=./ServerDB.db");
        //打开数据库连接
        conn.Open();
        //获取指令集操作对象
        cmd = conn.CreateCommand();
        //初始化一些表
        _InitTables();
    }

    public Player GetUserData(int id)
    {
        var dataTable = _ExecuteSelectTable(string.Format(QueryDefine.SELECT_PLAYER_DATA, PlayerTableName, id));
        //将结果保存成一条Player返回
        if (dataTable.Rows.Count <= 0) { return null; }
        string json = dataTable.Rows[0][0].ToString();
        //玩家的类可以序列化永久保存在数据库，也可以反序列化
        return JsonMapper.ToObject<Player>(json);
    }

    public void SavePlayerData(Player player)
    {   
        _ExecuteNoQuery(string.Format(QueryDefine.INSERT_WITH_UPDATE, PlayerTableName, player.thisId, JsonMapper.ToJson(player)));
     }
    public void Close()
    {
        conn.Close();
    }
    
    /// <summary>
    /// 初始化数据表
    /// </summary>
    private void _InitTables()
    {
        //执行
        _ExecuteNoQuery(string.Format(QueryDefine.CREATE_TABLE_BEGIN, PlayerTableName) + QueryDefine.CREATE_TABLE_END);

    }

    private DataTable _ExecuteSelectTable(string sql,SqliteParameter[] parameters = null)
    {
        cmd.CommandText = sql;
        if(parameters != null)
        {
            cmd.Parameters.AddRange(parameters);
        }
        //数据解析器
        SqliteDataAdapter adapter = new SqliteDataAdapter(cmd);
        //查询结果返回列表
        DataTable data = new DataTable();
        adapter.Fill(data);

        return data;
    }

    /// <summary>
    /// 执行一个查询语句，返回一个关联的SQLliteDataReader实力
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    private SqliteDataReader _ExecuteReader(string sql,SqliteParameter[] parameters)
    {
        cmd.CommandText = sql;
        if (parameters != null)
        {
            cmd.Parameters.AddRange(parameters);
        }
        conn.Open();
        return cmd.ExecuteReader(CommandBehavior.Default);
    }

    /// <summary>
    /// 执行非查询命令
    /// </summary>
    /// <param name="cmdStr"></param>
    /// <param name="parameters"></param>
    private void _ExecuteNoQuery(string cmdStr,SqliteParameter[] parameters = null)
    {
        //命令文本
        cmd.CommandText = cmdStr;
        //Utils.LogWithColor(cmdStr, "[write db]", "red");
        Debug.Log("<color=red>[write db]</color>" + cmdStr);

        if (parameters != null)
        {
            cmd.Parameters.AddRange(parameters);
        }
        //执行非查询语句
        cmd.ExecuteNonQuery();

        cmd.Parameters.Clear();
    }
}

/// <summary>
/// 通用的数据库查询模板
/// </summary>
public static class QueryDefine
{
    public const string CREATE_TABLE_BEGIN = "CREATE TABLE IF NOT EXISTS {0}(id integer NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,Data TEXT";
    public const string CREATE_TABLE_END = ")";

    public const string INSERT_WITH_UPDATE = "INSERT OR REPLACE INTO {0} VALUES ({1},'{2}')";
    public const string SELECT_PLAYER_DATA = "select Data from {0} where id = {1}";


}

