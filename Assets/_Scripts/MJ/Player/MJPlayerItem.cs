using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using DG.Tweening;

public class MJPlayerItem : MonoBehaviour {
    private Image head;//头像
    private Text scoreText;//分数text
    private int score=0;//分数
    private GameObject readyUI;//准备ui
    private GameObject offLine;//不在线图标
    private GameObject bankerImg;//地主图标
    private Text changeScore;//改变分数text
    private MJEmojiBox mJEmojiBox;//表情处理器
    private GameObject huIcon;//胡的图标

    public int avatarVOIndex=-1;//玩家index 
    private AvatarVO avatarVO=null;//玩家信息

    void Awake()
    {
        init();   
    } 

    void init()
    {
        head = transform.FindChild("Head/Mask/HeadImg").GetComponent<Image>();
        scoreText = transform.FindChild("Head/Score/Text").GetComponent<Text>();
        changeScore = transform.FindChild("ChangeScore").GetComponent<Text>();
        readyUI = transform.FindChild("Ready").gameObject;
        offLine = transform.FindChild("OffLine").gameObject;
        bankerImg = transform.FindChild("Head/BankerImg").gameObject;
        head.sprite = null;
        scoreText.text ="";
        changeScore.gameObject.SetActive(false);
        huIcon = transform.FindChild("Head/Hu").gameObject;
        setHu(false);

        //隐藏玩家
        head.transform.parent.parent.gameObject.SetActive(false);
        head.GetComponent<Button>().onClick.AddListener(() => {
            if (GlobalDataScript.isRecord)
            {
                return;
            }

            if (avatarVO!=null)
            {
                MJUIManager._instance.mJMyInfoPage.setInfo(head, avatarVO.account.nickname, avatarVO.account.id + "", avatarVO.locationName,avatarVO.account.uuid,avatarVO.winTimes,avatarVO.loseTimes,avatarVO.drawTimes);
            }
        });

        mJEmojiBox = GetComponent<MJEmojiBox>();

        setReady(false);
        isOnLine(true);
        setBanker(false);
    }

    /// <summary>
    /// 设置信息
    /// </summary>
    /// <param name="headUrl">头像路径</param>
    /// <param name="score">分数</param>
    public void setInfo(AvatarVO avatarVO,int index)
    {
        head.transform.parent.parent.gameObject.SetActive(true);
        scoreText.text = "" + avatarVO.scores;
        score = avatarVO.scores;
        this.avatarVO = avatarVO;
        avatarVOIndex = index;
        setReady(avatarVO.isReady);//设置准备
       
        setBanker(avatarVO.main);
        isOnLine(avatarVO.isOnLine);
        Debug.Log(avatarVO.account.nickname+"性别:" + avatarVO.account.sex+"|分数:"+avatarVO.scores);
        Debug.Log("胜利次数:"+avatarVO.winTimes);
        GameTools.instance.loadSpriteOnNet(loadSpriteCallBack, avatarVO.account.headicon);
    }

    /// <summary>
    /// 获取玩家index
    /// </summary>
    /// <returns></returns>
    public int getIndex()
    {
        return avatarVOIndex;
    }

    /// <summary>
    /// 获取名字
    /// </summary>
    /// <returns></returns>
    public string getName()
    {
        if (avatarVO==null)
        {
            return null;
        }
        else
        {
            return avatarVO.account.nickname;
        }
       
    }

    /// <summary>
    /// 设置准备
    /// </summary>
    /// <param name="isReady"></param>
    public void setReady(bool isReady)
    {
        readyUI.gameObject.SetActive(isReady);
    }
    
    /// <summary>
    /// 设置庄家
    /// </summary>
    public void setBanker(bool isTrue)
    {
       if (isTrue)
       {
            bankerImg.SetActive(true);
            if (avatarVO!=null)
            {
                avatarVO.main = true;
            }
           
        }
        else
        {
            bankerImg.SetActive(false);
            if (avatarVO != null)
            {
                avatarVO.main = false;
            }
        }
    }

    /// <summary>
    /// 获取玩家
    /// </summary>
    /// <returns></returns>
    public AvatarVO getPlayer()
    {
        return avatarVO;
    }

    /// <summary>
    /// 获取trasnfrom
    /// </summary>
    /// <returns></returns>
    public Transform getTransfrom()
    {
        return transform;
    }

    /// <summary>
    /// 获取用户头像
    /// </summary>
    /// <returns></returns>
    public Sprite getHeadSprite()
    {
        return head.sprite;
    }

    /// <summary>
    /// 设置玩家分数
    /// </summary>
    /// <param name="chnageScore"></param>
    public void setPlayerScore(int chnageScore)
    {
        score += chnageScore;
        if (scoreText == null)
        {
            return;
        }
        scoreText.text = "" + score;
    }

    /// <summary>
    /// 重置玩家
    /// </summary>
    public void reSet()
    {
        head.sprite = null;
        setReady(false);
        scoreText.text = "";
        avatarVOIndex = -1;
        avatarVO = null;
        head.transform.parent.parent.gameObject.SetActive(false);
        setHu(false);
    }

    /// <summary>
    /// 用户是否在线
    /// </summary>
    /// <param name="onLine"></param>
    public void isOnLine(bool onLine)
    {
        if (onLine)
        {
            head.transform.FindChild("Hide").gameObject.SetActive(false);
            offLine.SetActive(false);
        }
        else
        {
            head.transform.FindChild("Hide").gameObject.SetActive(true);
            offLine.SetActive(true);
        }
    }

    /// <summary>
    /// 显示改变分数
    /// </summary>
    /// <param name="score"></param>
    /// <param name="time"></param>
    public void showScore(int score,float time)
    {
        if (time<0)
        {
            return;
        }

        if (score>=0)
        {
            changeScore.font = Resources.Load<Font>(MyPath.UIFontsPath + "duiju_up_font");
            changeScore.text ="+"+score;
        }
        else
        {
            changeScore.font = Resources.Load<Font>(MyPath.UIFontsPath + "duiju_down_font");
            changeScore.text =score+"";
        }
        Debug.Log("显示分数:" + score);
        changeScore.gameObject.SetActive(true);
        changeScore.transform.DOPunchScale(Vector3.one*0.2f, time).SetUpdate(true).OnComplete(changeScoreAnimOnComplete);
    }

    /// <summary>
    /// 改变分数动画结束回调
    /// </summary>
    private void changeScoreAnimOnComplete()
    {
        changeScore.gameObject.SetActive(false);
    }

    /// <summary>
    /// 显示表情
    /// </summary>
    /// <param name="emojiIndex"></param>
    public void showEmoji(int emojiIndex)
    {
        mJEmojiBox.addEmoji(emojiIndex);
    }

    /// <summary>
    /// 加载图片回调
    /// </summary>
    /// <param name="o"></param>
    private void loadSpriteCallBack(HttpLoadModel model)
    {
        if (model.error!=null)
        {
            return;
        }
        head.sprite = model.sprite;
    }

    /// <summary>
    /// 设置胡
    /// </summary>
    /// <param name="isHu"></param>
    public void setHu(bool isHu)
    {
        huIcon.SetActive(isHu);
    }
}
