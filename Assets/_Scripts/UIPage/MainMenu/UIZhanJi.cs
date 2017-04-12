using UnityEngine;
using System.Collections.Generic;
using TinyTeam.UI;
using UnityEngine.UI;
/// <summary>
/// 战绩ui
/// </summary>
public class UIZhanJi: TTUIPage{
	public UIZhanJi() : base(UIType.PopUp, UIMode.HideOther, UICollider.None)
	{
		uiPath = MJPath.UIZhanJIPath;
	}
    private Transform Content;
    private int ListLength=8;
    private List<Transform> InfoList=new List<Transform>();
  
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
		this.transform.Find("Bg/BtnBack").GetComponent<Button>().onClick.AddListener(() => {
			Hide();
		});
	}

    /// <summary>
    /// 找到view
    /// </summary>
	private void findView(){
        Content = this.transform.Find("Bg/ScrollPanel/Content");
        for(int i = 0; i < ListLength; i++)
        {
            string name = "Row" + i;
            InfoList.Add(Content.Find(name));
        }
        //默认为隐藏状态
        foreach(Transform row in InfoList)
        {
            row.gameObject.SetActive(false);
        }
	}


    public void setInfoForRowAtIndex(int index, string time,string[] playerName,Sprite[] playerHeadImage,string[] score)
    {
        InfoList[index].gameObject.SetActive(true);
        InfoList[index].Find("Value/TimeLabel").GetComponent<Text>().text = time;
        for(int i = 0; i < playerName.Length; i++)
        {
            string name = "Player" + i;
            Transform player=InfoList[index].Find("Value/"+name);
            player.gameObject.SetActive(true);
            player.Find("HeadImage").GetComponent<Image>().overrideSprite=playerHeadImage[i];
            player.Find("Name").GetComponent<Text>().text=playerName[i];
            player.Find("Score").GetComponent<Text>().text = score[i];
        }
    }

}
