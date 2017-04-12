using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

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
        uiPath = MJPath.SelectRoomPagePath;
    }

    public override void Awake(GameObject go)
    {
        findView();
        setBtnClickListener();
    }

    public override void Refresh()
    {
        if (GlobalDataScript.loginResponseData!=null)
        {
            setPlayerHeadImage(GlobalDataScript.headSprite);
            setPlayerName(GlobalDataScript.loginResponseData.account.nickname);
            setRoomCardNumber(GlobalDataScript.loginResponseData.account.roomcard);
        }
        else
        {
            Debug.Log("没有登录");
        }
       
    }

    /// <summary>
    /// 找到view
    /// </summary>
    private void findView()
    {
        lblPlayerName = this.transform.Find("TopBar/PersonImage/LabelName").GetComponent<Text>();
        imgPlayer = this.transform.Find("TopBar/PersonImage/HeadImage").GetComponent<Image>();
        lblRoomCardNumber = this.transform.Find("TopBar/BtnRoomCard/LabelRoomCardNumber").GetComponent<Text>();
    }

    /// <summary>
    /// 设置按钮监听
    /// </summary>
    private void setBtnClickListener()
    {
        this.transform.Find("MidBar/BtnCreateRoom").GetComponent<Button>().onClick.AddListener(() =>
        {
            ShowPage<CreateRoomPage>();
        });
        this.transform.Find("MidBar/BtnJionRoom").GetComponent<Button>().onClick.AddListener(() =>
        {
            ShowPage<EnterRoomPage>();
        });

        this.transform.Find("TopBar/BtnBack").GetComponent<Button>().onClick.AddListener(() =>
        {
            ShowPage<MainMenuPage>();
            Hide();
        });
    }

    /// <summary>
    /// 设置玩家头像
    /// </summary>
    /// <param name="sprite"></param>
    public void setPlayerHeadImage(Sprite sprite)
    {
        imgPlayer.overrideSprite = sprite;
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
}
