using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 手牌控制
/// </summary>
public class MJHandCardArea : MonoBehaviour {
    public static JingResponse jingPai;

    private Vector3[] cardsPos = new Vector3[20];
    public Vector2 cardSize = MySize.HandCardSize;//牌的大小
    private int pointer = 0;
    private List<GameObject> handCards = new List<GameObject>();//手牌
    private List<GameObject> pengGangCards = new List<GameObject>();//碰杠牌
    private GameObject addCard;//取得牌
    private string createCardName = "2";//创建牌的名字
    private bool isHu = false;//是否胡牌

    //是否校准胡牌位置（防止胡牌不在中间）
    public bool isCheckPosInHuCard = false;//是否
    public Vector3 initPosInHuCard = Vector3.one;//胡牌前的位置
    public Vector3 huPosInHuCard = Vector3.one;//胡牌之后的位置
    private bool isRecord = false;


    void Awake()
    {
        init();
    }

    /// <summary>
    /// 创建3d位置
    /// </summary>
    private void createPostion()
    {
        MJCardAction.Instance.createPostion(cardsPos, Vector2.zero, cardSize, 20);
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void init()
    {
        createPostion();
        if (isCheckPosInHuCard)
        {
            transform.localPosition = initPosInHuCard;//赋值初始位置
        }

        //判断是否是战绩回放
        isRecord = GlobalDataScript.isRecord;
    }

    /// <summary>
    /// 创建手牌
    /// </summary>
    public void createHandCards()
    {
        for (int i=0;i<13; i++)
        {  
            handCards.Add(MJCardAction.Instance.createCard(createCardName, transform, cardsPos[i], Vector3.one, MyPostion.handCardEulerAngles));
        }
    }


    public void createHandCards(List<string> names)
    {
        for (int i = 0; i < 13; i++)
        {
            handCards.Add(MJCardAction.Instance.createCard(names[i], transform, cardsPos[i], Vector3.one, MyPostion.handCardEulerAngles));
        }

        if (isRecord)
        {
            for (int i = 0; i < handCards.Count; i++)
            {
                handCards[i].transform.GetChild(0).GetChild(0).localEulerAngles = MyPostion.MajiangRecordEulerAngles;
                handCards[i].AddComponent<MJCard>();
                handCards[i].GetComponent<MJCard>().weight = int.Parse(handCards[i].name);

                //设置牌的类型
                if (jingPai == null)
                {
                    continue; 
                }

                if (handCards[i].name.Equals("" + jingPai.zhengJingPai))
                {
                    Debug.Log("正精");
                    handCards[i].GetComponent<MJCard>().cardtype = CardType.ZhengJing;
                }
                else if (handCards[i].name.Equals("" + jingPai.fuJingPai))
                {
                    Debug.Log("副精");
                    handCards[i].GetComponent<MJCard>().cardtype = CardType.FuJing;
                }
                else
                {
                    handCards[i].GetComponent<MJCard>().cardtype = CardType.None;
                }
            }
        }

        MJCardAction.Instance.sort(handCards);
    }
    /// <summary>
    /// 出牌
    /// </summary>
    public void outCard(string name)
    {
        if (addCard != null && addCard.name.Equals(name))
        {
            Destroy(addCard);
            return;
        }
        if (handCards.Count <= 0)
        {
            return;
        }

        if (isRecord)
        {
            MJCardAction.Instance.removeCards(handCards, new List<string> { name });
            MJCardAction.Instance.removeEmptyNoAnim(handCards, cardsPos, pointer);
        }
        else
        {
            Debug.Log("剩余牌:" + handCards.Count);
            int index = Random.Range(0, handCards.Count - 1);
            Destroy(handCards[index]);
            handCards[index] = null;
            
        }
        addCardInsertHand();
    }

    /// <summary>
    /// 找到插入的位置
    /// </summary>
    private int findInsertIndex(List<GameObject> handCards,GameObject card)
    {
        if (handCards==null||handCards.Count==0)
        {
            return -1;
        }

        for (int i=0;i<handCards.Count;i++)
        {
            if (handCards[i]==null)
            {
                continue;
            }

            if (card.GetComponent<MJCard>().cardtype > handCards[i].GetComponent<MJCard>().cardtype)
            {
                return i;
            }
            else if (card.GetComponent<MJCard>().cardtype == handCards[i].GetComponent<MJCard>().cardtype && int.Parse(card.name) < int.Parse(handCards[i].name))
            {
                return i;
            }

        }
        return handCards.Count;
    }

    private void addCardInsertHand()
    {
        if (addCard==null)
        {
            MJCardAction.Instance.removeEmpty(handCards,cardsPos,pointer);
            return;
        }

        //获取空位置
        if (isRecord)
        {
            int pos = findInsertIndex(handCards, addCard);
            handCards.Add(null);
            for (int i=handCards.Count-1;i>pos;i--)
            {
                handCards[i] = handCards[i - 1];
                handCards[i].transform.localPosition = cardsPos[i + pointer];
            }
            handCards[pos] = null;
        }

        int index = handCards.IndexOf(null);
        if (index<0)
        {
            return;
        }
        handCards[index] = addCard;
        MJCardAction.Instance.insertAddCardToHand(addCard, cardsPos[pointer + index],transform);
        addCard = null;
    }

    /// <summary>
    /// 取牌
    /// </summary>
    /// <param name="name"></param>
    public void addCardInPile(string name)
    {
        if (isHu)
        {
            return;
        }

        if (addCard!=null)
        {
            Destroy(addCard);
        }
        Debug.Log(cardsPos.Length + "|" +( pointer + 1));
        addCard= MJCardAction.Instance.createCard(name, transform, cardsPos[handCards.Count+pointer+1], Vector3.one, MyPostion.handCardEulerAngles);
        addCard.AddComponent<MJCard>();
        addCard.GetComponent<MJCard>().weight = int.Parse(addCard.name);

        //设置牌的类型
        if (jingPai == null)
        {
            Debug.Log("无精牌数据");
            return;
        }

        if (name.Equals("" + jingPai.zhengJingPai))
        {
            addCard.GetComponent<MJCard>().cardtype = CardType.ZhengJing;
        }
        else if (name.Equals("" + jingPai.fuJingPai))
        {
            addCard.GetComponent<MJCard>().cardtype = CardType.FuJing;
        }
        else
        {
            addCard.GetComponent<MJCard>().cardtype = CardType.None;
        }

        if (isRecord)
        {
            addCard.transform.GetChild(0).GetChild(0).localEulerAngles = MyPostion.MajiangRecordEulerAngles;
        }  
    }

    /// <summary>
    /// 碰牌
    /// </summary>
    /// <param name="name"></param>
    public void pengCard(string name)
    {
        if (isHu||pointer + 3 > cardsPos.Length)
        {
            Debug.Log("不能碰牌,牌不够");
            return;
        }

        MJCardAction.Instance.findEmpty(handCards);
        if (pengGangCards.Count==0)
        {
            for (int i = 0; i < 3; i++)
            {
                pengGangCards.Add(MJCardAction.Instance.createCard(name, transform, cardsPos[pointer++], Vector3.one, MyPostion.outCardEulerAngles)) ;
            }
            pingYi();
            pointer++;
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                pengGangCards.Add(MJCardAction.Instance.createCard(name, transform, cardsPos[pointer++ - 1], Vector3.one, MyPostion.outCardEulerAngles));
            }
        }

        if (isRecord)
        {
            MJCardAction.Instance.removeCards(handCards, new List<string>() { name, name });
        }
        else
        {
            MJCardAction.Instance.removeCards(handCards, new List<string>() { createCardName, createCardName });
        }

        MJCardAction.Instance.removeEmpty(handCards, cardsPos, pointer);
    }

