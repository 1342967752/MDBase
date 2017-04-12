using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
/// <summary>
/// 卡牌管理器
/// </summary>
public class MJCardsManager : MonoBehaviour {

    public static MJCardsManager _instance;
    public JingResponse jingPai;

    void Awake()
    {
        _instance = this;
        init();
    }

    #region 公用
    /// <summary>
    /// 初始化
    /// </summary>
    public void init()
    {
        //寻找区域
        bottonHandCardContent = transform.FindChild("botton/handcard").gameObject;
        bottonHandCardInitPos = bottonHandCardContent.transform.localPosition;//记录初始位置
        bottonAddCardArea = transform.FindChild("botton/addcard").gameObject;//获取加牌区域
        bottonCurrentSelectArea = transform.FindChild("botton/CurrentSelectArea");//获取零时选择牌区域
 

        bottonOutCardArea = GameObject.Find(MJPath.MJOutCardAreaPath + MJName.BottonName).GetComponent<MJOutCardArea>();
        bottonHandCardArea = GameObject.Find(MJPath.MJHandCardPath + MJName.BottonName).GetComponent<MJHandCardArea>();
        leftOutCardArea = GameObject.Find(MJPath.MJOutCardAreaPath + MJName.LeftName).GetComponent<MJOutCardArea>();
        leftHandCardArea = GameObject.Find(MJPath.MJHandCardPath + MJName.LeftName).GetComponent<MJHandCardArea>();
        rightOutCardArea = GameObject.Find(MJPath.MJOutCardAreaPath + MJName.RightName).GetComponent<MJOutCardArea>();
        rightHandCardArea = GameObject.Find(MJPath.MJHandCardPath + MJName.RightName).GetComponent<MJHandCardArea>();
        topOutCardArea = GameObject.Find(MJPath.MJOutCardAreaPath + MJName.TopName).GetComponent<MJOutCardArea>();
        topHandCardArea = GameObject.Find(MJPath.MJHandCardPath + MJName.TopName).GetComponent<MJHandCardArea>();
        bottonAddCardAreaInitPos = bottonHandCardArea.transform.localPosition;//记录底部玩家添加牌区域初始位置

    }

    #endregion

    #region 底部玩家
    private List<Image> bottonHandCards = new List<Image>();
    private float[] bottonHandCardsPos = new float[13];
    private GameObject bottonHandCardContent;//牌容器
    private Vector3 bottonHandCardInitPos = Vector3.zero;//手牌容器的初始位置
    private GameObject bottonAddCardArea;//加牌区域
    private MJOutCardArea bottonOutCardArea;//出牌区
    private MJHandCardArea bottonHandCardArea;//3d手牌区
    private Transform bottonCurrentSelectArea;//单前被选中牌的区域
    private float bottonAddCardToHandCardsDis = 100;//添加的牌距手牌的最后一张牌的位置
    private Vector3 bottonAddCardAreaInitPos;//底部玩家添加牌区域初始位置
    private GameObject bottonAddCardGB=null;//底部玩家添加牌

    /// <summary>
    /// 获取添加的牌
    /// </summary>
    /// <returns></returns>
    public Image getBottonAddCard()
    {
        if (bottonAddCardGB==null)
        {
            return null;
        }
        return bottonAddCardGB.GetComponent<Image>();
    }

    /// <summary>
    /// 取得牌放入手牌
    /// </summary>
    private void bottonAddcardInsertHand()
    {
        if (getBottonAddCard()==null)
        {
            MJCardAction.Instance.removeEmpty(bottonHandCards, bottonHandCardsPos);
            return;
        }

        int emptyIndex = bottonHandCards.IndexOf(null);//找到空位置
        int pos = getInsertIndex(bottonHandCards, getBottonAddCard());//找到插入位置

        
        if (emptyIndex<pos&&pos!=bottonHandCards.Count)
        {
            pos--;
        }else if (pos==bottonHandCards.Count)
        {
            pos = bottonHandCards.Count - 1;
        }
        
        MJCardAction.Instance.insertAddCardToHand(bottonHandCards, bottonHandCardsPos, getBottonAddCard(), emptyIndex, pos,bottonHandCardContent.transform);
        bottonAddCardGB = null;
    }

