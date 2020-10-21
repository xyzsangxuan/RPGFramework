//目标头像
using UnityEngine;
using UnityEngine.UI;

public class TargetHead 
{
    public GameObject _root;

    Button _btnHead;

    Text _textName;

    Slider _hp;

    GameObject _menu;

    public TargetHead()
    {
        _root = UIManager.instance.Add("UIPrefabs/FightUI/TargetHead", UILayer.FightUI);
        _btnHead = _root.Find<Button>("Root");
        _btnHead.onClick.AddListener(OnHeadClick);
        _textName = _root.Find<Text>("Root/Name");
        _hp = _root.Find<Slider>("Root/SliderHp");
    }

    public void SetActive(bool bActive)
    {
        if(_root == null) { return; }

        _root.SetActive(bActive);

    }

    private void OnHeadClick()
    {
        _menu = UIManager.instance.Add("UIPrefabs/FightUI/MenuList", UILayer.FightUI);

        var outSide = _menu.AddComponent<TouchQutSideEx>();
        outSide.OutSideCallback = AutoCLoseMenu;
    }

    private void AutoCLoseMenu()
    {
        UIManager.instance.Remove(_menu);
    }

    public void SetInfo(string name, float curHp, float maxHp, bool bActive = true)
    {
        SetActive(bActive);

        _hp.value = curHp / maxHp;
        _textName.text = name;
    }

    public void Show()
    {
        SetActive(true);
    }

    public void Hide()
    {
        SetActive(false);
    }
}


