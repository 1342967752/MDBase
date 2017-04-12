using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// 碰杠胡吃按钮控制
/// </summary>
public class MJPGHCAction : MonoBehaviour {
    public static MJPGHCAction _instance;

    private List<Vector3> pos = new List<Vector3>();//记录位置

    private GameObject mulChi;//多个吃牌
    private GameObject btnBar;//按钮条
    private int index = 0;

    public bool isMulChi = false;
    private List<string> mulChiCardsName = new List<string>();//多种吃牌最小牌的名字集合
    private Dictionary<string, GameObject> btns = new Dictionary<string, GameObject>();//存放按钮

	void Awake()
    {
        _instance = this;
        init();
    }

    private void init()
    {
        btns.Add("peng", transform.FindChild("BtnBar/Peng").gameObject);
        btns.Add("hu", transform.FindChild("BtnBar/Hu").gameObject);
        btns.Add("chi", transform.FindChild("BtnBar/Chi").gameObject) ;
        btns.Add("guo", transform.FindChild("BtnBar/Guo").gameObject);
        btns.Add("gang", transform.FindChild("BtnBar/Gang").gameObject);

        btns["peng"].GetComponent<Button>().onClick.AddListener(pengOnClick);
        btns["hu"].GetComponent<Button>().onClick.AddListener(huOnClick);
        btns["chi"].GetComponent<Button>().onClick.AddListener(chiOnclick);
        btns["guo"].GetComponent<Button>().onClick.AddListener(guoOnClick);
        btns["gang"].GetComponent<Button>().onClick.AddListener(gangOnClick);

        mulChi = transform.FindChild("MulChi").gameObject;
        transform.FindChild("MulChi/Guo").GetComponent<Button>().onClick.AddListener(mulChiGuoBtnOnClick);

        btnBar = transform.FindChild("BtnBar").gameObject;

        //隐藏不为过的按钮
        foreach (string key in btns.Keys)
        {
            if (!key.Equals("guo"))
            {
                btns[key].SetActive(false);
            }
        }

        //记录位置
        pos.Add(btns["chi"].transform.localPosition);
        pos.Add(btns["peng"].transform.localPosition);
        pos.Add(btns["gang"].transform.localPosition);

        index = pos.Count - 1;

        mulChi.SetActive(false);
        gameObject.SetActive(false);
    }

    #region 按钮显示
    public void showPeng()
    {
        gameObject.SetActive(true);
        btns["peng"].SetActive(true);
        btns["peng"].transform.localPosition = pos[index--];
    }

    public void showChi()
    {
        gameObject.SetActive(true);
        btns["chi"].SetActive(true);
        btns["chi"].transform.localPosition = pos[index--];
    }

    public void showGang()
    {
        gameObject.SetActive(true);
        btns["gang"].SetActive(true);
        btns["gang"].transform.localPosition = pos[index--];
    }

    public void showHu()
    {
        gameObject.SetActive(true);
        btns["hu"].SetActive(true);
        btns["hu"].transform.localPosition = pos[index--];
    }

    /// <summary>
    /// 显示多个吃的按钮
    /// </summary>
    /// <param name="minNames"></param>
    public void showMulChi(List<string> minNames)
    {
        isMulChi = true;
        mulChiCardsName = minNames;
    }
    #endregion

    #region 事件
    private void chiOnclick()
    {
        if (isMulChi)
        {
            createChiItem(mulChiCardsName);
            mulChi.SetActive(true);
            btnBar.SetActive(false);
        }
        else
        {
            GameObject.Find("init").GetComponent<MyMahjongScript>().myChiCardBtnClick();
            close();
        }
    }

    private void gangOnClick()
    {
        GameObject.Find("init").GetComponent<MyMahjongScript>().myGangBtnClick();
        close();
    }

    private void pengOnClick()
    {
        GameObject.Find("init").GetComponent<MyMahjongScript>().myPengBtnClick();
        close();
    }

    private void guoOnClick()
    {
        GameObject.Find("init").GetComponent<MyMahjongScript>().myPassBtnClick();
        close();
    }

    private void huOnClick()
    {
        GameObject.Find("init").GetComponent<MyMahjongScript>().hupaiRequest();
        close();
    }

    #region 多种吃选择
    /// <summary>
    /// 多种吃牌过按钮点击事件
    /// </summary>
    private void mulChiGuoBtnOnClick()
    {
        btnBar.SetActive(true);
        mulChi.SetActive(false);
        destroyChiItem();
    }
    #endregion

    #endregion

    /// <summary>
    /// 关闭操作栏
    /// </summary>
    public void close()
    {
        foreach (string key in btns.Keys)
        {
            if (!key.Equals("guo"))
            {
                btns[key].SetActive(false);
            }
        }
        destroyChiItem();
        mulChi.SetActive(false);
        gameObject.SetActive(false);
        btnBar.SetActive(true);
        index = pos.Count - 1;
        isMulChi = false; 
    }

  
    /// <summary>
    /// 创建吃牌item
    /// </summary>
    private void createChiItem(List<string> minNames)
    {
        if (minNames==null)
        {
            return;
        }

        for (int i=0;i<minNames.Count;i++)
        {
            GameObject temp = Instantiate(Resources.Load<GameObject>(MJPath.MJChiItemPath));
            temp.transform.SetParent(mulChi.transform);
            temp.transform.localScale = Vector3.one;
            temp.GetComponent<MJChiItem>().setCards(mulChiCardsName[i]);
        }
    }

    /// <summary>
    /// 摧毁吃牌item
    /// </summary>
    private void destroyChiItem()
    {
        int count = mulChi.transform.childCount;
        for (int i=0;i<count;i++)
        {
            if (!mulChi.transform.GetChild(i).gameObject.name.Equals("Guo"))
            {
                Destroy(mulChi.transform.GetChild(i).gameObject);
            }
        }
    }

}
