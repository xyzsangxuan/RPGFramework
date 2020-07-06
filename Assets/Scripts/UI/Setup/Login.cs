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
        Debug.Log("ok btn");
        //链接服务器，等待返回数据
        //暂时用假数据，直接进选人界面
        SceneManager.LoadScene("SelectRole");
    }
}
