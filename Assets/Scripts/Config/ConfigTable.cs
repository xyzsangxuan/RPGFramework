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

    public ConfigTable()
    {
        //Load("");自己写
        Load("Config/" + GetType().ToString() + ".csv");
    }

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
            reader.ReadLine();//跳过注释（表中第一行）
            //字段名 反射
            var fieldNameStr = reader.ReadLine();//获取第一行数据
            var fieldNameArray = fieldNameStr.Split(',');//将第一行数据取成var数组
            List<FieldInfo> allFieldInfo = new List<FieldInfo>();//利用反射FieldInfo 初始化一个表名的空列
            foreach(var fieldName in fieldNameArray)
            {

                var fieldType = typeof(TDatabase).GetField(fieldName);//将数组中的每一个字符串元素，反射到TDatabase类中
                if(fieldType == null)
                {
                    Debug.LogError("表中字段未在程序中定义"+fieldName);
                    continue;
                }
                allFieldInfo.Add(fieldType);//将获取的对应的类型添加到空列中
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
                int fieldValue;
                if (!int.TryParse(data, out fieldValue))
                {
                    fieldValue = 0;
                }
                field.SetValue(db, fieldValue);
            }
            else if (field.FieldType == typeof(string))
            {
                field.SetValue(db, data);
            }
            else if (field.FieldType == typeof(float))
            {
                float fieldValue;
                if(!float.TryParse(data,out fieldValue))
                {
                    fieldValue = 0;
                }
                field.SetValue(db, fieldValue);
            }
            else if (field.FieldType == typeof(bool))
            {
                bool fieldValue;
                if (!bool.TryParse(data, out fieldValue))
                {
                    fieldValue = false;
                }
                field.SetValue(db, fieldValue);
            }
            else if (field.FieldType == typeof(List<int>))
            {
                var list = new List<int>();
                //1$2$12$3
                foreach(var itemStr in data.Split('$'))
                {
                    int fieldValue;
                    if (!int.TryParse(itemStr, out fieldValue))
                    {
                        fieldValue = 0;
                    }
                    list.Add(fieldValue);
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
                    float fieldValue;
                    if (!float.TryParse(itemStr, out fieldValue))
                    {
                        fieldValue = 0;
                    }
                    list.Add(fieldValue);
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
