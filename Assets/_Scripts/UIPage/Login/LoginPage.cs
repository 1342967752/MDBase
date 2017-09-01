using System.Collections;
using System.Collections.Generic;
using TinyTeam.UI;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 登录界面
/// </summary>
public class LoginPage : TTUIPage {
    private List<GameObject> btnLists = new List<GameObject>();
    private GameObject logining;//登录中显示ui

    public LoginPage() : base(UIType.Normal, UIMode.DoNothing, UICollider.None)
    {
        uiPath = MyPath.LoginPagePath;
    }

    public override void Awake(GameObject go)
    {
        MJUIManager._instance.loginPage = this;
        init();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void init()
    {
        btnLists.Clear();
        btnLists.Add(transform.FindChild("Login").gameObject);
        btnLists.Add(transform.FindChild("Login1").gameObject);
        btnLists.Add(transform.FindChild("Login2").gameObject);
        btnLists.Add(transform.FindChild("Login3").gameObject);
        btnLists.Add(transform.FindChild("Login4").gameObject);
        logining = transform.FindChild("Logining").gameObject;

        transform.FindChild("Login").GetComponent<Button>().onClick.AddListener(tologin);
        transform.FindChild("Login1").GetComponent<Button>().onClick.AddListener(login1);
        transform.FindChild("Login2").GetComponent<Button>().onClick.AddListener(login2);
        transform.FindChild("Login3").GetComponent<Button>().onClick.AddListener(login3);
        transform.FindChild("Login4").GetComponent<Button>().onClick.AddListener(login4);

        logining.SetActive(false);
        //transform.FindChild("Share").GetComponent<Button>().onClick.AddListener(shareImage);
        ShowPage<UpdatePage>();//更新请求
    }

    /// <summary>
    /// 登录
    /// </summary>
    private void tologin()
    {
        GameObject.Find("init").GetComponent<LoginSystemScript>().login();
        setAllBtn(false);
        setLoginText("正在登录");
    }

    #region 测试
    private void login1()
    {
        WechatOperateScript._instance.loginTest("oqbNexBvXinqBflK8LGvQJDgkvws", "oXvowv9yG3gTwQSz4urRU69Ha6DQ", 2);
        setAllBtn(false);
    }

    private void login2()
    {
        WechatOperateScript._instance.loginTest("player02", "dfewgfgwr23", 1);
        setAllBtn(false);
    }
    private void login3()
    {
        WechatOperateScript._instance.loginTest("player03", "dfvfewwr23", 2);
        setAllBtn(false);
    }
    private void login4()
    {
        WechatOperateScript._instance.loginTest("player04", "dfewfgwr23", 1); //魏勤伟id oqbNexC2Y5lP4zjpiFvCvXQ_B4Zg
        setAllBtn(false);
    }

    private void shareImage()
    {
        WechatOperateScript._instance.shareAchievementToWeChat(cn.sharesdk.unity3d.PlatformType.WeChat);;
    }
    #endregion
    
    /// <summary>
    /// 控制所有按钮显示
    /// </summary>
    private void setAllBtn(bool isShow)
    {
        if (btnLists==null||btnLists.Count==0)
        {
            return;
        }

        for (int i = 0; i < btnLists.Count; i++) 
        {
            btnLists[i].SetActive(isShow);
        }

        logining.SetActive(!isShow);

        if (!isShow)
        {
            MJUIManager._instance.StartCoroutine(loginErrorIE());
        }
    }

    /// <summary>
    /// 登录失败回调
    /// </summary>
    public void loginErrorCallBack()
    {
        if (btnLists == null || btnLists.Count == 0)
        {
            return;
        }
        setAllBtn(true);
        MJUIManager._instance.StopCoroutine(loginErrorIE());
    }

    /// <summary>
    /// 设置文字
    /// </summary>
    /// <param name=""></param>
    public void setLoginText(string str)
    {
        if (!logining.activeInHierarchy)
        {
            return;
        }
        logining.transform.FindChild("Text").GetComponent<Text>().text = str;
    }

    /// <summary>
    /// 登录成功回调
    /// </summary>
    public void loginSuccessCallBack()
    {
        setLoginText("登录成功");
        MJUIManager._instance.StopCoroutine(loginErrorIE());
    }

    /// <summary>
    /// 监听登录
    /// </summary>
    /// <returns></returns>
    IEnumerator loginErrorIE()
    {
        Debug.Log("登录监听");
        yield return new WaitForSeconds(10);
        loginErrorCallBack();
        WantedTextTool.Instance.addTip("网络错误", 1);
        Debug.Log("登录失败");

    }

   
}
