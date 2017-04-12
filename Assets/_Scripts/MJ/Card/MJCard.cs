using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum CardType:int
{
    None,
    FuJing,
    ZhengJing,  
}

public class MJCard : MonoBehaviour ,IPointerDownHandler,IPointerUpHandler{
    public static bool canDrag = true;//是否可以点击

    public int weight = 0;
    public CardType cardtype = CardType.None;

    private float pressPosY = 0;//记录按下的y值
    private int clickTime = 0;//点击次数

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!canDrag)
        {
            return;
        }
        pressPosY = eventData.position.y;
        MJCardsManager._instance.setSelectCard(gameObject.GetComponent<Image>());
        openSelectAnim();  
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!canDrag)
        {
            return;
        }

        clickTime++;
        
        //是否可以出牌
        if (!GlobalDataScript.isDrag)
        {
            return;
        }

        //点击两次出牌
        if (clickTime >= 2)
        {
            if (GlobalDataScript.isDrag)
            {
                MJCardsManager._instance.bottonOutCard();
                clickTime = 0;
            }
            clickTime = 1;
            return;
        }

        //滑动出牌
        if (eventData.position.y-pressPosY>100)
        {
            MJCardsManager._instance.bottonOutCard();
        }
        else
        {
            
        }
        closeSelectAnim();
        pressPosY = 0;
    }



    //打开选择动画
    private void openSelectAnim()
    {

       
    }

    //关闭选择动画
    private void closeSelectAnim()
    {
      
    }

    public void reset()
    {
        clickTime = 0;
    }
}
