using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;
/// <summary>
/// 分享页面
/// </summary>
public class UIShare : TTUIPage
{
	public UIShare() : base(UIType.PopUp, UIMode.HideOther, UICollider.None)
	{
		uiPath = MJPath.UISharePagePath;
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
		this.transform.Find("BtnBack").GetComponent<Button>().onClick.AddListener(() => {
			Hide();
		});
		this.transform.Find("BtnQQ").GetComponent<Button>().onClick.AddListener(() => {
		});
		this.transform.Find("BtnWeChat").GetComponent<Button>().onClick.AddListener(() => {

		});
		this.transform.Find("BtnQQSpace").GetComponent<Button>().onClick.AddListener(() => {

		});
		this.transform.Find("BtnWeChatSpace").GetComponent<Button>().onClick.AddListener(() => {

		});
	}
}
