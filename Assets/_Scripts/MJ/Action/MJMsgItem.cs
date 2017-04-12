using UnityEngine;
using UnityEngine.UI;

public class MJMsgItem : MonoBehaviour {
    public bool isup = true;//是否显示在上面
    private int pos = 1;//

    void Start()
    {
        if (isup)
        {
            pos = 2;
        }
        else
        {
            pos = 1;
        }
    }

    /// <summary>
    /// 显示消息
    /// </summary>
    /// <param name="index"></param>
	public void showMsg(int index)
    {
        Image iTemp = loadImage(index + "_" + pos);
        Vector3 localPos = iTemp.transform.localPosition;
        GameObject temp = Instantiate(iTemp.gameObject);
        temp.transform.SetParent(transform);
        temp.transform.localScale = Vector3.one;
        temp.transform.localPosition = localPos;
        temp.GetComponent<MJMsgAutoDestory>().time = getTime(index);
        temp.GetComponent<MJMsgAutoDestory>().begin();  
    }

    /// <summary>
    /// 加载图片
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private Image loadImage(string name)
    {
        return Resources.Load<Image>(MJPath.MJImagePath +name);
    }

    /// <summary>
    /// 获取消息的显示时间
    /// </summary>
    /// <returns></returns>
    private float getTime(int name)
    {
        int sex = GetComponent<MJPlayerItem>().getPlayer().account.sex;
        string acName = (sex == 1 ? "boy" : "gril");
        AudioClip ac = Resources.Load<AudioClip>(MJPath.MJAudioPath+acName+"/"+name);
        if (ac==null)
        {
            return 3;
        }
        else
        {
            return ac.length;
        }
    }

    /// <summary>
    /// 显示交谈ui
    /// </summary>
    public void showTalkingUI()
    {
        Image iTemp = Resources.Load<Image>(MJPath.MJTalkingUIPath+(isup?"up":"down"));
        Vector3 localPos = iTemp.transform.localPosition;
        GameObject temp = Instantiate(iTemp.gameObject);
        temp.transform.SetParent(transform);
        temp.transform.localScale = Vector3.one;
        temp.transform.localPosition = localPos;
        temp.GetComponent<MJMsgAutoDestory>().time = GlobalDataScript.talkingTime;
        temp.GetComponent<MJMsgAutoDestory>().begin();
    }
}
