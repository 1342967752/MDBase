using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;
using cn.sharesdk.unity3d;
/// <summary>
/// 分享页面
/// </summary>
public class UIShare : TTUIPage
{
	public UIShare() : base(UIType.PopUp, UIMode.HideOther, UICollider.None)
	{
		uiPath = MyPath.UISharePagePath;
	}

	public override void Awake(GameObject go)
	{
		setBtnClickListener();
	}

    /// <summary>
    /// 设置按钮监听
    /// </summary>
	private void setBtnClickListener()
	{
		transform.Find("BtnBack").GetComponent<Button>().onClick.AddListener(() => {
			Hide();
		});
		transform.Find("BtnQQ").GetComponent<Button>().onClick.AddListener(() => {
		});
		transform.Find("BtnWeChat").GetComponent<Button>().onClick.AddListener(() => {
            WechatOperateScript._instance.shareApp(PlatformType.WeChat);
		});
		transform.Find("BtnQQSpace").GetComponent<Button>().onClick.AddListener(() => {

		});
		transform.Find("BtnWeChatSpace").GetComponent<Button>().onClick.AddListener(() => {
            WechatOperateScript._instance.shareApp(PlatformType.WeChat);
        });
	}
}
