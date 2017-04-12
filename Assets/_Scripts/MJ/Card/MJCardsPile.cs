using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 牌堆管理
/// </summary>
public class MJCardsPile : MonoBehaviour {

    public static MJCardsPile _instance;

    private int currentPointer=0;//当前拿牌的位置
    private int startPoint=0;//开始位置
    private GameObject jingPaiParent;//精牌的父物体
    private List<GameObject> pileCards = new List<GameObject>();//牌堆里的牌对象
    private Vector3[] cardsPos ;//记录所有牌的位置

    //四个方向的牌堆
    private GameObject bottonPile;
    private GameObject leftPile;
    private GameObject rightPile;
    private GameObject topPile;

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
        bottonPile = transform.FindChild(MJName.BottonName).gameObject;
        topPile = transform.FindChild(MJName.TopName).gameObject;
        rightPile = transform.FindChild(MJName.RightName).gameObject;
        leftPile = transform.FindChild(MJName.LeftName).gameObject;

        jingPaiParent = bottonPile;
        pileCards.Clear();
        startPoint = 0;
        currentPointer = 0;
    }

    /// <summary>
    /// 创建牌堆
    /// </summary>
    public void createCardPile()
    {
        //产生位置
        int count = MJSize.CardsCount/4;
        Vector3[] temp = new Vector3[count];
        cardsPos = temp;
        MJCardAction.Instance.createPostion(temp, Vector3.zero, MJSize.CardSize3D, count / 2);
        //创建牌堆
        for (int i=0;i<temp.Length;i++)
        {
            pileCards.Add(MJCardAction.Instance.createCard("9", bottonPile.transform, temp[i], Vector3.one, MJPostion.pileCardEulerAngles));
        }

        for (int i = 0; i < temp.Length; i++)
        {
            pileCards.Add(MJCardAction.Instance.createCard("9", rightPile.transform, temp[i], Vector3.one, MJPostion.pileCardEulerAngles));
        }

        for (int i = 0; i < temp.Length; i++)
        {
            pileCards.Add(MJCardAction.Instance.createCard("9", topPile.transform, temp[i], Vector3.one, MJPostion.pileCardEulerAngles));
        }

        for (int i=0;i<temp.Length;i++)
        {
            pileCards.Add(MJCardAction.Instance.createCard("9", leftPile.transform, temp[i], Vector3.one, MJPostion.pileCardEulerAngles));
        }

       
       
        Debug.Log("牌堆牌数->" + pileCards.Count);
    }

    /// <summary>
    /// 牌堆重置
    /// </summary>
    public void reSet()
    {
        for (int i=0; i<pileCards.Count;i++)
        {
            if (pileCards[i]!=null)
            {
                Destroy(pileCards[i]);
            }
        }
        pileCards.Clear();
        currentPointer = 0;
        leaveCards = 136;
        jingPaiParent = bottonPile;
    }

    int leaveCards = 136;
    /// <summary>
    /// 摸一张牌
    /// </summary>
    public void getCard()
    {
        if (pileCards.Count<=0)
        {
            return;
        }
        leaveCards--;
        if (pileCards.Count-1<currentPointer)
        {
            currentPointer = 0;
        }
        else
        {
            Destroy(pileCards[currentPointer++]);
        }
        MJUIManager._instance.mJDeskPage.setleaveCards(leaveCards);
        Debug.Log("单前牌数量:" + currentPointer);
    }

    /// <summary>
    /// 获取一定数量的牌
    /// </summary>
    /// <param name="count"></param>
    public void getCardMul(int count)
    {
        for (int i=0;i<count;i++)
        {
            getCard();
        }
    }

    /// <summary>
    /// 设置精牌index
    /// </summary>
    public void setJing(int zheng,int pos)
    {
        int jingPos = pos + startPoint;
        if (jingPos>135)
        {
            jingPos = jingPos %136;
        }

        int posTemp = jingPos;
        Debug.Log("精牌位置:"+jingPos);
        Debug.Log("牌堆数量:" + pileCards.Count);
        //设置精牌的父物体
        if (pileCards[jingPos]!=null)
        {
            Destroy(pileCards[jingPos]);
        }
        if (jingPos<34)
        {
            jingPaiParent = bottonPile;
        }
        else if (jingPos>=34&&jingPos<68)
        {
            jingPaiParent = rightPile;
        }else if (jingPos>=68&&jingPos<102)
        {
            jingPaiParent = topPile;
        }
        else
        {
            jingPaiParent = leftPile;
        }
        
        jingPos = jingPos % 34;
        pileCards[posTemp] =MJCardAction.Instance.createCard(zheng+"", jingPaiParent.transform, cardsPos[jingPos], Vector3.one, MJPostion.pileCardEulerAngles);
        pileCards[posTemp].name = "" + zheng;
        pileCards[posTemp].transform.FindChild("group1").GetChild(0).localEulerAngles = MJPostion.jingAngles;
    }

    /// <summary>
    /// 设置色子点数
    /// </summary>
    /// <param name="one"></param>
    /// <param name="two"></param>
    public void setShaiZi(int one,int two,int bankerIndex)
    {

      
    }

    /// <summary>
    /// 设置庄家
    /// </summary>
    /// <param name="bankerID"></param>
    public void setBanker(int bankerID)
    {
        int pos2 = 0;//开始摸牌位置
        switch (MJPlayerManager._instance.getPlayerPos(bankerID))
        {
            case DirectionEnum.Bottom: ; break;
            case DirectionEnum.Left:pos2 = pos2 + 3*34; break;
            case DirectionEnum.Top: pos2 = pos2 + 2 * 34; break;
            case DirectionEnum.Right:  pos2 = pos2 +  34; break;
        }

        currentPointer = pos2;
        startPoint = currentPointer;
        Debug.Log("startpoint" + startPoint);
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
