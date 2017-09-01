using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;
using System.Collections.Generic;
/// <summary>
/// 消息处理
/// </summary>
public class MJMsgBox : MonoBehaviour {
    private GameObject msgPanel;
    private GameObject emojiPanel;
    private GameObject emojiContent;
    private GameObject msgBar;
    private GameObject emojiBar;
    private bool isOpen = false;//是否打开消息框
    private Animator msgAnimator;
    private bool isCanUse = true;//是否可用

    void Awake()
    {
        init();
    }

    private void init()
    {
        msgPanel = transform.FindChild("MsgBox/msg").gameObject;
        msgBar = transform.FindChild("MsgBox/MsgBar").gameObject;
        emojiPanel = transform.FindChild("MsgBox/emoji").gameObject;
        emojiBar = transform.FindChild("MsgBox/EmojiBar").gameObject;
        msgAnimator = transform.FindChild("MsgBox").GetComponent<Animator>();
        emojiContent = transform.FindChild("MsgBox/emoji/Viewport/Content").gameObject;

        emojiPanel.SetActive(false);
        msgPanel.SetActive(true);
        loadEmoji();

        isCanUse = GlobalDataScript.getMsgCanUse();//判断是否可用
    }

    /// <summary>
    /// 加载emoji
    /// </summary>
    private void loadEmoji()
    {
        Debug.Log("初始化表情");
        List<Sprite> sprites = new List<Sprite>();

        int count = 0;
        Vector2 size = emojiContent.GetComponent<GridLayoutGroup>().cellSize;
        while (true)
        {
            Sprite temp = Resources.Load<Sprite>(MyPath.MJFaceEmojiPath + count);
            if (temp!=null)
            {
                sprites.Add(temp);
            }
            else
            {
                break;
            }
            count++;
        }

        if (sprites==null||sprites.Count==0)
        {
            Debug.Log("文件夹没有表情:"+MyPath.MJFaceEmojiPath);
            return;
        }

        for (int i=0; i<sprites.Count; i++)
        {
            Sprite[] spriteChild = Resources.LoadAll<Sprite>(MyPath.MJFaceEmojiPath + i );
            if (spriteChild.Length==0)
            {
                continue;
            }

            GameObject temp = new GameObject("emoji"+i);
            temp.transform.SetParent(emojiContent.transform);
            temp.transform.localScale = Vector3.one*0.5f;
            temp.AddComponent<Image>();
            temp.GetComponent<Image>().sprite = spriteChild[0];
            temp.AddComponent<MJEmojiItem>();
            temp.GetComponent<MJEmojiItem>().emojiIndex = i;
            temp.GetComponent<MJEmojiItem>().btnUA = null;
            temp.GetComponent<MJEmojiItem>().btnUA = close;
        }

        emojiContent.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(emojiContent.GetComponent<Image>().rectTransform.sizeDelta.x, (((count - 1) / 4)+1) * (size.y + 5));
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="index"></param>
	public void sendMsg(int index)
    {
        CustomSocket.getInstance().sendMsg(new MessageBoxRequest(MesssageType.Msg,index, MJPlayerManager._instance.getMyUUID()));
        MJPlayerManager._instance.showMsgByIndex(MJPlayerManager._instance.getMyIndex(), index);
        AudioController.Instance.playSoundByName(index + "", MJPlayerManager._instance.getMySex());
        close();
    }

    /// <summary>
    /// 显示msg
    /// </summary>
    public void showMsg()
    {
        emojiBar.GetComponent<Image>().sprite = loadSprite("emoji_no_bar");
        emojiBar.transform.FindChild("emoji").GetComponent<Image>().sprite = loadSprite("emoji_no");

        msgBar.GetComponent<Image>().sprite = loadSprite("msg_yes_bar");
        msgBar.transform.FindChild("msg").GetComponent<Image>().sprite = loadSprite("msg_yes");

        emojiPanel.SetActive(false);
        msgPanel.SetActive(true);
    }

    /// <summary>
    /// 显示emoji
    /// </summary>
    public void showEmoji()
    {
        emojiBar.GetComponent<Image>().sprite = loadSprite("emoji_yes_bar");
        emojiBar.transform.FindChild("emoji").GetComponent<Image>().sprite = loadSprite("emoji_yes");

        msgBar.GetComponent<Image>().sprite = loadSprite("msg_no_bar");
        msgBar.transform.FindChild("msg").GetComponent<Image>().sprite = loadSprite("msg_no");

        emojiPanel.SetActive(true);
        msgPanel.SetActive(false);
    }

    /// <summary>
    /// 加载图片
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private Sprite loadSprite(string name)
    {
        return Resources.Load<Sprite>(MyPath.MJSpritePath + name);
    }

    #region 关闭或者打开消息框
    public void openOrCloseMsgBox()
    {
        if (!isCanUse)
        {
            return;
        }

        if (isOpen)
        {
            close();
        }
        else
        {
            open();
        }
    }

    private void close()
    {
        isOpen = false;
        msgAnimator.SetInteger("isopen", 2);
        showMsg();
    }

    private void open()
    {
        msgAnimator.SetInteger("isopen", 1);
        isOpen = true;
    }
    #endregion

}
