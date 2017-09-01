using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;
using System.Collections.Generic;
/// <summary>
/// 退房间请求
/// </summary>
public class MJQuitRoomPage : TTUIPage {
    private GameObject bg01;//退出房间界面、
    private Text bg01Text;
    private GameObject bg02;//请求申请退出房间界面
    private GameObject bg03;//申请退出房间界面

    private GameObject bg03OkBtn;//背景三上面的两个按钮
    private GameObject bg03CancelBtn;
    private Text bg03Title;

    //存放玩家的控件
    private Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();

	public MJQuitRoomPage() : base(UIType.PopUp, UIMode.DoNothing, UICollider.None)
    {
        uiPath = MyPath.MJQuitRoomPagePath;
    }

    public override void Awake(GameObject go)
    {
        MJUIManager._instance.mJQuitRoomPage = this;
        init();
    }

   
    void init()
    {
        players.Clear();
        bg01 = transform.FindChild("Bg01").gameObject;
        bg02 = transform.FindChild("Bg02").gameObject;
        bg03 = transform.FindChild("Bg03").gameObject;

        transform.FindChild("Bg01/OK").GetComponent<Button>().onClick.AddListener(bg01OKOnClick);
        transform.FindChild("Bg01/Cancel").GetComponent<Button>().onClick.AddListener(bg01CancelClick);
        bg01Text= bg01.transform.FindChild("Info").GetComponent<Text>();

        transform.FindChild("Bg02/OK").GetComponent<Button>().onClick.AddListener(bg02OkOnClick);
        transform.FindChild("Bg02/Cancel").GetComponent<Button>().onClick.AddListener(bg01CancelClick);

        bg03OkBtn = transform.FindChild("Bg03/OK").gameObject;
        bg03OkBtn.GetComponent<Button>().onClick.AddListener(bg03OkOnClick); ;
        bg03CancelBtn = transform.FindChild("Bg03/Cancel").gameObject;
        bg03CancelBtn.GetComponent<Button>().onClick.AddListener(bg03CancelOnClick);
        bg03Title = bg03.transform.FindChild("Title").GetComponent<Text>();

        //获取四个玩家控件
        for (int i = 0; i < 4; i++)
        {
            players.Add(i, bg03.transform.FindChild("player/" + i).gameObject);
            bg03.transform.FindChild("player/" + i + "/select").gameObject.SetActive(false);
            bg03.transform.FindChild("player/" + i + "/wait").gameObject.SetActive(true);
        }

       if (GlobalDataScript.isRecord)
       {
            bg01Text.text = MyName.QuitRecord;
        }
        else
        {
            bg01Text.text = MyName.QuitRoomNoVote;
        }
    }

    #region bg01
    public void showRequestQuit()
    {
        closeAllUI();
        bg01.SetActive(true);
    }

    private void bg01OKOnClick()
    {
        if (GlobalDataScript.isRecord)
        {
            MJScenesManager.Instance.loadSceneNotAnim(SceneName.MainMenu);
            GlobalDataScript.reinitData();
            return;
        }

        GameObject.Find("init").GetComponent<MyMahjongScript>().quiteRoom();
        ClosePage<MJQuitRoomPage>();
        MJUIManager._instance.loadingPage.Active();
    }

    private void bg01CancelClick()
    {
        Debug.Log("退出界面关闭");
        ClosePage<MJQuitRoomPage>();
    }
    #endregion

    #region bg02
    public void showRequestVote()
    {
        Debug.Log("显示请求对退出");
        closeAllUI();
        bg02.SetActive(true);
    }

    private void bg02OkOnClick()
    {
        GameObject.Find("init").GetComponent<MyMahjongScript>().doDissoliveRoomRequest(0 + "");//申请解散房间
        insertInfo(MJPlayerManager._instance.getMyUUID());
    }
    #endregion

    #region bg03
    private void showVote()
    {
        closeAllUI();
        bg03.SetActive(true);  
    }

    private void bg03OkOnClick()
    {
        GameObject.Find("init").GetComponent<MyMahjongScript>().doDissoliveRoomRequest(1 + "");
        bg03HintBtn();
    }

    private void bg03CancelOnClick()
    {
        GameObject.Find("init").GetComponent<MyMahjongScript>().doDissoliveRoomRequest(2 + "");
        bg03HintBtn();
    }

    public void bg03HintBtn()
    {
        bg03OkBtn.SetActive(false);
        bg03CancelBtn.SetActive(false);
    }

    /// <summary>
    /// 初始插入投票数据
    /// </summary>
    /// <param name="uuid"></param>
    public void insertInfo(int uuid)
    {
        showVote();
        GameObject temp;
        for (int i=0;i<MJPlayerManager._instance.getPlayerListCount();i++)
        {
            players[i].transform.FindChild("head").GetComponent<Image>().sprite = MJPlayerManager._instance.getPlayerByIndex(i).getHeadSprite();
            temp = players[i].transform.FindChild("select").gameObject;
            temp.SetActive(false);
            
            if (MJPlayerManager._instance.getPlayerByIndex(i).getPlayer().account.uuid==uuid)
            {
                temp.SetActive(true);
                temp.transform.parent.FindChild("wait").gameObject.SetActive(false);
                temp.GetComponent<Image>().sprite = Resources.Load<Sprite>(MyPath.MJSpritePath+"Quit/ok");
                bg03Title.text ="["+ MJPlayerManager._instance.getPlayerByIndex(i).getPlayer().account.nickname+ "]" + "发起投票解散对局";
            }
        }
    }

    /// <summary>
    /// 跟新投票信息
    /// </summary>
    /// <param name="uuid"></param>
    public void updateInfo(int uuid,bool isVote)
    {
        int index = MJPlayerManager._instance.getPlayerByUUID(uuid);
        if (!players.ContainsKey(index))
        {
            return;
        }
        players[index].transform.FindChild("wait").gameObject.SetActive(false);
        players[index].transform.FindChild("select").gameObject.SetActive(true);
        players[index].transform.FindChild("select").GetComponent<Image>().sprite = Resources.Load<Sprite>(MyPath.MJSpritePath + "Quit/" + (isVote ? "ok" : "cancel"));
    }
    #endregion
    /// <summary>
    /// 关闭所有ui
    /// </summary>
    private void closeAllUI()
    {
        bg01.SetActive(false);
        bg02.SetActive(false);
        bg03.SetActive(false);
    }

    /// <summary>
    /// 关闭ui
    /// </summary>
    public void close()
    {
        ClosePage<MJQuitRoomPage>();
        foreach (int i in players.Keys)
        {
            players[i].transform.FindChild("select").gameObject.SetActive(false);
            players[i].transform.FindChild("wait").gameObject.SetActive(true);
        }
    }

    public override void Refresh()
    {
        bg03OkBtn.SetActive(true);
        bg03CancelBtn.SetActive(true);
    }
}
