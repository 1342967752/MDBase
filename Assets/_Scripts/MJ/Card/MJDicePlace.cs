using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 色子盘管理
/// </summary>
public class MJDicePlace : MonoBehaviour {
    public static MJDicePlace _instance;

    private Animator dirAnimator;//方向anim
    private Animator shaiZiAnimator;//色子动画
    private GameObject shaiZi;//色子
    private TextMesh timeText;
    private int time=0;//剩余时间
    private bool isStartTimer = true;
    private GameObject dicePlaceGB;
    private Dictionary<string, string> directionDic;

    private int dianShu = 0;//色子点数
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
        directionDic = new Dictionary<string, string>();
        dirAnimator = transform.FindChild("DicePlace/DicePlace").GetComponent<Animator>();
        shaiZi = transform.FindChild("Shaizi").gameObject;
        shaiZiAnimator = shaiZi.GetComponent<Animator>();
        timeText = transform.FindChild("Time").GetComponent<TextMesh>();
        dicePlaceGB = transform.FindChild("DicePlace").gameObject;
        startTimer();
        shaiZiAnimator.SetInteger("dianshu", 0);
        dirAnimator.SetInteger("change", 0);
        shaiZi.SetActive(false);
        timeText.gameObject.SetActive(false);
    }

    /// <summary>
    /// 设置出牌方向
    /// </summary>
    /// <param name="direction">上下左右</param>
    /// <param name="time">显示时间</param>
    public void setPointer(string direction,int time)
    {
        if (!timeText.gameObject.activeInHierarchy)
        {
            timeText.gameObject.SetActive(true);
        }

        if (directionDic.ContainsKey(direction))
        {
            direction = directionDic[direction];
        }

        switch (direction)
        {
            case "E": dirAnimator.SetInteger("change",1);break;
            case "W": dirAnimator.SetInteger("change",3); break;
            case "S": dirAnimator.SetInteger("change", 2); break;
            case "N": dirAnimator.SetInteger("change", 4); break;
            case "C": dirAnimator.SetInteger("change", 0); break;//关闭
            default:Debug.Log("没有该方向->" + direction);break;
        }
        this.time = time;
    }

    /// <summary>
    /// 设置我的朝向
    /// </summary>
    /// <param name="direction">上下左右</param>
    public void setMyDirection(string direction)
    {
        directionDic.Clear();
        switch (direction)
        {
            case DirectionEnum.Bottom:
                directionDic.Add(DirectionEnum.Bottom,"E") ;
                directionDic.Add(DirectionEnum.Left,"N");
                directionDic.Add(DirectionEnum.Top,"W");
                directionDic.Add(DirectionEnum.Right, "S");
                dicePlaceGB.transform.localEulerAngles = Vector3.zero;
                break;
            case DirectionEnum.Left: dicePlaceGB.transform.localEulerAngles = Vector3.up * -90;
                directionDic.Add(DirectionEnum.Bottom, "N");
                directionDic.Add(DirectionEnum.Left, "W");
                directionDic.Add(DirectionEnum.Top, "S");
                directionDic.Add(DirectionEnum.Right, "E");
                break;
            case DirectionEnum.Top: dicePlaceGB.transform.localEulerAngles = Vector3.up *180;
                directionDic.Add(DirectionEnum.Bottom, "W");
                directionDic.Add(DirectionEnum.Left, "S");
                directionDic.Add(DirectionEnum.Top, "E");
                directionDic.Add(DirectionEnum.Right, "N");
                break;
            case DirectionEnum.Right: dicePlaceGB.transform.localEulerAngles = Vector3.up * 90;
                directionDic.Add(DirectionEnum.Bottom, "S");
                directionDic.Add(DirectionEnum.Left, "E");
                directionDic.Add(DirectionEnum.Top, "N");
                directionDic.Add(DirectionEnum.Right, "W"); break;
            default: Debug.Log("没有该方向->" + direction); break;
        }
    }

    /// <summary>
    /// 计时器
    /// </summary>
    /// <returns></returns>
    IEnumerator timerIE()
    {
        while (isStartTimer)
        {
            if (time<10)
            {
                timeText.text = "0" + time;
            }
            else
            {
                timeText.text = ""+time;
            }

            yield return new WaitForSeconds(1);
            if (time>0)
            {
                time--;
            }
        }

        timeText.text = "0" + 0;
    }

    /// <summary>
    /// 关闭计时
    /// </summary>
    public void closeTimer()
    {
        isStartTimer = false;
    }

    /// <summary>
    /// 启动计时器
    /// </summary>
    public void startTimer()
    {
        isStartTimer = true;
        StartCoroutine(timerIE());
    }


    /// <summary>
    /// 设置色子点数
    /// </summary>
    /// <param name="one"></param>
    /// <param name="two"></param>
    public void setShaiZi(int one,int two)
    {
        Debug.Log("one:" + one + "|" + "two:" + two);
        dianShu = one + two;
    }

    /// <summary>
    /// 启动色子动画
    /// </summary>
    public void startMoveShaiZi()
    {
        shaiZi.SetActive(true);
        Debug.Log("点数:" + dianShu);
        shaiZiAnimator.SetInteger("dianshu",dianShu);
    }

    /// <summary>
    /// 隐藏色子
    /// </summary>
    public void hintShaiZi()
    {
        shaiZiAnimator.SetInteger("dianshu", 0);
        shaiZi.SetActive(false);
    }

    /// <summary>
    /// 重置
    /// </summary>
    public void reset()
    {
        dirAnimator.SetInteger("change", 0);
        shaiZiAnimator.SetInteger("dainshu", 0);
        shaiZi.SetActive(false);
        timeText.gameObject.SetActive(false);
    }

    /// <summary>
    /// 初始化时间
    /// </summary>
    public void initTimer()
    {
        timeText.text = "00";
    }

    void OnDestroy()
    {
        if (_instance != null)
        {
            Destroy(_instance);
            _instance = null;
        }
    }
}
