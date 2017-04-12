using System.Collections.Generic;
using TinyTeam.UI;
using UnityEngine;
using AssemblyCSharp;

public class Test : MonoBehaviour {
    public int point = 11;

	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.W))
	    {
            //MJCardsManager._instance.test01();
            //MJDicePlace._instance.setMyDirection("S");
            //MJUIManager._instance.mainMenuPage.setBroadCast("888888");
            HupaiResponseItem hu = new HupaiResponseItem();
            hu.gangScore = 20;
            hu.nickname = "00";
            hu.totalScore = 50;
            hu.totalInfo = new TotalInfo();
            hu.totalInfo.hu = "55:4:zi_common:20";
            hu.paiArray = new int[14];
            for (int i=0;i<14;i++)
            {
                if (i==0)
                {
                    hu.paiArray[i] = 14;
                }
                else
                {
                    hu.paiArray[i] = 0;
                }
            }
            MJUIManager._instance.mJDanJIeSuan.setPlayerInfo(55, null, 99,hu);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            //MJDicePlace._instance.setDirection("S",10);
            MJCardsManager._instance.addCard(DirectionEnum.Bottom, point+"");
            GlobalDataScript.isDrag = true;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            //MJCardsManager._instance.test();
            //MJDicePlace._instance.setDirection("N",20);
            List<string> name = new List<string>();
            name.Add(32 + "");
            for (int i=0;i<13;i++)
            {
                name.Add((int)Random.Range(0, 17)+"");
            }
            JingResponse ji = new JingResponse();
            ji.fuJingPai = 33;
            ji.zhengJingPai =32;
            MJCardsManager._instance.jingPai = ji;
            MJUIManager._instance.mJDeskPage.setJing(ji.zhengJingPai,ji.fuJingPai);
            Debug.Log("手牌:" + name.Count);
            MJCardsManager._instance.startGame(name);
            MJCardsManager._instance.addCard(DirectionEnum.Bottom, name[13]);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            // MJCardsManager._instance.pengCard(DirectionEnum.Top, "13");
            //MJPriticleManager.Instance.playAnim(AnimType.Gang, DirectionEnum.Top);
            // GlobalDataScript.isDrag = true;
            // MJCardsManager._instance.chiCard(DirectionEnum.Top, 6,7,5);
            //GameObject.Find("init").GetComponent<MyMahjongScript>().reSet();
            //MJScenesManager.Instance.loadSceneNotAnim(SceneName.MaJiang);
            MJCardsManager._instance.gangCard(DirectionEnum.Bottom,point+"",false);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            // MJCardsManager._instance.gangCard(DirectionEnum.Top, "13", false);
            // MJPriticleManager.Instance.playAnim(AnimType.Gang, DirectionEnum.Left);
            // MJUIManager._instance.mJDeskPage.cleamJing();
            //MJCardsPile._instance.createCardPile();
            //// MJCardsPile._instance.setShaiZi(3,5,3);
            //MJCardsPile._instance.setBanker(2);
            //MJCardsPile._instance.setJing(10,10);
            //MJCardsManager._instance.gangCard(DirectionEnum.Top,"6",true);
            //AudioController.Instance.playBGMByName("bgm");
            MJCardsManager._instance.chiCard(DirectionEnum.Bottom,1, 2,3);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            // MJCardsManager._instance.outCard(DirectionEnum.Top, "13");
            //MJPriticleManager.Instance.playAnim(AnimType.Gang, DirectionEnum.Bottom);
            //AudioController.Instance.playSoundByName("1001",1);
            //MJUIManager._instance.mJDeskPage.setJing(4, 5);
            //MJCardsPile._instance.getCard();
            //HupaiResponseVo hupai = new HupaiResponseVo();
            //hupai.avatarList = new List<HupaiResponseItem>();
            //for (int i=0; i<4;i++)
            //{
            //    HupaiResponseItem h = new HupaiResponseItem();
            //    h.totalInfo = new TotalInfo();
            //    h.gangScore = 9;
            //    h.nickname = "nihao";
            //    h.totalScore = 77;
            //    h.totalInfo.scores = 30 + "";
            //    hupai.avatarList.Add(h);
            //    MJUIManager._instance.mJDanJIeSuan.setPlayerInfo(i, null, h);
            //}
            //MJCardsManager._instance.outCard(DirectionEnum.Top, "5");
            List<string> name = new List<string>();
            name.Add(32 + "");
            for (int i = 0; i < 13; i++)
            {
                name.Add((int)Random.Range(0, 17) + "");
            }

            MJCardsManager._instance.huCard(DirectionEnum.Top, name);
        }
    }
}
