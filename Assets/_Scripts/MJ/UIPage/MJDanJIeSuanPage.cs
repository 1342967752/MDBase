using UnityEngine;
using System.Collections.Generic;
using TinyTeam.UI;
using UnityEngine.UI;
using AssemblyCSharp;

public class MJDanJIeSuan:TTUIPage{
	public int PlayerNubmer=4;//本局玩家数量
    private GameObject checkBtn;//查看牌局按钮
    private Button continueBtn;//继续游戏按钮
    private GameObject bg;
    private int curPlayerInfoIndex = 1;//单前可以设置玩家信息的位置

	public MJDanJIeSuan() : base(UIType.Fixed, UIMode.HideOther, UICollider.None){
		uiPath =MJPath.MJDanJuJieSuanPath;
	}

	public override void Awake(GameObject go)
	{
        MJUIManager._instance.mJDanJIeSuan = this;
        m_paiList.Clear();
        m_playerList.Clear();
        bg = transform.FindChild("UIDanJIeSuan").gameObject;
		setBtnClickListener();
		findView();
	}

    //找到view
	private void findView(){
		//find players
		for (int i = 0; i < PlayerNubmer; i++) {
			string name = "Player" + i;
			m_playerList.Add(transform.Find("UIDanJIeSuan/Players/" + name));

		}
		//find pais
		for (int i = 0; i < 14; i++) {
			string name = "pai" + i;
			m_paiList.Add(transform.Find("UIDanJIeSuan/Pais/" + name));
		}
		initPai();
		initPlayer();
	}

	private List<Transform> m_playerList = new List<Transform>();
	private List<Transform> m_paiList=new List<Transform>();


	public void initPai(){
		foreach (Transform pai in m_paiList) {
			pai.gameObject.SetActive(false);
		}
	}

	public void initPlayer(){
		foreach(Transform player in m_playerList){
			player.gameObject.SetActive(false);
            player.FindChild("BaWang").gameObject.SetActive(false);
            player.FindChild("ChongGuan").gameObject.SetActive(false);
            player.FindChild("Jingdiao").gameObject.SetActive(false);
            player.FindChild("Tips/Pao").gameObject.SetActive(false);
            player.FindChild("Tips/Hu").gameObject.SetActive(false);
            player.FindChild("HeadImage/Tip").gameObject.SetActive(false);
		}
	}

