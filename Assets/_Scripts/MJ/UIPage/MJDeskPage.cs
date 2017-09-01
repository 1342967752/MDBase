using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

/// <summary>
/// 麻将游戏桌面
/// </summary>
public class MJDeskPage : TTUIPage {
    private Text leaveText;//局数显示
    private Text allText;
    private GameObject inviteBtn;//邀请按钮
    private GameObject jingParent;//精牌容器
    private Text leaveCards;//剩余牌数
    private GameObject jingContent;//精牌content
    private bool isRecord = false;//是够是战绩回放
    private Text speedText;//显示速度text
    private Text pauseText;//战绩回放暂停text

    public MJDeskPage() : base(UIType.Normal, UIMode.DoNothing, UICollider.Normal)
    {
        uiPath = MyPath.MJDeskPagePath;
    }

    public override void Awake(GameObject go)
    {
        MJUIManager._instance.mJDeskPage = this;

        leaveText = transform.FindChild("JuShu/LabelCurrent").GetComponent<Text>();
        allText = transform.FindChild("JuShu/LabelZong").GetComponent<Text>();
        jingParent = transform.FindChild("Jing").gameObject;
        transform.FindChild("Setting").GetComponent<Button>().onClick.AddListener(settingClick);
        transform.FindChild("Quit").GetComponent<Button>().onClick.AddListener(quit);
        inviteBtn = transform.FindChild("Invite").gameObject;
        inviteBtn.GetComponent<Button>().onClick.AddListener(inviteOnClick);
        leaveCards = transform.FindChild("LeaveCards/count").GetComponent<Text>();
        jingContent = transform.FindChild("Jing/jing").gameObject;
        jingContent.transform.parent.gameObject.SetActive(false);
        leaveCards.transform.parent.gameObject.SetActive(false);

        //是否战绩回放
        isRecord = GlobalDataScript.isRecord;
        if (isRecord)
        {
            //显示速度
            transform.FindChild("RecordXSpeed").gameObject.SetActive(true);
            speedText = transform.FindChild("RecordXSpeed/Text").GetComponent<Text>();
            transform.FindChild("RecordXSpeed").GetComponent<Button>().onClick.AddListener(()=>{
                speedText.text = "X" + MaJiangRecordScript.setSpeed();
            });
            speedText.text = "X" + MaJiangRecordScript.getSpeed();

            //显示暂停按钮
            transform.FindChild("RecordPauseBtn").gameObject.SetActive(true);
            pauseText = transform.FindChild("RecordPauseBtn/Text").GetComponent<Text>();
            transform.FindChild("RecordPauseBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                if (MaJiangRecordScript.isPause)
                {
                    MaJiangRecordScript.isPause = false;
                    pauseText.text = MyName.Pause;
                }
                else
                {
                    MaJiangRecordScript.isPause = true;
                    pauseText.text = MyName.Play;
                }
            });
        }
        else
        {
            transform.FindChild("RecordXSpeed").gameObject.SetActive(false);
            transform.FindChild("RecordPauseBtn").gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// 显示精牌
    /// </summary>
    /// <param name="zheng"></param>
    /// <param name="fu"></param>
    public void setJing(int zheng,int fu)
    {
        if (!jingContent.transform.parent.gameObject.activeInHierarchy)
        {
            jingContent.transform.parent.gameObject.SetActive(true);
        }

        if (jingContent.transform.childCount>0)
        {
            return;
        }

        Image card = Resources.Load<Image>(MyPath.MJBottonAddCardPath + "card");
        GameObject temp;
        //正精
        card.transform.FindChild("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(MyPath.MJBottonSpritePath+zheng);
        temp = GameObject.Instantiate(card.gameObject);
        temp.transform.FindChild("Jing").gameObject.SetActive(false);
        temp.transform.SetParent(jingContent.transform);
        temp.transform.localScale = Vector3.one;

        //副精
        card.transform.FindChild("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(MyPath.MJBottonSpritePath + fu);
        temp = GameObject.Instantiate(card.gameObject);
        temp.transform.FindChild("Jing").gameObject.SetActive(false);
        temp.transform.SetParent(jingContent.transform);
        temp.transform.localScale = Vector3.one;
    }

    /// <summary>
    /// 清除精牌
    /// </summary>
    private void cleamJing()
    {
        if (jingContent.transform.childCount>0)
        {
            int count = jingContent.transform.childCount;
            for (int i=0;i<count;i++)
            {
                GameObject.Destroy(jingContent.transform.GetChild(i).gameObject);
            }
        }
    }

    /// <summary>
    /// 设置剩余卡牌
    /// </summary>
    /// <param name="count"></param>
    public void setleaveCards(int count)
    {
        if (!leaveCards.transform.parent.gameObject.activeInHierarchy)
        {
            leaveCards.transform.parent.gameObject.SetActive(true);
        }

        leaveCards.text ="" + count;
    }

    /// <summary>
    /// 设置局数
    /// </summary>
    /// <param name="curTime">剩余局数</param>
    /// <param name="tolTime">总局数</param>
    public void setTurn(int curTime, int tolTime)
    {
        Debug.Log("单前局数:" + curTime);
        if (curTime==tolTime)
        {
            leaveText.transform.parent.gameObject.SetActive(false);
            return;
        }
        
        leaveText.text = (tolTime- curTime) + "";
        allText.text = "/" + tolTime;

        if (!leaveText.transform.parent.gameObject.activeInHierarchy)
        {
            leaveText.transform.parent.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 设置监听
    /// </summary>
    private void settingClick()
    {
        ShowPage<MJUISettingPage>();
    }

    private void quit()
    {
        if (GlobalDataScript.isRecord)
        {
            MJUIManager._instance.mJQuitRoomPage.showRequestQuit();//显示退出
            return;
        }

        if (GlobalDataScript.isStartGame)
        {
            MJUIManager._instance.mJQuitRoomPage.showRequestVote();
            MJUIManager._instance.mJQuitRoomPage.bg03HintBtn();//隐藏按钮
            Debug.Log("申请解散");
        }
        else
        {
            MJUIManager._instance.mJQuitRoomPage.showRequestQuit();//显示退出
        }
        Debug.Log("是否开始游戏" + GlobalDataScript.isStartGame);
    }

    /// <summary>
    /// 设置邀请按钮
    /// </summary>
    /// <param name="isShow"></param>
    public void setInviteBtn(bool isShow)
    {
        inviteBtn.SetActive(isShow);
    }

    /// <summary>
    /// 邀请按钮事件
    /// </summary>
    private void inviteOnClick()
    {
        WechatOperateScript._instance.inviteFriend();
    }

    /// <summary>
    /// 重置
    /// </summary>
    public void reset()
    {
        cleamJing();
        leaveCards.transform.parent.gameObject.SetActive(false);
        jingContent.transform.parent.gameObject.SetActive(false);
    }
}
