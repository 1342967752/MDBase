using AssemblyCSharp;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDataScript
{
    //是否获取达到ip
    public static bool isGetIP = false;

    //是否开始游戏
    public static bool isStartGame = false;

    //是否已经显示广告
    public static bool  isShowMainMenuAdPage=false;

    //主界面的状态
    public static MainMenuState mainMenuState = new MainMenuState();

    //麻将是否可以点击
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
   // public static string localPostion;//现在位置

	/**麻将剩余局数**/
	public static int surplusTimes=0 ;

	/**总局数**/
	public static int totalTimes=0;

	/**重新加入房间的数据**/
	public static RoomJoinResponseVo reEnterRoomData;

    public static bool isRecord = false;

    //交谈信息
    public static List<float[]> talkingInfos = new List<float[]>();

	/// <summary>
	/// 抽奖数据
	/// </summary>
	public static List<LotteryData> lotteryDatas;
	public static bool isonApplayExitRoomstatus = false;//是否处于申请解散房间状态
	public static bool isOverByPlayer = false;//是否由用用户选择退出而退出的游戏
	public static LoginVo loginVo;//登录数据
	public static List<string> noticeMegs = new List<string>();
    public static GamePlayResponseVo gamePlayResponseVo;

    /// <summary>
    /// 重新初始化数据
    /// </summary>
    public static void reinitData(){
		isDrag = false;
		roomJoinResponseData = null;
		roomVo=new RoomCreateVo(); 
		hupaiResponseVo = null;
		finalGameEndVo = null;
		roomAvatarVoList = null;
		surplusTimes = 0;
		totalTimes = 0;
		reEnterRoomData = null;
		lotteryDatas = null;
		isonApplayExitRoomstatus = false;
		isOverByPlayer = false;
		loginVo = null;

        isStartGame = false;//设置没有开始游戏
        totalTimes = 0;//总局数
        surplusTimes = 0;//剩余局数
        gamePlayResponseVo = null;//战绩回放信息
        isRecord = false;//设置战绩回放为否
        talkingInfos.Clear();//清除交谈信息
	}

    public static void reInitAllData()
    {
        reinitData();
        loginResponseData = null;//重置登录信息
        isShowMainMenuAdPage = false;//可以显示广告
    }

    /// <summary>
    /// 判断msg是否可用
    /// </summary>
    /// <returns></returns>
    public static bool getMsgCanUse()
    {
        if (isRecord)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// 判断语音是否可用
    /// </summary>
    /// <returns></returns>
    public static bool getTalkingCanUse()
    {
        if (isRecord)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
	
public class MainMenuState
{
    //是否显示战绩
    public bool isShowZhanji = false;
    public int zhanJiId = 0;//某一盘的id
    public Vector3 zhanjiContentPos=Vector3.zero;
    public Vector3 zhanjiXaingQingContextPos=Vector3.zero;

    /// <summary>
    /// 是否改变过
    /// </summary>
    /// <returns></returns>
    public bool isChange()
    {
        if (isShowZhanji)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 重置战绩详情信息
    /// </summary>
    public void zhanJiReSet()
    {
        isShowZhanji = false;
        zhanJiId = 0;
        zhanjiContentPos = Vector3.zero;
        zhanjiXaingQingContextPos = Vector3.zero;
    }
}