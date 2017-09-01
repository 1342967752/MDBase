using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;
/// <summary>
/// 帮助界面
/// </summary>
public class UIHallHelp : TTUIPage{
    private Toggle nanChangeMJToggle;
    private Toggle douDiZhuToggle;
    private Transform nanchangmjHelpPanel;
    private Transform doudiZhuHelpPanel;

    public UIHallHelp() : base(UIType.PopUp, UIMode.HideOther, UICollider.None)
	{
		uiPath = MyPath.UIHelpPagePath;
	}
	
	public override void Awake(GameObject go)
	{
		findView();
		setBtnClickListener();
	}

    /// <summary>
    /// 设置监听
    /// </summary>
	private void setBtnClickListener()
	{
		transform.Find("BtnBack").GetComponent<Button>().onClick.AddListener(() => {
			Hide();
		});
		nanChangeMJToggle.onValueChanged.AddListener(delegate (bool isOn) {
			nanchangmjHelpPanel.gameObject.SetActive(isOn);
            nanChangeMJToggle.transform.Find("Label").GetComponent<Shadow>().enabled = isOn;
			setGameToggleTextColor(nanChangeMJToggle.transform.Find("Label").GetComponent<Text>(), isOn);
		});
		douDiZhuToggle.onValueChanged.AddListener(delegate (bool isOn) {
            // doudiZhuHelpPanel.gameObject.SetActive(isOn);
            douDiZhuToggle.transform.Find("Label").GetComponent<Shadow>().enabled = isOn;
            setGameToggleTextColor(douDiZhuToggle.transform.Find("Label").GetComponent<Text>(), isOn);
		});
        //设置介绍界面上的几个toggle的事件
        nanchangmjHelpPanel.Find("BaseRule").GetComponent<Toggle>().onValueChanged.AddListener(delegate(bool isOn)
        {
            nanchangmjHelpPanel.Find("NanChangMJBaseRulePanel").gameObject.SetActive(isOn);
            setHelpToggleTextColor(nanchangmjHelpPanel.Find("BaseRule/Label").GetComponent<Text>(), isOn);
        });
        nanchangmjHelpPanel.Find("JingRule").GetComponent<Toggle>().onValueChanged.AddListener(delegate (bool isOn)
        {
            nanchangmjHelpPanel.Find("NanChangMJJingRulePanel").gameObject.SetActive(isOn);
            setHelpToggleTextColor(nanchangmjHelpPanel.Find("JingRule/Label").GetComponent<Text>(), isOn);
        });
        nanchangmjHelpPanel.Find("ScoreRule").GetComponent<Toggle>().onValueChanged.AddListener(delegate (bool isOn)
        {
            nanchangmjHelpPanel.Find("NanChangMJScoreRulePanel").gameObject.SetActive(isOn);
            setHelpToggleTextColor(nanchangmjHelpPanel.Find("ScoreRule/Label").GetComponent<Text>(), isOn);
        });
        nanchangmjHelpPanel.Find("SpecialRule").GetComponent<Toggle>().onValueChanged.AddListener(delegate (bool isOn)
        {
            nanchangmjHelpPanel.Find("NanChangMJSpecialRulePanel").gameObject.SetActive(isOn);
            setHelpToggleTextColor(nanchangmjHelpPanel.Find("SpecialRule/Label").GetComponent<Text>(), isOn);
        });
    }

    /// <summary>
    /// 找到view
    /// </summary>
	private void findView(){
		nanChangeMJToggle = transform.Find("GameToggleGroup/TgNanChangeMJ").GetComponent<Toggle>();
		douDiZhuToggle = transform.Find("GameToggleGroup/TgDouDiZhu").GetComponent<Toggle>();
        nanchangmjHelpPanel = transform.Find("Panels/nanchangMJToggleGroup");
        Debug.Log("nanchangmjHelpPanel"+nanchangmjHelpPanel);
	}

    /// <summary>
    /// 设置toggle text颜色
    /// </summary>
    /// <param name="text"></param>
    /// <param name="isOn"></param>
	private void setGameToggleTextColor(Text text,bool isOn){
		if (isOn) {
			text.color = Color.white;
		} else {
			text.color = new Color(111 / 255.0f, 75 / 255.0f, 56 / 255.0f);
		}
	}

    private void setHelpToggleTextColor(Text text,bool isOn)
    {
        if (isOn)
        {
            text.color = new Color(123/ 255.0f,61/ 255.0f, 34/ 255.0f);
        }else
        {
            text.color = new Color(61 / 255.0f, 108 / 255.0f, 6 / 255.0f);
        }
    }
}
