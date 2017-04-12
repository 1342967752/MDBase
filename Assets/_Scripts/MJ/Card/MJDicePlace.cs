using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 色子盘管理
/// </summary>
public class MJDicePlace : MonoBehaviour {
    public static MJDicePlace _instance;

    private Animator animator;
    private TextMesh timeText;
    private int time=0;//剩余时间
    private bool isStartTimer = true;
    private GameObject dicePlaceGB;
    private Dictionary<string, string> directionDic;
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
        animator = transform.FindChild("DicePlace/DicePlace").GetComponent<Animator>();
        timeText = transform.FindChild("Time").GetComponent<TextMesh>();
        dicePlaceGB = transform.FindChild("DicePlace").gameObject;
        startTimer();
        animator.SetInteger("change", 0);
    }

    /// <summary>
    /// 设置出牌方向
    /// </summary>
    /// <param name="direction">"E"东 "W"西 "S"南 "N"北</param>
    /// <param name="time">显示时间</param>
    public void setPointer(string direction,int time)
    {
        direction = directionDic[direction];
        switch (direction)
        {
            case "E":animator.SetInteger("change",1);break;
            case "W":animator.SetInteger("change",3); break;
            case "S": animator.SetInteger("change", 2); break;
            case "N": animator.SetInteger("change", 4); break;
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
        switch (direction)
        {
            case DirectionEnum.Bottom:
                directionDic.Add(DirectionEnum.Bottom,"E") ;
                directionDic.Add(DirectionEnum.Left,"N");
                directionDic.Add(DirectionEnum.Top,"W");
                directionDic.Add(DirectionEnum.Right, "S");
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

    }

    /// <summary>
    /// 重置
    /// </summary>
    public void reset()
    {
        animator.SetInteger("change", 0);
    }

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
