using UnityEngine;
using AssemblyCSharp;
using System.Collections.Generic;
/// <summary>
/// 玩家管理器
/// </summary>
public class MJPlayerManager : MonoBehaviour {
    public static MJPlayerManager _instance;
    private MJThrowEmojiBox mJThrowEmojiBox;//扔表情处理

    private MJPlayerItem botton;
    private MJPlayerItem top;
    private MJPlayerItem left;
    private MJPlayerItem right;

    private List<MJPlayerItem> playersVO = new List<MJPlayerItem>();//index对应玩家
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

        //填满前面的玩家
        for (int i = 0; i <myIndex; i++)
        {
            if (i==myIndex-1)
            {
                left.setInfo(players[i], i);
                playersVO.Add(left);
                playerPoss.Add(i, DirectionEnum.Left);
            }else if (i==myIndex-2)
            {
                top.setInfo(players[i], i);
                playersVO.Add(top);
                playerPoss.Add(i, DirectionEnum.Top);
            }else if (i==myIndex-3)
            {
                right.setInfo(players[i], i);
                playersVO.Add(right);
                playerPoss.Add(i, DirectionEnum.Right);
            }
            Debug.Log("设置玩家:" + i);
        }

        //加上自己的位置
        playersVO.Add(botton);
        playersVO[myIndex].setInfo(players[myIndex], myIndex);
        playerPoss.Add(myIndex, DirectionEnum.Bottom);
        Debug.Log("我的位置:" + myIndex);

        //记录位置
        for (int i=myIndex+1;i<4;i++)
        {
            if (i==1+myIndex)
            {
                playerPoss.Add(i, DirectionEnum.Right);
            }else if (i==2 + myIndex)
            {
                playerPoss.Add(i, DirectionEnum.Top);
            }else if (i==3 + myIndex)
            {
                playerPoss.Add(i, DirectionEnum.Left);
            }
        }

        //填满后面的人
        for (int i=myIndex+1;i<players.Count;i++)
        {
            if (i==myIndex+1)
            {
                right.setInfo(players[i], i);
                playersVO.Add(right);
            }else if (i==myIndex+2)
            {
                top.setInfo(players[i], i);
                playersVO.Add(top);
            }else if (i==myIndex+3)
            {
                left.setInfo(players[i], i);
                playersVO.Add(left);
            }
            Debug.Log("设置玩家:" + i);
        }
      
        //旋转色子盘 并且设置位置
        switch (myIndex)
        {
            case 0:MJDicePlace._instance.setMyDirection(DirectionEnum.Bottom);break;
            case 1:MJDicePlace._instance.setMyDirection(DirectionEnum.Right);break;
            case 2:MJDicePlace._instance.setMyDirection(DirectionEnum.Top);break;
            case 3:MJDicePlace._instance.setMyDirection(DirectionEnum.Left);break;
        }

