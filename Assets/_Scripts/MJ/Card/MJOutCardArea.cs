using UnityEngine;

/// <summary>
/// 出牌区管理
/// </summary>
public class MJOutCardArea : MonoBehaviour {

    public Vector2 cardSize = new Vector2(0.015f, 0.02f);//牌的大小
    private Vector3[] cardsPos = new Vector3[30];//存储位置的数组
    private int pointer = 0;//
    private string pos = "";

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
        GameObject.Find("init").GetComponent<MyMahjongScript>().outCardInfo = new MJOutCardInfo();
        GameObject.Find("init").GetComponent<MyMahjongScript>().outCardInfo.setOutInfo(MJCardAction.Instance.createCard(name, transform, cardsPos[pointer++], Vector3.one, MJPostion.outCardEulerAngles)
            ,pos);
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
    /// 摧毁出手的牌
    /// </summary>
    public void destroyOutCard()
    {
        Destroy(transform.GetChild(transform.childCount - 1).gameObject);
        pointer--;
    }
}
