using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;
using DG.Tweening;
using AssemblyCSharp;
/// <summary>
/// 主页面
/// </summary>
public class MainMenuPage : TTUIPage {

    private Text lblPlayerName;
    private Image imgPlayer;//玩家头像
    private Text lblRoomCardNumber;
    private Text lblRoomBroadCast;
    private Transform BtnLeft;
    private Transform BtnRight;
    private Scrollbar scrollPanel;
    public MainMenuPage() : base(UIType.Normal, UIMode.DoNothing, UICollider.None)
	{
        uiPath = MyPath.MainMenuPagePath;
    }

    public override void Awake(GameObject go)
    {
        MJUIManager._instance.mainMenuPage = this;
        findView();
        setBtnClickListener();
        initAnimation();
    }

    public override void Refresh()
    {
        initUI();
        addListenner();   
    }

    //public override void Hide()
    //{
    //    gameObject.SetActive(false);
    //    removeListenner();
    //}

    /// <summary>
    /// 添加监听
    /// </summary>
    private void addListenner()
    {
        if (SocketEventHandle.getInstance().gameBroadcastNotice == null)
        {
            SocketEventHandle.getInstance().gameBroadcastNotice += broadCastResponse;
        }

        if (SocketEventHandle.getInstance().contactInfoResponse == null)
        {
            SocketEventHandle.getInstance().contactInfoResponse += contactInfoCallBack;
        }
    }

    /// <summary>
    /// 移除监听
    /// </summary>
    private void removeListenner()
    {
        if (SocketEventHandle.getInstance().gameBroadcastNotice !=null)
        {
            SocketEventHandle.getInstance().gameBroadcastNotice -= broadCastResponse;
        }

        if (SocketEventHandle.getInstance().contactInfoResponse != null)
        {
            SocketEventHandle.getInstance().contactInfoResponse -= contactInfoCallBack;
        }

    }

    /// <summary>
    /// 初始化ui
    /// </summary>
    private void initUI()
    {
        if (GlobalDataScript.loginResponseData != null)
        {
            setPlayerName(GlobalDataScript.loginResponseData.account.nickname);
            setRoomCardNumber(GlobalDataScript.loginResponseData.account.roomcard);
            setPlayerHeadImage();
            Debug.Log(GlobalDataScript.loginResponseData.scores);
        }
        GlobalDataScript.isStartGame = false;
        if (!GlobalDataScript.isShowMainMenuAdPage)
        {
            MJUIManager._instance.adPage.showAd();//显示广告
            GlobalDataScript.isShowMainMenuAdPage = true;
        }
    }

    /// <summary>
    /// 请求更新房卡回调
    /// </summary>
    /// <param name="response"></param>
    private void contactInfoCallBack(ClientResponse response)
    {
        MJUIManager._instance.backWindow.setInfo(response.message);
    }

    /// <summary>
    /// 找到view
    /// </summary>
    private void findView()
    {
        lblPlayerName = this.transform.Find("TopBar/PersonImage/LabelName").GetComponent<Text>();
        imgPlayer = this.transform.Find("TopBar/PersonImage/HeadImage/HeadImage").GetComponent<Image>();
        lblRoomCardNumber = this.transform.Find("TopBar/BtnRoomCard/LabelRoomCardNumber").GetComponent<Text>();
        lblRoomBroadCast = this.transform.Find("Broadcast/bg/Text").GetComponent<Text>();
        BtnLeft = this.transform.Find("MidBar/BtnLeft");
        BtnRight = this.transform.Find("MidBar/BtnRight");
        scrollPanel = this.transform.Find("MidBar/ScrollerPanel/Scrollbar").GetComponent<Scrollbar>();
        scrollPanel.onValueChanged.AddListener(scrollPanelChanged);
        imgPlayer.sprite = null;
        BtnLeft.gameObject.SetActive(false);
        BtnRight.gameObject.SetActive(true);

        if (GlobalDataScript.mainMenuState.isChange())
        {
            if (GlobalDataScript.mainMenuState.isShowZhanji)
            {
                ShowPage<UIZhanJi>();
            }
        }
    }

