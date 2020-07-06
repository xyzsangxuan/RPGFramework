using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

public class TableDatabase
{
    public int id;
}


/// <summary>
/// 表格基类
/// </summary>
public class ConfigTable<TDatabase,T>: Singleton<T> 
    where T:new()
    where TDatabase: TableDatabase,new()//new()必须在最后面
{
    //相同
    //数据存储方式
    //id,数据条目
    Dictionary<int, TDatabase> _cache = new Dictionary<int, TDatabase>();

    protected void Load(string tablePath)
    {
        MemoryStream tableStream;

#if UNITY_EDITOR
        //开发期 读Project/Config
        //Unity为我们定义的众多平台宏定义之一，检测到有UNITY_EDITOR走上面的分支，否则走下面的分支
        var srcPath = Application.dataPath + "/../"+ tablePath;
        tableStream = new MemoryStream(File.ReadAllBytes(srcPath));
#else
        //发布之后读Resources/Config
        //读表
        //Config/NpcTable.csv(.bytes)
        //var table = Resources.Load<TextAsset>(tablePath);
        var table = ResourcesManager.instance.GetResources<TextAsset>(tablePath);
        //内存流
        tableStream = new MemoryStream(table.bytes);
#endif
        //using()自动关闭using开启的区域
        //读取器
        using (var reader = new StreamReader(tableStream, Encoding.GetEncoding("gb2312")))
        {
            //第一行字段名 反射
            var fieldNameStr = reader.ReadLine();
            var fieldNameArray = fieldNameStr.Split(',');
            List<FieldInfo> allFieldInfo = new List<FieldInfo>();
            foreach(var fieldName in fieldNameArray)
            {
                allFieldInfo.Add(typeof(TDatabase).GetField(fieldName));
            }
            //正式数据
            var lineStr = reader.ReadLine();
            while (lineStr != null)
            {
                TDatabase db = ReadLine(allFieldInfo, lineStr);
                _cache[db.id] = db;
                //循环
                lineStr = reader.ReadLine();
            }
        }
    }

    private static TDatabase ReadLine(List<FieldInfo> allFieldInfo, string lineStr)
    {
        //读到内存，处理数据
        var itemStrArray = lineStr.Split(',');
        var db = new TDatabase();
        //对每个字段循环解析
        //foreach(var field in allFieldIinfo)
        for (int i = 0; i < allFieldInfo.Count; ++i)
        {
            var field = allFieldInfo[i];
            var data = itemStrArray[i];
            //int float string bool array(list)
            if (field.FieldType == typeof(int))
            {
                field.SetValue(db, int.Parse(data));
            }
            else if (field.FieldType == typeof(string))
            {
                field.SetValue(db, data);
            }
            else if (field.FieldType == typeof(float))
            {
                field.SetValue(db, float.Parse(data));
            }
            else if (field.FieldType == typeof(bool))
            {
                field.SetValue(db, bool.Parse(data));
            }
            else if (field.FieldType == typeof(List<int>))
            {
                var list = new List<int>();
                //1$2$12$3
                foreach(var itemStr in data.Split('$'))
                {
                    list.Add(int.Parse(itemStr));
                }
                field.SetValue(db, list);
            }
            else if (field.FieldType == typeof(List<string>))
            {
                field.SetValue(db, new List<string>(data.Split('$')) );
            }
            else if (field.FieldType == typeof(List<float>))
            {
                var list = new List<float>();
                //1$2$12$3
                foreach (var itemStr in data.Split('$'))
                {
                    list.Add(float.Parse(itemStr));
                }
                field.SetValue(db, list);
            }
        }

        return db;
    }

    //获取表格数据
    //像数组一样随机访问 获取表格数据，索引器
    public TDatabase this[int index]
    {
        get
        {
            TDatabase db;
            _cache.TryGetValue(index, out db);
            return db;
        }
    }
    public Dictionary<int, TDatabase> GetAll()
    {
        return _cache;
    }
    //索引方式


    //不同
    //数据类型
    //文件路径

}
