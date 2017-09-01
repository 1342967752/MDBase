﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

/// <summary>
/// 卡牌动作
/// </summary>
public class MJCardAction : MonoBehaviour {
    private static MJCardAction _instance=null;

    public static MJCardAction Instance
    {
        get
        {
            if (_instance==null)
            {
                GameObject temp = new GameObject("MJCardAction");
                _instance = temp.AddComponent<MJCardAction>();
            }
            return _instance;
        }
    }


    #region 牌加入手牌
    //2d信息
    List<Image> handCards;
    float[] cardsPos;
    Image addCard;
    int emptyIndex;
    int pos;

    //3d信息
    GameObject addCard3D;
    Vector3 toPos;

    /// <summary>
    /// 2d牌放入手牌
    /// </summary>
    /// <param name="handCards">手牌</param>
    /// <param name="cardsPos">摆放牌的位置</param>
    /// <param name="addCard">添加的牌</param>
    /// <param name="emptyIndex">出牌的位置</param>
    /// <param name="pos">插入的位置</param>
    public void insertAddCardToHand(List<Image> handCards, float[] cardsPos, Image addCard, int emptyIndex, int pos,Transform praent)
    {
        if (emptyIndex<0||pos<0||addCard==null)
        {
            Debug.Log("不可放入");
            return;
        }

        this.addCard = addCard;
        this.handCards = handCards;
        this.cardsPos = cardsPos;
        this.emptyIndex = emptyIndex;
        this.pos = pos;

        addCard.transform.SetParent(praent);//设置子物体
        moveLeftPos = cardsPos[pos];//水平移动到的位置
        MJCard.canDrag = false;//设置牌不能点击
        addCard.transform.DOLocalMoveY(addCard.rectTransform.sizeDelta.y, 0.2f).OnComplete(moveUpCallBack).SetUpdate(true);
    }

    /// <summary>
    /// 3d插入手牌
    /// </summary>
    /// <param name="addCard">天加的牌</param>
    /// <param name="toPos">目标位置</param>
    /// <param name="parent">父物体</param>
    public void insertAddCardToHand(GameObject addCard, Vector3 toPos, Transform parent)
    {
        if (addCard.transform.parent!=parent)
        {
            addCard.transform.SetParent(parent);
        }

        addCard3D = addCard;
        this.toPos = toPos;

        addCard3D.transform.DOLocalMoveY(addCard.transform.localPosition.y+MySize.CardSize3D.z,0.2f).OnComplete(moveUpCallBack3D).SetUpdate(true);
        Debug.Log("向上");
    }

    float moveLeftPos = 0;//向左移动的位移
    /// <summary>
    /// 向上移动回调
    /// </summary>
    private void moveUpCallBack()
    {
        addCard.transform.DOLocalMoveX(moveLeftPos, 0.4f).OnComplete(moveLeftCallBack).SetUpdate(true);
        addCard.transform.DOLocalRotate(Vector3.forward * -15, 0.4f).SetUpdate(true);
    }

    private void moveUpCallBack3D()
    {
        addCard3D.transform.DOLocalMoveX(toPos.x, 0.3f).OnComplete(moveLeftCallBack3D);
        Debug.Log("向左");
    }
    /// <summary>
    /// 向左移动回调
    /// </summary>
    private void moveLeftCallBack()
    {
        if (emptyIndex < pos)
        {
            for (int i = emptyIndex + 1; i <= pos; i++)
            {
                handCards[i].transform.DOLocalMoveX(cardsPos[i - 1], 0.2f);
                handCards[i - 1] = handCards[i];
            }
        }
        else
        {
            for (int i = emptyIndex - 1; i >= pos; i--)
            {
                handCards[i].transform.DOLocalMoveX(cardsPos[i + 1], 0.2f);
                handCards[i + 1] = handCards[i];
            }
        }
        addCard.transform.DOLocalRotate(Vector3.zero, 0);
        addCard.transform.DOLocalMoveY(0, 0.2f).OnComplete(moveDownCallBack);
    }

