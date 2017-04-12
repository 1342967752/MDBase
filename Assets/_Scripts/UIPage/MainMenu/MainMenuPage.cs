using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;
using System.Collections;
using DG.Tweening;
/// <summary>
/// 主页面
/// </summary>
public class MainMenuPage : TTUIPage {

    private Text lblPlayerName;
    private Image imgPlayer;
    private Text lblRoomCardNumber;
    private Text lblRoomBroadCast;
    private Transform BtnLeft;
    private Transform BtnRight;
    private Scrollbar scrollPanel;
    private Transform shaizi0;//麻将图标上的色子
    private Transform shaizi1;//麻将图标上的色子
    private Transform pai;//斗地主图标上的纸牌
    private string headUrl;
    public MainMenuPage() : base(UIType.Normal, UIMode.DoNothing, UICollider.None)
	{
        uiPath = MJPath.MainMenuPagePath;
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
        
        if (SocketEventHandle.getInstance().gameBroadcastNotice==null)
        {
            SocketEventHandle.getInstance().gameBroadcastNotice += broadCastResponse;
        }
    }

    /// <summary>
    /// 初始化ui
    /// </summary>
    private void initUI()
    {
        if (GlobalDataScript.loginResponseData != null)
        {
            headUrl = GlobalDataScript.loginResponseData.account.headicon;
            setPlayerName(GlobalDataScript.loginResponseData.account.nickname);
            setRoomCardNumber(GlobalDataScript.loginResponseData.account.roomcard);

            if (GlobalDataScript.headSprite == null)
            {
                MJUIManager._instance.StartCoroutine(LoadImg());
            }
            else
            {
                setPlayerHeadImage(GlobalDataScript.headSprite);
            }
        }
 
        GlobalDataScript.isStartGame = false;
    }

    /// <summary>
    /// 找到view
    /// </summary>
    private void findView()
    {
        lblPlayerName = this.transform.Find("TopBar/PersonImage/LabelName").GetComponent<Text>();
        imgPlayer = this.transform.Find("TopBar/PersonImage/HeadImage").GetComponent<Image>();
        lblRoomCardNumber = this.transform.Find("TopBar/BtnRoomCard/LabelRoomCardNumber").GetComponent<Text>();
        lblRoomBroadCast = this.transform.Find("Broadcast/bg/Text").GetComponent<Text>();
        shaizi0 = this.transform.Find("MidBar/ScrollerPanel/GameBtns/BtnMaJiang/Image");
        shaizi1 = this.transform.Find("MidBar/ScrollerPanel/GameBtns/BtnMaJiang/Image1");
        pai = this.transform.Find("MidBar/ScrollerPanel/GameBtns/BtnDouDiZhu/Image");
        BtnLeft = this.transform.Find("MidBar/BtnLeft");
        BtnRight = this.transform.Find("MidBar/BtnRight");
        scrollPanel = this.transform.Find("MidBar/ScrollerPanel/Scrollbar").GetComponent<Scrollbar>();
        scrollPanel.onValueChanged.AddListener(scrollPanelChanged);
        BtnLeft.gameObject.SetActive(false);
        BtnRight.gameObject.SetActive(true);
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
        transform.Find("TopBar/BtnRoomCard").GetComponent<Button>().onClick.AddListener(() =>
        {
            //添加房卡
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
    }

    /// <summary>
    /// 设置头像
    /// </summary>
    /// <param name="sprite"></param>
    public void setPlayerHeadImage(Sprite sprite)
    {
        imgPlayer.overrideSprite = sprite;
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
        float shaiziPos;
        broadCastTweener = lblRoomBroadCast.transform.DOLocalMoveX(-lblRoomBroadCast.transform.position.x, 25);
        broadCastTweener.SetAutoKill(false);
        broadCastTweener.Pause();
        shaiziSequence = DOTween.Sequence();
        shaiziPos = shaizi1.transform.position.y;
        //shaiziSequence.Append(shaizi1.transform.DOMoveY(shaiziPos- 0.1f, 1.0f));
        shaiziSequence.Append(shaizi1.transform.DOLocalMoveY(shaiziPos + 0.1f, 1.0f));
        shaiziSequence.PrependInterval(1);
        shaiziSequence.Play().SetLoops(30);
    }

    Sequence shaiziSequence;
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

    Texture2D texture2D;
    /// <summary>
    ///加载图片
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadImg()
    {
        //开始下载图片
        if (headUrl != null && headUrl != "")
        {
            WWW www = new WWW(headUrl);
            yield return www;
            //下载完成，保存图片到路径filePath
            try
            {
                texture2D = www.texture;
                byte[] bytes = texture2D.EncodeToPNG();
                //将图片赋给场景上的Sprite
                Sprite tempSp = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0));
                GlobalDataScript.headSprite=tempSp;
                setPlayerHeadImage(tempSp);//设置头像
            }
            catch (System.Exception e)
            {
                WantedTextTool.Instance.addTip("LoadImg" + e.Message, 0);
            }
        }
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
