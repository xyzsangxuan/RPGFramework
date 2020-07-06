/// <summary>
/// 单例模式基类模块
/// 需要具备：
/// 1.C#中 泛型的知识
/// 2.设计模式中 单例模式的知识
/// PS.多线程时加双锁
/// </summary>
public class Singleton<T> where T : new() //添加泛型约束—T必须要无参构造函数new()才能作为i参数传进来
{
    //静态具有唯一性
    private static T _instance;

    static Singleton()
    {
        _instance = new T();
    }

    public static T instance
    {
        get
        {
            return _instance;
        }
    }
}