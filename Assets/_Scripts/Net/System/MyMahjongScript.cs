using System;
using UnityEngine;
using System.Collections.Generic;
using AssemblyCSharp;
using LitJson;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

public class MyMahjongScript : MonoBehaviour
{
	public double lastTime;

	public int otherPengCard;
	public int otherGangCard;

    //存储吃的card
    public CardVO chiCard;

    //是否已经获取色子
    private bool isGetShaiZi = false;
    //是否已经获取精牌
    private bool isGetJing = false;

	private List<List<int>> mineList;//所有玩家的牌
	private int gangKind;
	
	/// <summary>
	/// 庄家的索引
	/// </summary>
	private int bankerId;
	private int curDirIndex;


	public int putOutCardPoint = -1;//打出的牌
	private string outDir;

	/// <summary>
	/// 当前的方向字符串
	/// </summary>
	private string curDirString = "B";


	private int showTimeNumber = 0;
	private int showNoticeNumber = 0;
	private bool timeFlag = false;


	private bool isFirstOpen = true;

	/**是否为抢胡 游戏结束时需置为false**/
	private bool isQiangHu = false;


	private string passType = "";

    public MJOutCardInfo outCardInfo= new MJOutCardInfo();//出牌对象

	void Start()
	{
		randShowTime ();
		timeFlag = true;
		addListener ();
        init();
	}

    private void init()
    {
        //初始化玩家信息
        if (GlobalDataScript.reEnterRoomData != null)
        {
            GlobalDataScript.loginResponseData.roomId = GlobalDataScript.reEnterRoomData.roomId;
            //玩家信息
            MJPlayerManager._instance.setPlayerInfo(GlobalDataScript.reEnterRoomData.playerList);//重新进入房间

            //牌信息
            reEnterRoom();
        }
        else
        {
            //加入房间
            if (GlobalDataScript.roomJoinResponseData != null)
            {
                //设置玩家信息
                MJPlayerManager._instance.setPlayerInfo(GlobalDataScript.roomJoinResponseData.playerList);
                //设置局数
                GlobalDataScript.totalTimes = GlobalDataScript.roomJoinResponseData.roundNumber;//记录总局数
                GlobalDataScript.surplusTimes = GlobalDataScript.roomJoinResponseData.roundNumber;//剩余局数
                MJUIManager._instance.mJDeskPage.setTurn(GlobalDataScript.surplusTimes, GlobalDataScript.totalTimes);
                
            }
            else//创建房间
            {
                //设置玩家信息
                List<AvatarVO> list = new List<AvatarVO>();
                list.Add(GlobalDataScript.loginResponseData);
                MJPlayerManager._instance.setPlayerInfo(list);
                //设置局数
                
                GlobalDataScript.totalTimes = GlobalDataScript.roomVo.roundNumber;//记录总局数
                GlobalDataScript.surplusTimes = GlobalDataScript.roomVo.roundNumber;//剩余局数

                MJUIManager._instance.mJDeskPage.setTurn(GlobalDataScript.surplusTimes, GlobalDataScript.totalTimes);
            }
        }
        GlobalDataScript.reEnterRoomData = null;
        readyGame();//准备游戏
    }

	private void randShowTime(){
		showTimeNumber = (int)(UnityEngine.Random.Range(5000,10000));
	}


	public void addListener(){
		SocketEventHandle.getInstance().StartGameNotice += startGame;
		SocketEventHandle.getInstance().pickCardCallBack += pickCard;
		SocketEventHandle.getInstance().otherPickCardCallBack += otherPickCard;
		SocketEventHandle.getInstance().putOutCardCallBack += otherPutOutCard;
		SocketEventHandle.getInstance().otherUserJointRoomCallBack += otherUserJointRoom;
		SocketEventHandle.getInstance().PengCardCallBack += otherPeng;
		SocketEventHandle.getInstance().GangCardCallBack += gangResponse;
		SocketEventHandle.getInstance().gangCardNotice += otherGang;
		SocketEventHandle.getInstance ().btnActionShow += actionBtnShow;
		SocketEventHandle.getInstance ().HupaiCallBack += hupaiCallBack;
		SocketEventHandle.getInstance ().FinalGameOverCallBack += finalGameOverCallBack;
		SocketEventHandle.getInstance ().outRoomCallback += outRoomCallbak;
		SocketEventHandle.getInstance ().dissoliveRoomResponse += dissoliveRoomResponse;
		SocketEventHandle.getInstance ().gameReadyNotice += gameReadyNotice;
		SocketEventHandle.getInstance ().offlineNotice += offlineNotice;
		SocketEventHandle.getInstance ().messageBoxNotice += messageBoxNotice;
		SocketEventHandle.getInstance ().returnGameResponse += returnGameResponse;
		SocketEventHandle.getInstance().onlineNotice += onlineNotice;
		//CommonEvent.getInstance ().readyGame += markselfReadyGame;
		CommonEvent.getInstance ().closeGamePanel += exitOrDissoliveRoom;
		SocketEventHandle.getInstance ().micInputNotice += micInputNotice;
		SocketEventHandle.getInstance ().gameFollowBanderNotice += gameFollowBanderNotice;
        SocketEventHandle.getInstance().chiCardCallBack += ChiCardResopnse;
        SocketEventHandle.getInstance().jingCardCallBack += jingCardResponse;
    }

	private void removeListener(){
		SocketEventHandle.getInstance().StartGameNotice -= startGame;
		SocketEventHandle.getInstance().pickCardCallBack -= pickCard;
		SocketEventHandle.getInstance().otherPickCardCallBack -= otherPickCard;
		SocketEventHandle.getInstance().putOutCardCallBack -= otherPutOutCard;
		SocketEventHandle.getInstance().otherUserJointRoomCallBack -= otherUserJointRoom;
		SocketEventHandle.getInstance().PengCardCallBack -= otherPeng;
		SocketEventHandle.getInstance().GangCardCallBack -= gangResponse;
		SocketEventHandle.getInstance().gangCardNotice -= otherGang;
		SocketEventHandle.getInstance ().btnActionShow -= actionBtnShow;
		SocketEventHandle.getInstance ().HupaiCallBack -= hupaiCallBack;
		//SocketEventHandle.getInstance ().FinalGameOverCallBack -= finalGameOverCallBack;
		SocketEventHandle.getInstance ().outRoomCallback -= outRoomCallbak;
		SocketEventHandle.getInstance ().dissoliveRoomResponse -= dissoliveRoomResponse;
		SocketEventHandle.getInstance ().gameReadyNotice -= gameReadyNotice;
		SocketEventHandle.getInstance ().offlineNotice -= offlineNotice;
		SocketEventHandle.getInstance().onlineNotice -= onlineNotice;
		SocketEventHandle.getInstance ().messageBoxNotice -= messageBoxNotice;
		SocketEventHandle.getInstance ().returnGameResponse -= returnGameResponse;
		//CommonEvent.getInstance ().readyGame -= markselfReadyGame;
		CommonEvent.getInstance ().closeGamePanel -= exitOrDissoliveRoom;
		SocketEventHandle.getInstance ().micInputNotice -= micInputNotice;
		SocketEventHandle.getInstance ().gameFollowBanderNotice -= gameFollowBanderNotice;
        SocketEventHandle.getInstance().chiCardCallBack -= ChiCardResopnse;
        SocketEventHandle.getInstance().jingCardCallBack -= jingCardResponse;
    }