    /// <summary>
    /// 从牌里摸牌
    /// </summary>
    /// <param name="name"></param>
    private void bottonAddCard(string name)
    {
        if (!MJCard.canDrag)
        {
            //延时摸牌
            BottonAddCardPoint = int.Parse(name);
            StartCoroutine(dealyAddCard());
            Debug.Log("延时加牌开始");
            return;
        }
       

        MJCardAction.Instance.destroyAllChild(bottonAddCardArea.transform);//摧毁之前添加的牌
        if (getSpriteByName(name) == null)
        {
            return;
        }

        //产生一张牌
        bottonAdjustAddCardPos();
        GameObject temp = Instantiate(Resources.Load<GameObject>(MJPath.MJCardPath));
        temp.name = name;
        temp.transform.FindChild("Image").GetComponent<Image>().sprite = getSpriteByName(name);
        temp.transform.SetParent(bottonAddCardArea.transform);
        temp.transform.localPosition = Vector3.zero;
        temp.transform.localScale = Vector3.one;
        temp.name = name;
        temp.AddComponent<MJCard>();
        temp.GetComponent<MJCard>().weight = int.Parse(name);//设置牌的权值

        //设置牌的类型
        if (jingPai==null)
        {
            return;
        }

        if (name.Equals(""+jingPai.zhengJingPai))
        {
            temp.GetComponent<MJCard>().cardtype = CardType.ZhengJing;
            temp.transform.FindChild("Jing").gameObject.SetActive(true);
        }else if (name.Equals(""+jingPai.fuJingPai))
        {
            temp.GetComponent<MJCard>().cardtype = CardType.FuJing;
            temp.transform.FindChild("Jing").gameObject.SetActive(true);
        }
        else
        {
            temp.GetComponent<MJCard>().cardtype = CardType.None;
            temp.transform.FindChild("Jing").gameObject.SetActive(false);
        }

        bottonAddCardGB = temp;//记录添加的牌
        removeSelectCard();//移除选中的牌
        Debug.Log("添加牌类型:" + (int)temp.GetComponent<MJCard>().cardtype);
    }

