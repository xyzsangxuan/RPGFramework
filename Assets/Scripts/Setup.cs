using UnityEngine;

public class Setup : MonoBehaviour
{
    private void Awake()
    {
        //闪屏、更新、最后登录
        GameManager.instance.Init();

        Debug.Log(RoleTable.instance[1].name);
        Debug.Log(RoleTable.instance[2].name);
        Debug.Log(RoleTable.instance[3].name);
        Debug.Log(NpcTable.instance[1].name);
        Debug.Log(NpcTable.instance[2].name);
        Debug.Log(NpcTable.instance[3].name);
        Debug.Log(NpcTable.instance[4].name);
    }
}