    /// <summary>
    /// 加个计数器，避免多次set
    /// </summary>
    /// <param name="value"></param>
    private void scrollPanelChanged(float value)
    {
        if (value < 0.5)
        {
            BtnLeft.gameObject.SetActive(false);
            BtnRight.gameObject.SetActive(true);
        }
        else
        {
            BtnLeft.gameObject.SetActive(true);
            BtnRight.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 设置按钮监听
    /// </summary>
    private void setBtnClickListener()
    {
        transform.Find("MidBar/BtnRight").GetComponent<Button>().onClick.AddListener(() =>
        {
            scrollPanel.value = 1.0f;
            Debug.Log("left");
        });
        transform.Find("MidBar/BtnLeft").GetComponent<Button>().onClick.AddListener(() =>
        {
            scrollPanel.value = 0.0f;
        });
        transform.Find("TopBar/BtnBack").GetComponent<Button>().onClick.AddListener(() =>
        {
            ShowPage<ReturnLoginWantedPage>();
        });
        transform.Find("TopBar/BtnRoomCard/AddImage").GetComponent<Button>().onClick.AddListener(() =>
        {
            //请求添加房卡
            Debug.Log("添加房卡");
            CustomSocket.getInstance().sendMsg(new GetContactInfoRequest());
        });
        transform.Find("BottomBar/BtnFanKui").GetComponent<Button>().onClick.AddListener(() =>
        {
            ShowPage<UIHallFeedBack>();
        });

        transform.Find("BottomBar/BtnSetting").GetComponent<Button>().onClick.AddListener(() =>
        {
            ShowPage<MJUISettingPage>();
        });
        transform.Find("BottomBar/BtnZhanJi").GetComponent<Button>().onClick.AddListener(() =>
        {
            ShowPage<UIZhanJi>();
        });
        transform.Find("BottomBar/BtnShare").GetComponent<Button>().onClick.AddListener(() =>
        {
            ShowPage<UIShare>();
        });
        transform.Find("BottomBar/BtnHelp").GetComponent<Button>().onClick.AddListener(() =>
        {
            ShowPage<UIHallHelp>();
        });

        transform.Find("MidBar/ScrollerPanel/GameBtns/BtnMaJiang").GetComponent<Button>().onClick.AddListener(() =>
        {
            ShowPage<SelectRoomPage>();
            Hide();
        });

        transform.Find("MidBar/ScrollerPanel/GameBtns/BtnDouDiZhu").GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("斗地主");
        });
        transform.Find("MidBar/ScrollerPanel/GameBtns/BtnMore").GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("更多");
        });

        imgPlayer.GetComponent<Button>().onClick.AddListener(() => {
            MJUIManager._instance.mJMyInfoPage.setInfo(imgPlayer,GlobalDataScript.loginResponseData.account.nickname,GlobalDataScript.loginResponseData.account.id+"",GlobalDataScript.loginResponseData.locationName,GlobalDataScript.loginResponseData.account.uuid,GlobalDataScript.loginResponseData.winTimes,
                GlobalDataScript.loginResponseData.loseTimes,GlobalDataScript.loginResponseData.drawTimes);
        });
    }

    /// <summary>
    /// 设置头像
    /// </summary>
    /// <param name="sprite"></param>
    public void setPlayerHeadImage()
    {
       if (imgPlayer==null)
       {
            return;
       }
        Debug.Log("加载头像");
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
            Debug.Log(model.error);
            return;
        }
        Debug.Log("加载成功");
        imgPlayer.sprite = model.sprite;
    }

    /// <summary>
    /// 设置名字
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
    /// 初始化动画
    /// </summary>
    private void initAnimation()
    {
        broadCastTweener = lblRoomBroadCast.transform.DOLocalMoveX(-lblRoomBroadCast.transform.position.x, 25);
        broadCastTweener.SetAutoKill(false);
        broadCastTweener.SetLoops(10);
        broadCastTweener.Pause();
    }

    Tweener broadCastTweener;
    /// <summary>
    /// 设置广播
    /// </summary>
    /// <param name="broadcast"></param>
    public void setBroadCast(string broadcast)
    {
        if (lblRoomBroadCast==null)
        {
            SocketEventHandle.getInstance().gameBroadcastNotice -= broadCastResponse;
            return;
        }

        lblRoomBroadCast.text = broadcast;
        broadCastTweener.Restart();
        Debug.Log("广播:" + broadcast);
    }

    /// <summary>
    /// 广播回调
    /// </summary>
    /// <param name="response"></param>
    private void broadCastResponse(ClientResponse response)
    {
        setBroadCast(response.message);
    }
  
}
