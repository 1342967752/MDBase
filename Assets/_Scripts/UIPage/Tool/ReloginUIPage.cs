using UnityEngine;
using TinyTeam.UI;
using UnityEngine.Events;
using UnityEngine.UI;
/// <summary>
/// 重新登录界面
/// </summary>
public class ReloginUIPage : TTUIPage {
    private UnityAction btnUA;
    private Text info;

	public ReloginUIPage() : base(UIType.PopUp, UIMode.HideOther, UICollider.None)
    {
        uiPath = MJPath.UIReLoginPath;
    }

    public override void Awake(GameObject go)
    {
        MJUIManager._instance.reloginUIPage = this;
        init();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void init()
    {
        transform.FindChild("Bg/Login").GetComponent<Button>().onClick.AddListener(btnOnClick);
        info= transform.FindChild("Bg/Text").GetComponent<Text>();
    }

    /// <summary>
    /// 按钮事件
    /// </summary>
    private void btnOnClick()
    {
        if (btnUA!=null)
        {
            btnUA = null;
        }
        else
        {
            MJScenesManager.Instance.loadSceneNotAnim(SceneName.Login);  
        }
        ClosePage<ReloginUIPage>();
    }

    /// <summary>
    /// 设置按钮事件
    /// </summary>
    /// <param name="ua"></param>
    public void setOnClick(UnityAction ua)
    {
        btnUA = ua;
    }
 
    /// <summary>
    /// 设置信息
    /// </summary>
    /// <param name="info"></param>
    public void setInfo(string info)
    {
        this.info.text = info;
    }
}
