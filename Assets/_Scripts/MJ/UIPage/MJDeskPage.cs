using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;


public class MJDeskPage : TTUIPage {
    private Text leaveText;//局数显示
    private Text allText;
    private GameObject inviteBtn;//邀请按钮
    private GameObject jingParent;//精牌容器
    private Text leaveCards;//剩余牌数
    private GameObject jingContent;//精牌content

    public MJDeskPage() : base(UIType.Normal, UIMode.DoNothing, UICollider.Normal)
    {
        uiPath = MJPath.MJDeskPagePath;
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
        setleaveCards(136);
    }


    /// <summary>
    /// 显示精牌
    /// </summary>
    /// <param name="zheng"></param>
    /// <param name="fu"></param>
    public void setJing(int zheng,int fu)
    {
        if (jingContent.transform.childCount>0)
        {
            return;
        }

        Image card = Resources.Load<Image>(MJPath.MJBottonAddCardPath + "card");
        GameObject temp;
        //正精
        card.transform.FindChild("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(MJPath.MJBottonSpritePath+zheng);
        temp = GameObject.Instantiate(card.gameObject);
        temp.transform.FindChild("Jing").gameObject.SetActive(false);
        temp.transform.SetParent(jingContent.transform);
        temp.transform.localScale = Vector3.one;

        //副精
        card.transform.FindChild("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(MJPath.MJBottonSpritePath + fu);
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
        leaveCards.text ="" + count;
    }

    /// <summary>
    /// 设置局数
    /// </summary>
    /// <param name="curTime">剩余局数</param>
    /// <param name="tolTime">总局数</param>
    public void setTurn(int curTime, int tolTime)
    {
        leaveText.text = (tolTime- curTime) + "";
        allText.text = "/" + tolTime;
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
        setleaveCards(136);
        cleamJing();
    }
}
