using UnityEngine;

/// <summary>
/// Audio Controller.音效管理器
/// </summary>
/**背景音乐状态：正常，静音，暂停**/
public enum BGMState:int{
	normal,//正常状态
	mute,//静音状态
	pause//暂停状态
}
/**背景音乐状态：正常，静音**/
public enum SoundState:int
{
	normal,//正常状态
	mute,//静音状态
}
public class AudioController : MonoBehaviour {
	private static AudioController _instance;
	public static AudioController Instance{
		get{
			if (_instance == null) {
				GameObject audioObj = new GameObject("AudioSource");
				audioObj.AddComponent<AudioSource>();
				_instance = audioObj.AddComponent<AudioController>();
			}
			return _instance;
		}
    }
	private AudioSource BGMAudioSource;
	private AudioSource SoundAudioSource;
	public BGMState bgmState;
	public SoundState soundState;

	void Awake(){
		BGMAudioSource = this.gameObject.AddComponent<AudioSource>();
		BGMAudioSource.loop = true;
		SoundAudioSource = this.gameObject.AddComponent<AudioSource>();
		bgmState = (BGMState)DateManger.Instance.getBGMState();
		soundState = (SoundState)DateManger.Instance.getSoundState();
		setBGMVolume(DateManger.Instance.getBGMVolume());
		setSoundVolume(DateManger.Instance.getSoundVolume());
	}
	/*********设置背景音乐音量**********/
	public void setBGMVolume(float volume){
		BGMAudioSource.volume = volume;
		DateManger.Instance.setBGMVolume(volume);
	}

	public float getBGMVolume(){
		return BGMAudioSource.volume;
	}

	/*********设置音效音量**********/
	public void setSoundVolume(float volume){
		SoundAudioSource.volume = volume;
		DateManger.Instance.setSoundVolume(volume);
	}
	public float getSoundVolume(){
		return SoundAudioSource.volume;
	}

	/****设置全局静音:true就静音，false就解除静音*****/
	public void setMute(bool isMute){
		if(isMute){
			changeBGMState(BGMState.mute);
			changeSoundState(SoundState.mute);
		}else{
			changeBGMState(BGMState.normal);
			changeSoundState(SoundState.normal);
		}
	}

	public void setBGMMute(bool isMute){
		if(isMute){
			changeBGMState(BGMState.mute);
		}else{
			changeBGMState(BGMState.normal);
		}
	}

	public void setSoundMute(bool isMute){
		if(isMute){
			changeSoundState(SoundState.mute);
		}else{
			changeSoundState(SoundState.normal);
		}
	}

	/****背景音乐状态控制*********/
	public void setBGMPause(bool isPause){
		if(isPause){
			changeBGMState(BGMState.pause);
		}else{
			changeBGMState(BGMState.normal);
		}
	}

	/********改变音频状态:辅助函数不用管********/
	public void changeBGMState(BGMState state)
	{
        DateManger.Instance.setBGMState((int)state);
        switch (state) {
			case BGMState.normal:
				BGMAudioSource.volume = 1.0f;
				BGMAudioSource.UnPause();
				break;
			case BGMState.mute:
				BGMAudioSource.mute = true;
				BGMAudioSource.Pause();
				break;
			case BGMState.pause:
				BGMAudioSource.Pause();
				break;
		}
		//设置最新的状态
		this.bgmState = state;
	}

	public void changeSoundState(SoundState state)
	{
        DateManger.Instance.setSoundState((int)state);
        switch (state) {
			case SoundState.normal:
				SoundAudioSource.mute =false;
				break;
			case SoundState.mute:
				SoundAudioSource.mute = true;
				break;
		}
		//设置最新的状态
		this.soundState = state;
	}
	
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="pathAndName">名字</param>
    /// <param name="sex">性别</param>
	public void playSoundByName(string pathAndName,int sex)
	{
        string s = "";
        if (sex==1)
        {
            s = "boy/";
        }
        else
        {
            s = "gril/";
        }
		playSoundByName(MJPath.MJAudioPath +s+ pathAndName, 1.0f);
	}

	private void playSoundByName(string pathAndName, float volume)
	{
		if (soundState == SoundState.normal) {//正常状态才允许播放
			try{
				AudioClip objPrefab = (AudioClip)Resources.Load(pathAndName);
                Debug.Log("播放音乐长度:" + objPrefab.length);
                if (objPrefab==null)
                {
                    return;
                }
				SoundAudioSource.volume = volume;
				SoundAudioSource.PlayOneShot(objPrefab);
               
			}catch(System.Exception e){
				Debug.Log("AudioController------>:" + e.ToString());
			}
		}
	}

	/*****根据音乐的路径和名字播放******/
	public void playBGMByName(string Name)
	{
		playBGMByName(MJPath.MJAudioPath+Name, 1.0f);
	}

	public void playBGMByName(string Name, float volume)
	{
		playBGMByName(Name, volume, true);
	}

	private void playBGMByName(string Name, float volume, bool loop)
	{
        if (bgmState == BGMState.pause)
        {
            try
            {
                AudioClip objPrefab = (AudioClip)Resources.Load(Name);
                Debug.Log("OBJ------>" + objPrefab.ToString());
                BGMAudioSource.clip = objPrefab;
                BGMAudioSource.volume = volume;
                BGMAudioSource.loop = loop;
                BGMAudioSource.Pause();
            }
            catch (System.Exception e)
            {
                Debug.Log("AudioController------>:" + e.ToString());
            }
        }
        if (bgmState == BGMState.normal) {
		    try {
                AudioClip objPrefab = (AudioClip)Resources.Load(Name);
				Debug.Log("OBJ------>"+objPrefab.ToString());
                BGMAudioSource.clip = objPrefab;
                BGMAudioSource.volume = volume;
                BGMAudioSource.loop = loop;
                BGMAudioSource.Play();
            } catch (System.Exception e) {
				Debug.Log("AudioController------>:" + e.ToString());
			}

		}
	}

}
