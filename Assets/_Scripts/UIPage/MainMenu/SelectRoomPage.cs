using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;
using AssemblyCSharp;

/// <summary>
/// 选择房间界面
/// </summary>
public class SelectRoomPage : TTUIPage {

    private Text lblPlayerName;
    private Image imgPlayer;
    private Text lblRoomCardNumber;
    private Text lblRoomBroadCast;
    private Transform BtnLeft;
    private Transform BtnRight;
    private Scrollbar scrollPanel;

    public SelectRoomPage() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
	{
        uiPath = MyPath.SelectRoomPagePath;
    }

    public override void Awake(GameObject go)
    {
        findView();
        setBtnClickListener();
        addListenner();
    }

    public override void Refresh()
    {
        if (GlobalDataScript.loginResponseData != null)
        {
            setPlayerHeadImage();
            setPlayerName(GlobalDataScript.loginResponseData.account.nickname);
            setRoomCardNumber(GlobalDataScript.loginResponseData.account.roomcard);
        }
        else
        {
            Debug.Log("没有登录");
        }

    }

    public override void Hide()
    {
        removeListenner();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 添加监听
    /// </summary>
    private void addListenner()
    {
        SocketEventHandle.getInstance().contactInfoResponse+= contactInfoCallBack;
    }

    /// <summary>
    /// 移除监听
    /// </summary>
    private void removeListenner()
    {
        SocketEventHandle.getInstance().contactInfoResponse -= contactInfoCallBack;
    }

    /// <summary>
    /// 找到view
    /// </summary>
    private void findView()
    {
        lblPlayerName = this.transform.Find("TopBar/PersonImage/LabelName").GetComponent<Text>();
        imgPlayer = this.transform.Find("TopBar/PersonImage/HeadImage/HeadImage").GetComponent<Image>();
        lblRoomCardNumber = this.transform.Find("TopBar/BtnRoomCard/LabelRoomCardNumber").GetComponent<Text>();
        imgPlayer.sprite = null;
    }

    /// <summary>
    /// 设置按钮监听
    /// </summary>
    private void setBtnClickListener()
    {
        transform.Find("MidBar/BtnCreateRoom").GetComponent<Button>().onClick.AddListener(() =>
        {
            ShowPage<CreateRoomPage>();
        });
        transform.Find("MidBar/BtnJionRoom").GetComponent<Button>().onClick.AddListener(() =>
        {
            ShowPage<EnterRoomPage>();
        });

        transform.Find("TopBar/BtnBack").GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("返回主界面");
            ShowPage<MainMenuPage>();
            Hide();
        });

        transform.Find("TopBar/BtnRoomCard/AddImage").GetComponent<Button>().onClick.AddListener(() =>
        {
            //请求添加房卡
            Debug.Log("添加房卡");
            CustomSocket.getInstance().sendMsg(new GetContactInfoRequest());
        });
    }

    /// <summary>
    /// 设置玩家头像
    /// </summary>
    /// <param name="sprite"></param>
    public void setPlayerHeadImage()
    {
        if (imgPlayer.sprite!=null)
        {
            return;
        }
        GameTools.instance.loadSpriteOnNet(loadSpriteCallBack, GlobalDataScript.loginResponseData.account.headicon);
    }

    /// <summary>
    /// 加载图片回调
    /// </summary>
    /// <param name="o"></param>
    private void loadSpriteCallBack(HttpLoadModel model)
    {
        if (model.error!=null)
        {
            return;
        }
        imgPlayer.sprite =model.sprite;
    }

    /// <summary>
    /// 设置玩家名字
    /// </summary>
    /// <param name="name"></param>
    public void setPlayerName(string name)
    {
        lblPlayerName.text = name;
    }

    /// <summary>
    /// 设置房卡
    /// </summary>
    /// <param name="number"></param>
    public void setRoomCardNumber(int number)
    {
        lblRoomCardNumber.text = "" + number;
    }

    /// <summary>
    /// 请求更新房卡回调
    /// </summary>
    /// <param name="response"></param>
    private void contactInfoCallBack(ClientResponse response)
    {
        MJUIManager._instance.backWindow.setInfo(response.message);
    }

}
