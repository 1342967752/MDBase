using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class WantedWindowPage : TTUIPage {

    private Text midBtnText;//按钮上显示文字
    private Text leftBtnText;
    private Text rightBtnText;

    private Text info;//提醒内容

    private UnityAction midUA;//点击按钮跳转事件
    private UnityAction leftUA;
    private UnityAction rightUA;

    private GameObject bg;//背景

	public WantedWindowPage() : base(UIType.PopUp, UIMode.HideOther, UICollider.None)
    {
        uiPath = MJPath.WantedWindowPath;
    }

    public override void Awake(GameObject go)
    {
        ToolsManager.Instance.wantedWindowPage=this;

        midBtnText = transform.FindChild("Bg/Mid/Text").GetComponent<Text>();
        leftBtnText= transform.FindChild("Bg/Left/Text").GetComponent<Text>();
        rightBtnText= transform.FindChild("Bg/Right/Text").GetComponent<Text>();
        info= transform.FindChild("Bg/Text").GetComponent<Text>();

        bg = transform.FindChild("Bg").gameObject;

        transform.FindChild("Bg/Mid").GetComponent<Button>().onClick.AddListener(midFun);
        transform.FindChild("Bg/Left").GetComponent<Button>().onClick.AddListener(leftFun);
        transform.FindChild("Bg/Right").GetComponent<Button>().onClick.AddListener(rightFun);
    }

    public override void Refresh()
    {
        gameObject.SetActive(false);
    }

    private void midFun()
    {
        if (midUA!=null)
        {
            midUA();
        }
        ClosePage<WantedWindowPage>();
    }

    private void leftFun()
    {
        if (leftUA!=null)
        {
            leftUA();
        }
        ClosePage<WantedWindowPage>();
    }

    private void rightFun()
    {
        if (rightUA!=null)
        {
            rightUA();
        }
        ClosePage<WantedWindowPage>();
    }

    /// <summary>
    /// 显示一个按钮的窗口
    /// </summary>
    /// <param name="info">显示内容</param>
    /// <param name="midBtnText">中间按钮上显示文字</param>
    /// <param name="midUA">点击按钮跳转事件</param>
    public void showWantedWindow(string info,string midBtnText,UnityAction midUA)
    {
        this.info .text= info;
        this.midBtnText.text = midBtnText;
        this.midUA = midUA;

        this.midBtnText.transform.parent.gameObject.SetActive(true);
        rightBtnText.transform.parent.gameObject.SetActive(false);
        leftBtnText.transform.parent.gameObject.SetActive(false);
        gameObject.SetActive(true);
        anim();
    }

    /// <summary>
    /// 显示两个按钮的窗体
    /// </summary>
    /// <param name="info">显示内容</param>
    /// <param name="leftBtnText">左边按钮显示文字</param>
    /// <param name="leftUA">左边按钮按下跳转事件</param>
    /// <param name="rightBtnText">右边按钮显示文字</param>
    /// <param name="rightUA">右边按钮按下跳转事件</param>
    public void showWantedWindow(string info, string leftBtnText, UnityAction leftUA,string rightBtnText,UnityAction rightUA)
    {
        this.info.text = info;
        this.leftBtnText.text = leftBtnText;
        this.rightBtnText.text = rightBtnText;
        this.leftUA = leftUA;
        this.rightUA = rightUA;

        midBtnText.transform.parent.gameObject.SetActive(false);
        this.rightBtnText.transform.parent.gameObject.SetActive(true);
        this.leftBtnText.transform.parent.gameObject.SetActive(true);
        gameObject.SetActive(true);
        anim();
    }

    /// <summary>
    /// 动画
    /// </summary>
    private void anim()
    {
        bg.transform.localScale = Vector2.one * 0.8f;
        bg.transform.DOScale(1, 0.5f).SetEase(Ease.OutElastic);
    }
}
