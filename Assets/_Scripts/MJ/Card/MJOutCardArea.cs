using UnityEngine;

/// <summary>
/// 出牌区管理
/// </summary>
public class MJOutCardArea : MonoBehaviour {

    public Vector2 cardSize = new Vector2(0.015f, 0.02f);//牌的大小
    private Vector3[] cardsPos = new Vector3[30];//存储位置的数组
    private int pointer = 0;//
    private string pos = "";
    private MaJiangRecordScript maJiangRecordScript;//麻将回放主脚本
    private MyMahjongScript myMahjongScript;//麻将游戏主脚本
    private bool isRecord = false;//是否战绩回放

    void Awake()
    {
        init();
    }
    /// <summary>
    /// 初始化
    /// </summary>
    private void init()
    {
        switch (gameObject.name)
        {
            case "Botton":pos = DirectionEnum.Bottom;break;
            case "Left":pos = DirectionEnum.Left;break;
            case "Right":pos = DirectionEnum.Right;break;
            case "Top":pos = DirectionEnum.Top;break;
        }
        createPostion();

        //给主脚本赋值
        if (MJScenesManager.Instance.curSceneName().Equals(SceneName.MaJiang))
        {
            myMahjongScript = GameObject.Find("init").GetComponent<MyMahjongScript>();
            isRecord = false;
        }
        else if (MJScenesManager.Instance.curSceneName().Equals(SceneName.MaJiangRecord))
        {
            maJiangRecordScript= GameObject.Find("init").GetComponent<MaJiangRecordScript>();
            isRecord = true;
        }
    }

    /// <summary>
    /// 创建3d位置
    /// </summary>
    private void createPostion()
    {
        MJCardAction.Instance.createPostion(cardsPos, Vector2.zero, cardSize,6); 
    }

    /// <summary>
    /// 出牌
    /// </summary>
    /// <param name="name"></param>
    public void outCard(string name)
    {
       if (!isRecord)
       {
            myMahjongScript.outCardInfo = new MJOutCardInfo();
            myMahjongScript.outCardInfo.setOutInfo(MJCardAction.Instance.createCard(name, transform, cardsPos[pointer++], Vector3.one, MyPostion.outCardEulerAngles)
                , pos);
            MJCardsManager._instance.setZhuiZi(myMahjongScript.outCardInfo.outCardGB.transform);
        }
        else
        {
            maJiangRecordScript.outCardInfo = new MJOutCardInfo();
            maJiangRecordScript.outCardInfo.setOutInfo(MJCardAction.Instance.createCard(name, transform, cardsPos[pointer++], Vector3.one, MyPostion.outCardEulerAngles)
                , pos);
            MJCardsManager._instance.setZhuiZi(maJiangRecordScript.outCardInfo.outCardGB.transform);
        }
        Debug.Log("出牌:" + name);
    }

    /// <summary>
    /// 重置
    /// </summary>
    public void reSet()
    {
        MJCardAction.Instance.destroyAllChild(transform);
        pointer = 0;
    }

    /// <summary>
    /// 获取最后一张牌的信息
    /// </summary>
    /// <returns></returns>
    public MJOutCardInfo getOutCardInfo()
    {
        if (transform.childCount==0)
        {
            return null;
        }
        GameObject card = transform.GetChild(transform.childCount - 1).gameObject;

        MJOutCardInfo outInfo = new MJOutCardInfo();
        outInfo.setOutInfo(card, pos);

        MJCardsManager._instance.setZhuiZi(card.transform);//设置出牌的锥子
        return outInfo;
    }

    /// <summary>
    /// 摧毁出手的牌
    /// </summary>
    public void destroyOutCard()
    {
        Destroy(transform.GetChild(transform.childCount - 1).gameObject);
        pointer--;
    }
}
