using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MJMsgItem : MonoBehaviour {
    public bool isup = true;//是否显示在上面
    private int pos = 1;//
    private AudioSource chatAudioSource;//语音播放
    private GameObject talkTemp;//交谈ui
    private List<float[]> talks = new List<float[]>();//播放的语音资源
    private bool isCanTalk = true;

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

        chatAudioSource = gameObject.AddComponent<AudioSource>();
        
    }

    void FixedUpdate()
    {
        if (isCanTalk&&talks.Count>0)
        {
            playChatAudio(talks[0]);
            talks.RemoveAt(0);
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
        return Resources.Load<Image>(MyPath.MJImagePath +name);
    }

    /// <summary>
    /// 获取消息的显示时间
    /// </summary>
    /// <returns></returns>
    private float getTime(int name)
    {
        int sex = GetComponent<MJPlayerItem>().getPlayer().account.sex;
        string acName = (sex == 1 ? "boy" : "gril");
        AudioClip ac = Resources.Load<AudioClip>(MyPath.MJAudioPath+acName+"/"+name);
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
    private void showTalkingUI()
    {
        if (talkTemp!=null)
        {
            Destroy(talkTemp);
        }

        Image iTemp = Resources.Load<Image>(MyPath.MJTalkingUIPath+(isup?"up":"down"));
        Vector3 localPos = iTemp.transform.localPosition;
        GameObject temp = Instantiate(iTemp.gameObject);
        temp.transform.SetParent(transform);
        temp.transform.localScale = Vector3.one;
        temp.transform.localPosition = localPos;

        talkTemp = temp;
    }

    /// <summary>
    /// 添加语音资源
    /// </summary>
    /// <param name="samples"></param>
    public void addTalking(float[] samples)
    {
        talks.Add(samples);
    }

    /// <summary>
    /// 播放语音
    /// </summary>
    private void playChatAudio(float[] samples)
    {
        chatAudioSource.clip = AudioClip.Create("playRecordClip", samples.Length * 2, 1, 8000, false, false);

        chatAudioSource.clip.SetData(samples, 0);
        chatAudioSource.mute = false;
        chatAudioSource.volume = 1;
        chatAudioSource.Play();

        AudioController.Instance.setBGMVolume(0.1f);
        AudioController.Instance.setSoundVolume(0.1f);
        isCanTalk = false;
        showTalkingUI();
        StopCoroutine(talkingListenner());
        StartCoroutine(talkingListenner());
    }

    /// <summary>
    /// 停止播放语音
    /// </summary>
    private void stopChatAudio()
    {
        chatAudioSource.Stop();
        AudioController.Instance.setBGMVolume(1);
        AudioController.Instance.setSoundVolume(1);
        if (talkTemp!=null)
        {
            Destroy(talkTemp);
        }
        isCanTalk = true;
    }

    /// <summary>
    /// 监听语音
    /// </summary>
    /// <returns></returns>
    IEnumerator talkingListenner()
    {
        Debug.Log("语音监听启动");
        while (true)
        {
            if (!chatAudioSource.isPlaying)
            {
                stopChatAudio();
                Debug.Log("语音播放完毕");
                break;
            }
            else
            {
                AudioController.Instance.setBGMVolume(0.1f);
                AudioController.Instance.setSoundVolume(0.1f);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
