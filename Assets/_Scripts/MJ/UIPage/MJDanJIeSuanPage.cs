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
    private Transform paiContent;//牌的容器
    private float cardSizeX = 58;//牌的宽度
    private int huPaiPlayerIndex = 0;//胡牌玩家的index
    private int notHuPlayerCount = 0;

	public MJDanJIeSuan() : base(UIType.Fixed, UIMode.HideOther, UICollider.None){
		uiPath =MyPath.MJDanJuJieSuanPath;
	}

	public override void Awake(GameObject go)
	{
        MJUIManager._instance.mJDanJIeSuan = this;
        m_playerList.Clear();
        bg = transform.FindChild("UIDanJIeSuan").gameObject;
		setBtnClickListener();
		findView();
	}

    public override void Refresh()
    {
        bg.SetActive(true);
        checkBtn.SetActive(false);
        if (!GlobalDataScript.isStartGame)
        {
            continueBtn.transform.FindChild("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(MyPath.MJJiSuanUIPath + "ZongJieSuanImg");
            continueBtn.onClick.RemoveAllListeners();
            continueBtn.onClick.AddListener(jieShuGame);
        }
        else
        {
            continueBtn.transform.FindChild("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(MyPath.MJJiSuanUIPath + "ContinueImg");
            continueBtn.onClick.RemoveAllListeners();
            continueBtn.onClick.AddListener(readyGame);
        }
        curPlayerInfoIndex = 1;
    }

    public override void Active()
    {
        //重置玩家信息
        initPlayer();
        notHuPlayerCount = 0;
        gameObject.SetActive(true);
        if (paiContent.childCount > 0)
        {
            MJCardAction.Instance.destroyAllChild(paiContent);
        }
    }

    /// <summary>
    /// 找到view
    /// </summary>
	private void findView(){
		//找到玩家
		for (int i = 0; i < PlayerNubmer; i++) {
			string name = "Player" + i;
			m_playerList.Add(transform.Find("UIDanJIeSuan/Players/" + name));

		}
		initPlayer();

        paiContent = transform.FindChild("UIDanJIeSuan/Pais");
	}

	private List<Transform> m_playerList = new List<Transform>();

	public void initPlayer(){
		foreach(Transform player in m_playerList){
			player.gameObject.SetActive(false);
            player.FindChild("ShangJing").gameObject.SetActive(false);
            player.FindChild("HuType").gameObject.SetActive(false);
            player.FindChild("HuMode").gameObject.SetActive(false);
            player.FindChild("HeadImage/Banker").gameObject.SetActive(false);
		}
	}

    /// <summary>
    /// 添加一条玩家信息
    /// </summary>
    /// <param name="index"></param>
    /// <param name="hupaiResponseVo"></param>
    public void setPlayerInfo(int uuid,Sprite headSprite,HuipaiObj huipaiObj,HupaiResponseItem hupaiResponseItem)
    {
        int index=0;
        //如果是自己，则放第一位
        if (MJPlayerManager._instance!=null&&uuid==MJPlayerManager._instance.getMyUUID())
        {
            index = 0;
            m_playerList[index].FindChild("BG").GetComponent<Image>().sprite = Resources.Load<Sprite>(MyPath.MJJiSuanUIPath + "my_Bg");
        }
        else
        {
            index = curPlayerInfoIndex++;
            m_playerList[index].FindChild("BG").GetComponent<Image>().sprite = Resources.Load<Sprite>(MyPath.MJJiSuanUIPath + "other_Bg");
        }
        
        //设置庄家图标
        if (MJPlayerManager._instance!=null&&MJPlayerManager._instance.getBankerUUID()==uuid)
        {
            setBanker(index);
        }

       
        #region 填充胡牌数据
        if (hupaiResponseItem.totalInfo != null)
        {
            //设置精的类型(霸王 冲关)
            if (hupaiResponseItem.totalInfo.jing!=null)
            {
                string[] jingInfo = hupaiResponseItem.totalInfo.jing.Split(':');//分数+冲关+霸王

                if (jingInfo[1].Equals("1")&& jingInfo[2].Equals("0"))
                {
                    setShangJingType(index, 0);
                }else if (jingInfo[1].Equals("0") && jingInfo[2].Equals("1"))
                {
                    setShangJingType(index, 1);
                }
                else if (jingInfo[1].Equals("1") && jingInfo[2].Equals("1"))
                {
                    setShangJingType(index, 2);
                }
              
            }

            //胡牌类型
            if (hupaiResponseItem.totalInfo.hu != null && hupaiResponseItem.totalInfo.hu.Contains(":"))
            {
                string[] huInfo = hupaiResponseItem.totalInfo.hu.Split(':');

                if (huInfo.Length >2)
                {
                    if (huInfo[2].Equals("zi_common")|| huInfo[2].Equals("d_self"))
                    {
                        hupaiResponseItem.paiArray[int.Parse(huInfo[1])]--;//将胡的那张牌去掉
                        setHuCardList(toList(hupaiResponseItem.paiArray));
                        setHuCard(huInfo[1], toList(hupaiResponseItem.paiArray).Count);
                        huPaiPlayerIndex = index;
                        Debug.Log("手牌总数(不算摸牌或者点炮的牌):" + toList(hupaiResponseItem.paiArray).Count);
                    }else if (huInfo[2].Equals("d_other"))
                    {
                        setHuPaiMode(index,5);
                    }
                }
            }else if (hupaiResponseItem.totalInfo.hu== null)//记录没有胡的个数
            {
                notHuPlayerCount++;
            }
        }
        #endregion

        setHeadImageWithIndex(index, headSprite);
        setNickeName(index, hupaiResponseItem.nickname);
        setShangjingwithIndex(index,""+ hupaiResponseItem.jingScore+"");//上精
        setXiaJingwithIndex(index, ""+0);//下精
        setGangFenwithIndex(index, hupaiResponseItem.gangScore+"");//杠分
        setHuPaiwithIndex(index, hupaiResponseItem.huScore+"");//胡牌分数
        setBenJuwithIndex(index, hupaiResponseItem.totalScore + "");//本局分数
        setZongFenwithIndex(index,huipaiObj.currentScore+"");//总分

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
    /// <param name="type">0.冲关 1.霸王 2.冲关霸王</param>
    public void setShangJingType(int index,int type){
        Image sprite = Resources.Load<Image>(MyPath.MJShangJingType + type);

        if (sprite==null)
        {
            Debug.Log("精类型为空");
            return;
        }
        m_playerList[index].Find("ShangJing/Image").GetComponent<Image>().sprite = sprite.sprite;
        m_playerList[index].Find("ShangJing/Image").GetComponent<Image>().rectTransform.sizeDelta = sprite.rectTransform.sizeDelta;
        m_playerList[index].Find("ShangJing").gameObject.SetActive(true);
    }


    /// <summary>
    /// 设置胡牌类型
    /// </summary>
    /// <param name="index"></param>
    /// <param name="type"></param>
    private void setHuTypeWithIndex(int index, int type)
	{
        Image sprite = Resources.Load<Image>(MyPath.MJHUTypePath + type);
        if (sprite==null)
        {
            return;
        }

        m_playerList[index].Find("HuType/Image").GetComponent<Image>().sprite = sprite.sprite;
        m_playerList[index].Find("HuType/Image").GetComponent<Image>().rectTransform.sizeDelta = sprite.rectTransform.sizeDelta;
        m_playerList[index].Find("HuType").gameObject.SetActive(true);
    }

    /// <summary>
    /// 是否自摸
    /// </summary>
    /// <param name="index"></param>
    /// <param name="type"></param>
    private void setHuModeWithIndex(int index,int type)
    {
        if (type==-1||notHuPlayerCount==4)//没胡
        {
            return;
        }

        Image sprite= Resources.Load<Image>(MyPath.MJHUModePath + type);
        if (sprite==null)
        {
            return;
        }
        m_playerList[index].Find("HuMode/Image").GetComponent<Image>().sprite = sprite.sprite;
        m_playerList[index].Find("HuMode/Image").GetComponent<Image>().rectTransform.sizeDelta = sprite.rectTransform.sizeDelta;
        m_playerList[index].Find("HuMode").gameObject.SetActive(true);

    }

    /// <summary>
    /// 设置胡牌的信息
    /// </summary>
    /// <param name="cardsName"></param>
    public void setHuCardList(List<string> cardsName)
    {
        if (cardsName==null)
        {
            return;
        }

        GameObject card= Resources.Load<GameObject>(MyPath.MJCardPath); 
        for (int i=0;i<cardsName.Count;i++)
        {
            GameObject temp = GameObject.Instantiate(card);
            temp.transform.SetParent(paiContent);
            temp.transform.FindChild("Image").GetComponent<Image>().sprite = MJCardAction.Instance.getSpriteByName(cardsName[i]);
            temp.transform.localScale = Vector3.one;
            

            if (MJCardsManager._instance.jingPai!=null)
            {
                if (cardsName[i].Equals(MJCardsManager._instance.jingPai.zhengJingPai + "") || cardsName[i].Equals(MJCardsManager._instance.jingPai.fuJingPai + ""))
                {
                    temp.transform.FindChild("Jing").gameObject.SetActive(true);
                }
                else
                {
                    temp.transform.FindChild("Jing").gameObject.SetActive(false);
                }
            } 
            temp.transform.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 设置胡的那一张牌
    /// </summary>
    /// <param name="point"></param>
    public void setHuCard(string point,int handCardCount)
    {
        GameObject card = Resources.Load<GameObject>(MyPath.MJCardPath);

        GameObject temp = GameObject.Instantiate(card);
        temp.transform.FindChild("Image").GetComponent<Image>().sprite = MJCardAction.Instance.getSpriteByName(point);
        temp.transform.SetParent(paiContent);
        temp.transform.localScale = Vector3.one;
        temp.AddComponent<LayoutElement>().ignoreLayout = true;
        temp.GetComponent<Image>().rectTransform.sizeDelta = paiContent.GetComponent<GridLayoutGroup>().cellSize;


        temp.transform.FindChild("Image").GetComponent<Image>().sprite = MJCardAction.Instance.getSpriteByName(point);
        if (MJCardsManager._instance.jingPai != null)
        {
            if (point.Equals(MJCardsManager._instance.jingPai.zhengJingPai + "") || point.Equals(MJCardsManager._instance.jingPai.fuJingPai + ""))
            {
                temp.transform.FindChild("Jing").gameObject.SetActive(true);
            }
            else
            {
                temp.transform.FindChild("Jing").gameObject.SetActive(false);
            }
        }
        temp.transform.gameObject.SetActive(true);
        temp.transform.localPosition = new Vector3(-(handCardCount+1) * cardSizeX / 2,0, 0);//设置胡牌位置
    }

    /// <summary>
    /// 设置胡牌方式
    /// </summary>
    /// <param name="mode">0.天胡 1.地胡 2.杠上开花 3.抢杠胡 4.自摸 5.放炮 6.胡</param>
    public void setHuPaiMode(int index,int mode)
    {
        switch (mode)
        {
            case -1: Debug.Log("没胡"); break;//没胡
            case 0://天胡地胡暂时显示在胡牌方式
                //setHuModeWithIndex(index, 0);
                setHuTypeWithIndex(huPaiPlayerIndex, 17);
                break;//天胡
            case 1:
                //setHuModeWithIndex(index, 1);
                setHuTypeWithIndex(huPaiPlayerIndex, 18);
                break;//地胡
            case 2: setHuModeWithIndex(index, 2); break;//杠上开花
            case 3: setHuModeWithIndex(index, 3); break;//抢杠胡
            case 4: setHuModeWithIndex(index, 4); break;//自摸
            case 5: setHuModeWithIndex(index, 5); break;//放炮
            case 6: setHuModeWithIndex(index, 6); break;//胡
        }
        Debug.Log("胡牌方式:"+mode);
    }

    /// <summary>
    /// 获取胡牌的人index
    /// </summary>
    /// <returns></returns>
    public int getHuPlayerIndex()
    {
        return huPaiPlayerIndex;
    }

    /// <summary>
    /// 设置花牌类型
    /// </summary>
    /// <param name="type">0.没胡 ,1.南昌平胡 2.碰碰胡 3.七小对 4.十三烂 5.七星十三烂  6.德国平胡 7.德国碰碰胡 8.德国七小对 9.德国十三烂 10.德国七星十三烂 11.精吊平胡 12.精吊德国平胡 13精吊七小对 14精吊德国七小对 15精吊碰碰胡 16精吊德国碰碰胡</param>
    public void setHuPaiType(int type)
    {
        switch (type)
        {
            case 0: Debug.Log("没有胡牌"); break;//没有胡牌
            case 1: setHuTypeWithIndex(huPaiPlayerIndex,1); break;//南昌平胡
            case 2: setHuTypeWithIndex(huPaiPlayerIndex, 2); break;//碰碰胡
            case 3: setHuTypeWithIndex(huPaiPlayerIndex, 3); break;//七小对
            case 4: setHuTypeWithIndex(huPaiPlayerIndex, 4); break;//十三烂
            case 5: setHuTypeWithIndex(huPaiPlayerIndex, 5); break;//七星十三烂
            case 6: setHuTypeWithIndex(huPaiPlayerIndex, 6); break;//德国平胡
            case 7: setHuTypeWithIndex(huPaiPlayerIndex, 7); break;//德国碰碰胡
            case 8: setHuTypeWithIndex(huPaiPlayerIndex, 8); break;//德国七小对
            case 9: setHuTypeWithIndex(huPaiPlayerIndex, 9); break;//德国十三烂
            case 10: setHuTypeWithIndex(huPaiPlayerIndex, 10); break;//德国七星十三烂
            case 11: setHuTypeWithIndex(huPaiPlayerIndex, 11); break;//精吊平胡
            case 12: setHuTypeWithIndex(huPaiPlayerIndex, 12); break;//精吊德国平胡
            case 13: setHuTypeWithIndex(huPaiPlayerIndex, 13); break;//精吊七小对
            case 14: setHuTypeWithIndex(huPaiPlayerIndex, 14); break;//精吊德国七小对
            case 15: setHuTypeWithIndex(huPaiPlayerIndex, 15); break;//精吊碰碰胡
            case 16: setHuTypeWithIndex(huPaiPlayerIndex, 16); break;//精吊德国碰碰胡
                //额外加入
            case 17:Debug.Log("天胡");setHuTypeWithIndex(huPaiPlayerIndex,17) ;break;//天胡
            case 18:Debug.Log("地胡");setHuTypeWithIndex(huPaiPlayerIndex,18);break;//地胡
        }
        Debug.Log("胡牌类型:" + type);

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

        checkBtn = transform.FindChild("Check").gameObject;
        checkBtn.GetComponent<Button>().onClick.AddListener(() => {
            checkBtn.SetActive(false);
            bg.SetActive(true);
            Debug.Log("执行事件");
        });

    }

    /// <summary>
    /// 设置庄家
    /// </summary>
    /// <param name="index"></param>
    private void setBanker(int index)
    {
        if (m_playerList==null||m_playerList.Count==0)
        {
            return;
        }

        for (int i=0;i<m_playerList.Count;i++)
        {
            if (i==index)
            {
                m_playerList[i].transform.FindChild("HeadImage/Banker").gameObject.SetActive(true);
            }
            else
            {
                m_playerList[i].transform.FindChild("HeadImage/Banker").gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 准备游戏
    /// </summary>
    private void readyGame()
    {
        GameObject.Find("init").GetComponent<MyMahjongScript>().reSet();
        GameObject.Find("init").GetComponent<MyMahjongScript>().readyGame();
        ClosePage<MJDanJIeSuan>();
    }

    /// <summary>
    /// 结束游戏,显示结束界面
    /// </summary>
    private void jieShuGame()
    {
        ShowPage<MJZongJieSuan>();
        ClosePage<MJDanJIeSuan>();
    }

}
