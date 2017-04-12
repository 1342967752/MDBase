using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;
/// <summary>
/// 提示返回登录ui
/// </summary>
public class ReturnLoginWantedPage : TTUIPage {

    public ReturnLoginWantedPage() : base(UIType.PopUp, UIMode.HideOther, UICollider.None)
    {
        uiPath = MJPath.UIReturnLoginPath;
    }

    public override void Awake(GameObject go)
    {
        transform.FindChild("Bg/Ok").GetComponent<Button>().onClick.AddListener(okOnClick);
        transform.FindChild("Bg/Cancel").GetComponent<Button>().onClick.AddListener(cancelOnClick);
    }

    /// <summary>
    /// ok按钮事件
    /// </summary>
    private void okOnClick()
    {
        MJScenesManager.Instance.loadSceneNotAnim(SceneName.Login);
    }

    /// <summary>
    /// 取消按钮点击事件
    /// </summary>
    private void cancelOnClick()
    {
        ClosePage<ReturnLoginWantedPage>();
    }
}
