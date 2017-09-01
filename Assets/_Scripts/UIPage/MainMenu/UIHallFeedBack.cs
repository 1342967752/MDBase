using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;
using AssemblyCSharp;
/// <summary>
/// 反馈页面
/// </summary>
public class UIHallFeedBack : TTUIPage {
	private ToggleGroup tgGroup;
    private bool isCanSend = true;//是否可以发送

	public UIHallFeedBack() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
	{
		uiPath = MyPath.UIFedBackPath;
	}

	public override void Awake(GameObject go)
	{
		setBtnClickListener();
    }

    public override void Refresh()
    {
        SocketEventHandle.getInstance().messageBoxNotice += feedBackResponse;
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
            CustomSocket.getInstance().sendMsg(new FeedBackRequest(new FeedModel(title,content)));
            isCanSend = false;
            Debug.Log("发送");
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

    /// <summary>
    /// 反馈回调
    /// </summary>
    /// <param name="respone"></param>
    private void feedBackResponse(ClientResponse respone)
    {
        MJUIManager._instance.backWindow.setInfo(respone.message);
        isCanSend = true;
    }

    public override void   Hide()
    {
        gameObject.SetActive(false);
        SocketEventHandle.getInstance().messageBoxNotice -= feedBackResponse;
    }
}
