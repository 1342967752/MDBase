using System;
using UnityEngine;
using AssemblyCSharp;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;

public class GlobalDataScript
{
    //用户头像
    public static Sprite headSprite = null;

    //是否开始游戏
    public static bool isStartGame = false;

    public static bool isDrag = false;
	/**登陆返回数据**/
	public static AvatarVO loginResponseData;
	/**加入房间返回数据**/
	public static RoomJoinResponseVo roomJoinResponseData;
	/**房间游戏规则信息**/
	public static RoomCreateVo roomVo=new RoomCreateVo(); 
	/**单局游戏结束服务器返回数据**/
	public static HupaiResponseVo hupaiResponseVo;
	/**全局游戏结束服务器返回数据**/
	public static FinalGameEndVo finalGameEndVo;

	public static int mainUuid;
	/**房间成员信息**/
	public static List<AvatarVO> roomAvatarVoList;

    //交谈时间
    public static float talkingTime = 3;

	/**麻将剩余局数**/
	public static int surplusTimes=0 ;
	/**总局数**/
	public static int totalTimes=0;

	/**重新加入房间的数据**/
	public static RoomJoinResponseVo reEnterRoomData;

	/// <summary>
	/// 声音开关
	/// </summary>
	public static bool soundToggle = true;

	public static List<String> messageBoxContents = new List<string>();
	/// <summary>
	/// 单局结算面板
	/// </summary>
	public static List<GameObject> singalGameOverList = new List<GameObject>();


	public static bool isonLoginPage ;//是否在登陆页面

	//public SocketEventHandle socketEventHandle;
	/// <summary>
	/// 抽奖数据
	/// </summary>
	public static List<LotteryData> lotteryDatas;
	public static bool isonApplayExitRoomstatus = false;//是否处于申请解散房间状态
	public static bool isOverByPlayer = false;//是否由用用户选择退出而退出的游戏
	public static LoginVo loginVo;//登录数据
	public static List<string> noticeMegs = new List<string>();


	/**
	 * 重新初始化数据
	*/
	public static void reinitData(){
		isDrag = false;
		loginResponseData = null;
		roomJoinResponseData = null;
		roomVo=new RoomCreateVo(); 
		hupaiResponseVo = null;
		finalGameEndVo = null;
		roomAvatarVoList = null;
		surplusTimes = 0;
		totalTimes = 0;
		reEnterRoomData = null;
		singalGameOverList =   new List<GameObject>();
		lotteryDatas = null;
		isonApplayExitRoomstatus = false;
		isOverByPlayer = false;
		loginVo = null;
	}


	private static GlobalDataScript _instance;
	public static GlobalDataScript getInstance(){
		if (_instance == null) {
			_instance = new GlobalDataScript ();
		}
		return _instance;
	}

	public string getIpAddress()
	{
		string tempip = "";
		try
		{
            WebRequest wr = WebRequest.Create("http://1212.ip138.com/ic.asp");
            Stream s = wr.GetResponse().GetResponseStream();
            StreamReader sr = new StreamReader(s, Encoding.Default);
            string all = sr.ReadToEnd(); //读取网站的数据

            int start = all.IndexOf("[") + 1;
            int end = all.IndexOf("]");
            int count = end - start;
            tempip = all.Substring(start, count);
            sr.Close();
            s.Close();
        }
		catch
		{
		}
		return tempip;
	}



}
	
