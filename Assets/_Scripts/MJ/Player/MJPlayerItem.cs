using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;

public class MJPlayerItem : MonoBehaviour {
    private Image head;//头像
    private Text scoreText;//分数text
    private int score=0;//分数
    private GameObject readyUI;//准备ui
    private GameObject offLine;//不在线图标
    private GameObject bankerImg;//地主图标

    private int avatarVOIndex;//玩家index
    private AvatarVO avatarVO=null;//玩家信息

    void Awake()
    {
        init();   
    } 

    void init()
    {
        head = transform.FindChild("Head").GetComponent<Image>();
        scoreText = transform.FindChild("Score/Text").GetComponent<Text>();
        readyUI = transform.FindChild("Ready").gameObject;
        offLine = transform.FindChild("OffLine").gameObject;
        bankerImg = transform.FindChild("Head/BankerImg").gameObject;
        head.sprite = null;
        scoreText.text ="";

        //隐藏玩家
        head.transform.parent.gameObject.SetActive(false);

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
        head.transform.parent.gameObject.SetActive(true);
        scoreText.text = "" + avatarVO.scores;
        score = avatarVO.scores;
        this.avatarVO = avatarVO;
        avatarVOIndex = index;
        setReady(avatarVO.isReady);//设置准备
        if (avatarVO.main)
        {
            setBanker(true);
        }
        Debug.Log("玩家性别:" + avatarVO.account.sex);
        StartCoroutine(LoadImg());
    }

    Texture2D texture2D;
    IEnumerator LoadImg()
    {
        //开始下载图片
        if (avatarVO.account.headicon != null && avatarVO.account.headicon != "")
        {
            WWW www = new WWW(avatarVO.account.headicon);
            yield return www;
            //下载完成，保存图片到路径filePath
            try
            {
                texture2D = www.texture;
               // byte[] bytes = texture2D.EncodeToPNG();
                //将图片赋给场景上的Sprite
                Sprite tempSp = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0));
                head.sprite = tempSp;
            }
            catch (System.Exception e)
            {
                WantedTextTool.Instance.addTip("LoadImg" + e.Message, 0);
            }
        }
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
        }
        else
        {
            bankerImg.SetActive(false);
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
        avatarVOIndex = 0;
        avatarVO = null;
        head.transform.parent.gameObject.SetActive(false);
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
}