    /// <summary>
    /// 杠牌
    /// </summary>
    /// <param name="name"></param>
    public void gangCard(string name,bool isAn)
    {
        if (isHu||pointer+4>cardsPos.Length)
        {
            Debug.Log("不能杠牌,牌不够");
            return;
        }

       

        if (pengGangCards.Count==0)
        {
            if (isAn)
            {
                

                for (int i = 0; i < 4; i++)
                {
                    GameObject temp= MJCardAction.Instance.createCard(name, transform, cardsPos[pointer++], Vector3.one, MyPostion.outCardEulerAngles);
                    temp.transform.FindChild("group1").GetChild(0).localEulerAngles = Vector3.up * 180;
                    pengGangCards.Add(temp);
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    pengGangCards.Add(MJCardAction.Instance.createCard(name, transform, cardsPos[pointer++], Vector3.one, MyPostion.outCardEulerAngles));
                }
            }
            pingYi();
            pointer++;
        }
        else
        {
            if (isAn)
            {
                for (int i = 0; i < 4; i++)
                {
                    GameObject temp = MJCardAction.Instance.createCard(name, transform, cardsPos[pointer++-1], Vector3.one, MyPostion.outCardEulerAngles);
                    temp.transform.FindChild("group1").GetChild(0).localEulerAngles = Vector3.up * 180;
                    pengGangCards.Add(temp);
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    pengGangCards.Add( MJCardAction.Instance.createCard(name, transform, cardsPos[pointer++-1], Vector3.one, MyPostion.outCardEulerAngles));
                }
            }
        }


        if (isAn)
        {
            if (addCard != null)
            {
                Destroy(addCard);
                if (isRecord)
                {
                    MJCardAction.Instance.removeCards(handCards, new List<string> { name, name, name });
                }
                else
                {
                    MJCardAction.Instance.removeCards(handCards, new List<string> { createCardName, createCardName, createCardName });
                }

            }
            else
            {
                if (isRecord)
                {
                    MJCardAction.Instance.removeCards(handCards, new List<string> { name, name, name, name });
                }
                else
                {
                    MJCardAction.Instance.removeCards(handCards, new List<string> { createCardName, createCardName, createCardName, createCardName });
                }
            }

        }
        else
        {
            if (isRecord)
            {
                MJCardAction.Instance.removeCards(handCards, new List<string> { name, name, name });
            }
            else
            {
                MJCardAction.Instance.removeCards(handCards, new List<string> { createCardName, createCardName, createCardName });
            }
        }

        MJCardAction.Instance.removeEmpty(handCards, cardsPos, pointer);
    }

