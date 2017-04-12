using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;
public class MJUISettingPage : TTUIPage{
	public MJUISettingPage() : base(UIType.PopUp, UIMode.HideOther, UICollider.None){
		uiPath = MJPath.MJUISettingPath;
	}
    private MJSwitchButton musicSwitch;
    private MJSwitchButton soundSwitch;
    private MJSwitchButton fangyanSwitch;
	public override void Awake(GameObject go)
	{
        findView();
		setBtnClickListener();
        MJUIManager._instance.switchButtonListener += musicSwitchButtonEvent;
        MJUIManager._instance.switchButtonListener += soundSwitchButtonEvent;
        MJUIManager._instance.switchButtonListener += fangYanSwitchButtonEvent;
        initSwitchButton();
	}

    void initSwitchButton()
    {
        musicSwitch.init();
        if ((BGMState)DateManger.Instance.getBGMState() == BGMState.pause)
        {
            musicSwitch.isOn = false;
        }
        else{
            musicSwitch.isOn = true;
        }
        musicSwitch.init();
        soundSwitch.init();
        if ((SoundState)DateManger.Instance.getSoundState() == SoundState.mute)
        {
            soundSwitch.isOn = false;
        }
        else
        {
            soundSwitch.isOn = true;
        }
        soundSwitch.init();
        //方言

    }

	void setBtnClickListener()
	{
		this.transform.Find("BtnBack").GetComponent<Button>().onClick.AddListener(() => {
			Hide();
		});
	}

    void findView()
    {
        musicSwitch = this.transform.Find("MusicSwitch").GetComponent<MJSwitchButton>();
        soundSwitch = this.transform.Find("SoundSwitch").GetComponent<MJSwitchButton>();
        fangyanSwitch = this.transform.Find("FangYanSwitch").GetComponent<MJSwitchButton>();
    }

	private void musicSwitchButtonEvent(bool isOn, GameObject sender)
	{
		if (sender.name.Equals("MusicSwitch")) {
			AudioController.Instance.setBGMPause(!isOn);
			Debug.Log("音乐switch");
		}
	}
	private void soundSwitchButtonEvent(bool isOn, GameObject sender)
	{
		if (sender.name.Equals("SoundSwitch")) {
			Debug.Log("音效switch");
			AudioController.Instance.setSoundMute(!isOn);
		}
	}
	private void fangYanSwitchButtonEvent(bool isOn,GameObject sender){
		if (sender.name.Equals("FangYanSwitch")){
			Debug.Log("方言switch");
		}
	}
}