    /**
    *准备游戏
	*/
    public void readyGame()
    {
        isGetJing = false;
        isGetShaiZi = false;
        CustomSocket.getInstance().sendMsg(new GameReadyRequest());
    }

    /// <summary>
    /// 准备回调
    /// </summary>
    /// <param name="response"></param>
    public void gameReadyNotice(ClientResponse response)
    {
        //===============================================
        JsonData json = JsonMapper.ToObject(response.message);
        int avatarIndex = Int32.Parse(json["avatarIndex"].ToString());
        MJPlayerManager._instance.setReady(avatarIndex, true);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    /// <param name="response">Response.</param>
    public void startGame(ClientResponse response)
	{
        Debug.Log("开始游戏");
        
        MJUIManager._instance.mJDeskPage.setInviteBtn(false);//隐藏邀请按钮
        MJPlayerManager._instance.closeAllREadyUI();//关闭准备ui
        GlobalDataScript.isStartGame = true;//开始游戏
        GlobalDataScript.roomAvatarVoList = MJPlayerManager._instance.getPlayerList();
		StartGameVO sgvo = JsonMapper.ToObject<StartGameVO>(response.message);
		bankerId = sgvo.bankerId;//获取庄家索引
        curDirString = getDirection (bankerId);//获取庄家的位置
        GlobalDataScript.surplusTimes--;//局数减一
        MJUIManager._instance.mJDeskPage.setTurn(GlobalDataScript.surplusTimes, GlobalDataScript.totalTimes);//设置局数
        //是否第一次开启
        if (!isFirstOpen) {
			MJPlayerManager._instance.getPlayerByIndex(bankerId).getPlayer().main = true;
		}

		GlobalDataScript.mainUuid = MJPlayerManager._instance.getPlayerByIndex(bankerId).getPlayer().account.uuid;//设置庄家的uuid
		curDirString = getDirection (bankerId);
		isFirstOpen = false;
		mineList = sgvo.paiArray;
        
		if (curDirString == DirectionEnum.Bottom) {
			GlobalDataScript.isDrag = true;
            
		} else {
			GlobalDataScript.isDrag = false;
		}

        MJDicePlace._instance.setPointer(getDirection(bankerId), 16);//设置方向
        MJPriticleManager.Instance.playAnim(AnimType.Duiju, DirectionEnum.Bottom);
        Invoke("dealyStartGame", 2);//延时开始游戏
    }

    /// <summary>
    /// 延时创建手牌
    /// </summary>
    private void dealyStartGame()
    {
        initMyCardListAndOtherCard();//创建手牌
    }
    
    /// <summary>
    /// 筛子牌回调
    /// </summary>
    /// <param name="response"></param>
    private void shaiZiResponse(ClientResponse response)
    {
        ShaiZiResponse shaiZi = JsonMapper.ToObject<ShaiZiResponse>(response.message);
        MJDicePlace._instance.setShaiZi(shaiZi.ponitOne, shaiZi.ponitTwo);
        MJCardsPile._instance.setShaiZi(shaiZi.ponitOne, shaiZi.ponitTwo,bankerId);
        isGetShaiZi = true;
    }

    /// <summary>
    /// 精牌回调
    /// </summary>
    /// <param name="response"></param>
    private void jingCardResponse(ClientResponse response)
    {
        JingResponse jingResponse = JsonMapper.ToObject<JingResponse>(response.message);
        MJCardsManager._instance.jingPai = jingResponse;//赋值精牌
        Debug.Log("设置精牌");
        MJCardsPile._instance.createCardPile();//创建牌堆
        MJUIManager._instance.mJDeskPage.setJing(jingResponse.zhengJingPai, jingResponse.fuJingPai);
        //牌堆设置
        MJCardsPile._instance.setBanker(bankerId);
        MJCardsPile._instance.setJing(jingResponse.zhengJingPai, jingResponse.zhengJingIndex);
        isGetJing = true;
    }

    /// <summary>
    /// 出牌请求
    /// </summary>
    /// <param name="cardpoint"></param>
    public void outCardRequest(int cardpoint)
    {
        CardVO cardvo = new CardVO();
        cardvo.cardPoint = cardpoint;
        CustomSocket.getInstance().sendMsg(new PutOutCardRequest(cardvo));
        GlobalDataScript.isDrag = false;
    }

	/// <summary>
	/// 别人摸牌通知
	/// </summary>
	/// <param name="response">Response.</param>
	public void otherPickCard(ClientResponse response)
	{
		JsonData json = JsonMapper.ToObject(response.message);
		//下一个摸牌人的索引
		int avatarIndex = (int) json["avatarIndex"];
        curDirString = getDirection(avatarIndex);
        MJCardsManager._instance.addCard(curDirString, "1");
        MJCardsPile._instance.getCard();
        MJDicePlace._instance.setPointer(curDirString, 16);
    }


	/// <summary>
	/// 自己摸牌
	/// </summary>
	/// <param name="response">Response.</param>
	public void pickCard(ClientResponse response)
	{
		CardVO cardvo = JsonMapper.ToObject<CardVO>(response.message);
        curDirString = DirectionEnum.Bottom;
        MJCardsManager._instance.addCard(DirectionEnum.Bottom, cardvo.cardPoint + "");
		GlobalDataScript.isDrag = true;
        MJCardsPile._instance.getCard();//获取一张牌
        MJDicePlace._instance.setPointer(DirectionEnum.Bottom, 16);
    }

    

	/// <summary>
	/// 胡，杠，碰，吃，pass按钮显示.
	/// </summary>
	/// <param name="response">Response.</param>
	public void actionBtnShow(ClientResponse response){
		GlobalDataScript.isDrag = false;
		string[] strs=response.message.Split (new char[1]{','});
		if (curDirString == DirectionEnum.Bottom) {
			passType = "selfPickCard";
		} else {
			passType = "otherPickCard";
        }

		for (int i = 0; i < strs.Length; i++) {
			if (strs [i].Equals ("hu")) {

                MJPGHCAction._instance.showHu();
			}else if(strs[i].Contains("qianghu")){
				
			
			}else if(strs[i].Contains("peng")){

                MJPGHCAction._instance.showPeng();
            }
            else if(strs[i].Contains("chi")){

                MJPGHCAction._instance.showChi();
                string[] temps = strs[i].Split(':');
                //记录出牌点数
                chiCard = new CardVO();
                chiCard.cardPoint = putOutCardPoint;
                if (temps.Length>3)
                {
                    int count = temps.Length;
                    List<string> minNames = new List<string>();
                    for (int j=1;j<count-1;j++)
                    {
                        string[] temps01 = temps[j].Split('-');
                        minNames.Add("" + findMin(putOutCardPoint,int.Parse(temps01[0]),int.Parse(temps01[1])));
                    }
                    
                    MJPGHCAction._instance.showMulChi(minNames);
                }
                else
                {
                    //只有一种情况直接记录
                    string[] temps02 = temps[1].Split('-');
                    chiCard.twoPoint =int.Parse(temps02[1]);
                    chiCard.onePoint = int.Parse(temps02[0]);
                }
               
            }else if(strs[i].Contains("gang")){
                MJPGHCAction._instance.showGang();
			}
		}
	}

    //找到最小数
    private int findMin(int one,int two,int three)
    {
        if (one<two&&one<three)
        {
            return one;
        }else if (two<one&&two<three)
        {
            return two;
        }
        else
        {
            return three;
        }
    }

    /// <summary>
    /// 插入我的13张牌
    /// </summary>
    /// <param name="topCount"></param>
    /// <param name="leftCount"></param>
    /// <param name="rightCount"></param>
	private void initMyCardListAndOtherCard(){
        //插入我的13张牌
        List<string> myCards = new List<string>();
        for(int i = 0; i < mineList[0].Count; i++)
        {
            if (mineList[0][i] > 0)
            {
                for (int j = 0; j < mineList[0][i]; j++)
                {
                    myCards.Add("" + i);
                }
            }
        }
        MJCardsManager._instance.startGame(myCards);//创建牌

        if (myCards.Count==14)
        {
            MJCardsManager._instance.addCard(DirectionEnum.Bottom, myCards[13]);
        }
        
        MJCardsPile._instance.getCardMul(53);//清除53张牌
	}

    /// <summary>
    /// 我的吃牌请求
    /// </summary>
    public void myChiCardBtnClick()
    {
        if (chiCard==null)
        {
            return;
        }
        CustomSocket.getInstance().sendMsg(new ChiRequest(chiCard));
    }

    /// <summary>
    /// 吃牌回调
    /// </summary>
    /// <param name="response"></param>
    private void ChiCardResopnse(ClientResponse response)
    {
        JsonData json = JsonMapper.ToObject(response.message);
        int curAvatarIndex =(int) json["avatarId"];
        curDirString = getDirection(curAvatarIndex);
        int sex = MJPlayerManager._instance.getSexByIndex(curAvatarIndex);
        int one = (int)json["onePoint"];
        int two = (int)json["twoPoint"];
        int chi = (int)json["cardPoint"];

        MJPriticleManager.Instance.playAnim(AnimType.Chi, curDirString);
        AudioController.Instance.playSoundByName("chi", sex);//播放音乐
        MJCardsManager._instance.chiCard(curDirString,
            one, two, chi);
        MJDicePlace._instance.setPointer(curDirString, 16);
        if (curDirString != DirectionEnum.Bottom)
        {
            MJPGHCAction._instance.close();
        }
        else
        {
            GlobalDataScript.isDrag = true;
        }

        //摧毁出的牌
        if (outCardInfo!=null&&outCardInfo.cardPoint==chi)
        {
            MJCardsManager._instance.destroySigleOutCard(outCardInfo.pos);
            outCardInfo = null;
        }
    }

	/// <summary>
	/// 接收到其它人的出牌通知
	/// </summary>
	/// <param name="response">Response.</param>
	public void otherPutOutCard(ClientResponse response)
	{
		JsonData json = JsonMapper.ToObject(response.message);
		int cardPoint = (int) json["cardIndex"];
		int curAvatarIndex = (int) json["curAvatarIndex"];
        putOutCardPoint = cardPoint;//记录出的牌
        int sex = MJPlayerManager._instance.getSexByIndex(curAvatarIndex);
        string audioPath = "";
       
        audioPath += cardPoint;
        AudioController.Instance.playSoundByName(audioPath,sex);//播放音乐
        MJCardsManager._instance.outCard(MJPlayerManager._instance.getPlayerPos(curAvatarIndex), cardPoint + "");
	}

	/// <summary>
	/// 根据一个人在数组里的索引，得到这个人所在的方位，L-左，T-上,R-右，B-下（自己的方位永远都是在下方）
	/// </summary>
	/// <returns>The direction.</returns>
	/// <param name="avatarIndex">Avatar index.</param>
	private string getDirection(int avatarIndex)
	{
        return MJPlayerManager._instance.getPlayerPos(avatarIndex);
	}

    /// <summary>
    /// 其他人碰牌
    /// </summary>
    /// <param name="response"></param>
	public void otherPeng(ClientResponse response)
	{
		OtherPengGangBackVO cardVo = JsonMapper.ToObject<OtherPengGangBackVO>(response.message);
		curDirString = getDirection (cardVo.avatarId);
        int sex = MJPlayerManager._instance.getSexByIndex(cardVo.avatarId);
        MJCardsManager._instance.pengCard(curDirString, cardVo.cardPoint+"");
        MJPriticleManager.Instance.playAnim(AnimType.Peng, curDirString);//播放动画
        MJPGHCAction._instance.close();
        AudioController.Instance.playSoundByName("peng", sex);
        MJDicePlace._instance.setPointer(curDirString, 16);

        if (curDirString!=DirectionEnum.Bottom)
        {
            MJPGHCAction._instance.close();
        }
        else
        {
            GlobalDataScript.isDrag = true;
        }

        //摧毁出的牌
        if (outCardInfo != null && outCardInfo.cardPoint == cardVo.cardPoint)
        {
            MJCardsManager._instance.destroySigleOutCard(outCardInfo.pos);
            outCardInfo = null;
        }
    }

    /// <summary>
    /// 其他人杠牌
    /// </summary>
    /// <param name="response"></param>
	private void otherGang(ClientResponse response) 
	{
		GangNoticeVO gangNotice = JsonMapper.ToObject<GangNoticeVO>(response.message);
		int cardPoint = gangNotice.cardPoint;
		int type = gangNotice.type;
        int sex = MJPlayerManager._instance.getSexByIndex(gangNotice.avatarId);
		Vector3 tempvector3 = new Vector3(0, 0, 0);
		curDirString = getDirection(gangNotice.avatarId);

        if (type==0)
        {
            MJCardsManager._instance.gangCard(curDirString,cardPoint+"", false);
        }
        else
        {
            MJCardsManager._instance.gangCard(curDirString, cardPoint + "", true);
        }
       
        MJPGHCAction._instance.close();
        MJPriticleManager.Instance.playAnim(AnimType.Gang, curDirString);//播放动画
        AudioController.Instance.playSoundByName("gang", sex);
        MJDicePlace._instance.setPointer(curDirString, 16);

        if (curDirString != DirectionEnum.Bottom)
        {
            MJPGHCAction._instance.close();
        }

        //摧毁出的牌
        if (outCardInfo != null && outCardInfo.cardPoint == cardPoint)
        {
            MJCardsManager._instance.destroySigleOutCard(outCardInfo.pos);
            outCardInfo = null;
        }
    }

	/// <summary>
	/// 点击放弃请求
	/// </summary>
	public void myPassBtnClick()
	{
		if (passType == "selfPickCard") {
			GlobalDataScript.isDrag = true;
		}
		CustomSocket.getInstance().sendMsg(new GaveUpRequest());
	}

    /// <summary>
    /// 碰牌请求
    /// </summary>
	public void myPengBtnClick()
	{
		CardVO cardvo = new CardVO ();
		cardvo.cardPoint = putOutCardPoint;
        MJDicePlace._instance.setPointer(DirectionEnum.Bottom, 16);
		CustomSocket.getInstance().sendMsg(new PengCardRequest(cardvo));
	}
    
    /// <summary>
    /// 我的杠牌回调
    /// </summary>
    /// <param name="response"></param>
	public void gangResponse(ClientResponse response)
	{
		GangBackVO gangBackVo = JsonMapper.ToObject<GangBackVO>(response.message);
		gangKind = gangBackVo.type;
        curDirString = DirectionEnum.Bottom;
        Image addCard = MJCardsManager._instance.getBottonAddCard();
        int cardPoint = 0;
        int sex = MJPlayerManager._instance.getSexByIndex(MJPlayerManager._instance.getMyIndex());
        //获取杠牌
        if (addCard==null)
        {
            cardPoint = putOutCardPoint;
           
        }
        else
        {
            cardPoint = int.Parse(addCard.gameObject.name);
        }

        if (gangKind == 0)//明杠
        {

            MJCardsManager._instance.gangCard(DirectionEnum.Bottom, cardPoint + "", false);
        }
        else//暗杠
        {
            MJCardsManager._instance.gangCard(DirectionEnum.Bottom, cardPoint + "", true);
        }
        AudioController.Instance.playSoundByName("gang", sex);
        MJPriticleManager.Instance.playAnim(AnimType.Gang, DirectionEnum.Bottom);
        MJDicePlace._instance.setPointer(DirectionEnum.Bottom, 16);

        //摧毁出的牌
        if (outCardInfo != null && outCardInfo.cardPoint == cardPoint&&addCard==null)
        {
            MJCardsManager._instance.destroySigleOutCard(outCardInfo.pos);
            outCardInfo = null;
        }
    }

    /// <summary>
    /// 杠牌请求
    /// </summary>
	public void myGangBtnClick()
	{
		GlobalDataScript.isDrag = true;
        Image addcard = MJCardsManager._instance.getBottonAddCard();
        int cardPoint = 0;
        int type = 0;
		if (addcard==null)
		{
            cardPoint = putOutCardPoint;
            type = 0;
        }
        else
        {
            cardPoint = int.Parse(addcard.gameObject.name);
            type = 1;
        }
        CustomSocket.getInstance().sendMsg(new GangCardRequest(cardPoint, type));
        return;
	}


	public void setRoomRemark(){
		RoomCreateVo roomvo = GlobalDataScript.roomVo;
		GlobalDataScript.totalTimes = roomvo.roundNumber;
		GlobalDataScript.surplusTimes = roomvo.roundNumber;
	}

	public void otherUserJointRoom(ClientResponse response){
		AvatarVO avatar = JsonMapper.ToObject<AvatarVO> (response.message);
        MJPlayerManager._instance.setOtherPlayerInfo(avatar);
	}


    /// <summary>
    /// 胡牌请求
    /// </summary>
	public void hupaiRequest(){
        int cardPoint = MJCardsManager._instance.getBottonAddCard() == null ? putOutCardPoint : int.Parse(MJCardsManager._instance.getBottonAddCard().gameObject.name);//需修改成正确的胡牌cardpoint
        CardVO requestVo = new CardVO();
        requestVo.cardPoint = cardPoint;
        if (isQiangHu)
        {
            requestVo.type = "qianghu";
            isQiangHu = false;
        }
        string sendMsg = JsonMapper.ToJson(requestVo);
        CustomSocket.getInstance().sendMsg(new HupaiRequest(sendMsg));
    }

    /**
	 * 胡牌请求回调
	 */
    private void hupaiCallBack(ClientResponse response)
    {
        //删除这句，未区分胡家是谁
        GlobalDataScript.hupaiResponseVo = new HupaiResponseVo();
        GlobalDataScript.hupaiResponseVo = JsonMapper.ToObject<HupaiResponseVo>(response.message);

        string scores = GlobalDataScript.hupaiResponseVo.currentScore;
        if (GlobalDataScript.hupaiResponseVo.type == "0")
        {
            for (int i = 0; i < GlobalDataScript.hupaiResponseVo.avatarList.Count; i++)
            {
                if (checkAvarHupai(GlobalDataScript.hupaiResponseVo.avatarList[i]) == 1)
                {//胡
                    
                    MJPriticleManager.Instance.playAnim(AnimType.Hu, getDirection(i));//播放动画
                }
                else if (checkAvarHupai(GlobalDataScript.hupaiResponseVo.avatarList[i]) == 2)
                {//自摸

                    MJPriticleManager.Instance.playAnim(AnimType.Zimo, getDirection(i));//播放动画
                }
                else
                {//没胡
                    
                }
                MJCardsManager._instance.huCard(getDirection(i), toString(GlobalDataScript.hupaiResponseVo.avatarList[i].paiArray));//显示牌
            }
        }
        else if (GlobalDataScript.hupaiResponseVo.type == "1")
        {//留局
            for (int i = 0; i < GlobalDataScript.hupaiResponseVo.avatarList.Count; i++)
            {
                if (checkAvarHupai(GlobalDataScript.hupaiResponseVo.avatarList[i]) == 1)
                {//胡

                    MJPriticleManager.Instance.playAnim(AnimType.Hu, getDirection(i));//播放动画
                }
                else if (checkAvarHupai(GlobalDataScript.hupaiResponseVo.avatarList[i]) == 2)
                {//自摸

                    MJPriticleManager.Instance.playAnim(AnimType.Zimo, getDirection(i));//播放动画
                }
                else
                {//没胡

                }
                MJCardsManager._instance.huCard(getDirection(i), toString(GlobalDataScript.hupaiResponseVo.avatarList[i].paiArray));//显示牌
            }
        }
        else
        {

        }
        MJPGHCAction._instance.close();
        MJDicePlace._instance.reset();
        Invoke("danJuJieSuan", 1);//唤醒单局结算
    }

    /// <summary>
    /// 转字符
    /// </summary>
    /// <param name="names"></param>
    /// <returns></returns>
    private List<string> toString(int[] names)
    {
        if (names==null)
        {
            return null;
        }
        List<string> temps = new List<string>();
        for (int i=0; i<names.Length;i++)
        {
           if (names[i]!=0)
           {
                for (int j=0;j<names[i];j++)
                {
                    temps.Add(i + "");
                }
           }
        }
        return temps;
    }

    /// <summary>
    /// 检测某人是否胡牌 
    /// </summary>
    /// <param name="itemData"></param>
    /// <returns></returns>
    public int checkAvarHupai( HupaiResponseItem itemData){
		string hupaiStr = itemData.totalInfo.hu;
		HuipaiObj hupaiObj = new HuipaiObj ();
		if(hupaiStr!=null && hupaiStr.Length>0){
            hupaiObj.uuid = hupaiStr.Split(new char[1] { ':' })[0];
            hupaiObj.cardPiont = int.Parse(hupaiStr.Split(new char[1] { ':' })[1]);
            hupaiObj.type = hupaiStr.Split(new char[1] { ':' })[2];
            //增加判断是否是自己胡牌的判断

            if (hupaiStr.Contains("d_other"))
            {//排除一炮多响的情况
                return 0;
            }
            else if (hupaiObj.type == "zi_common")
            {
                return 2;

            }
            else if (hupaiObj.type == "d_self")
            {
                return 1;
            }
            else if (hupaiObj.type == "qiyise")
            {
                return 1;
            }
            else if (hupaiObj.type == "zi_qingyise")
            {
                return 2;
            }
            else if (hupaiObj.type == "qixiaodui")
            {
                return 1;
            }
            else if (hupaiObj.type == "self_qixiaodui")
            {
                return 2;
            }
            else if (hupaiObj.type == "gangshangpao")
            {
                return 1;
            }
            else if (hupaiObj.type == "gangshanghua")
            {
                return 2;
            }


        }
		return 0;
	}

    /// <summary>
    /// 单局结算
    /// </summary>
    public void danJuJieSuan()
    {
        HupaiResponseVo hupaiResponseVo = GlobalDataScript.hupaiResponseVo;

        string[] benJuscores = hupaiResponseVo.currentScore.Split(',');
        //存下每一个人的分数
        Dictionary<int, int> playerBenJuScore = new Dictionary<int, int>();//uuid+胡牌分数
        for (int i=0;i<benJuscores.Length;i++)
        {
            if (benJuscores[i].Equals(""))
            {
                continue;
            }
            Debug.Log(benJuscores[i]);
            string[] temp = benJuscores[i].Split(':');
            playerBenJuScore.Add(int.Parse(temp[0]), int.Parse(temp[1]));
        }

        int score = 0;
        for (int i=0;i<hupaiResponseVo.avatarList.Count;i++)
        {
            if (playerBenJuScore.ContainsKey(MJPlayerManager._instance.getPlayerByIndex(i).getPlayer().account.uuid))
            {
                score = playerBenJuScore[MJPlayerManager._instance.getPlayerByIndex(i).getPlayer().account.uuid];
            }
            else
            {
                score = 0;
            }
            MJUIManager._instance.mJDanJIeSuan.setPlayerInfo(MJPlayerManager._instance.getPlayerByIndex(i).getPlayer().account.uuid, 
                MJPlayerManager._instance.getPlayerByIndex(i).getHeadSprite(),score, hupaiResponseVo.avatarList[i]);
        }

        MJUIManager._instance.mJQuitRoomPage.Hide();
        Debug.Log("单局结算");
    }


	//全局结束请求回调
	private void finalGameOverCallBack(ClientResponse response){
		GlobalDataScript.finalGameEndVo = JsonMapper.ToObject<FinalGameEndVo> (response.message);
        FinalGameEndVo finalGameEndVo = GlobalDataScript.finalGameEndVo;
        scoreSort(finalGameEndVo.totalInfo);
        int index = 0;
        for (int i=0;i<finalGameEndVo.totalInfo.Count;i++)
        {
            index = MJPlayerManager._instance.getPlayerByUUID(finalGameEndVo.totalInfo[i].uuid);
            MJUIManager._instance.mJZongJieSuan.setPlayerInfoWithOrder(i,
                MJPlayerManager._instance.getPlayerByIndex(index).getHeadSprite(), finalGameEndVo.totalInfo[i].getNickname(), 4+"",""+ 1,
                finalGameEndVo.totalInfo[i].scores+"");
        }
        MJUIManager._instance.mJZongJieSuan.Hide();
        Debug.Log("终局结算");
        finalGameOver();
    }

    /// <summary>
    /// 按分数排序
    /// </summary>
    private void scoreSort(List<FinalGameEndItemVo> finalIten)
    {
        FinalGameEndItemVo temp = new FinalGameEndItemVo();
        for (int i=0;i<finalIten.Count;i++)
        {
            for (int j=i+1;j<finalIten.Count;j++)
            {
                if (finalIten[i].scores>finalIten[j].scores)
                {
                    temp = finalIten[i];
                    finalIten[i] = finalIten[j];
                    finalIten[j] = temp;
                }
            }
        }
    }

	private void finalGameOver(){
		exitOrDissoliveRoom ();
	}
	

    //退出房间请求
    public void quiteRoom()
    {
        OutRoomRequestVo vo = new OutRoomRequestVo();
        vo.roomId = GlobalDataScript.roomVo.roomId;
        string sendMsg = JsonMapper.ToJson(vo);
        CustomSocket.getInstance().sendMsg(new OutRoomRequest(sendMsg));
    }

    public void outRoomCallbak(ClientResponse response){
		OutRoomResponseVo responseMsg = JsonMapper.ToObject<OutRoomResponseVo> (response.message);
		if (responseMsg.status_code.Equals("0") ) {
            //如果是自己，或者为地主则退出房间
           if (responseMsg.uuid==MJPlayerManager._instance.getMyUUID()||MJPlayerManager._instance.getPlayerByIndex(MJPlayerManager._instance.getPlayerByUUID(responseMsg.uuid)).getPlayer().main)
           {
                MJScenesManager.Instance.loadSceneNotAnim(SceneName.MainMenu);
                Debug.Log("退出房间成功");
                GlobalDataScript.isStartGame = false;
            }
            else
            {
                MJPlayerManager._instance.removePlayerByIndex(responseMsg.uuid);
            }
        } else {
            WantedTextTool.Instance.addTip("退出房间失败：" + responseMsg.error, 1);
		}
	}

    int refuseQuitRoomPlayerCount = 0;
    /// <summary>
    /// 申请解散房间回调
    /// </summary>
    /// <param name="response"></param>
    public void dissoliveRoomResponse( ClientResponse response){
		DissoliveRoomResponseVo dissoliveRoomResponseVo = JsonMapper.ToObject<DissoliveRoomResponseVo> (response.message);
		int uuid = dissoliveRoomResponseVo.uuid;
		if (dissoliveRoomResponseVo.type == "0") {//显示解散ui
            if (dissoliveRoomResponseVo.uuid!=MJPlayerManager._instance.getMyUUID())
            {
                MJUIManager._instance.mJQuitRoomPage.insertInfo(uuid);
                Debug.Log("显示解散ui");
            }
            
		} else if (dissoliveRoomResponseVo.type == "3") {//解散
            Debug.Log("解散房间");
            if (GlobalDataScript.isStartGame)
            {
                MJUIManager._instance.mJQuitRoomPage.close();
            }
            else
            {
                MJScenesManager.Instance.loadSceneNotAnim(SceneName.MainMenu);
               
            }
            GlobalDataScript.isStartGame = false;

        }
        else if (dissoliveRoomResponseVo.type == "1")//同意
		{
            MJUIManager._instance.mJQuitRoomPage.updateInfo(uuid, true);
            Debug.Log(getDirection(MJPlayerManager._instance.getPlayerByUUID(uuid)) +":同意");

		}else if (dissoliveRoomResponseVo.type == "2")//拒绝
		{
            MJUIManager._instance.mJQuitRoomPage.updateInfo(uuid, false);
            Debug.Log(getDirection(MJPlayerManager._instance.getPlayerByUUID(uuid)) + ":拒绝");
            refuseQuitRoomPlayerCount++;
            if (refuseQuitRoomPlayerCount>=2)
            {
                refuseQuitRoomPlayerCount = 0;
                MJUIManager._instance.mJQuitRoomPage.close();
                WantedTextTool.Instance.addTip("投票结束" + refuseQuitRoomPlayerCount + "/4，不能退出房间",1);
            }
        }
	}

    /// <summary>
    /// 申请或同意解散房间请求
    /// </summary>
    /// <param name="index">1.同意 2.拒绝</param>
	public void  doDissoliveRoomRequest(string index){
		DissoliveRoomRequestVo dissoliveRoomRequestVo = new DissoliveRoomRequestVo ();
		dissoliveRoomRequestVo.roomId = GlobalDataScript.loginResponseData.roomId;
		dissoliveRoomRequestVo.type = index;
		string sendMsg = JsonMapper.ToJson (dissoliveRoomRequestVo);
		CustomSocket.getInstance().sendMsg(new DissoliveRoomRequest(sendMsg));
		GlobalDataScript.isonApplayExitRoomstatus = true;
	}

    /// <summary>
    /// 退出房间重置
    /// </summary>
	public void exitOrDissoliveRoom(){
		GlobalDataScript.loginResponseData.resetData ();//复位房间数据
		GlobalDataScript.loginResponseData.roomId = 0;//复位房间数据
		GlobalDataScript.roomVo.roomId = 0;
		GlobalDataScript.soundToggle = true;
        GlobalDataScript.isStartGame = false;
		removeListener ();	
	}

    /// <summary>
    /// 跟庄回调
    /// </summary>
    /// <param name="response"></param>
	private void gameFollowBanderNotice(ClientResponse response){
		
	}

    /// <summary>
    /// 断线重连
    /// </summary>
    private void reEnterRoom(){
		
		if (GlobalDataScript.reEnterRoomData != null) {
			//显示房间基本信息
			GlobalDataScript.roomVo.addWordCard = GlobalDataScript.reEnterRoomData.addWordCard;
			GlobalDataScript.roomVo.hong = GlobalDataScript.reEnterRoomData.hong;
			GlobalDataScript.roomVo.name = GlobalDataScript.reEnterRoomData.name;
			GlobalDataScript.roomVo.roomId = GlobalDataScript.reEnterRoomData.roomId;
			GlobalDataScript.roomVo.roomType = GlobalDataScript.reEnterRoomData.roomType;
			GlobalDataScript.roomVo.roundNumber = GlobalDataScript.reEnterRoomData.roundNumber;
			GlobalDataScript.roomVo.sevenDouble = GlobalDataScript.reEnterRoomData.sevenDouble;
			GlobalDataScript.roomVo.xiaYu = GlobalDataScript.reEnterRoomData.xiaYu;
			GlobalDataScript.roomVo.ziMo = GlobalDataScript.reEnterRoomData.ziMo;
			GlobalDataScript.roomVo.magnification = GlobalDataScript.reEnterRoomData.magnification;
			GlobalDataScript.roomVo.ma = GlobalDataScript.reEnterRoomData.ma;

            //设置座位
            setRoomRemark();

            //设置局数
            GlobalDataScript.totalTimes = GlobalDataScript.reEnterRoomData.roundNumber;
            GlobalDataScript.surplusTimes = GlobalDataScript.totalTimes-GlobalDataScript.reEnterRoomData.currentRound;
            MJUIManager._instance.mJDeskPage.setTurn(GlobalDataScript.surplusTimes, GlobalDataScript.totalTimes);//设置局数

            List<AvatarVO> avatarList = GlobalDataScript.reEnterRoomData.playerList;

            //找到地主
            for (int i=0;i<avatarList.Count;i++)
            {
                if (avatarList[i].main)
                {
                    bankerId = i;
                    break;
                }
            }

            //已经开始游戏
            if (avatarList.Count==4)
            {
                MJUIManager._instance.mJDeskPage.setInviteBtn(false);//隐藏邀请按钮
                GlobalDataScript.isStartGame = true;
            }

			GlobalDataScript.roomAvatarVoList = GlobalDataScript.reEnterRoomData.playerList;

			int[][] selfPaiArray = GlobalDataScript.reEnterRoomData.playerList [MJPlayerManager._instance.getMyIndex ()].paiArray;
			if (selfPaiArray == null || selfPaiArray.Length == 0) {//游戏还没有开始
			  

			} else {//游戏开始
                GlobalDataScript.isStartGame = true;

                //关闭ui
                MJUIManager._instance.mJDeskPage.setInviteBtn(false);
                MJPlayerManager._instance.closeAllREadyUI();

                //设置色子信息
                if (GlobalDataScript.reEnterRoomData.pointOne!=-1)
                {
                    Debug.Log("重新登录设置色子");
                    MJDicePlace._instance.setShaiZi(GlobalDataScript.reEnterRoomData.pointOne, GlobalDataScript.reEnterRoomData.pointTwo);
                    MJCardsPile._instance.setShaiZi(GlobalDataScript.reEnterRoomData.pointOne, GlobalDataScript.reEnterRoomData.pointTwo, bankerId);
                }

                MJCardsPile._instance.createCardPile();//创建牌堆

                //设置精牌信息
                if (GlobalDataScript.reEnterRoomData.jingPaiVo!=null)
                {
                    Debug.Log("重新登录设置精牌");
                    MJCardsManager._instance.jingPai = GlobalDataScript.reEnterRoomData.jingPaiVo;//赋值精牌
                    MJUIManager._instance.mJDeskPage.setJing(GlobalDataScript.reEnterRoomData.jingPaiVo.zhengJingPai, GlobalDataScript.reEnterRoomData.jingPaiVo.fuJingPai);
                    //牌堆设置
                    MJCardsPile._instance.setBanker(bankerId);
                    MJCardsPile._instance.setJing(GlobalDataScript.reEnterRoomData.jingPaiVo.zhengJingPai, GlobalDataScript.reEnterRoomData.jingPaiVo.zhengJingIndex);
                }
                

                #region 显示出的牌
                for (int i = 0; i < GlobalDataScript.reEnterRoomData.playerList.Count; i++)
                {
                    int[] chupai = GlobalDataScript.reEnterRoomData.playerList[i].chupais;
                    outDir = getDirection(getIndexByUUID(GlobalDataScript.reEnterRoomData.playerList[i].account.uuid));
                    if (chupai != null && chupai.Length > 0)
                    {
                        for (int j = 0; j < chupai.Length; j++)
                        {
                            MJCardsManager._instance.showOutCard(outDir, chupai[j] + "");
                        }
                    }

                }
                #endregion

                #region 显示其他人手牌
                for (int i = 0; i < GlobalDataScript.reEnterRoomData.playerList.Count; i++)
                {
                    string dir = getDirection(getIndexByUUID(GlobalDataScript.reEnterRoomData.playerList[i].account.uuid));
                    int count = GlobalDataScript.reEnterRoomData.playerList[i].commonCards;
                    if (dir != DirectionEnum.Bottom)
                    {
                        MJCardsManager._instance.showOtherHandCard(dir);
                    }

                }
                #endregion

                #region 显示杠牌
                int gangCount = 0;//杠的次数
                for (int i = 0; i < GlobalDataScript.reEnterRoomData.playerList.Count; i++)
                {
                    int[] paiArrayType = GlobalDataScript.reEnterRoomData.playerList[i].paiArray[1];
                    string dirstr = getDirection(getIndexByUUID(GlobalDataScript.reEnterRoomData.playerList[i].account.uuid));
                    if (paiArrayType.Contains<int>(2))
                    {
                        string gangString = GlobalDataScript.reEnterRoomData.playerList[i].huReturnObjectVO.totalInfo.gang;
                        if (gangString != null)
                        {
                            string[] gangtemps = gangString.Split(new char[1] { ',' });
                            for (int j = 0; j < gangtemps.Length; j++)
                            {
                                string item = gangtemps[j];
                                GangpaiObj gangpaiObj = new GangpaiObj();
                                gangpaiObj.uuid = item.Split(new char[1] { ':' })[0];
                                gangpaiObj.cardPiont = int.Parse(item.Split(new char[1] { ':' })[1]);
                                gangpaiObj.type = item.Split(new char[1] { ':' })[2];
                                //增加判断是否为自己的杠牌的操作
                                GlobalDataScript.reEnterRoomData.playerList[i].paiArray[0][gangpaiObj.cardPiont] -= 4;


                                if (gangpaiObj.type == "an")
                                {
                                    MJCardsManager._instance.gangCard(dirstr, gangpaiObj.cardPiont+"",true);

                                }
                                else
                                {
                                    MJCardsManager._instance.gangCard(dirstr, gangpaiObj.cardPiont + "", false);
                                }

                                if (dirstr.Equals(DirectionEnum.Bottom))
                                {
                                    gangCount++;
                                }
                            }
                        }
                    }

                }
                #endregion

                #region 显示碰牌
                int pengCount =0;//碰的次数
                for (int i = 0; i < GlobalDataScript.reEnterRoomData.playerList.Count; i++)
                {
                    int[] paiArrayType = GlobalDataScript.reEnterRoomData.playerList[i].paiArray[1];
                    string dirstr = getDirection(getIndexByUUID(GlobalDataScript.reEnterRoomData.playerList[i].account.uuid));
                    if (paiArrayType.Contains<int>(1))
                    {
                        for (int j = 0; j < paiArrayType.Length; j++)
                        {
                            if (paiArrayType[j] == 1 && GlobalDataScript.reEnterRoomData.playerList[i].paiArray[0][j] > 0)
                            {
                                GlobalDataScript.reEnterRoomData.playerList[i].paiArray[0][j] -= 3;
                                MJCardsManager._instance.pengCard(dirstr, j+"");
                                if (dirstr.Equals(DirectionEnum.Bottom))
                                {
                                    pengCount++;
                                }

                            }
                           
                        }
                    }
                }
                MJCardsManager._instance.bottonMoveHandCard(gangCount, pengCount);//移动手牌
                #endregion

                #region 显示自己的手牌
                mineList = ToList(GlobalDataScript.reEnterRoomData.playerList[MJPlayerManager._instance.getMyIndex()].paiArray);
                List<string> name = new List<string>();
                for (int i = 0; i < mineList[0].Count; i++)
                {
                    if (mineList[0][i] > 0)
                    {
                        for (int j = 0; j < mineList[0][i]; j++)
                        {
                            name.Add(i + "");
                        }

                    }
                }

                if (name.Count==14)
                {
                    MJCardsManager._instance.addCard(DirectionEnum.Bottom, name[13]);
                }
                MJCardsManager._instance.createCards(name);//创建底部牌

                #endregion

                
                GlobalDataScript.loginResponseData.account = GlobalDataScript.reEnterRoomData.playerList[MJPlayerManager._instance.getMyIndex()].account;//记录我的信息
                CustomSocket.getInstance ().sendMsg (new CurrentStatusRequest ());
            }

        }

	}

    /// <summary>
    /// 二维数组转list
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
	private List<List<int>> ToList(int [][] param){
		List<List<int>> returnData = new List<List<int>> ();
		for(int i= 0;i<param.Length;i++){
			List<int> temp = new List<int> ();
			for (int j = 0; j < param [i].Length; j++) {
				temp.Add (param [i] [j]);
			}
			returnData.Add (temp);
		}
		return returnData;
	}

    /// <summary>
    /// 通过uuid获取index
    /// </summary>
    /// <param name="uuid"></param>
    private int getIndexByUUID(int uuid)
    {
        return MJPlayerManager._instance.getPlayerByUUID(uuid);
    }

    /// <summary>
    /// 用户离线回调
    /// </summary>
    /// <param name="response"></param>
    public void offlineNotice(ClientResponse response){
		int uuid =int.Parse( response.message);
		int index = getIndexByUUID(uuid);
		string dirstr = getDirection (index);
        Debug.Log("用户离线:" + index);
        MJPlayerManager._instance.setOnLineByIndex(index,false);
	}

    /// <summary>
    /// 用户上线提醒
    /// </summary>
    /// <param name="response"></param>
    public void onlineNotice(ClientResponse  response){
		int uuid =int.Parse( response.message);
		int index = getIndexByUUID (uuid);
		string dirstr = getDirection (index);
        Debug.Log("用户上限:"+index);
        MJPlayerManager._instance.setOnLineByIndex(index, true);
    }

    /// <summary>
    /// msg回调
    /// </summary>
    /// <param name="response"></param>
	public void messageBoxNotice(ClientResponse response){
		string[] arr = response.message.Split (new char[1]{ '|' });
		int uuid = int.Parse(arr[1]);
		int curAvaIndex = getIndexByUUID (uuid);
        int msg = int.Parse(arr[0]);
        MJPlayerManager._instance.showMsgByIndex(curAvaIndex, msg);
        AudioController.Instance.playSoundByName(msg+"", MJPlayerManager._instance.getSexByIndex(curDirIndex));
    }

	
    /// <summary>
    /// 语音回调
    /// </summary>
    /// <param name="response"></param>
	public void micInputNotice(ClientResponse response){
		int sendUUid = int.Parse(response.message);
		if (sendUUid > 0) {
            MJPlayerManager._instance.showTalkingUI(sendUUid);
        }
	}

    private bool isReturnGame = false;//是否回到游戏
    /// <summary>
    /// 返回游戏回调
    /// </summary>
    /// <param name="response"></param>
    public void returnGameResponse(ClientResponse response) {
        if (isReturnGame)
        {
            return;
        }
        else
        {
            isReturnGame = true;
        }

        string returnstr = response.message;
        //1.显示剩余牌的张数和圈数
        JsonData returnJsonData = JsonMapper.ToObject(response.message);
        string surplusCards = returnJsonData["surplusCards"].ToString();
        MJUIManager._instance.mJDeskPage.setleaveCards(int.Parse(surplusCards));//设置剩余牌
        int gameRound = int.Parse(returnJsonData["gameRound"].ToString());
       


        int curAvatarIndexTemp = -1;//当前出牌人的索引
        int pickAvatarIndexTemp = -1; //当前摸牌人的索引
        int putOffCardPointTemp = -1;//当前打得点数
        int currentCardPointTemp = -1;//当前摸的点数

        //不是自己摸牌
        try {

            curAvatarIndexTemp = int.Parse(returnJsonData["curAvatarIndex"].ToString());//当前打牌人的索引
            putOffCardPointTemp = int.Parse(returnJsonData["putOffCardPoint"].ToString());//当前打得点数
            putOutCardPoint = putOffCardPointTemp;//设置单前打的点数
           

            putOutCardPoint = putOffCardPointTemp;//碰
            pickAvatarIndexTemp = int.Parse(returnJsonData["pickAvatarIndex"].ToString()); //当前摸牌牌人的索引
            MJDicePlace._instance.setPointer(getDirection(pickAvatarIndexTemp), 16);//设置出牌方向

            /**这句代码有可能引发catch  所以后面的 SelfAndOtherPutoutCard = currentCardPointTemp; 可能不执行**/
            currentCardPointTemp = int.Parse(returnJsonData["currentCardPoint"].ToString());//当前摸得的点数  
            MJCardsManager._instance.addCard(getDirection(pickAvatarIndexTemp), currentCardPointTemp + "");//摸牌
            curDirString = getDirection(pickAvatarIndexTemp);


        } catch (Exception ex) {

        }


        if (pickAvatarIndexTemp == MJPlayerManager._instance.getMyIndex())
        {//自己摸牌
            if (currentCardPointTemp == -2)
            {

                GlobalDataScript.isDrag = true;
                curDirString = DirectionEnum.Bottom;
                Debug.Log("自己摸牌");

            }
            else
            {
                GlobalDataScript.isDrag = true;
                curDirString = DirectionEnum.Bottom;
                Debug.Log("自己摸牌");

            }
        }
        else
        { //别人摸牌
            curDirString = getDirection(pickAvatarIndexTemp);
            MJCardsManager._instance.addCard(curDirString, "7");
        }

        //设置剩余牌

        Debug.Log("余牌:" + surplusCards);
        
        MJCardsPile._instance.getCardMul(136 - int.Parse(surplusCards) - 1);
        MJUIManager._instance.mJDeskPage.setleaveCards(int.Parse(surplusCards));
	}

    /// <summary>
    /// 重置桌面
    /// </summary>
    public void reSet()
    {
        MJDicePlace._instance.reset();
        MJCardsPile._instance.reSet();
        MJCardsManager._instance.reSet();
        MJUIManager._instance.mJDeskPage.reset();
    }

}
public class DirectionEnum
{
    public const string Bottom = "B";
    public const string Right = "R";
    public const string Top = "T";
    public const string Left = "L";
}