    /// <summary>
    /// 杆之前是否杆过
    /// </summary>
    public bool isPengAndGang(string gangName)
    {
        if (pengGangCards.Count==0)
        {
            return false;
        }

        int index = -1;//记录碰牌的第一个位置
        for (int i=2; i<pengGangCards.Count;i++)
        {
            if ((pengGangCards[i-1].name.Equals(gangName)&&!pengGangCards[i].name.Equals(gangName))||
                pengGangCards[i].name.Equals(gangName) && i == pengGangCards.Count - 1)
            {
                index = i;
                break;
            }
        }

        if (index==-1)
        {
            return false;
        }
        else//将杠牌加入
        {
            //判断是否是最后一个
            Debug.Log("添加杠牌位置:" + index+"("+pengGangCards[index].gameObject.name+")");
            if (index==pengGangCards.Count-1)
            {
                GameObject temp = MJCardAction.Instance.createCard(gangName, transform, cardsPos[pengGangCards.Count], Vector3.one, MyPostion.outCardEulerAngles);//记录最后一个
                pengGangCards.Add(temp); 
              
            }
            else//中间
            { 
                GameObject temp = MJCardAction.Instance.createCard(gangName, transform, cardsPos[index], Vector3.one, MyPostion.outCardEulerAngles);//记录最后一个
                GameObject lastTemp = pengGangCards[pengGangCards.Count - 1];
                pingYi(pengGangCards, index);//向后以一个单位
                pengGangCards.Add(lastTemp);
                pengGangCards[pengGangCards.Count - 1].transform.localPosition = cardsPos[pengGangCards.Count - 1];
                pengGangCards[index] = temp;
                
            }

            if (addCard!=null)
            {
                Destroy(addCard);
            }

            pingYi();
            pointer++;
            return true;
        }
    }

    /// <summary>
    /// 从指定位置平移一个单位
    /// </summary>
    /// <param name="list"></param>
    /// <param name="start"></param>
    private void pingYi(List<GameObject> list,int start)
    {
        if (list==null||list.Count==0||start>=list.Count)
        {
            return;
        }

        //位置往后移动一个单位
        for (int i=start;i<list.Count;i++)
        {
            list[i].transform.localPosition = cardsPos[i + 1];
        }

        //在list里面的位置往后移动一个单位
        for (int i=list.Count-1;i>start;i--)
        {
            list[i] = list[i - 1];
        }
    }

    /// <summary>
    /// 吃牌
    /// </summary>
    /// <param name="minName">最小牌的名字</param>
    public void chiCard(string minName,string one,string two)
    {
        if (isHu||pointer + 3 > cardsPos.Length)
        {
            Debug.Log("不能吃牌,牌不够");
            return;
        }

       
        if (pengGangCards.Count==0)
        {
            for (int i = 0; i < 3; i++)
            {
               pengGangCards.Add(MJCardAction.Instance.createCard((int.Parse(minName) + i) + "", transform, cardsPos[pointer++], Vector3.one, MyPostion.outCardEulerAngles));
            }
            pingYi();
            pointer++;
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                pengGangCards.Add(MJCardAction.Instance.createCard((int.Parse(minName) + i) + "", transform, cardsPos[pointer++-1], Vector3.one, MyPostion.outCardEulerAngles));
            }
        }

        if (isRecord)
        {
            MJCardAction.Instance.removeCards(handCards, new List<string> { one, two });
        }
        else
        {
            MJCardAction.Instance.removeCards(handCards, new List<string> { createCardName, createCardName });
        }

        MJCardAction.Instance.removeEmpty(handCards, cardsPos, pointer);

    }

    /// <summary>
    /// 胡牌
    /// </summary>
    /// <param name="names"></param>
    public void huPai(List<string> names)
    {
        if (isCheckPosInHuCard)
        {
            transform.localPosition = huPosInHuCard;
        }

        MJCardAction.Instance.destroyAllChild(transform);

        //再生成牌
        pointer=0;
        for (int i = 0; i < names.Count; i++)
        {
            MJCardAction.Instance.createCard(names[i],transform,cardsPos[pointer++], Vector3.one, MyPostion.outCardEulerAngles);
        }

        isHu = true;
    }

    /// <summary>
    /// 重置
    /// </summary>
    public void reSet()
    {
        isHu = false;
        handCards.Clear();
        pengGangCards.Clear();
        pointer = 0;
        addCard = null;
        if (isCheckPosInHuCard)
        {
            transform.localPosition = initPosInHuCard;
        }
        MJCardAction.Instance.destroyAllChild(transform);
    }

    /// <summary>
    /// 向后平移一个单位平移
    /// </summary>
    /// <param name="handCards"></param>
    private void pingYi()
    {
        if (pointer+1>cardsPos.Length)
        {
            return;
        }
       
        for (int i=0;i<handCards.Count;i++)
        {
            handCards[i].transform.localPosition = cardsPos[i+1 + pointer];
        }
    }
}
