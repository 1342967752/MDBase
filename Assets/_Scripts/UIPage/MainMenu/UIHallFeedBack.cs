using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;
/// <summary>
/// 反馈页面
/// </summary>
public class UIHallFeedBack : TTUIPage {
	private ToggleGroup tgGroup;
	public UIHallFeedBack() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
	{
		uiPath = MJPath.UIFedBackPath;
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

		this.transform.Find("BtnSend").GetComponent<Button>().onClick.AddListener(() => {
			string content = getContent();
			string title = getTitle();

		});


	}

	private string getContent(){
		return this.transform.Find("InputContent").GetComponent<InputField>().text;
	}

    /// <summary>
    /// 获取标题
    /// </summary>
    /// <returns></returns>
	private string getTitle(){
		return this.transform.Find("InputTitle").GetComponent<InputField>().text;
	}
}