    private void moveLeftCallBack3D()
    {
        addCard3D.transform.DOLocalMoveY(toPos.y, 0.2f).SetUpdate(true);
        Debug.Log("向下");
    }

    /// <summary>
    /// 向下移动回调
    /// </summary>
    private void moveDownCallBack()
    {
        handCards[pos] = addCard;
        findEmpty(handCards);
        MJCard.canDrag = true;
    }
    #endregion

    #region 碰牌
    public void removeCards(List<Image> handCards,string bumpName,float[] poss,int count)
    {
        int curCount = 0;
        for (int i=0;i<handCards.Count&&curCount<count;i++)
        {
            if (handCards[i].gameObject.name.Equals(bumpName))
            {
                Destroy(handCards[i].gameObject);
                handCards[i] = null;
                curCount++;
                
            }
        }
        Debug.Log("摧毁:" + curCount);
        removeEmpty(handCards, poss);
    }

    /// <summary>
    /// 移除指定牌
    /// </summary>
    /// <param name="handCards"></param>
    /// <param name="cardName"></param>
    public void removeCards(List<GameObject> handCards,List<string> cardName)
    {
        if (handCards.Count<cardName.Count)
        {
            Debug.Log("牌数量不足，不可移除");
            return;
        }

        if (cardName==null)
        {
            for (int i = 0; i < cardName.Count; i++)
            {
                Destroy(handCards[0]);
                handCards.RemoveAt(0);
            }
        }
        else
        {
            for (int j=0;j<cardName.Count;j++)
            {
                for (int i = 0; i < handCards.Count; i++)
                {
                    if (handCards[i].name.Equals(cardName[j]))
                    {
                        Destroy(handCards[i]);
                        handCards.RemoveAt(i);
                        break;
                    }
                }
            }
            
        }
    }
    #endregion

    #region 创建牌
    /// <summary>
    /// 创建一个牌
    /// </summary>
    /// <param name="name">名字</param>
    /// <param name="parent">父物体</param>
    /// <param name="pos">位置</param>
    /// <param name="localScale"></param>
    /// <param name="localEulerAngles">角度</param>
    public GameObject createCard(string name,Transform parent,Vector3 pos,Vector3 localScale,Vector3 localEulerAngles)
    {
        GameObject temp = Instantiate(Resources.Load<GameObject>(MyPath.MJModelPath + name));
        temp.name = name;
        temp.transform.SetParent(parent);
        temp.transform.localPosition = pos;
        temp.transform.localScale = localScale;
        temp.transform.localEulerAngles = localEulerAngles;
        return temp;
    }

    /// <summary>
    /// 设置位置
    /// </summary>
    /// <param name="temp"></param>
    /// <param name="pos"></param>
    /// <param name="localEulerAngles"></param>
    public GameObject setPostion(GameObject temp,Vector3 pos, Vector3 localEulerAngles)
    {
        temp.transform.localPosition = pos;
        temp.transform.localEulerAngles = localEulerAngles;
        return temp;
    }
    #endregion

    #region 清除空位置
    /// <summary>
    /// 清除空位置
    /// </summary>
    /// <param name="handCards"></param>
    /// <param name="poss">放牌的位置</param>
    public void removeEmpty(List<Image> handCards,float[] poss)
    {
        int empty = 0;
        int count = 0;
        //找到第一个空位置
        for (int i=0;i<handCards.Count;i++)
        {
            if (handCards[i]==null)
            {
                empty = i;
                count++;
            }
        }

        empty = empty + 1 - count;

        //填充位置
        MJCard.canDrag = false;
        for (int i = empty+count; i < handCards.Count; i++)
        {
            if (handCards.Count-1==i)
            {
                handCards[i].transform.DOLocalMoveX(poss[i - count], 0.2f).SetUpdate(true).OnComplete(animCompleteCallBack);
            }
            else
            {
                handCards[i].transform.DOLocalMoveX(poss[i - count], 0.2f).SetUpdate(true);
            }
            
        }

        //如果没有执行上面循环则执行下面if语句
        if (empty+count>=handCards.Count)
        {
            animCompleteCallBack();
        }

        //移除两个空位置

        for (int i = 0; i < count; i++)
        {
            handCards.RemoveAt(empty);
        }
    }

