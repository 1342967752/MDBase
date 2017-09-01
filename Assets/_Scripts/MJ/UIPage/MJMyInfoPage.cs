using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;

public class MJMyInfoPage : TTUIPage {

    private Transform animContent;//动画容器
    private GameObject myInfo;
    private GameObject otherInfo;



	public MJMyInfoPage() : base(UIType.Fixed, UIMode.HideOther, UICollider.None)
    {
        uiPath = MyPath.MJMyInfoPagePath;
    }

    public override void Awake(GameObject go)
    {
        MJUIManager._instance.mJMyInfoPage = this;
        init();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void init()
    {
        myInfo = transform.FindChild("MyBg").gameObject;
        otherInfo = transform.FindChild("OtherBg").gameObject;

        transform.GetComponent<Button>().onClick.AddListener(() =>
        {
            Hide();
        });

        myInfo.transform.FindChild("Close").GetComponent<Button>().onClick.AddListener(() => {
            Hide();
        });

        otherInfo.transform.FindChild("Close").GetComponent<Button>().onClick.AddListener(() =>
        {
            Hide();
        });

        animContent = otherInfo.transform.FindChild("Emoji/Viewport/Content");

        myInfo.SetActive(false);
        otherInfo.SetActive(false);
    }

    /// <summary>
    /// 设置信息
    /// </summary>
    /// <param name="headImg"></param>
    /// <param name="nickName"></param>
    /// <param name="id"></param>
    /// <param name="address"></param>
    public void setInfo(Image headImg,string nickName,string id,string address,int uuid,int winCount,int loseCount,int pingCount)
    {
        if (uuid==GlobalDataScript.loginResponseData.account.uuid)
        {
            myInfo.transform.FindChild("Head").GetComponent<Image>().sprite = headImg.sprite;
            myInfo.transform.FindChild("NickName").GetComponent<Text>().text = nickName;
            myInfo.transform.FindChild("ID").GetComponent<Text>().text = "ID:" + uuid;
            myInfo.transform.FindChild("Address").GetComponent<Text>().text = address;
            myInfo.SetActive(true);
            otherInfo.SetActive(false);
        }
        else
        {
            otherInfo.transform.FindChild("Head").GetComponent<Image>().sprite = headImg.sprite;
            otherInfo.transform.FindChild("NickName").GetComponent<Text>().text = nickName;
            otherInfo.transform.FindChild("ID").GetComponent<Text>().text = "ID:" + uuid;
            otherInfo.transform.FindChild("Address").GetComponent<Text>().text = address;
            myInfo.SetActive(false);
            otherInfo.SetActive(true);
            loadAnim(uuid);
        }
       
    }
 
    /// <summary>
    /// 加载动画
    /// </summary>
    private void loadAnim(int uuid)
    {
        if (animContent==null)
        {
            return;
        }

        MJCardAction.Instance.destroyAllChild(animContent);//摧毁之前的子物体
        GameObject[] anims = Resources.LoadAll<GameObject>(MyPath.MJThrowAnimPath);
        if (anims==null||anims.Length==0)
        {
            return;
        }

        GameObject temp;
        Vector2 size = animContent.GetComponent<GridLayoutGroup>().cellSize;
        for (int i=0;i<anims.Length; i++)
        {
            temp = GameObject.Instantiate(anims[i]);
            temp.transform.SetParent(animContent);
            temp.transform.localScale = Vector3.one;
            temp.AddComponent<MJThrowEmojiItem>();
            temp.GetComponent<MJThrowEmojiItem>().imgId = i;
            temp.GetComponent<MJThrowEmojiItem>().uuid = uuid;
        }

        animContent.GetComponent<Image>().rectTransform.sizeDelta = new Vector2((size.x + 5) * anims.Length ,size.y);

    }
}