    /// <summary>
    /// 添加一条玩家信息
    /// </summary>
    /// <param name="index"></param>
    /// <param name="hupaiResponseVo"></param>
    public void setPlayerInfo(int uuid,Sprite headSprite,int benJuScore,HupaiResponseItem hupaiResponseItem)
    {
        int index=0;
        //如果是自己，则放第一位

        if (MJPlayerManager._instance!=null&&uuid==MJPlayerManager._instance.getMyUUID())
        {
            index = 0;
        }
        else
        {
            index = curPlayerInfoIndex++;
        }

        #region 填充胡牌数据
        if (hupaiResponseItem.totalInfo != null)
        {
            if (hupaiResponseItem.totalInfo.hu != null && hupaiResponseItem.totalInfo.hu.Contains(":"))
            {
                string[] huInfo = hupaiResponseItem.totalInfo.hu.Split(':');

                //胡牌类型
                if (huInfo.Length >2)
                {
                    if (huInfo[2].Equals("zi_common") || huInfo[2].Equals("d_self"))
                    {
                        setHuTipWithIndex(index, true);
                        setHuCardList(toList(hupaiResponseItem.paiArray));
                        setHuCard(huInfo[1]);
                    }
                    else if (huInfo[2].Equals("d_other"))
                    {
                        setPaoTipWithIndex(index, true);
                    }
                }
            }
        }
        #endregion

        setHeadImageWithIndex(index, headSprite);
        setNickeName(index, hupaiResponseItem.nickname);
        setShangjingwithIndex(index,""+ hupaiResponseItem.jingScore+"");//上精
        setXiaJingwithIndex(index, ""+0);//下精
        setGangFenwithIndex(index, hupaiResponseItem.gangScore+"");//杠分
        setHuPaiwithIndex(index, hupaiResponseItem.huScore+"");//胡牌分数
        setBenJuwithIndex(index,benJuScore+"");//本局分数
        setZongFenwithIndex(index, hupaiResponseItem.totalScore+"");//总分

        MJPlayerManager._instance.setPlayerSocreByIndex(MJPlayerManager._instance.getPlayerByUUID(uuid), hupaiResponseItem.totalScore);
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="index"></param>
    /// <param name="nickName"></param>
    private void setNickeName(int index,string nickName)
    {
        m_playerList[index].Find("LabelNikeName").GetComponent<Text>().text = nickName;
    }

    /// <summary>
    /// 设置头像
    /// </summary>
    /// <param name="index"></param>
    /// <param name="headsprite"></param>
	private void setHeadImageWithIndex(int index,Sprite headsprite){
		m_playerList[index].Find("HeadImage").GetComponent<Image>().sprite=headsprite;
		m_playerList[index].gameObject.SetActive(true);
	}

    /// <summary>
    /// 上精分
    /// </summary>
    /// <param name="index"></param>
    /// <param name="shangjing"></param>
	private void setShangjingwithIndex(int index,string shangjing){
		m_playerList[index].Find("LabelShangejing").GetComponent<Text>().text = "上精:"+shangjing;
	}

    /// <summary>
    /// 下精分
    /// </summary>
    /// <param name="index"></param>
    /// <param name="xiajing"></param>
    private void setXiaJingwithIndex(int index, string xiajing)
	{
		m_playerList[index].Find("LabelXiaJing").GetComponent<Text>().text = "下精:" + xiajing;
	}

    /// <summary>
    /// 杠分
    /// </summary>
    /// <param name="index"></param>
    /// <param name="gangfen"></param>
    private void setGangFenwithIndex(int index, string gangfen)
	{
		m_playerList[index].Find("LabelGangFen").GetComponent<Text>().text = "杠分:" + gangfen;
	}

    /// <summary>
    /// 胡牌分数
    /// </summary>
    /// <param name="index"></param>
    /// <param name="hupai"></param>
    private void setHuPaiwithIndex(int index, string hupai)
	{
		m_playerList[index].Find("LabelHuPai").GetComponent<Text>().text = "胡牌:" + hupai;
	}

    /// <summary>
    /// 本局分数
    /// </summary>
    /// <param name="index"></param>
    /// <param name="benju"></param>
    private void setBenJuwithIndex(int index, string benju)
	{
		m_playerList[index].Find("LabelBenJu").GetComponent<Text>().text = "本局:" + benju;
	}

    /// <summary>
    /// 总分
    /// </summary>
    /// <param name="index"></param>
    /// <param name="zongfen"></param>
    private void setZongFenwithIndex(int index, string zongfen)
	{
		m_playerList[index].Find("LabelZongFen").GetComponent<Text>().text = "总分:" + zongfen;
	}

    /// <summary>
    /// 是否是霸王
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isBaWang"></param>
    private void setBaWangWithIndex(int index,bool isBaWang){
		m_playerList[index].Find("BaWang").gameObject.SetActive(isBaWang);
	}

    /// <summary>
    /// 是否冲关
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isChongGuan"></param>
    private void setChongGuanWithIndex(int index,bool isChongGuan){
		m_playerList[index].Find("ChongGuan").gameObject.SetActive(isChongGuan);
	}

    /// <summary>
    /// 是够是精吊
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isJingdiao"></param>
    private void setJingDiaoWithIndex(int index, bool isJingdiao)
	{
		m_playerList[index].Find("Jingdiao").gameObject.SetActive(isJingdiao);
	}

    /// <summary>
    /// 是否胡牌
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isHu"></param>
    private void setHuTipWithIndex(int index,bool isHu){
		m_playerList[index].Find("Tips/Hu").gameObject.SetActive(isHu);
	}

    /// <summary>
    /// 是否点炮
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isPao"></param>
    private void setPaoTipWithIndex(int index, bool isPao)
	{
		m_playerList[index].Find("Tips/Pao").gameObject.SetActive(isPao);
	}

    /// <summary>
    /// 设置胡牌的信息
    /// </summary>
    /// <param name="cardsName"></param>
    private void setHuCardList(List<string> cardsName)
    {
        if (cardsName==null||cardsName.Count>14)
        {
            return;
        }

        for (int i=0;i<13;i++)
        {
            m_paiList[i].FindChild("Image").GetComponent<Image>().sprite = MJCardAction.Instance.getSpriteByName(cardsName[i]);
            if (MJCardsManager._instance.jingPai!=null)
            {
                if (cardsName[i].Equals(MJCardsManager._instance.jingPai.zhengJingIndex + "") || cardsName[i].Equals(MJCardsManager._instance.jingPai.fuJingPai + ""))
                {
                    m_paiList[i].FindChild("Jing").gameObject.SetActive(true);
                }
                else
                {
                    m_paiList[i].FindChild("Jing").gameObject.SetActive(false);
                }
            } 
            m_paiList[i].gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 设置胡的那一张牌
    /// </summary>
    /// <param name="point"></param>
    private void setHuCard(string point)
    {
        m_paiList[13].FindChild("Image").GetComponent<Image>().sprite = MJCardAction.Instance.getSpriteByName(point);
        if (MJCardsManager._instance.jingPai != null)
        {
            if (point.Equals(MJCardsManager._instance.jingPai.zhengJingIndex + "") || point.Equals(MJCardsManager._instance.jingPai.fuJingPai + ""))
            {
                m_paiList[13].FindChild("Jing").gameObject.SetActive(true);
            }
            else
            {
                m_paiList[13].FindChild("Jing").gameObject.SetActive(false);
            }
        }
        m_paiList[13].gameObject.SetActive(true);
    }

    /// <summary>
    /// 数组转list
    /// </summary>
    /// <param name="pais"></param>
    /// <returns></returns>
    private List<string> toList(int[] pais)
    {
        List<string> paiList = new List<string>();
        for (int i=0;i<pais.Length;i++)
        {
            if (pais[i]!=0)
            {
                for (int j=0;j<pais[i];j++)
                {
                    paiList.Add(i + "");
                }
            }
        }
        return paiList;
    }
    /// <summary>
    /// 设置监听
    /// </summary>
	private void setBtnClickListener()
	{
		transform.Find("UIDanJIeSuan/BtnBack").GetComponent<Button>().onClick.AddListener(() => {
            if (GlobalDataScript.isStartGame)
            {
                checkBtn.SetActive(true);
                bg.SetActive(false);
            }
            else//如果游戏结束退出游戏
            {
                ShowPage<MJZongJieSuan>();
                ClosePage<MJDanJIeSuan>();
            }
        });
		transform.Find("UIDanJIeSuan/BtnChaKan").GetComponent<Button>().onClick.AddListener(() => {
            checkBtn.SetActive(true);
            bg.SetActive(false);
        });
        continueBtn = transform.Find("UIDanJIeSuan/BtnJiXu").GetComponent<Button>();
        continueBtn.onClick.AddListener(() =>
        {
            GameObject.Find("init").GetComponent<MyMahjongScript>().reSet();
            GameObject.Find("init").GetComponent<MyMahjongScript>().readyGame();
            ClosePage<MJDanJIeSuan>();
        });

        checkBtn = transform.FindChild("Check").gameObject;
        checkBtn.GetComponent<Button>().onClick.AddListener(() => {
            checkBtn.SetActive(false);
            bg.SetActive(true);
            Debug.Log("执行事件");
        });

    }

    public override void Refresh()
    {
        //重置玩家信息
        initPlayer();
        initPai();

        bg.SetActive(true);
        checkBtn.SetActive(false);
        if (!GlobalDataScript.isStartGame)
        {
            continueBtn.interactable = false;
        }
        else
        {
            continueBtn.interactable = true;
        }
        curPlayerInfoIndex = 1;
    }

  
}
