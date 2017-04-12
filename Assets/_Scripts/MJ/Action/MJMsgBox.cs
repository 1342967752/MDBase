using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;
/// <summary>
/// 消息处理
/// </summary>
public class MJMsgBox : MonoBehaviour {
    private GameObject msgPanel;
    private GameObject emojiPanel;
    private GameObject msgBar;
    private GameObject emojiBar;
    private bool isOpen = false;//是否打开消息框
    private Animator msgAnimator;

    void Awake()
    {
        init();
    }

    void init()
    {
        msgPanel = transform.FindChild("MsgBox/msg").gameObject;
        msgBar = transform.FindChild("MsgBox/MsgBar").gameObject;
        emojiPanel = transform.FindChild("MsgBox/emoji").gameObject;
        emojiBar = transform.FindChild("MsgBox/EmojiBar").gameObject;
        msgAnimator = transform.FindChild("MsgBox").GetComponent<Animator>();
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="index"></param>
	public void sendMsg(int index)
    {
        CustomSocket.getInstance().sendMsg(new MessageBoxRequest(index, MJPlayerManager._instance.getMyUUID()));
        MJPlayerManager._instance.showMsgByIndex(MJPlayerManager._instance.getMyIndex(), index);
        AudioController.Instance.playSoundByName(index + "", MJPlayerManager._instance.getMyIndex());
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
        return Resources.Load<Sprite>(MJPath.MJSpritePath + name);
    }

    #region 关闭或者打开消息框
    public void openOrCloseMsgBox()
    {
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
    }

    private void open()
    {
        msgAnimator.SetInteger("isopen", 1);
        isOpen = true;
    }
    #endregion

}
