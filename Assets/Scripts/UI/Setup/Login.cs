using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 登录界面
/// </summary>
public class Login : MonoBehaviour
{
    private InputField _inputAccount;
    private InputField _inputPassword;
    private Button _btnOK;
    private void Awake()
    {
        //_inputAccount = transform.Find("InputAccount").GetComponent<InputField>();
        _inputAccount = gameObject.Find<InputField>("InputAccount");
        _inputPassword = gameObject.Find<InputField>("InputPassword");
        //_inputPassword = transform.Find("InputPassword").GetComponent<InputField>();
        //_btnOK = transform.Find("BtnLogin").GetComponent<Button>();
        _btnOK = gameObject.Find<Button>("BtnLogin");
        _btnOK.onClick.AddListener(OnBtnOKClick);

    }

    private void OnBtnOKClick()
    {
        var account = _inputAccount.text;
        var password = _inputPassword.text;
        //账号密码格式检验
        if (string.IsNullOrEmpty(account)|| string.IsNullOrEmpty(password))
        {
            return;
        }

        _inputAccount.interactable = false;
        _inputPassword.interactable = false;
        _btnOK.interactable = false;

        Net.instance.ConnectServer(DoSuccess,DoFailed);
    
    }

    private void DoFailed()
    {
        _inputAccount.interactable = true;
        _inputPassword.interactable = true;
        _btnOK.interactable = true;
    }

    private void DoSuccess()
    {
        var account = _inputAccount.text;
        var password = _inputPassword.text;
        var cmd = new LoginCmd() { account = account, password = password };
        Net.instance.SendCmd(cmd);
    }
}
