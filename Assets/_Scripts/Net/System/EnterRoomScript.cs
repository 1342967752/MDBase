using UnityEngine;
using AssemblyCSharp;
using LitJson;

public class EnterRoomScript : MonoBehaviour{

	 void Start () {
		SocketEventHandle.getInstance().JoinRoomCallBack += onJoinRoomCallBack;
	}

    private void removeListener()
    {
        SocketEventHandle.getInstance().JoinRoomCallBack -= onJoinRoomCallBack;
    }

    public void sureRoomNumber(){
        string roomNumber = MJUIManager._instance.enterRoomPage.getNumber();
		RoomJoinVo roomJoinVo = new  RoomJoinVo ();
		roomJoinVo.roomId =int.Parse(roomNumber);
		string sendMsg = JsonMapper.ToJson (roomJoinVo);
        Debug.Log("房间号:" + roomNumber);
		CustomSocket.getInstance().sendMsg(new JoinRoomRequest(sendMsg));
        Debug.Log("发送房间号成功");
	}

    /// <summary>
    /// 加入房间成功回调
    /// </summary>
    /// <param name="response"></param>
	public void onJoinRoomCallBack(ClientResponse response){
      
		if (response.status == 1) {
            Debug.Log("进入房间");
            GlobalDataScript.roomJoinResponseData = JsonMapper.ToObject<RoomJoinResponseVo> (response.message);
			GlobalDataScript.roomVo.addWordCard = GlobalDataScript.roomJoinResponseData.addWordCard;
			GlobalDataScript.roomVo.hong = GlobalDataScript.roomJoinResponseData.hong;
			GlobalDataScript.roomVo.ma = GlobalDataScript.roomJoinResponseData.ma;
			GlobalDataScript.roomVo.name = GlobalDataScript.roomJoinResponseData.name;
			GlobalDataScript.roomVo.roomId = GlobalDataScript.roomJoinResponseData.roomId;
			GlobalDataScript.roomVo.roomType = GlobalDataScript.roomJoinResponseData.roomType;
			GlobalDataScript.roomVo.roundNumber = GlobalDataScript.roomJoinResponseData.roundNumber;
			GlobalDataScript.roomVo.sevenDouble = GlobalDataScript.roomJoinResponseData.sevenDouble;
			GlobalDataScript.roomVo.xiaYu = GlobalDataScript.roomJoinResponseData.xiaYu;
			GlobalDataScript.roomVo.ziMo = GlobalDataScript.roomJoinResponseData.ziMo;
			GlobalDataScript.roomVo.magnification = GlobalDataScript.roomJoinResponseData.magnification;
			GlobalDataScript.surplusTimes = GlobalDataScript.roomJoinResponseData.roundNumber;
			GlobalDataScript.loginResponseData.roomId = GlobalDataScript.roomJoinResponseData.roomId;
            WantedTextTool.Instance.addTip("进入房间成功", 2);
            MJScenesManager.Instance.loadSceneNotAnim("MaJiang");
		} else {
            Debug.Log("进入房间"+response.status);
        }

	}
}