    public void removeEmptyNoAnim(List<Image> handCards, float[] poss)
    {
        int empty = 0;
        int count = 0;
        //找到第一个空位置
        for (int i = 0; i < handCards.Count; i++)
        {
            if (handCards[i] == null)
            {
                empty = i;
                count++;
            }
        }

        empty = empty + 1 - count;

        //填充位置
        MJCard.canDrag = false;
        for (int i = empty + count; i < handCards.Count; i++)
        {
            if (handCards.Count - 1 == i)
            {
                handCards[i].transform.DOLocalMoveX(poss[i - count], 0.2f).SetUpdate(true).OnComplete(animCompleteCallBack);
            }
            else
            {
                handCards[i].transform.DOLocalMoveX(poss[i - count], 0.2f).SetUpdate(true);
            }

        }

        //如果没有执行上面循环则执行下面if语句
        if (empty + count >= handCards.Count)
        {
            animCompleteCallBack();
        }

        //移除两个空位置

        for (int i = 0; i < count; i++)
        {
            handCards.RemoveAt(empty);
        }
    }

    /// <summary>
    /// 动画完毕回调
    /// </summary>
    private void animCompleteCallBack()
    {
        MJCard.canDrag = true;
    }

    //移除所有空位置
    public void removeEmpty(List<GameObject> handCards,Vector3[] poss,int pointer)
    {
        int emptyCount = 0;
        
        for (int i = 0; i < handCards.Count; i++)
        {
            if (handCards[i]==null)
            {
                emptyCount++;
                continue;
            }
            handCards[i - emptyCount] = handCards[i];
            handCards[i].transform.DOLocalMoveX(poss[pointer+i-emptyCount].x, 0.2f).SetUpdate(true);
        }

        //移除空位置
        for (int i=0;i<emptyCount;i++)
        {
            handCards.RemoveAt(handCards.Count - 1);
        }
        Debug.Log("移除:" + emptyCount);
    }

    public void removeEmptyNoAnim(List<GameObject> handCards, Vector3[] poss, int pointer)
    {
        int emptyCount = 0;

        for (int i = 0; i < handCards.Count; i++)
        {
            if (handCards[i] == null)
            {
                emptyCount++;
                continue;
            }
            handCards[i - emptyCount] = handCards[i];
            handCards[i].transform.localPosition= poss[pointer + i - emptyCount];
        }

        //移除空位置
        for (int i = 0; i < emptyCount; i++)
        {
            handCards.RemoveAt(handCards.Count - 1);
        }
        Debug.Log("移除:" + emptyCount);
    }
    #endregion

    /// <summary>
    /// 找到空位置
    /// </summary>
    /// <param name="list"></param>
    public void findEmpty(List<Image> list)
    {
        for (int i=0; i<list.Count;i++)
        {
            if (list[i]==null)
            {
                Debug.Log("存在空->" + i);
            }

        }
    }

