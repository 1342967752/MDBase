using UnityEngine;
using System.Collections.Generic;
using TinyTeam.UI;
using UnityEngine.UI;
public class MJZongJieSuan : TTUIPage
{
	public int PlayerNumber=4;//本局玩家数量
    public bool isShow = false;//是否显示

	public MJZongJieSuan() : base(UIType.Fixed, UIMode.HideOther, UICollider.None){
		uiPath = MJPath.MJZongJieSuanPath;
	}
	public override void Awake(GameObject go)
	{
        MJUIManager._instance.mJZongJieSuan = this;
		setBtnClickListener();
        m_playerList.Clear();
		findView();
	}

    /// <summary>
    /// 设置按钮监听
    /// </summary>
	private void setBtnClickListener()
	{
		transform.Find("BtnBack").GetComponent<Button>().onClick.AddListener(() => {
            ClosePage<MJZongJieSuan>();
            MJScenesManager.Instance.loadSceneNotAnim(SceneName.MainMenu);
        });
		transform.Find("BtnTuiChu").GetComponent<Button>().onClick.AddListener(()=>{
            MJScenesManager.Instance.loadSceneNotAnim(SceneName.MainMenu);

		});
		transform.Find("BtnFenXiang").GetComponent<Button>().onClick.AddListener(()=>{
			Debug.Log("分享");
		});
	}

	private List<Transform> m_playerList=new List<Transform>();
    /// <summary>
    /// 找到view
    /// </summary>
    private void findView()
    {
        //find player
        for (int i = 0; i < PlayerNumber; i++)
        {
            string name = "Player" + i;
            m_playerList.Add(transform.Find("Players/" + name));
            m_playerList[i].gameObject.SetActive(false);
        }
    }

    /****排名，头像，昵称，对局书，胜局数，累计分*****/
    public void setPlayerInfoWithOrder(int order,Sprite headImage,string name,string duijuShu,string shengju,string leiji){
		m_playerList[order].gameObject.SetActive(true);
		m_playerList[order].Find("HeadImage").GetComponent<Image>().overrideSprite=headImage;
		m_playerList[order].Find("LabelName").GetComponent<Text>().text=name;
		m_playerList[order].Find("LabelDuiJuShu").GetComponent<Text>().text = duijuShu;
		m_playerList[order].Find("LabelShengJu").GetComponent<Text>().text=shengju;
		m_playerList[order].Find("LabelLeiJi").GetComponent<Text>().text=leiji;
	}

    public void close()
    {
        ClosePage<MJZongJieSuan>();
    }
}