        Debug.Log("现有玩家:"+playersVO.Count);
        Debug.Log("现有位置:" + playerPoss.Count);
    }

    /// <summary>
    /// 设置其他玩家进入信息
    /// </summary>
    /// <param name="player"></param>
    public void setOtherPlayerInfo(AvatarVO player)
    {
        //新近入的玩家
        if (playersVO==null||checkeUUID(player.account.uuid))
        {
            return;
        }

        switch (playersVO.Count-getMyIndex())
        {
            case 0:botton.setInfo(player, playersVO.Count);
                playersVO.Add(botton); break;
            case 1:right.setInfo(player, playersVO.Count);
                playersVO.Add(right); break;
            case 2:top.setInfo(player, playersVO.Count);
                playersVO.Add(top); break;
            case 3:left.setInfo(player, playersVO.Count);
                playersVO.Add(left); break;
        }

        Debug.Log("玩家进入:"+player.account.nickname);
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
        if (playersVO!=null&&playersVO.Count>=index)
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
            if (playersVO[i]!=null&&playersVO[i].getPlayer().account.uuid==uuid)
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
        if (index<playersVO.Count)
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
            if (playersVO[i]!=null)
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
        if (avaIndex>=playersVO.Count)
        {
            return;
        }
        getTransfromByPos(playerPoss[avaIndex]).GetComponent<MJMsgItem>().showMsg(msgIndex);
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
        if (playerPoss.ContainsKey(avaIndex)&&GlobalDataScript.talkingInfos.Count>0)
        {
            getTransfromByPos(playerPoss[avaIndex]).GetComponent<MJMsgItem>().addTalking(GlobalDataScript.talkingInfos[0]);
            GlobalDataScript.talkingInfos.RemoveAt(0);
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
            if (playersVO[i]!=null)
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
        if (index<playersVO.Count)
        {
            return playersVO[index].getPlayer().account.sex;
        }
        Debug.Log("没有该玩家性别->" + index);
        return -1;
    }

    /// <summary>
    /// 获取我的性别
    /// </summary>
    /// <returns></returns>
    public int getMySex()
    {
        if (playersVO==null||playersVO.Count==0)
        {
            return -1;
        }

        return getSexByIndex(getMyIndex());
    }
    /// <summary>
    /// 移除一个玩家信息
    /// </summary>
    public void removePlayerByIndex(int uuid)
    {
        int index = getPlayerByUUID(uuid);//获取退出人的位置
        int myIndex = getMyIndex();//获取我的位置
        
        if (index>myIndex)
        {
            MJPlayerItem temp;
            for (int i=index;i<playersVO.Count-1;i++)
            {
                temp = playersVO[i + 1];
                playersVO[i].setInfo(temp.getPlayer(), i);
            }
            playersVO[playersVO.Count - 1].reSet();
            playersVO.RemoveAt(playersVO.Count - 1);
        }
        else
        {
            MJDicePlace._instance.setMyDirection(DirectionEnum.Bottom);//色子盘重置
            List<AvatarVO> players = new List<AvatarVO>();
            playersVO.RemoveAt(index);
            for (int i=0;i<playersVO.Count;i++)
            {
                players.Add(playersVO[i].getPlayer());
            }
            botton.reSet();
            left.reSet();
            top.reSet();
            right.reSet();
            playerPoss.Clear();
            playersVO.Clear();
            setPlayerInfo(players);
        }
    }

    /// <summary>
    /// 设置用户是否在线
    /// </summary>
    /// <param name="index"></param>
    /// <param name="onLine"></param>
    public void setOnLineByIndex(int index,bool onLine)
    {
        if (index>=playersVO.Count)
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
        if (playersVO == null ||index>=playersVO.Count)
        {
            return;
        }

        playersVO[index].setPlayerScore(changeScore);

    }

    /// <summary>
    /// 通过index设置庄家
    /// </summary>
    /// <param name="index"></param>
    public void setBankerByIndex(int index)
    {
        if (index>=playersVO.Count)
        {
            return;
        }

        for (int i=0;i<playersVO.Count;i++)
        {
            if (i==index)
            {
                playersVO[i].setBanker(true);
            }
            else
            {
                playersVO[i].setBanker(false);
            }
        }
    }

    /// <summary>
    /// 获取庄家的uuid
    /// </summary>
    /// <returns></returns>
    public int getBankerUUID()
    {
        for (int i=0;i<playersVO.Count;i++)
        {
            if (playersVO[i].getPlayer().main)
            {
                return playersVO[i].getPlayer().account.uuid;
            }
        }

        return -1;
    }

    /// <summary>
    /// 获取我的头像transfrom
    /// </summary>
    /// <returns></returns>
    public Transform getMyHeadTransfrom()
    {
        if (getTransfromByPos(DirectionEnum.Bottom)==null)
        {
            return null;
        }

        return getTransfromByPos(DirectionEnum.Bottom).transform.FindChild("Head").transform;
    }

    /// <summary>
    /// 获取名字
    /// </summary>
    /// <returns></returns>
    public string getMyNickName()
    {
        if (botton.getPlayer()!=null)
        {
            return botton.getPlayer().account.nickname;
        }
        return "";
    }
    /// <summary>
    /// 检查是否有重复uuid
    /// </summary>
    /// <param name="uuid"></param>
    /// <returns></returns>
    private bool checkeUUID(int uuid)
    {
        if (playersVO==null||playersVO.Count==0)
        {
            return false;
        }

        for (int i=0;i<playersVO.Count;i++)
        {
            if (playersVO[i].getPlayer().account.uuid==uuid)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 改变分数
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="score"></param>
    public void changeScoreByPos(string pos,int score)
    {
        switch (pos)
        {
            case DirectionEnum.Bottom:botton.showScore(score,1f) ;break;
            case DirectionEnum.Top: top.showScore(score, 1f); break;
            case DirectionEnum.Right: right.showScore(score, 1f); break;
            case DirectionEnum.Left: left.showScore(score, 1f); break;
        }
    }

    /// <summary>
    /// 扔表情
    /// </summary>
    /// <param name="startUUID"></param>
    /// <param name="endUUID"></param>
    /// <param name="animIndex"></param>
    public void throwAnim(int startUUID,int endUUID,int animIndex)
    {
        if (mJThrowEmojiBox==null)
        {
            mJThrowEmojiBox = gameObject.AddComponent<MJThrowEmojiBox>();
        }

        mJThrowEmojiBox.setAnim(animIndex, getHeadByUUID(startUUID), getHeadByUUID(endUUID));
    }

    /// <summary>
    /// 获取头像transform
    /// </summary>
    /// <param name="uuid"></param>
    private Transform getHeadByUUID(int uuid)
    {
        return getTransfromByPos(playerPoss[getPlayerByUUID(uuid)]).FindChild("Head");
    }

    /// <summary>
    /// 显示表情
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="emojiIndex"></param>
    public void showEmoji(string pos,int emojiIndex)
    {
        switch (pos)
        {
            case DirectionEnum.Bottom: botton.showEmoji(emojiIndex); break;
            case DirectionEnum.Top: top.showEmoji(emojiIndex); break;
            case DirectionEnum.Right: right.showEmoji(emojiIndex); break;
            case DirectionEnum.Left: left.showEmoji(emojiIndex); break;
        }
    }

    /// <summary>
    /// 根据位置获取index
    /// </summary>
    /// <returns></returns>
    public int getIndexByPos(string pos)
    {
        switch (pos)
        {
            case DirectionEnum.Bottom: return botton.getIndex();
            case DirectionEnum.Top: return top.getIndex();
            case DirectionEnum.Right: return right.getIndex();
            case DirectionEnum.Left: return left.getIndex();
        }
        return -1;
    }

    /// <summary>
    /// 设置hu
    /// </summary>
    /// <param name="isHu"></param>
    public void setHu(string pos,bool isHu)
    {
        switch (pos)
        {
            case DirectionEnum.Bottom: botton.setHu(isHu); break;
            case DirectionEnum.Top: top.setHu(isHu); break;
            case DirectionEnum.Right: right.setHu(isHu); break;
            case DirectionEnum.Left: left.setHu(isHu); break;
        }
    }
    void OnDestroy()
    {
        if (_instance != null)
        {
            playerPoss.Clear();
            playersVO.Clear();
            Destroy(_instance);
            _instance = null;
        }
    }
}
