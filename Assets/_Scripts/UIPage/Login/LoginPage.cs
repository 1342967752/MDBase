using TinyTeam.UI;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 登录界面
/// </summary>
public class LoginPage : TTUIPage {

    public LoginPage() : base(UIType.Normal, UIMode.DoNothing, UICollider.None)
    {
        uiPath = MJPath.LoginPagePath;
    }

    public override void Awake(GameObject go)
    {
        transform.FindChild("Login").GetComponent<Button>().onClick.AddListener(tologin);
        transform.FindChild("Login1").GetComponent<Button>().onClick.AddListener(login1);
        transform.FindChild("Login2").GetComponent<Button>().onClick.AddListener(login2);
        transform.FindChild("Login3").GetComponent<Button>().onClick.AddListener(login3);
        transform.FindChild("Login4").GetComponent<Button>().onClick.AddListener(login4);
    }

    /// <summary>
    /// 登录
    /// </summary>
    private void tologin()
    {
        GameObject.Find("init").GetComponent<LoginSystemScript>().login();
        WantedTextTool.Instance.addTip("登录",0);
    }

    private void login1()
    {
        WechatOperateScript.Instance.loginTest("player01","dfewwr23",2);
        WantedTextTool.Instance.addTip("登录", 0);
    }

    private void login2()
    {
        WechatOperateScript.Instance.loginTest("player02", "dfewgfgwr23", 1);
        WantedTextTool.Instance.addTip("登录", 0);
    }
    private void login3()
    {
        WechatOperateScript.Instance.loginTest("player03", "dfvfewwr23", 2);
        WantedTextTool.Instance.addTip("登录", 0);
    }
    private void login4()
    {
        WechatOperateScript.Instance.loginTest("player04", "dfewfgwr23", 1);
        WantedTextTool.Instance.addTip("登录", 0);
    }
}
