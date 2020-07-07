using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;

public interface IDataBase
{
    void Init();
    //PlayerData GetUserData(int id);
    //void SavePlayerData(PlayerData playerData);
    void Close();
}

public class SQLiteMgr : Singleton<SQLiteMgr>, IDataBase
{
    private SqliteConnection conn;
    private SqliteCommand cmd;

    const string PlayerTableName = "PlayerData";
    public void Init()
    {
        conn = new SqliteConnection("Data Source=./ServerDB.db");
        conn.Open();
        cmd = conn.CreateCommand();

        _InitTables();
    }

    /*public PlayerData GetUserData(int id)
    {
        var dataTable = _executeSelectTable(string.Format(QueryDefine.SELECT_PLAYER_DATA, PlayerTableName, id));
        if (fdataTable.Rows.Count <= 0) { return null; }
        string json = dataTable.Rows[0][0].ToString();
        return JsonMapper.ToObject<PlayerData>(Json);
    }*/

    /* public void SavePlayerData(PlayerData playerData)
     {
         _excuteNoQuery(string.For)
     }*/
    public void Close()
    {
        conn.Close();
    }
    
    
    /// <summary>
    /// 初始化数据表
    /// </summary>
    private void _InitTables()
    {
        _ExecuteNoQuery(string.Format(QueryDefine.CREATE_TABLE_BEGIN, PlayerTableName) + QueryDefine.CREATE_TABLE_END);

    }

    private DataTable _ExecuteSelectTable(string sql,SqliteParameter[] parameters = null)
    {
        cmd.CommandText = sql;
        if(parameters != null)
        {
            cmd.Parameters.AddRange(parameters);
        }
        SqliteDataAdapter adapter = new SqliteDataAdapter(cmd);
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
        cmd.CommandText = cmdStr;
        //Utils.LogWithColor(cmdStr, "[write db]", "red");
        Debug.Log("<color= red>[write db]</color>" + cmdStr);

        if (parameters != null)
        {
            cmd.Parameters.AddRange(parameters);
        }
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

    public const string INSERT_WITH_UPDATE = "INSERT OR REPLACE INTO {0} VALUES{1},'{2}')";
    public const string SELECT_PLAYER_DATA = "select Data from {0} where id = {1}";


}

