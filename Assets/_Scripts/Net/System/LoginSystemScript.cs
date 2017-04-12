using UnityEngine;
using AssemblyCSharp;
using LitJson;

public class LoginSystemScript : MonoBehaviour {
	

	void Start () {

		CustomSocket.hasStartTimer = false;
        SocketEventHandle.getInstance().LoginCallBack += LoginCallBack;//登录回调
        SocketEventHandle.getInstance().RoomBackResponse += RoomBackResponse;//重新进入房间回调
		GlobalDataScript.isonLoginPage = true;
        
    }
	
    /// <summary>
    /// 登录入口
    /// </summary>
	public void login(){
		
		if (!CustomSocket.getInstance ().isConnected) {
            CustomSocket.getInstance().Connect();
            ChatSocket.getInstance().Connect();
            return;
		}
        GlobalDataScript.reinitData ();//初始化界面数据
        doLogin();
	}

	public void doLogin(){
		WechatOperateScript.Instance.login ();
	}

    /// <summary>
    /// 登录成功回调
    /// </summary>
    /// <param name="response"></param>
    public void LoginCallBack(ClientResponse response)
    {
        if (response.status == 1)
        {
            Debug.Log("登陆成功");
            GlobalDataScript.loginResponseData = JsonMapper.ToObject<AvatarVO>(response.message);
            ChatSocket.getInstance().sendMsg(new LoginChatRequest(GlobalDataScript.loginResponseData.account.uuid));
            MJUIManager._instance.closeLoginUI();
            MJScenesManager.Instance.loadSceneNotAnim("MainMenu");
            WantedTextTool.Instance.addTip("登录成功",0);
            removeListener();
        }
        else
        {
            WantedTextTool.Instance.addTip("登录失败", 0);
        }
    }

    private void removeListener(){
		SocketEventHandle.getInstance ().LoginCallBack -= LoginCallBack;
		SocketEventHandle.getInstance ().RoomBackResponse -= RoomBackResponse;
	}

    /// <summary>
    /// 重新进入房间回调
    /// </summary>
    /// <param name="response"></param>
	private void RoomBackResponse(ClientResponse response){

		GlobalDataScript.reEnterRoomData = JsonMapper.ToObject<RoomJoinResponseVo> (response.message);
        for (int i = 0; i < GlobalDataScript.reEnterRoomData.playerList.Count; i++) {
			AvatarVO itemData =	GlobalDataScript.reEnterRoomData.playerList [i];
			if (itemData.account.openid == GlobalDataScript.loginResponseData.account.openid) {
				GlobalDataScript.loginResponseData.account.uuid = itemData.account.uuid;
                GlobalDataScript.loginResponseData.account.roomcard = itemData.account.roomcard;
                ChatSocket.getInstance ().sendMsg (new LoginChatRequest(GlobalDataScript.loginResponseData.account.uuid));
				break;
			}
		}
        
        MJScenesManager.Instance.loadSceneNotAnim("MaJiang");
        removeListener ();
	}
}
