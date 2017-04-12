using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;
public class UIHallHelp : TTUIPage{
	public UIHallHelp() : base(UIType.PopUp, UIMode.HideOther, UICollider.None)
	{
		uiPath = MJPath.UIHelpPagePath;
	}
	private Toggle nanChangeMJToggle;
	private Toggle douDiZhuToggle;
	private Transform nanchangmjHelpPanel;
	private Transform doudiZhuHelpPanel;
	public override void Awake(GameObject go)
	{
		findView();
		setBtnClickListener();
	}
	void setBtnClickListener()
	{
		this.transform.Find("BtnBack").GetComponent<Button>().onClick.AddListener(() => {
			Hide();
		});
		nanChangeMJToggle.onValueChanged.AddListener(delegate (bool isOn) {
			nanchangmjHelpPanel.gameObject.SetActive(isOn);
			setGameToggleTextColor(nanChangeMJToggle.transform.Find("Label").GetComponent<Text>(), isOn);

		});
		douDiZhuToggle.onValueChanged.AddListener(delegate (bool isOn) {
           // doudiZhuHelpPanel.gameObject.SetActive(isOn);
			setGameToggleTextColor(douDiZhuToggle.transform.Find("Label").GetComponent<Text>(), isOn);
		});
	}
	void findView(){
		nanChangeMJToggle = transform.Find("GameToggleGroup/TgNanChangeMJ").GetComponent<Toggle>();
		douDiZhuToggle = transform.Find("GameToggleGroup/TgDouDiZhu").GetComponent<Toggle>();
        nanchangmjHelpPanel = transform.Find("Panels/NanChangMjHelpPanel");
		
	}
	void setGameToggleTextColor(Text text,bool isOn){
		if (isOn) {
			text.color = Color.white;
		} else {
			text.color = new Color(111 / 255.0f, 75 / 255.0f, 56 / 255.0f);
		}
	}
}
