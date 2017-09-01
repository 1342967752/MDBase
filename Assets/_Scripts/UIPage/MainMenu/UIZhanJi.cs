using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;
using AssemblyCSharp;
/// <summary>
/// 战绩ui
/// </summary>
public class UIZhanJi: TTUIPage{
    private Transform zhanJiContent;
    private Vector2 itemSize;
    private Text wantedText;

	public UIZhanJi() : base(UIType.PopUp, UIMode.HideOther, UICollider.None)
	{
		uiPath = MyPath.UIZhanJIPath;
	}
  
	public override void Awake(GameObject go)
	{
        MJUIManager._instance.uiZhanJi = this;
        init();
	}

    /// <summary>
    /// 初始化
    /// </summary>
    private void init()
    {
        zhanJiContent = transform.FindChild("Bg/ScrollPanel/Content");
        itemSize = zhanJiContent.GetComponent<GridLayoutGroup>().cellSize;
        wantedText = transform.FindChild("Bg/ScrollPanel/Wanted").GetComponent<Text>();
        wantedText.gameObject.SetActive(false);
        transform.FindChild("Bg/BtnBack").GetComponent<Button>().onClick.AddListener(() =>
        {
            Hide();
        });
    }

    /// <summary>
    /// 设置战绩信息
    /// </summary>
    /// <param name="roomlist"></param>
    public void setZhanJi(ZhanjiRoomList roomlist,Vector3 contentPos)
    {
        if (roomlist==null||roomlist.roomDataList==null||roomlist.roomDataList.Count==0)
        {
            wantedText.text = MyName.NoZhanJiRecord;
            wantedText.gameObject.SetActive(true);
            return;
        }
        //摧毁之前的
        MJCardAction.Instance.destroyAllChild(zhanJiContent);

        GameObject temp = Resources.Load<GameObject>(MyPath.MJZhanJIItenPath);
       
       for (int i=0;i<roomlist.roomDataList.Count;i++)
       {
            GameObject zhanjiItem = GameObject.Instantiate(temp);
            zhanjiItem.transform.SetParent(zhanJiContent);
            zhanjiItem.transform.localScale = Vector3.one;

            zhanjiItem.GetComponent<ZhanjiRoomItemScript>().setUI(roomlist.roomDataList[i], i+1);
        }

        zhanJiContent.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(itemSize.x,(itemSize.y+5)*roomlist.roomDataList.Count);

        if (contentPos!=Vector3.zero)
        {
            zhanJiContent.localPosition =contentPos;
        }
        else
        {
            zhanJiContent.localPosition = new Vector3(zhanJiContent.localPosition.x, 0, zhanJiContent.localPosition.z);
        }

       

    }

    /// <summary>
    /// 获取容器的状态
    /// </summary>
    /// <returns></returns>
    public Vector3 getContentPos()
    {
        return zhanJiContent.localPosition;
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
        GlobalDataScript.mainMenuState.zhanJiReSet();
    }

}
