using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;
/// <summary>
/// 多种吃牌item
/// </summary>
public class MJChiItem : MonoBehaviour {

    private int cardPoint = 0;//最小牌的点数

    void Awake()
    {
        transform.GetComponent<Button>().onClick.AddListener(btnOnClick);
    }

    /// <summary>
    /// 设置卡片信息
    /// </summary>
    /// <param name="minName"></param>
	public void setCards(string minName)
    {
        transform.FindChild("1/Image").GetComponent<Image>().sprite= Resources.Load<Sprite>(MJPath.MJBottonSpritePath + minName);
        transform.FindChild("2/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(MJPath.MJBottonSpritePath + (int.Parse(minName)+1));
        transform.FindChild("3/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(MJPath.MJBottonSpritePath + (int.Parse(minName) +2));

        cardPoint = int.Parse(minName);
    }

  
    /// <summary>
    /// 按钮事件
    /// </summary>
    private void btnOnClick()
    {
        CardVO cardvo = new CardVO();
        cardvo.cardPoint = GameObject.Find("init").GetComponent<MyMahjongScript>().chiCard.cardPoint;
        if (cardvo.cardPoint==cardPoint)
        {
            cardvo.onePoint = cardPoint + 1;
            cardvo.twoPoint = cardPoint + 2;
        }
        else if(cardPoint+1==cardvo.cardPoint)
        {
            cardvo.onePoint = cardPoint;
            cardvo.twoPoint = cardPoint + 2;
        }
        else
        {
            cardvo.onePoint = cardPoint;
            cardvo.twoPoint = cardPoint + 1;
        }
        GameObject.Find("init").GetComponent<MyMahjongScript>().chiCard = cardvo;

        GameObject.Find("init").GetComponent<MyMahjongScript>().myChiCardBtnClick();
        MJPGHCAction._instance.close();
    }
}
