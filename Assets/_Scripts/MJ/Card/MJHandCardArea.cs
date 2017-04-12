using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 手牌控制
/// </summary>
public class MJHandCardArea : MonoBehaviour {

    private Vector3[] cardsPos = new Vector3[16];
    public Vector2 cardSize = MJSize.HandCardSize;//牌的大小
    private int pointer = 0;
    private List<GameObject> handCards = new List<GameObject>();//手牌
    private List<GameObject> pengGangCards = new List<GameObject>();//碰杠牌
    private GameObject addCard;//取得牌

    void Awake()
    {
        init();
    }

    /// <summary>
    /// 创建3d位置
    /// </summary>
    private void createPostion()
    {
        MJCardAction.Instance.createPostion(cardsPos, Vector2.zero, cardSize, 16);
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void init()
    {
        createPostion();
    }

    /// <summary>
    /// 创建手牌
    /// </summary>
    public void createHandCards()
    {
        for (int i=0;i<13; i++)
        {  
            handCards.Add(MJCardAction.Instance.createCard("2", transform, cardsPos[i], Vector3.one, MJPostion.handCardEulerAngles));
        }
    }

    /// <summary>
    /// 出牌
    /// </summary>
    public void outCard()
    {
        if (addCard==null)
        {
            return;
        }

        int index = Random.Range(0, handCards.Count - 1);
        Destroy(handCards[index]);
        handCards[index] = null;
        addCardInsertHand();
    }

    private void addCardInsertHand()
    {
        if (addCard==null)
        {
            MJCardAction.Instance.removeEmpty(handCards,cardsPos);
            return;
        }

        //获取空位置
        int index = handCards.IndexOf(null);
        Debug.Log("空位置->" + index);
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
        if (addCard!=null)
        {
            Destroy(addCard);
        }
        addCard= MJCardAction.Instance.createCard(name, transform, cardsPos[cardsPos.Length - 1], Vector3.one, MJPostion.handCardEulerAngles);
    }

    /// <summary>
    /// 碰牌
    /// </summary>
    /// <param name="name"></param>
    public void pengCard(string name)
    {
        if (pointer + 3 > cardsPos.Length)
        {
            Debug.Log("不能碰牌,牌不够");
            return;
        }

        MJCardAction.Instance.removeCards(handCards, 3);
       
        if (pengGangCards.Count==0)
        {
            for (int i = 0; i < 3; i++)
            {
                pengGangCards.Add(MJCardAction.Instance.createCard(name, transform, cardsPos[pointer++], Vector3.one, MJPostion.outCardEulerAngles)) ;
            }
            pingYi();
            pointer++;
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                pengGangCards.Add(MJCardAction.Instance.createCard(name, transform, cardsPos[pointer++ - 1], Vector3.one, MJPostion.outCardEulerAngles));
            }
        }
      
    }

    /// <summary>
    /// 杠牌
    /// </summary>
    /// <param name="name"></param>
    public void gangCard(string name,bool isAn)
    {
        if (pointer+4>cardsPos.Length)
        {
            Debug.Log("不能杠牌,牌不够");
            return;
        }

        MJCardAction.Instance.removeCards(handCards, 4);

        if (pengGangCards.Count==0)
        {
            if (isAn)
            {
                for (int i = 0; i < 4; i++)
                {
                    GameObject temp= MJCardAction.Instance.createCard(name, transform, cardsPos[pointer++], Vector3.one, MJPostion.outCardEulerAngles);
                    temp.transform.FindChild("group1").GetChild(0).localEulerAngles = Vector3.up * 180;
                    pengGangCards.Add(temp);
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    pengGangCards.Add(MJCardAction.Instance.createCard(name, transform, cardsPos[pointer++], Vector3.one, MJPostion.outCardEulerAngles));
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
                    MJCardAction.Instance.createCard(name, transform, cardsPos[pointer++-1], Vector3.one, MJPostion.anGangAngles);
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    MJCardAction.Instance.createCard(name, transform, cardsPos[pointer++-1], Vector3.one, MJPostion.outCardEulerAngles);
                }
            }
        }
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
        for (int i=0; i<pengGangCards.Count;i++)
        {
            if (pengGangCards[i].name.Equals(gangName))
            {
                index = i;
                break;
            }
        }

        if (index==-1)
        {
            return false;
        }
        else
        {
            //将杠牌加入
            GameObject temp = MJCardAction.Instance.createCard(gangName, transform, cardsPos[pengGangCards.Count], Vector3.one, MJPostion.outCardEulerAngles);//记录最后一个
            pingYi(pengGangCards, index);//向后以一个单位
            temp.transform.localPosition = cardsPos[pengGangCards.Count];
            pengGangCards.Add(temp);
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

        for (int i=start;i<list.Count;i++)
        {
            list[i].transform.localPosition = cardsPos[i];
        }
    }

    /// <summary>
    /// 吃牌
    /// </summary>
    /// <param name="minName">最小牌的名字</param>
    public void chiCard(string minName)
    {
        if (pointer + 3 > cardsPos.Length)
        {
            Debug.Log("不能吃牌,牌不够");
            return;
        }

        MJCardAction.Instance.removeCards(handCards, 3);
       

        if (pengGangCards.Count==0)
        {
            for (int i = 0; i < 3; i++)
            {
               pengGangCards.Add(MJCardAction.Instance.createCard((int.Parse(minName) + i) + "", transform, cardsPos[pointer++], Vector3.one, MJPostion.outCardEulerAngles));
            }
            pingYi();
            pointer++;
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                pengGangCards.Add(MJCardAction.Instance.createCard((int.Parse(minName) + i) + "", transform, cardsPos[pointer++ - 1], Vector3.one, MJPostion.outCardEulerAngles));
            }
        }

    }

    /// <summary>
    /// 胡牌
    /// </summary>
    /// <param name="names"></param>
    public void huPai(List<string> names)
    {
        //先把当前手牌摧毁
        //MJCardAction.Instance.removeCards(handCards, handCards.Count);
        MJCardAction.Instance.destroyAllChild(transform);

        //再生成牌
        pointer=0;
        for (int i = 0; i < names.Count; i++)
        {
            MJCardAction.Instance.createCard(names[i],transform,cardsPos[pointer++], Vector3.one, MJPostion.outCardEulerAngles);
        }
    }

    /// <summary>
    /// 重置
    /// </summary>
    public void reSet()
    {
        handCards.Clear();
        pengGangCards.Clear();
        pointer = 0;
        addCard = null;
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
