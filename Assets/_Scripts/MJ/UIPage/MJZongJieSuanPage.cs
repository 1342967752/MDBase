using UnityEngine;
using System.Collections.Generic;
using TinyTeam.UI;
using UnityEngine.UI;
using cn.sharesdk.unity3d;

public class MJZongJieSuan : TTUIPage
{
	public int PlayerNumber=4;//本局玩家数量
    public bool isShow = false;//是否显示

	public MJZongJieSuan() : base(UIType.Fixed, UIMode.HideOther, UICollider.None){
		uiPath = MyPath.MJZongJieSuanPath;
	}

	public override void Awake(GameObject go)
	{
        MJUIManager._instance.mJZongJieSuan = this;
		setBtnClickListener();
		findView();
	}

    /// <summary>
    /// 设置按钮监听
    /// </summary>
	private void setBtnClickListener()
	{
		transform.Find("BtnBack").GetComponent<Button>().onClick.AddListener(() => {
            ClosePage<MJZongJieSuan>();
            MJUIManager._instance.loadingPage.Active();
            MJScenesManager.Instance.loadSceneNotAnim(SceneName.MainMenu);
        });
		transform.Find("BtnTuiChu").GetComponent<Button>().onClick.AddListener(()=>{
            ClosePage<MJZongJieSuan>();
            MJUIManager._instance.loadingPage.Active();
            MJScenesManager.Instance.loadSceneNotAnim(SceneName.MainMenu);

		});
		transform.Find("BtnFenXiang").GetComponent<Button>().onClick.AddListener(()=>{
            WechatOperateScript._instance.shareAchievementToWeChat(PlatformType.WeChat);
		});
	}

	private List<Transform> m_playerList=new List<Transform>();

    /// <summary>
    /// 找到view
    /// </summary>
    private void findView()
    {
        m_playerList.Clear();
        for (int i = 0; i < PlayerNumber; i++)
        {
            string name = "Player" + i;
            m_playerList.Add(transform.Find("Players/" + name));
            m_playerList[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 设置信息
    /// </summary>
    /// <param name="order">排名</param>
    /// <param name="headImage">头像</param>
    /// <param name="name">昵称</param>
    /// <param name="duijuShu">对局书></param>
    /// <param name="shengju">胜局数</param>
    /// <param name="leiji">累计分</param>
    /// <param name="uuid"></param>
    public void setPlayerInfoWithOrder(int order,Sprite headImage,string name,string duijuShu,string shengju,string leiji,int uuid){
		m_playerList[order].gameObject.SetActive(true);
		m_playerList[order].Find("HeadImage").GetComponent<Image>().overrideSprite=headImage;
		m_playerList[order].Find("LabelName").GetComponent<Text>().text=name;
		m_playerList[order].Find("LabelDuiJuShu").GetComponent<Text>().text = duijuShu;
		m_playerList[order].Find("LabelShengJu").GetComponent<Text>().text=shengju;
		m_playerList[order].Find("LabelLeiJi").GetComponent<Text>().text=leiji;

        if (uuid==MJPlayerManager._instance.getMyUUID())
        {
            m_playerList[order].GetComponent<Image>().sprite = Resources.Load<Sprite>(MyPath.MJJiSuanUIPath + "Zong_My_Bg");
        }
        else
        {
            m_playerList[order].GetComponent<Image>().sprite = Resources.Load<Sprite>(MyPath.MJJiSuanUIPath + "Zong_Other_Bg");
        }
	}
}
