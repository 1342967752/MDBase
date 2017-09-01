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

	public  ShareSDK shareSdk;
	private string picPath;

    void Awake()
    {
        if (_instance!=null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

	void Start () {
		if (shareSdk != null) {
			shareSdk.showUserHandler += getUserInforCallback;
			shareSdk.shareHandler += onShareCallBack;
			shareSdk.authHandler += AuthResultHandler;
		}
        DontDestroyOnLoad(gameObject);
	}

    /// <summary>
    /// 授权
    /// </summary>
    /// <param name="reqID"></param>
    /// <param name="state"></param>
    /// <param name="type"></param>
    /// <param name="result"></param>
	void AuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)  
	{     
		if (state == ResponseState.Success)  
		{
            Debug.Log("微信授权成功");
			//授权成功的话，获取用户信息  
			shareSdk.GetUserInfo(type);  

		}  
		else if (state == ResponseState.Fail)  
		{
            WantedTextTool.Instance.addTip("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"], 0);
            Debug.Log("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
		}  
		else if (state == ResponseState.Cancel)  
		{
            WantedTextTool.Instance.addTip("cancel !",0);
		}  
	}

    /// <summary>
    /// 登录，提供给button使用
    /// </summary>
    public void login(){
		//TODO Login
		//shareSdk.Authorize(PlatformType.WeChat);
       if (DateManger.Instance.getIsFirstLogin()==0)
       {
            shareSdk.Authorize(PlatformType.WeChat);
            DateManger.Instance.setIsFirstLogin(1);
       }
       else
       {
            shareSdk.GetUserInfo(PlatformType.WeChat);
       } 
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
        Debug.Log("回调成功:"+data);
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
                GlobalDataScript.loginResponseData.account.province = loginvo.province;
                GlobalDataScript.loginResponseData.account.city = loginvo.city;
                GlobalDataScript.loginResponseData.account.openid = loginvo.openId;
                GlobalDataScript.loginResponseData.account.nickname = loginvo.nickName;
                GlobalDataScript.loginResponseData.account.headicon = loginvo.headIcon;
                GlobalDataScript.loginResponseData.account.unionid = loginvo.city;
                GlobalDataScript.loginResponseData.account.sex = loginvo.sex;
                GlobalDataScript.loginResponseData.IP = loginvo.IP;

                Debug.Log("获取位置:" + loginvo.province+loginvo.city);
                MJUIManager._instance.loginPage.setLoginText("获取个人信息成功" + loginvo.nickName);

			} catch (Exception e) {
                MJUIManager._instance.loginPage.loginErrorCallBack();
                WantedTextTool.Instance.addTip("请先打开你的微信客户端",0);
				return;
			}
		} else {
            MJUIManager._instance.loginPage.loginErrorCallBack();
            WantedTextTool.Instance.addTip("微信登录失败",0);
		}




	}


    /// <summary>
    /// 分享战绩成功回调
    /// </summary>
    /// <param name="reqID"></param>
    /// <param name="state"></param>
    /// <param name="type"></param>
    /// <param name="result"></param>
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
        StartCoroutine(GetCapture(platformType));
    }

	/**
	 * 执行分享到朋友圈的操作
	 */ 
	private void shareAchievement(PlatformType platformType){
		ShareContent customizeShareParams = new ShareContent();
		customizeShareParams.SetText("我的战绩回调");
		customizeShareParams.SetImagePath(picPath);
		customizeShareParams.SetShareType(ContentType.Image);
		customizeShareParams.SetObjectID(MyName.MJGameName);
		customizeShareParams.SetShareContentCustomize(platformType, customizeShareParams);
		shareSdk.ShareContent (platformType, customizeShareParams);
	}

    /// <summary>
    /// 截屏
    /// </summary>
    /// <param name="platformType"></param>
    /// <returns></returns>
    IEnumerator GetCapture(PlatformType platformType)
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
    /// 分享app
    /// </summary>
    /// <param name="platformType"></param>
    public void shareApp(PlatformType platformType)
    {
        ShareContent content = new ShareContent();
        content.SetText("一起来玩这个游戏吧!");
        content.SetImageUrl(APIS.ImgUrl);
        content.SetTitle(MyName.MJGameName);
        //content.SetTitleUrl("http://www.mob.com");
        //content.SetSite("Mob-ShareSDK");
       // content.SetSiteUrl("http://www.mob.com");
        //content.SetUrl("http://www.mob.com");
        content.SetComment("test description");
        //content.SetMusicUrl("http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3");
        content.SetShareType(ContentType.Webpage);
        content.SetShareContentCustomize(platformType, content);
        shareSdk.ShareContent(platformType, content);
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
