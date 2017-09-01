using UnityEngine;
using AssemblyCSharp;
using System;
using LitJson;



public class CrateRoomSettingScript : MonoBehaviour {
	

	private int roomCardCount;//房卡数
	private GameObject gameSence;
	private RoomCreateVo sendVo;//创建房间的信息
	void Start () {
		SocketEventHandle.getInstance ().CreateRoomCallBack += onCreateRoomCallback;

	}
	
	/***
	 * 打开转转麻将设置面板
	 */ 
	public void openZhuanzhuanSeetingPanel(){

	
	
	}

	/***
	 * 打开长沙麻将设置面板
	 */ 
	public void openChangshaSeetingPanel(){
		
	}

	/***
	 * 打开划水麻将设置面板
	 */ 
	public void openHuashuiSeetingPanel(){
		
	}

	public void openDevloping(){
		
	}



    /// <summary>
    /// 创建南昌麻将房间
    /// </summary>
    /// <param name="ma">色子个数</param>
    /// <param name="roundNumber">局数</param>
    /// <param name="isZimo">是否自摸</param>
    /// <param name="hasHong">红中癞子</param>
    /// <param name="isSevenDoube">七小对</param>
    public void createNamChangRoom(int ma,int roundNumber,int isZimo,bool hasHong,bool isSevenDoube)
    {
	
		sendVo = new RoomCreateVo ();
		sendVo.ma = ma;
		sendVo.roundNumber = roundNumber;
		sendVo.ziMo = isZimo;
		sendVo.hong = hasHong;
		sendVo.sevenDouble = isSevenDoube;
		sendVo.roomType = GameConfig.GAME_TYPE_NANCHANG;
        sendVo.addWordCard = true;
		string sendmsgstr = JsonMapper.ToJson (sendVo);
		if (GlobalDataScript.loginResponseData !=null&& GlobalDataScript.loginResponseData.account.roomcard > 0) {
			CustomSocket.getInstance ().sendMsg (new CreateRoomRequest (sendmsgstr));
		} else {
            WantedTextTool.Instance.addTip("你的房卡数量不足，不能创建房间", 1);
		}


	}

	/**
	 * 创建长沙麻将房间
	 */
	public void createChangshaRoom(){
		int roundNumber = 4;//房卡数量
		bool isZimo=false;//自摸
		int maCount = 0;
		
		sendVo = new RoomCreateVo ();
		sendVo.magnification = maCount;
		sendVo.roundNumber = roundNumber;
		//sendVo.ziMo = isZimo?1:0;
		sendVo.roomType = GameConfig.GAME_TYPE_CHANGSHA;
		string sendmsgstr = JsonMapper.ToJson (sendVo);
		if (GlobalDataScript.loginResponseData.account.roomcard > 0) {
			CustomSocket.getInstance ().sendMsg (new CreateRoomRequest (sendmsgstr));
		} else {
            WantedTextTool.Instance.addTip("你的房卡数量不足，不能创建房间", 1);
        }

	}

	/**
	 * 创建划水麻将房间
	 */
	public void createHuashuiRoom(){
		int roundNumber = 4;//房卡数量
		bool isZimo=false;//自摸
		bool isFengpai =false;//七小对
		int maCount = 0;
		

		sendVo = new RoomCreateVo ();
		sendVo.xiaYu = maCount;
		sendVo.roundNumber = roundNumber;
		sendVo.ziMo = isZimo?1:0;
		sendVo.roomType = GameConfig.GAME_TYPE_HUASHUI;
		sendVo.addWordCard = isFengpai;
		sendVo.sevenDouble = true;
		string sendmsgstr = JsonMapper.ToJson (sendVo);
		if (GlobalDataScript.loginResponseData.account.roomcard > 0) {
			CustomSocket.getInstance ().sendMsg (new CreateRoomRequest (sendmsgstr));
		} else {
            WantedTextTool.Instance.addTip("你的房卡数量不足，不能创建房间", 1);
        }

	}


	public void onCreateRoomCallback(ClientResponse response){

		if (response.status == 1) {
			
			int roomid = Int32.Parse(response.message);
			sendVo.roomId = roomid;
			GlobalDataScript.roomVo = sendVo;
			GlobalDataScript.loginResponseData.roomId = roomid;
			GlobalDataScript.loginResponseData.main = true;
			GlobalDataScript.loginResponseData.isOnLine = true;
            GlobalDataScript.roomJoinResponseData = null;//设置加入房间信息为空
            
            MJScenesManager.Instance.loadSceneNotAnim("MaJiang");

		} else {
            WantedTextTool.Instance.addTip(response.message, 2);
		}
	}

}
