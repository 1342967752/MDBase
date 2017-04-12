using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;
using System.Collections.Generic;
/// <summary>
/// 创建房间page
/// </summary>
public class CreateRoomPage : TTUIPage {

    private List<Toggle> wanFaToggleList = new List<Toggle>();
    private string wanfaString = "标准场/";
    private ToggleGroup jingPaiToggleGroup;
    private ToggleGroup jingDiaoToggleGroup;
    private ToggleGroup jushuToggleGroup;

    public CreateRoomPage() : base(UIType.Fixed, UIMode.HideOther, UICollider.None)
	{
        uiPath =MJPath.CreateRoomPagePath;
    }
    

    public override void Awake(GameObject go)
    {
        findView();
        setBtnClickListener();
    }

    /// <summary>
    /// 设置按钮监听
    /// </summary>
    private void setBtnClickListener()
    {
        this.transform.Find("BtnBack").GetComponent<Button>().onClick.AddListener(() =>
        {
            Hide();
        });
        this.transform.Find("BtnConfuseCreate").GetComponent<Button>().onClick.AddListener(() =>
        {
            string jingpaiSuanfa = getSuanfa(jingPaiToggleGroup);
            string jingdiaoSuanfa = getSuanfa(jingDiaoToggleGroup);
            string jushuSuanfa = getSuanfa(jushuToggleGroup);
            string zongSuanfa = wanfaString + jingpaiSuanfa + "/" + jingdiaoSuanfa + "/" + jushuSuanfa + "/";   //归总字符串，	

            int pvpTime = 8;
            //到这里加一个发送到服务器端的代码
            if (zongSuanfa.Contains("8"))
            {
                pvpTime = 8;
            }
            else if (zongSuanfa.Contains("16"))
            {
                pvpTime = 16;
            }
           
            transform.GetComponent<CrateRoomSettingScript>().createNamChangRoom(0, pvpTime, 0, false, true);
        });
        foreach (Toggle t in wanFaToggleList)
        {
            t.onValueChanged.AddListener(delegate (bool isOn)
            {
                wanfaString = "";
                foreach (Toggle tog in wanFaToggleList)
                {
                    if (tog.isOn)
                    {
                        wanfaString += tog.transform.Find("Label").GetComponent<Text>().text + "/";
                        tog.transform.Find("Label").GetComponent<Text>().color = Color.red;
                    }
                    else
                    {
                        tog.transform.Find("Label").GetComponent<Text>().color = new Color(131 / 255.0f, 95 / 255.0f, 76 / 255.0f);
                    }
                }
                //Debug.Log(wanfaString);
            });
        }
        //设置Toggle的字体颜色随选中状态改变
        AddToggleListener(jingPaiToggleGroup.transform.Find("SuanFenToggle").GetComponent<Toggle>());
        AddToggleListener(jingPaiToggleGroup.transform.Find("BuSuanFenToggle").GetComponent<Toggle>());
        AddToggleListener(jingDiaoToggleGroup.transform.Find("SuanFenToggle").GetComponent<Toggle>());
        AddToggleListener(jingDiaoToggleGroup.transform.Find("BuSuanFenToggle").GetComponent<Toggle>());
        AddToggleListener(jushuToggleGroup.transform.Find("8JuToggle").GetComponent<Toggle>());
        AddToggleListener(jushuToggleGroup.transform.Find("16JuToggle").GetComponent<Toggle>());
    }


    #region 通过Toggle对象改变Toggle组件上Text组件的颜色
    public void AddToggleListener(Toggle toggle)
    {
        toggle.onValueChanged.AddListener(delegate (bool isOn)
        {
            setTextColorWithBool(toggle.transform.Find("Label").GetComponent<Text>(), isOn);
        });
    }
    public void setTextColorWithBool(Text text,bool isOn)
    {
        if (isOn)
        {
            text.color = Color.red;
        }
        else
        {
            text.color = new Color(131 / 255.0f, 95 / 255.0f, 76 / 255.0f);
        }
    }
    #endregion

    /// <summary>
    /// 通过toggleGroup获得算法设定
    /// </summary>
    /// <param name="tg"></param>
    /// <returns></returns>
    private string getSuanfa(ToggleGroup tg)
    {
        string suanfa = "";
        foreach (Toggle toggle in tg.ActiveToggles())
        {
            suanfa += toggle.transform.Find("Label").GetComponent<Text>().text;
        }
        return suanfa;
    }

    /// <summary>
    /// 找到view
    /// </summary>
    private void findView()
    {
        wanFaToggleList.Add(transform.Find("WanFaXuanZeGroup/BiaoZhunToggle").GetComponent<Toggle>());
        wanFaToggleList.Add(transform.Find("WanFaXuanZeGroup/HuiTouYiXiaoToggle").GetComponent<Toggle>());
        wanFaToggleList.Add(transform.Find("WanFaXuanZeGroup/MaiDiLeiToggle").GetComponent<Toggle>());
        wanFaToggleList.Add(transform.Find("WanFaXuanZeGroup/TongYiShouGeToggle").GetComponent<Toggle>());
        jingPaiToggleGroup = transform.Find("JingPaiSuanFaGroup").GetComponent<ToggleGroup>();
        jingDiaoToggleGroup = transform.Find("JingDiaoSuanFaGroup").GetComponent<ToggleGroup>();
        jushuToggleGroup = transform.Find("JuShuXuanZeGroup").GetComponent<ToggleGroup>();
    }
}