    public void findEmpty(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == null)
            {
                Debug.Log(list.Count);
                Debug.Log("存在空->" + i);
            }

        }
    }

    /// <summary>
    /// 创建牌的位置
    /// </summary>
    /// <param name="cardsPos">存储数组</param>
    /// <param name="initPos">开始位置</param>
    /// <param name="cardSize">牌的大小</param>
    public void createPostion(Vector3[] cardsPos,Vector3 initPos,Vector2 cardSize,int lineMaxCount)
    {
        float startX = initPos.x;
        float row = 0;//第几列
        int nowCount = 0;
        for (int i=0;i<cardsPos.Length; i++)
        {
            if (nowCount!=0&&nowCount%lineMaxCount==0)
            {
                nowCount = 0;
                startX = initPos.x;
                row++;
            }
            nowCount++;
            cardsPos[i].x = (startX + (nowCount* cardSize.x));
            
            cardsPos[i].y = 0;
            cardsPos[i].z = (initPos.y-row * cardSize.y);
        }
    }

    public void createPostion(Vector3[] cardsPos,Vector3 initPos,Vector3 cardSize,int lineCount)
    {
        int row = 0;
        for (int i = 0; i < 2*lineCount; i=i+2)
        {
            
            cardsPos[i].x = initPos.x + row* cardSize.x;
            cardsPos[i].y = initPos.y+cardSize.y;
            cardsPos[i].z = initPos.z;

            cardsPos[i+1].x = initPos.x + row * cardSize.x;
            cardsPos[i+1].y = initPos.y ;
            cardsPos[i+1].z = initPos.z;

            row++;
        }
    }

    /// <summary>
    /// 加载图片
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Sprite getSpriteByName(string name)
    {
        return Resources.Load<Sprite>(MyPath.MJBottonSpritePath + name);
    }

    /// <summary>
    /// 交换list里面两个gb
    /// </summary>
    /// <param name="list"></param>
    /// <param name="one"></param>
    /// <param name="two"></param>
    public void changeGB(List<GameObject> list,int one,int two)
    {
        if (list==null||list.Count==0||one<0||two<0||one==two||one>list.Count-1||two>list.Count-1)
        {
            Debug.Log("不能交换" + one + "|" + two);
            return;
        }

        GameObject temp = list[one];
        list[one] = list[two];
        list[two] = temp;
    }

    /// <summary>
    /// 去重
    /// </summary>
    /// <param name="list"></param>
    public void removeTheSame(List<string> list)
    {
        if (list==null||list.Count==0)
        {
            return;
        }

        //去重
        for (int i = 0; i < list.Count; i++)  //外循环是循环的次数
        {
            for (int j = i + 1; j < list.Count; j++)  //内循环是 外循环一次比较的次数
            {

                if (list[i].Equals(list[j]))
                {
                    list.RemoveAt(j);
                    Debug.Log("重复:" + list[i]);
                }
            }
        }
    }

    /// <summary>
    /// 排序 （从小到大）
    /// </summary>
    public void sort(List<string> list)
    {
        if (list==null||list.Count==0)
        {
            return;
        }

        string temp = "";
        for (int i=0;i<list.Count;i++)
        {
            for (int j=i+1;i<list.Count;i++)
            {
                if (int.Parse(list[i])>int.Parse(list[j]))
                {
                    temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;
                }
            }
        }
    }

    /// <summary>
    /// 卡牌排序
    /// </summary>
    /// <param name="list"></param>
    public void sort(List<GameObject> list)
    {
        if (list.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < list.Count; i++)
        {
            for (int j = i + 1; j < list.Count; j++)
            {
                if (list[i].GetComponent<MJCard>().cardtype < list[j].GetComponent<MJCard>().cardtype)
                {
                    changePos(list, i, j);
                }
                else if (list[i].GetComponent<MJCard>().cardtype == list[j].GetComponent<MJCard>().cardtype && int.Parse(list[i].gameObject.name) > int.Parse(list[j].gameObject.name))
                {
                    changePos(list, i, j);
                }
            }
        }
    }

    /// <summary>
    /// 两个图片交换位置
    /// </summary>
    /// <param name="img1"></param>
    /// <param name="img2"></param>
    private void changePos(List<GameObject> handCards, int index01, int index02)
    {
        GameObject temp = handCards[index01];
        
        //数组交换位置
        handCards[index01] = handCards[index02];
        handCards[index02] = temp;

        //场景中交换位置
        Vector3 pos1 = handCards[index01].transform.localPosition;
        handCards[index01].transform.localPosition = handCards[index02].transform.localPosition;
        handCards[index02].transform.localPosition = pos1;
    }

    /// <summary>
    /// 摧毁所有子物体
    /// </summary>
    /// <param name="parent">父物体</param>
    public void destroyAllChild(Transform parent)
    {
        if (parent.childCount > 0)
        {
            int count = parent.childCount;
            for (int i = 0; i < count; i++)
            {
                Destroy(parent.GetChild(i).gameObject);
            }
        }
    }
}
