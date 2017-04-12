using UnityEngine;
using AssemblyCSharp;
using System.Collections.Generic;
/// <summary>
/// 玩家管理器
/// </summary>
public class MJPlayerManager : MonoBehaviour {
    public static MJPlayerManager _instance;

    private MJPlayerItem botton;
    private MJPlayerItem top;
    private MJPlayerItem left;
    private MJPlayerItem right;

    private int playerSit;//单前未座的位置
    private int currentPos = 0;
    private Dictionary<int, MJPlayerItem> playersVO = new Dictionary<int, MJPlayerItem>();//index对应玩家
    private Dictionary<int, string> playerPoss = new Dictionary<int, string>();//index对应玩家座位

    void Awake()
    {
        _instance = this;
        init();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void init()
    {
        botton = transform.FindChild("Botton").GetComponent<MJPlayerItem>();
        top = transform.FindChild("Top").GetComponent<MJPlayerItem>();
        left = transform.FindChild("Left").GetComponent<MJPlayerItem>();
        right = transform.FindChild("Right").GetComponent<MJPlayerItem>();
    }

    /// <summary>
    /// 设置玩家信息
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="players">0 botton 1 right 2 top 3 left</param>
    public void setPlayerInfo(List<AvatarVO> players)
    {
        if (players==null||players.Count<=0||GlobalDataScript.loginResponseData==null)
        {
            return;
        }

        //找自己的位置
        int myIndex = 0;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].account.uuid == GlobalDataScript.loginResponseData.account.uuid) 
            {
                myIndex = i;
                break; 
            }
        }

        //设置我的位置
        botton.setInfo(players[myIndex],myIndex);
        playersVO.Add(myIndex, botton);//添加我的信息
        playerPoss.Add(myIndex, DirectionEnum.Bottom);//添加我的位置

        //将我后面的玩家填充
        int curSit = 0;//0为下家 1位右家 以此类推
        curSit++;
        for (int i=myIndex+1;i<players.Count;i++)
        {
            switch (curSit)
            {
                case 1:right.setInfo(players[i], i);
                        playersVO.Add(i,right);
                        playerPoss.Add(i,DirectionEnum.Right);
                        break;
                case 2:top.setInfo(players[i], i);
                        playersVO.Add(i,top);
                        playerPoss.Add(i,DirectionEnum.Top);
                        break;
                case 3:left.setInfo(players[i], i);
                    playersVO.Add(i, left);
                    playerPoss.Add(i, DirectionEnum.Left);
                    ; break;
            }
            curSit++;
        }

        //将我前面的人填充
        curSit = 0;//0为下家 -1位左家 以此类推
        curSit--;
        for (int i=myIndex-1;i>=0;i--)
        {
            switch (curSit)
            {
                case -1: left.setInfo(players[i], i);
                    playersVO.Add(i, left);
                    playerPoss.Add(i, DirectionEnum.Left); break;
                case -2: top.setInfo(players[i], i);
                    playersVO.Add(i, top);
                    playerPoss.Add(i, DirectionEnum.Top); break;
                case -3: right.setInfo(players[i], i);
                    playersVO.Add(i, right);
                    playerPoss.Add(i, DirectionEnum.Right); break;
            }
            curSit--;
        }

        //旋转色子盘 并且设置位置
        switch (myIndex)
        {
            case 0:MJDicePlace._instance.setMyDirection(DirectionEnum.Bottom);break;
            case 1:MJDicePlace._instance.setMyDirection(DirectionEnum.Right);break;
            case 2:MJDicePlace._instance.setMyDirection(DirectionEnum.Top);break;
            case 3:MJDicePlace._instance.setMyDirection(DirectionEnum.Left);break;
        }
        playerSit =1;//从我的座位排起
        currentPos = players.Count ;//记录当前玩家人数
    }

    /// <summary>
    /// 设置其他玩家进入信息
    /// </summary>
    /// <param name="player"></param>
    public void setOtherPlayerInfo(AvatarVO player)
    {
        if (playersVO.Count > 0)//先填充之前退出的玩家位置
        {
            for (int i = 0; i < playersVO.Count; i++)
            {
                if (playersVO.ContainsKey(i)&&playersVO[i]==null)
                {
                    playersVO[i].setInfo(player, playersVO[i].getIndex());
                    return;
                }
            }
        }

        //新近入的玩家
        switch (playerSit)
        {
            case 0:botton.setInfo(player, playerSit);
                playersVO.Add(currentPos, botton);
                playerPoss.Add(currentPos, DirectionEnum.Bottom); break;
            case 1:right.setInfo(player, playerSit);
                playersVO.Add(currentPos, right);
                playerPoss.Add(currentPos, DirectionEnum.Right); break;
            case 2:top.setInfo(player, playerSit);
                playersVO.Add(currentPos, top);
                playerPoss.Add(currentPos, DirectionEnum.Top); break;
            case 3:left.setInfo(player, playerSit);
                playersVO.Add(currentPos, left);
                playerPoss.Add(currentPos, DirectionEnum.Left); break;
        }
        Debug.Log("玩家进入:"+player.account.nickname);
        playerSit++;
        currentPos++;
    }

    /// <summary>
    /// 根据index获取玩家的位置
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string getPlayerPos(int index)
    {
        if (playerPoss.ContainsKey(index))
        {
            return playerPoss[index];
        }
        Debug.Log("没有该玩家位置->" + index);
        return "";
    }

    /// <summary>
    /// 获取玩家对象
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public MJPlayerItem getPlayerByIndex(int index)
    {
        if (playersVO.ContainsKey(index))
        {
            return playersVO[index];
        }
        Debug.Log("没有该玩家->" + index);
        return null;
    }

    /// <summary>
    /// 获取所有玩家的名字
    /// </summary>
    /// <returns></returns>
    public List<string> getAllPlayerName()
    {
        List<string> names = new List<string>();
        if (!botton.getName().Equals(""))
        {
            names.Add(botton.getName());
        }

        if (!left.getName().Equals(""))
        {
            names.Add(left.getName());
        }

        if (!right.getName().Equals(""))
        {
            names.Add(right.getName());
        }

        if (!top.getName().Equals(""))
        {
            names.Add(top.getName());
        }
        return names;
    }

    /// <summary>
    /// 获取玩家位置
    /// </summary>
    /// <param name="uuid"></param>
    /// <returns></returns>
    public int getPlayerByUUID(int uuid)
    {
        for (int i=0;i<playersVO.Count;i++)
        {
            if (playersVO.ContainsKey(i)&&playersVO[i].getPlayer().account.uuid==uuid)
            {
                return i;
                
            }
        }

        return -1;
    }

    /// <summary>
    /// 获取我的位置
    /// </summary>
    /// <returns></returns>
    public int getMyIndex()
    {
        return botton.getIndex();
    }

    /// <summary>
    /// 获取我的uuid
    /// </summary>
    /// <returns></returns>
    public int getMyUUID()
    {
        if (botton==null||botton.getPlayer()==null)
        {
            return -1;
        }
        return botton.getPlayer().account.uuid;
    }

    /// <summary>
    /// 设置准备
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isReady"></param>
    public void setReady(int index,bool isReady)
    {
        if (playersVO.ContainsKey(index))
        {
            playersVO[index].setReady(isReady);
            return;
        }
        Debug.Log("没有该玩家:" + index);
    }

    /// <summary>
    /// 关闭所有准备ui
    /// </summary>
    public void closeAllREadyUI()
    {
        for (int i=0;i<playersVO.Count;i++)
        {
            if (playersVO.ContainsKey(i))
            {
                playersVO[i].setReady(false);
            }
        }
    }

    /// <summary>
    /// 获取transfrom
    /// </summary>
    /// <param name="pos"></param>
    public Transform getTransfromByPos(string pos)
    {
        switch (pos)
        {
            case DirectionEnum.Bottom: return botton.getTransfrom();
            case DirectionEnum.Top: return top.getTransfrom();
            case DirectionEnum.Right: return right.getTransfrom();
            case DirectionEnum.Left: return left.getTransfrom();
        }
        return null;
    } 

    /// <summary>
    /// 显示msg
    /// </summary>
    public void showMsgByIndex(int avaIndex,int msgIndex)
    {
        if (playerPoss.ContainsKey(avaIndex))
        {
            getTransfromByPos(playerPoss[avaIndex]).GetComponent<MJMsgItem>().showMsg(msgIndex);
        }  
    }

    /// <summary>
    /// 显示交谈ui
    /// </summary>
    /// <param name="avaIndex"></param>
    public void showTalkingUI(int uuid)
    {
        int avaIndex = getPlayerByUUID(uuid);
        if (avaIndex==-1)
        {
            return;
        }
        if (playerPoss.ContainsKey(avaIndex))
        {
            getTransfromByPos(playerPoss[avaIndex]).GetComponent<MJMsgItem>().showTalkingUI();
        }
    }

    /// <summary>
    /// 返回玩家数量
    /// </summary>
    /// <returns></returns>
    public int getPlayerListCount()
    {
        return playersVO.Count;
    }

    public List<AvatarVO> getPlayerList()
    {
        List<AvatarVO> players = new List<AvatarVO>();
        for (int i=0;i<playersVO.Count;i++)
        {
            if (playersVO.ContainsKey(i))
            {
                players.Add(playersVO[i].getPlayer());
            }
        }
        return players;
    }

    /// <summary>
    /// 获取性别
    /// </summary>
    public int getSexByIndex(int index)
    {
        if (playersVO.ContainsKey(index))
        {
            return playersVO[index].getPlayer().account.sex;
        }
        Debug.Log("没有该玩家性别->" + index);
        return 1;
    }

    /// <summary>
    /// 移除一个玩家信息
    /// </summary>
    public void removePlayerByIndex(int uuid)
    {
        int index = getPlayerByUUID(uuid);
        if (playersVO.Count>0&&playersVO.ContainsKey(index))
        {
            playersVO[index].reSet();
        }
    }

    /// <summary>
    /// 设置用户是否在线
    /// </summary>
    /// <param name="index"></param>
    /// <param name="onLine"></param>
    public void setOnLineByIndex(int index,bool onLine)
    {
        if (!playersVO.ContainsKey(index))
        {
            return;
        }

        playersVO[index].isOnLine(onLine);
    }

    /// <summary>
    /// 设置玩家分数
    /// </summary>
    /// <param name="index"></param>
    /// <param name="changeScore">改变的分数</param>
    public void setPlayerSocreByIndex(int index, int changeScore)
    {
        if (playersVO == null || !playersVO.ContainsKey(index))
        {
            return;
        }

        playersVO[index].setPlayerScore(changeScore);

    }

    void OnDestroy()
    {
        if (_instance != null)
        {
            Destroy(_instance);
            _instance = null;
        }
        Destroy(gameObject);
    }
}