    int BottonAddCardPoint = -1;
    /// <summary>
    /// 延时摸牌
    /// </summary>
    /// <returns></returns>
    IEnumerator dealyAddCard()
    {
        
        while (true)
        {
            if (MJCard.canDrag)
            {
                Debug.Log("延时加牌成功");
                addCard(DirectionEnum.Bottom, BottonAddCardPoint + "");
                
                break;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    /// <summary>
    /// 调整添加手牌的位置
    /// </summary>
    private void bottonAdjustAddCardPos()
    {
        if (bottonHandCards!=null&&bottonHandCards.Count==0)
        {
            return;
        }
        Debug.Log("最后一张牌:"+bottonHandCards[bottonHandCards.Count - 1].gameObject.name+"|位置:"+ bottonHandCards[bottonHandCards.Count - 1].transform.localPosition.x);
        float x = bottonHandCards[bottonHandCards.Count - 1].transform.parent.localPosition.x;
        bottonAddCardArea.transform.localPosition = new Vector3(bottonHandCards[bottonHandCards.Count - 1].transform.localPosition.x + bottonAddCardToHandCardsDis+x, bottonAddCardArea.transform.localPosition.y, bottonAddCardArea.transform.localPosition.z);
    }

    /// <summary>
    /// 创建手牌
    /// </summary>
    /// <param name="cardsName">自己手牌name</param>
    public void createCards(List<string> cardsName)
    {
        if (cardsName.Count<=0)
        {
            return;
        }
        Image botton = Resources.Load<Image>(MJPath.MJCardPath);//加载卡片
        //产生位置
        createPostion(0, botton.rectTransform.sizeDelta.x - 3, bottonHandCardsPos);
        int count = cardsName.Count > 13 ? 13 : cardsName.Count;
        //产生卡片
        for (int i = 0; i < count; i++)
        {
            bottonHandCards.Add(Instantiate(botton));
            bottonHandCards[i].transform.SetParent(bottonHandCardContent.transform);
            bottonHandCards[i].gameObject.AddComponent<MJCard>();
            bottonHandCards[i].transform.localScale = Vector3.one;
            bottonHandCards[i].transform.localPosition = Vector3.zero;
            bottonHandCards[i].transform.FindChild("Image").GetComponent<Image>().sprite = getSpriteByName(cardsName[i]);
            bottonHandCards[i].gameObject.name = cardsName[i];

            bottonHandCards[i].GetComponent<MJCard>().weight = int.Parse(cardsName[i]);//设置牌的权值

            //设置牌的类型
            if (jingPai == null)
            {
                continue;;
            }

            if (cardsName[i].Equals("" + jingPai.zhengJingPai))
            {
                Debug.Log("正精:" + cardsName[i]);
                bottonHandCards[i].GetComponent<MJCard>().cardtype = CardType.ZhengJing;
                bottonHandCards[i].transform.FindChild("Jing").gameObject.SetActive(true);
            }
            else if (cardsName[i].Equals("" + jingPai.fuJingPai))
            {
                Debug.Log("副精:" + cardsName[i]);
                bottonHandCards[i].GetComponent<MJCard>().cardtype = CardType.FuJing;
                bottonHandCards[i].transform.FindChild("Jing").gameObject.SetActive(true);
            }
            else
            {
                bottonHandCards[i].GetComponent<MJCard>().cardtype = CardType.None;
                bottonHandCards[i].transform.FindChild("Jing").gameObject.SetActive(false);
            }
        }
        //排序
        sort(bottonHandCards);
        //放置卡片
        putCards(bottonHandCards, bottonHandCardsPos);
    }

    /// <summary>
    /// 移动手牌
    /// </summary>
    /// <param name="gang">杠次数</param>
    /// <param name="peng">碰次数</param>
    public void bottonMoveHandCard(int gang,int peng)
    {
        Debug.Log("碰：" + peng + "|" + "杠" + gang);
        MJCard.canDrag = false;
        bottonHandCardContent.transform.DOLocalMoveX((bottonHandCardContent.transform.localPosition.x + 2 *peng* MJSize.CardSize2D.x+ bottonHandCardContent.transform.localPosition.x + 3 * gang * MJSize.CardSize2D.x), 0.5f).OnComplete(bottonAnimCompleteCallBack);
    }

    /// <summary>
    /// 将牌摆放到桌面上
    /// </summary>
    /// <param name="cards">牌</param>
    /// <param name="poss">位置</param>
    private void putCards(List<Image> cards, float[] poss)
    {
        for (int i = 0; i < cards.Count && i < poss.Length; i++)
        {
            cards[i].transform.localPosition = new Vector2(poss[i], 0);
        }
    }

    /// <summary>
    /// 产生牌的位置
    /// </summary>
    /// <param name="initPos">初始位置</param>
    /// <param name="cardWidth">牌的宽度</param>
    /// <param name="poss">存储位置的数组</param>
    private void createPostion(float initPos, float cardWidth, float[] poss)
    {
        poss[6] = initPos;

        for (int i = 0; i < 6; i++)
        {
            poss[poss.Length - 1 - i] = initPos + (6 - i) * cardWidth;
            poss[5 - i] = initPos - (i + 1) * cardWidth;
        }
    }

    /// <summary>
    /// 根据牌的名字获取牌list
    /// </summary>
    /// <param name="handCards">手牌</param>
    /// <param name="name">名字</param>
    /// <param name="count">获取数量</param>
    /// <returns></returns>
    private List<Image> getCardsByName(List<Image> handCards, string name, int count)
    {
        List<Image> list = new List<Image>();
        for (int i = 0; i < handCards.Count - 1; i++)
        {
            if (handCards[i].name.Equals(name))
            {
                for (int j = 0; j < count; j++)
                {
                    list.Add(handCards[j]);
                }
            }
        }
        return list;
    }

    /// <summary>
    /// 通过名字加载图片
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private Sprite getSpriteByName(string name)
    {
        return Resources.Load<Sprite>(MJPath.MJBottonSpritePath + name);
    }

    /// <summary>
    /// 卡牌排序
    /// </summary>
    /// <param name="list"></param>
    private void sort(List<Image> list)
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
                    changePos(list,i, j);
                }else if (list[i].GetComponent<MJCard>().cardtype ==list[j].GetComponent<MJCard>().cardtype && int.Parse(list[i].gameObject.name) > int.Parse(list[j].gameObject.name))
                {
                    changePos(list,i, j);
                }
            }
        }
    }

    /// <summary>
    /// 两个图片交换位置
    /// </summary>
    /// <param name="img1"></param>
    /// <param name="img2"></param>
    private void changePos(List<Image> handCards, int index01,int index02)
    {
        //string name = img1.gameObject.name;
        //Sprite sprite = img1.transform.FindChild("Image").GetComponent<Image>().sprite;
        //img1.gameObject.name = img2.gameObject.name;
        //img1.transform.FindChild("Image").GetComponent<Image>().sprite = img2.transform.FindChild("Image").GetComponent<Image>().sprite;
        //img2.gameObject.name = name;
        //img2.transform.FindChild("Image").GetComponent<Image>().sprite = sprite;

        GameObject temp = handCards[index01].gameObject;
        handCards[index01] = handCards[index02];
        handCards[index02] = temp.GetComponent<Image>();
    }

    /// <summary>
    /// 获取插入的位置
    /// </summary>
    /// <param name="handCards"></param>
    /// <param name="card"></param>
    /// <returns></returns>
    private int getInsertIndex(List<Image> handCards, Image card)
    {
        for (int i = 0; i < handCards.Count; i++)
        {
            if (handCards[i] != null)
            {
                if ((int)card.GetComponent<MJCard>().cardtype> (int)handCards[i].GetComponent<MJCard>().cardtype)
                {
                    Debug.Log("插入牌比较" + (int)card.GetComponent<MJCard>().cardtype + "|" + (int)handCards[i].GetComponent<MJCard>().cardtype);
                    return i;
                }else if ((int)card.GetComponent<MJCard>().cardtype ==(int)handCards[i].GetComponent<MJCard>().cardtype)
                {
                    if (int.Parse(card.gameObject.name) < int.Parse(handCards[i].gameObject.name))
                    {
                        Debug.Log("插入牌比较"+card.GetComponent<MJCard>().cardtype + "|" + handCards[i].GetComponent<MJCard>().cardtype+"  名字:"+card.gameObject.name+"|"+handCards[i].gameObject.name);
                        return i;
                    }
                }
                
            }

        }
        
        return handCards.Count ;
    }

    /// <summary>
    /// 出牌
    /// </summary>
    public void bottonOutCard()
    {
        Image outCard = getSelectCard();
        if (bottonHandCards.Contains(outCard))
        { 
            bottonHandCards[bottonHandCards.IndexOf(outCard)]=null;
            bottonAddcardInsertHand();
        }
        bottonOutCardArea.outCard(outCard.gameObject.name);
        AudioController.Instance.playSoundByName(outCard.gameObject.name, MJPlayerManager._instance.getSexByIndex(MJPlayerManager._instance.getMyIndex()));
        GameObject.Find("init").GetComponent<MyMahjongScript>().outCardRequest(int.Parse(outCard.gameObject.name));//出牌请求
        Destroy(outCard.gameObject);
    }

    /// <summary>
    /// 显示出的手牌
    /// </summary>
    /// <param name="name"></param>
    public void bottonShowOutCard(string name)
    {
        bottonOutCardArea.outCard(name);
    }

    /// <summary>
    /// 碰牌
    /// </summary>
    public void bottonPengCard(string name)
    {
        removeSelectCard();
        MJCardAction.Instance.removeCards(bottonHandCards, name,bottonHandCardsPos,2);
        bottonHandCardArea.pengCard(name);
        MJCard.canDrag = false;
        bottonHandCardContent.transform.DOLocalMoveX(bottonHandCardContent.transform.localPosition.x + 2*MJSize.CardSize2D.x,0.5f).OnComplete(bottonAnimCompleteCallBack);

    }

    /// <summary>
    /// 吃牌
    /// </summary>
    /// <param name="one">吃的牌</param>
    /// <param name="two"></param>
    /// <param name="chi">被吃的牌</param>
    /// <param name="min">最小的牌</param>
    private void bottonChiCard(string one,string two,string chi,string min)
    {
        removeSelectCard();
        MJCardAction.Instance.removeCards(bottonHandCards, one, bottonHandCardsPos,1);
        MJCardAction.Instance.removeCards(bottonHandCards, two, bottonHandCardsPos,1);
        bottonHandCardArea.chiCard(min);
        MJCard.canDrag = false;
        bottonHandCardContent.transform.DOLocalMoveX(bottonHandCardContent.transform.localPosition.x + 2* MJSize.CardSize2D.x, 0.5f).OnComplete(bottonAnimCompleteCallBack);
    }

    /// <summary>
    /// 杠牌
    /// </summary>
    /// <param name="name"></param>
    public void bottonGangCard(string name,bool isAn)
    {
        //如果有添加牌
        removeSelectCard();
        if (getBottonAddCard()!=null)
        {
            MJCard.canDrag = false;
            Invoke("bottonAnimCompleteCallBack", 0.6f);
            Destroy(getBottonAddCard().gameObject);
        }
        
        MJCardAction.Instance.removeCards(bottonHandCards, name, bottonHandCardsPos,4);//四代表最大移除四张牌

        //判断以前是否碰过
        if (bottonHandCardArea.isPengAndGang(name))
        {
            return;
        }

        bottonHandCardArea.gangCard(name, isAn);
        MJCard.canDrag = false;
        bottonHandCardContent.transform.DOLocalMoveX(bottonHandCardContent.transform.localPosition.x + 3 * MJSize.CardSize2D.x, 0.5f).OnComplete(bottonAnimCompleteCallBack);
    }

    /// <summary>
    /// 胡牌
    /// </summary>
    /// <param name="names"></param>
    public void bottonHuPai(List<string> names)
    {
        removeSelectCard();
        bottonHandCardArea.huPai(names);
        bottonUIReset();
    }

    /// <summary>
    /// 底部玩家动画完成回调
    /// </summary>
    private void bottonAnimCompleteCallBack()
    {
        MJCard.canDrag = true;
    }

    /// <summary>
    /// 底部玩家重置
    /// </summary>
    private void bottonUIReset()
    {
        removeSelectCard();
        bottonHandCards.Clear();//清除手牌
        MJCardAction.Instance.destroyAllChild(bottonHandCardContent.transform);
        MJCardAction.Instance.destroyAllChild(bottonAddCardArea.transform);
        bottonHandCardContent.transform.localPosition = bottonHandCardInitPos;
        bottonAddCardArea.transform.localPosition = bottonAddCardAreaInitPos;
    }

    private void bottonReset()
    {
        bottonUIReset();
        bottonOutCardArea.reSet();
        bottonHandCardArea.reSet();
    }

    /// <summary>
    /// 底部玩家摧毁一张出的牌，最近打的一张牌开始
    /// </summary>
    private void bottonDestroySigleOutCard()
    {
        bottonOutCardArea.destroyOutCard();
    }

    #region 被选中的手牌
    private Image beSelectCard = null;

    /// <summary>
    /// 设置被选中的卡牌
    /// </summary>
    /// <param name="card"></param>
    public void setSelectCard(Image card)
    {
        if (beSelectCard != null&&beSelectCard!=card)
        {
            //添加的牌和手牌父物体不一样
            if (beSelectCard.gameObject==bottonAddCardGB)
            {
                beSelectCard.transform.SetParent(bottonAddCardArea.transform);
            }
            else
            {
                beSelectCard.transform.SetParent(bottonHandCardContent.transform);
            }

            beSelectCard.transform.localPosition = new Vector3(beSelectCard.transform.localPosition.x, 0, beSelectCard.transform.localPosition.z);
            beSelectCard.transform.FindChild("Select").gameObject.SetActive(false);
            beSelectCard.GetComponent<MJCard>().reset();
        }
        card.transform.localPosition = new Vector3(card.transform.localPosition.x, 30, card.transform.localPosition.z);
        beSelectCard = card;
        beSelectCard.transform.SetParent(bottonCurrentSelectArea);
        beSelectCard.transform.FindChild("Select").gameObject.SetActive(true);
    }

    /// <summary>
    /// 获取被选中的卡牌
    /// </summary>
    /// <returns></returns>
    public Image getSelectCard()
    {
        return beSelectCard;
    }

    /// <summary>
    /// 移除被选中的卡牌
    /// </summary>
    public void removeSelectCard()
    {
        if (beSelectCard != null)
        {
            if (beSelectCard.gameObject == bottonAddCardGB)
            {
                beSelectCard.transform.SetParent(bottonAddCardArea.transform);
            }
            else
            {
                beSelectCard.transform.SetParent(bottonHandCardContent.transform);
            }

            beSelectCard.transform.localPosition = new Vector3(beSelectCard.transform.localPosition.x, 0, beSelectCard.transform.localPosition.z);
            beSelectCard.transform.FindChild("Select").gameObject.SetActive(false);
            beSelectCard.GetComponent<MJCard>().reset();
        }
        beSelectCard = null;
    }
    #endregion


    #endregion

    #region 左边玩家
    private MJOutCardArea leftOutCardArea;//出牌区
    private MJHandCardArea leftHandCardArea;//手牌区

    /// <summary>
    /// 初始化手牌
    /// </summary>
    public void initLeftHandCard()
    {
        leftHandCardArea.createHandCards();
    }

    /// <summary>
    /// 加牌
    /// </summary>
    /// <param name="name"></param>
    private void leftAddCard(string name)
    {
        leftHandCardArea.addCardInPile(name);
    }

    /// <summary>
    /// 出牌
    /// </summary>
    public void leftOutCard(string name)
    {
        leftOutCardArea.outCard(name);
        leftHandCardArea.outCard();
    }

    /// <summary>
    /// 显示出的牌
    /// </summary>
    /// <param name="name"></param>
    public void leftShowOutCard(string name)
    {
        leftOutCardArea.outCard(name);
    }

    /// <summary>
    /// 碰牌
    /// </summary>
    /// <param name="name"></param>
    public void leftPengCard(string name)
    {
        leftHandCardArea.pengCard(name);
    }

    /// <summary>
    /// 杠牌
    /// </summary>
    /// <param name="name"></param>
    public void leftGangCard(string name,bool isAn)
    {
        if (leftHandCardArea.isPengAndGang(name))
        {
            return;
        }

        leftHandCardArea.gangCard(name,isAn);
    }

    /// <summary>
    /// 吃牌
    /// </summary>
    /// <param name="minName"></param>
    private void leftChiCard(string minName)
    {
        leftHandCardArea.chiCard(minName);
    }

    /// <summary>
    /// 胡牌
    /// </summary>
    /// <param name="names"></param>
    public void leftHuPai(List<string> names)
    {
        leftHandCardArea.huPai(names);
    }

    /// <summary>
    /// 左边玩家摧毁一张出的牌，最近打的一张牌开始
    /// </summary>
    private void leftDestroySigleOutCard()
    {
        leftOutCardArea.destroyOutCard();
    }

    private void leftReset()
    {
        leftHandCardArea.reSet();
        leftOutCardArea.reSet();
    }
    #endregion

    #region 右边玩家
    private MJOutCardArea rightOutCardArea;//出牌区
    private MJHandCardArea rightHandCardArea;//手牌区

    /// <summary>
    /// 初始化手牌
    /// </summary>
    private void initRightHandCard()
    {
        rightHandCardArea.createHandCards();
    }

    /// <summary>
    /// 加牌
    /// </summary>
    /// <param name="name"></param>
    private void rightAddCard(string name)
    {
        rightHandCardArea.addCardInPile(name);
    }

    /// <summary>
    /// 出牌
    /// </summary>
    public void rightOutCard(string name)
    {
        rightOutCardArea.outCard(name);
        rightHandCardArea.outCard();
    }

    /// <summary>
    /// 显示出的牌
    /// </summary>
    /// <param name="name"></param>
    public void rightShowOutCard(string name)
    {
        rightOutCardArea.outCard(name);
    }

    /// <summary>
    /// 碰牌
    /// </summary>
    /// <param name="name"></param>
    public void rightPengCard(string name)
    {
        rightHandCardArea.pengCard(name);
    }

    /// <summary>
    /// 吃牌
    /// </summary>
    /// <param name="minName"></param>
    private void rightChiCard(string minName)
    {
        rightHandCardArea.chiCard(minName);
    }

    /// <summary>
    /// 杠牌
    /// </summary>
    /// <param name="name"></param>
    public void rightGangCard(string name,bool isAn)
    {
        if (rightHandCardArea.isPengAndGang(name))
        {
            return;
        }

        rightHandCardArea.gangCard(name,isAn);
    }

    /// <summary>
    /// 胡牌
    /// </summary>
    /// <param name="names"></param>
    public void rightHuPai(List<string> names)
    {
        rightHandCardArea.huPai(names);
    }

    /// <summary>
    /// 右边玩家摧毁一张出的牌，最近打的一张牌开始
    /// </summary>
    private void rightDestroySigleOutCard()
    {
        rightOutCardArea.destroyOutCard();
    }

    private void rightReset()
    {
        rightHandCardArea.reSet();
        rightOutCardArea.reSet();
    }
    #endregion

    #region 顶部玩家
    private MJOutCardArea topOutCardArea;//出牌区
    private MJHandCardArea topHandCardArea;//手牌区

    /// <summary>
    /// 初始化手牌
    /// </summary>
    private void inittopHandCard()
    {
        topHandCardArea.createHandCards();
    }

    /// <summary>
    /// 加牌
    /// </summary>
    /// <param name="name"></param>
    private void topAddCard(string name)
    {
        topHandCardArea.addCardInPile(name);
    }

    /// <summary>
    /// 出牌
    /// </summary>
    private void topOutCard(string name)
    {
        topOutCardArea.outCard(name);
        topHandCardArea.outCard();
    }

    /// <summary>
    /// 显示出的牌
    /// </summary>
    /// <param name="name"></param>
    public void topShowOutCard(string name)
    {
        topOutCardArea.outCard(name);
    }

    /// <summary>
    /// 碰牌
    /// </summary>
    /// <param name="name"></param>
    private void topPengCard(string name)
    {
        topHandCardArea.pengCard(name);
    }

    /// <summary>
    /// 吃牌
    /// </summary>
    /// <param name="minName"></param>
    private void topChiCard(string minName)
    {
        topHandCardArea.chiCard(minName);
    }

    /// <summary>
    /// 杠牌
    /// </summary>
    /// <param name="name"></param>
    private void topGangCard(string name,bool isAn)
    {
        if (topHandCardArea.isPengAndGang(name))
        {
            return;
        }

        topHandCardArea.gangCard(name,isAn);
    }

    /// <summary>
    /// 胡牌
    /// </summary>
    /// <param name="names"></param>
    private void topHuPai(List<string> names)
    {
        topHandCardArea.huPai(names);
    }

    /// <summary>
    /// 顶部玩家摧毁一张出的牌，最近打的一张牌开始
    /// </summary>
    private void topDestroySigleOutCard()
    {
        topOutCardArea.destroyOutCard();
    }

    private void topReset()
    {
        topHandCardArea.reSet();
        topOutCardArea.reSet();
    }
    #endregion

    #region 游戏控制
   
    /// <summary>
    /// 开始游戏
    /// </summary>
    /// <param name="names"></param>
    public void startGame(List<string> names)
    {
        createCards(names);//创建底端牌
        initLeftHandCard();
        initRightHandCard();
        inittopHandCard();
    }

    /// <summary>
    /// 重置
    /// </summary>
    public void reSet()
    {
        bottonReset();
        leftReset();
        rightReset();
        topReset();
    }

    /// <summary>
    /// 添加手牌
    /// </summary>
    public void addCard(string pos,string name)
    {
        switch (pos)
        {
            case DirectionEnum.Bottom:bottonAddCard(name); break;
            case DirectionEnum.Left:leftAddCard(name); break;
            case DirectionEnum.Top:topAddCard(name);break;
            case DirectionEnum.Right:rightAddCard(name); break;
        }
    }

    /// <summary>
    /// 出牌
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="name"></param>
    public void outCard(string pos, string name)
    {
        switch (pos)
        {
            case DirectionEnum.Bottom: Debug.Log("底部玩家出牌") ; break;
            case DirectionEnum.Left: leftOutCard(name); break;
            case DirectionEnum.Top: topOutCard(name); ; break;
            case DirectionEnum.Right:rightOutCard(name); break;
        }
        Debug.Log(pos + "出牌:" + name);
    }

    /// <summary>
    /// 显示出的牌
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="name"></param>
    public void showOutCard(string pos, string name)
    {
        switch (pos)
        {
            case DirectionEnum.Bottom:bottonShowOutCard(name); break;
            case DirectionEnum.Left: leftShowOutCard(name); break;
            case DirectionEnum.Top: topShowOutCard(name); ; break;
            case DirectionEnum.Right: rightShowOutCard(name); break;
        }
        Debug.Log(pos + "出牌:" + name);
    }

    /// <summary>
    /// 碰牌
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="name"></param>
    public void pengCard(string pos,string name)
    {
        switch (pos)
        {
            case DirectionEnum.Bottom: bottonPengCard(name) ; break;
            case DirectionEnum.Left: leftPengCard(name); break;
            case DirectionEnum.Top: topPengCard(name); ; break;
            case DirectionEnum.Right: rightPengCard(name); break;
        }
        Debug.Log(pos + "碰牌:" + name);
    }
    
    /// <summary>
    /// 吃牌
    /// </summary>
    /// <param name="pos">位置</param>
    /// <param name="one"></param>
    /// <param name="two"></param>
    /// <param name="chi">被吃的牌</param>
    public void chiCard(string pos,int  one,int two,int chi)
    {
        int min = 0;
        if (one<two&&one<chi)
        {
            min = one;
        }else if (two<one&&two<chi)
        {
            min = two;
        }
        else
        {
            min = chi;
        }

        switch (pos)
        {
            case DirectionEnum.Bottom: bottonChiCard(one+"",two+"",chi+"",min+""); break;
            case DirectionEnum.Left: leftChiCard(min+""); break;
            case DirectionEnum.Top: topChiCard(min+""); ; break;
            case DirectionEnum.Right: rightChiCard(min+""); break;
        }
        Debug.Log(pos + "吃牌:" + chi);
    }

    /// <summary>
    /// 杠牌
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="name"></param>
    /// <param name="isAn"></param>
    public void gangCard(string pos,string name,bool isAn)
    {
        switch (pos)
        {
            case DirectionEnum.Bottom: bottonGangCard(name,isAn); break;
            case DirectionEnum.Left: leftGangCard(name,isAn); break;
            case DirectionEnum.Top: topGangCard(name,isAn); ; break;
            case DirectionEnum.Right: rightGangCard(name,isAn); break;
        }
        Debug.Log(pos + "杠牌:" + name);
    }

    /// <summary>
    /// 显示其他人手牌
    /// </summary>
    /// <param name="pos"></param>
    public void showOtherHandCard(string pos)
    {
        switch (pos)
        {
            case DirectionEnum.Bottom: ; break;
            case DirectionEnum.Left: initLeftHandCard(); ; break;
            case DirectionEnum.Top: inittopHandCard(); ; break;
            case DirectionEnum.Right: initRightHandCard(); break;
        }
    }

    /// <summary>
    /// 胡牌
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="name"></param>
    public void huCard(string pos,List<string> names )
    {
        switch (pos)
        {
            case DirectionEnum.Bottom:bottonHuPai(names); break;
            case DirectionEnum.Left: leftHuPai(names); ; break;
            case DirectionEnum.Top: topHuPai(names); ; break;
            case DirectionEnum.Right: rightHuPai(names); break;
        }
    }

    /// <summary>
    /// 摧毁一张出手的牌
    /// </summary>
    /// <param name="pos"></param>
    public void destroySigleOutCard(string pos)
    {
        switch (pos)
        {
            case DirectionEnum.Bottom: bottonDestroySigleOutCard(); break;
            case DirectionEnum.Left: leftDestroySigleOutCard(); ; break;
            case DirectionEnum.Top: topDestroySigleOutCard(); ; break;
            case DirectionEnum.Right: rightDestroySigleOutCard(); break;
        }
    }

    /// <summary>
    /// 重新开始
    /// </summary>
    public void replay()
    {
        reSet();
    }
    #endregion

    #region 测试
    public void test()
    {
        //List<string> list = new List<string>();
        //for (int i = 0; i < 13; i++)
        //{
        //    list.Add("" + (int)Random.Range(0, 13));
        //}
        //init();
        //createCards(list);
        //bottonAddCard("0");

        //list.Clear();
        
        topHandCardArea.addCardInPile("19");
    }

    public void test01()
    {
        //bottonGangCard("9");
        //leftGangCard("9");
        //leftOutCard("9");
        //rightGangCard("9");
        //topGangCard("9");

        topOutCard("5");
    }

    public void test02()
    {
        //bottonAddcardInsertHand();
        List<string> names = new List<string>();
        for (int i = 0; i < 10; i++)
        {
            names.Add("" + i);
        }
        bottonHuPai(names);
    }
    #endregion
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
