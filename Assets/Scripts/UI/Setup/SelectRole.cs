using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 角色选择
/// </summary>
public class SelectRole : MonoBehaviour
{
    private GameObject _roleListContent;
    private Button _btnEnter;
    private ToggleGroup _roleListToggleGroup;

    private GameObject _modelStudio;//模型摄影棚
    private GameObject _modelPositon;//模型放置节点

    private TouchRotate _modelTouchRotate; //模型旋转节点

    private int _selectRoleIndex = -1;
    private int _lastRoleIndex = 2;//假装记录上次选择的角色
    private void Awake()
    {
        _roleListContent = transform.Find("RoleList/Viewport/Content").gameObject;
        _btnEnter = transform.Find("BtnEnter").GetComponent<Button>();
        _roleListToggleGroup = _roleListContent.GetComponent<ToggleGroup>();

        _btnEnter.onClick.AddListener(onBtnEnterClick);

        _modelTouchRotate = gameObject.Find<TouchRotate>("TouchRotate");

        //处理模型摄影棚部分
        _modelStudio = ResourcesManager.instance.GetInstance("UIPrefabs/SelectRole/ModelStudio");
        _modelPositon = _modelStudio.Find<Transform>("ModelPostion").gameObject;
        //前面的地址变成了后面的地址
        //_modelTouchRotate.target = _modelPositon.transform;
        _modelTouchRotate.DragCallBack = OnTouchRotate; 

        //初始化角色列表,用表格读取角色从1开始累加/通过服务器读取从0开始累加
        int i = 0;
        foreach (var roleInfo in UserData.instance.allRole)
        //foreach(var roleInfo in RoleTable.instance.GetAll())
        {
            var roleItem = ResourcesManager.instance.GetInstance(("UIPrefabs/SelectRole/RoleItem"), _roleListContent.transform);
            
            //获取roleItem的text和image
            var textName = roleItem.transform.Find("Label").GetComponent<Text>();
            var toggle = roleItem.GetComponent<Toggle>();
            toggle.group = _roleListToggleGroup;
            textName.text = roleInfo.name;
            //textName.text = roleInfo.Value.name;
            //用闭包实现角色索引和Toggle的绑定
            var index = i;
            ++i;
            toggle.onValueChanged.AddListener((isOn)=> { OnToggleValueChanged(index, isOn); });
            //默认选中
            toggle.isOn = index == _lastRoleIndex;
        }
    }
    //解耦重构
    private void OnTouchRotate(PointerEventData obj)
    {
        _modelPositon.transform.Rotate(new Vector3(0, -obj.delta.x, 0));
    }

    private void OnToggleValueChanged(int roleindex,bool isOn)
    {
        if (isOn)
        {
            //记录选择的索引
            if(_selectRoleIndex == roleindex) { return; }

            //先清除之前留下的模型（清空ModelPosition下面的所有物体）
            _modelPositon.DestroyAllChildren();
            _modelPositon.transform.rotation = Quaternion.Euler(0,0,0);
            //记录选择的索引
            _selectRoleIndex = roleindex;
            var curRoleInfo = UserData.instance.allRole[roleindex];
            //var curRoleInfo = RoleTable.instance[roleindex];
            try
            {
                var modelPath = RoleTable.instance[curRoleInfo.modelId].modelPath;
                var model = ResourcesManager.instance.GetInstance((modelPath), _modelPositon.transform);
            }
            catch(Exception message)
            {
                Debug.LogError("生成模型出错！"+message);
            }
            
            
        }
    }

    private void onBtnEnterClick()
    {
        Debug.Log("选中了角色：" + _selectRoleIndex);

        //发送一个选中角色消息
        SelectRoleCmd cmd = new SelectRoleCmd() { index = _selectRoleIndex };
        Net.instance.SendCmd(cmd);
    }
}
