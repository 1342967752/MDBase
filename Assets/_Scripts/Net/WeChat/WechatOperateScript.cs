using UnityEngine;
using System.Collections;
using cn.sharesdk.unity3d;
using System.IO;
using System;
using LitJson;
using AssemblyCSharp;

//微信操作 
public class WechatOperateScript : MonoBehaviour {
    public static WechatOperateScript _instance;

	public ShareSDK shareSdk;
	private string picPath;

    void Awake()
    {
        _instance = this;
    }

    public static WechatOperateScript Instance
    {
        get
        {
            if (_instance==null)
            {
                GameObject temp = new GameObject("WechatOperateScript");
                _instance = temp.AddComponent<WechatOperateScript>();
                DontDestroyOnLoad(temp);
            }
            return _instance;
        }
    }

	void Start () {
		if (shareSdk != null) {
			shareSdk.showUserHandler += getUserInforCallback;
			shareSdk.shareHandler += onShareCallBack;
			shareSdk.authHandler += AuthResultHandler;
		}

	}
	void AuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)  
	{     
		if (state == ResponseState.Success)  
		{
            WantedTextTool.Instance.addTip("authorize success !",0);
			//授权成功的话，获取用户信息  
			shareSdk.GetUserInfo(type);  

		}  
		else if (state == ResponseState.Fail)  
		{
            WantedTextTool.Instance.addTip("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"],0);
		}  
		else if (state == ResponseState.Cancel)  
		{
            WantedTextTool.Instance.addTip("cancel !",0);
		}  
	}  

	/**
	 * 登录，提供给button使用
	 * 
	 */ 
	public void login(){
		//TODO Login
		//shareSdk.Authorize(PlatformType.WeChat);
       // WantedTextTool.Instance.showTip("登录");
        shareSdk.GetUserInfo(PlatformType.WeChat);	
    }

    /// <summary>
    /// 登录测试
    /// </summary>
    /// <param name="openId"></param>
    /// <param name="unionid"></param>
    /// <param name="sex"></param>
    public void loginTest(string openId,string unionid,int sex)
    {
        LoginVo loginvo = new LoginVo();

        loginvo.openId = openId;
        loginvo.nickName = openId;
        loginvo.headIcon = null;
        loginvo.unionid = unionid;
        loginvo.province = "province";
        loginvo.city = "city";
        loginvo.sex = sex;
        string msg = JsonMapper.ToJson(loginvo);

        CustomSocket.getInstance().sendMsg(new LoginRequest(msg));
        GlobalDataScript.loginVo = loginvo;
        GlobalDataScript.loginResponseData = new AvatarVO();
        GlobalDataScript.loginResponseData.account = new Account();
        GlobalDataScript.loginResponseData.account.city = loginvo.city;
        GlobalDataScript.loginResponseData.account.openid = loginvo.openId;
        GlobalDataScript.loginResponseData.account.nickname = loginvo.nickName;
        GlobalDataScript.loginResponseData.account.headicon = loginvo.headIcon;
        GlobalDataScript.loginResponseData.account.unionid = loginvo.city;
        GlobalDataScript.loginResponseData.account.sex = loginvo.sex;
        GlobalDataScript.loginResponseData.IP = loginvo.IP;
    }

	/**
	 * 获取微信个人信息成功回调,登录
	 *
	 */ 
	public void getUserInforCallback(int reqID, ResponseState state, PlatformType type, Hashtable data){

		if (data != null) {
			try {
                LoginVo loginvo = new LoginVo();

                loginvo.openId = (string)data["openid"];
                loginvo.nickName = (string)data["nickname"];
                loginvo.headIcon = (string)data["headimgurl"];
                loginvo.unionid = (string)data["unionid"];
                loginvo.province = (string)data["province"];
                loginvo.city = (string)data["city"];
                loginvo.sex = int.Parse(data["sex"].ToString()); 
                string msg = JsonMapper.ToJson (loginvo);

                CustomSocket.getInstance ().sendMsg (new LoginRequest (msg));
                GlobalDataScript.loginVo = loginvo;
                GlobalDataScript.loginResponseData = new AvatarVO();
                GlobalDataScript.loginResponseData.account = new Account();
                GlobalDataScript.loginResponseData.account.city = loginvo.city;
                GlobalDataScript.loginResponseData.account.openid = loginvo.openId;
                GlobalDataScript.loginResponseData.account.nickname = loginvo.nickName;
                GlobalDataScript.loginResponseData.account.headicon = loginvo.headIcon;
                GlobalDataScript.loginResponseData.account.unionid = loginvo.city;
                GlobalDataScript.loginResponseData.account.sex = loginvo.sex;
                GlobalDataScript.loginResponseData.IP = loginvo.IP;

                WantedTextTool.Instance.addTip("获取个人信息成功" + loginvo.nickName,0);

			} catch (Exception e) {
                WantedTextTool.Instance.addTip("请先打开你的微信客户端",0);
				return;
			}
		} else {
            WantedTextTool.Instance.addTip("微信登录失败",0);
		}




	}


	/***
	 * 分享战绩成功回调
	 */ 
	public void onShareCallBack(int reqID,ResponseState state,PlatformType type,Hashtable result){
		if (state == ResponseState.Success) {
            WantedTextTool.Instance.addTip("分享成功",0);

		} else if(state == ResponseState.Fail){
            WantedTextTool.Instance.addTip("分享失败:" + result["error_msg"],0);
		}
	}

	/**
	 * 分享战绩、战绩
	 */ 
	public void shareAchievementToWeChat(PlatformType platformType){
        ShareContent customizeShareParams = new ShareContent();
        customizeShareParams.SetTitle("麻将");
        customizeShareParams.SetText ("1234");
        shareSdk.ShowShareContentEditor(PlatformType.WeChat, customizeShareParams);
        //StartCoroutine(GetCapture(platformType));
    }

	/**
	 * 执行分享到朋友圈的操作
	 */ 
	private void shareAchievement(PlatformType platformType){
		ShareContent customizeShareParams = new ShareContent();
		customizeShareParams.SetText("");
		customizeShareParams.SetImagePath(picPath);
		customizeShareParams.SetShareType(ContentType.Image);
		customizeShareParams.SetObjectID("");
		customizeShareParams.SetShareContentCustomize(platformType, customizeShareParams);
		shareSdk.ShareContent (platformType, customizeShareParams);
	}

    /// <summary>
    /// 截屏
    /// </summary>
    /// <param name="platformType"></param>
    /// <returns></returns>
    private IEnumerator GetCapture(PlatformType platformType)
	{
		yield return new WaitForEndOfFrame();
		if(Application.platform==RuntimePlatform.Android || Application.platform==RuntimePlatform.IPhonePlayer)  

			picPath=Application.persistentDataPath;  

		else if(Application.platform==RuntimePlatform.WindowsPlayer)  

			picPath=Application.dataPath;  

		else if(Application.platform==RuntimePlatform.WindowsEditor)

		{  
			picPath=Application.dataPath;  
			picPath= picPath.Replace("/Assets",null);  
		}   

		picPath = picPath + "/screencapture.png";

		Debug.Log ("picPath:" + picPath);

		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D(width,height,TextureFormat.RGB24,false);
		tex.ReadPixels(new Rect(0,0,width,height),0,0,true);
		tex.Apply ();
		byte[] imagebytes = tex.EncodeToPNG();//转化为png图
		tex.Compress(false);//对屏幕缓存进行压缩
        Debug.Log("imagebytes:" + imagebytes);
		if (File.Exists (picPath)) {
			File.Delete (picPath);
		}
		File.WriteAllBytes(picPath,imagebytes);//存储png图
		Destroy(tex);
		shareAchievement(platformType);
	}

    /// <summary>
    /// 邀请好友
    /// </summary>
    public void inviteFriend()
    {
        if (GlobalDataScript.roomVo != null)
        {
            RoomCreateVo roomvo = GlobalDataScript.roomVo;
            GlobalDataScript.totalTimes = roomvo.roundNumber;
            GlobalDataScript.surplusTimes = roomvo.roundNumber;
            string str = "房间号：" + roomvo.roomId;



            string title = "南昌麻将    \n" ;
            ShareContent customizeShareParams = new ShareContent();
            customizeShareParams.SetTitle(title);
            customizeShareParams.SetText(str);
            //customizeShareParams.SetImageUrl(APIS.ImgUrl+"icon96.png");
            //customizeShareParams.SetUrl ("http://"+APIS.mainHost+"/downLoad/index.html");
            //customizeShareParams.SetShareType(ContentType.Webpage);
            //customizeShareParams.SetObjectID("");
            shareSdk.ShowShareContentEditor(PlatformType.WeChat, customizeShareParams);
        }
    }

}
