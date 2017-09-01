using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;
using System.Collections.Generic;
/// <summary>
/// 创建房间page
/// </summary>
public class CreateRoomPage : TTUIPage {
    private ToggleGroup wanfaToggleGroup;
    private ToggleGroup huitouToggleGroup;
    private ToggleGroup jingPaiToggleGroup;
    private ToggleGroup jingDiaoToggleGroup;
    private ToggleGroup jushuToggleGroup;

    public CreateRoomPage() : base(UIType.Fixed, UIMode.HideOther, UICollider.None)
	{
        uiPath =MyPath.CreateRoomPagePath;
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
        this.transform.Find("Bg/BtnBack").GetComponent<Button>().onClick.AddListener(() =>
        {
            Hide();
        });
        this.transform.Find("BtnConfuseCreate").GetComponent<Button>().onClick.AddListener(() =>
        {
            string wanfaString = getSuanfa(wanfaToggleGroup);
            string huitouyixiaoString = getSuanfa(huitouToggleGroup);
            string jingpaiSuanfa = getSuanfa(jingPaiToggleGroup);
            string jingdiaoSuanfa = getSuanfa(jingDiaoToggleGroup);
            string jushuSuanfa = getSuanfa(jushuToggleGroup);
            string zongSuanfa = wanfaString+"/"+huitouyixiaoString+"/"+ jingpaiSuanfa + "/" + jingdiaoSuanfa + "/" + jushuSuanfa + "/";   //归总字符串，	

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
            Debug.Log(zongSuanfa);
            transform.GetComponent<CrateRoomSettingScript>().createNamChangRoom(0, pvpTime, 0, false, true);
            MJUIManager._instance.loadingPage.Active();
        });
        //设置Toggle的字体颜色随选中状态改变
        //wan fa
        AddToggleListener(wanfaToggleGroup.transform.Find("BiaoZhunToggle").GetComponent<Toggle>());
        AddToggleListener(wanfaToggleGroup.transform.Find("ShangXiaFanToggle").GetComponent<Toggle>());
        AddToggleListener(wanfaToggleGroup.transform.Find("MaiDiLeiToggle").GetComponent<Toggle>());
        AddToggleListener(wanfaToggleGroup.transform.Find("TongYiShouGeToggle").GetComponent<Toggle>());
        //hui tou yi xiao
        AddToggleListener(huitouToggleGroup.transform.Find("YesToggle").GetComponent<Toggle>());
        AddToggleListener(huitouToggleGroup.transform.Find("NoToggle").GetComponent<Toggle>());
        //JingPai
        AddToggleListener(jingPaiToggleGroup.transform.Find("SuanFenToggle").GetComponent<Toggle>());
        AddToggleListener(jingPaiToggleGroup.transform.Find("BuSuanFenToggle").GetComponent<Toggle>());
        //JingDiao
        AddToggleListener(jingDiaoToggleGroup.transform.Find("SuanFenToggle").GetComponent<Toggle>());
        AddToggleListener(jingDiaoToggleGroup.transform.Find("BuSuanFenToggle").GetComponent<Toggle>());
        //jushu
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
        wanfaToggleGroup = transform.Find("WanFaXuanZeGroup").GetComponent<ToggleGroup>();
        huitouToggleGroup = transform.Find("HuiTouYiXiaoGroup").GetComponent<ToggleGroup>();
        jingPaiToggleGroup = transform.Find("JingPaiSuanFaGroup").GetComponent<ToggleGroup>();
        jingDiaoToggleGroup = transform.Find("JingDiaoSuanFaGroup").GetComponent<ToggleGroup>();
        jushuToggleGroup = transform.Find("JuShuXuanZeGroup").GetComponent<ToggleGroup>();
    }
}
