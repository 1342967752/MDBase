using System.Collections.Generic;

using UnityEngine;


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
            //HupaiResponseItem hu = new HupaiResponseItem();
            //hu.gangScore = 20;
            //hu.nickname = "00";
            //hu.totalScore = 50;
            //hu.totalInfo = new TotalInfo();
            //hu.totalInfo.hu = "55:4:zi_common:20";
            List<string> list = new List<string>();
            for (int i=0;i<15;i++)
            {
                list.Add("3");
            }

            MJUIManager._instance.mJDanJIeSuan.setHuCardList(list);
            MJUIManager._instance.mJDanJIeSuan.setHuCard("4", list.Count);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            //MJDicePlace._instance.setDirection("S",10);
            // TTUIPage.ClosePage<MJDanJIeSuan>();
            //MJPGHCAction._instance.showChi();
            //MJPGHCAction._instance.showMulChi(new List<string>() { "0", "6", "7" });
            // MJCardsManager._instance.bottonAddCard("3");
            //MJUIManager._instance.mJDanJIeSuan.setHuPaiMode(0, 1);
            //MJPlayerManager._instance.changeScoreByPos(DirectionEnum.Bottom, 20);
            //MJPGHCAction._instance.showHu();
            //HornInfo info = new HornInfo();
            //info.nickName = "baba";
            //info.info = "oo";
            //Horn.instance.addHorn(info);
            //MJUIManager._instance.adPage.Refresh();
            MJCardsManager._instance.addCard(DirectionEnum.Left,"9");
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
            MJHandCardArea.jingPai = ji;
            MJUIManager._instance.mJDeskPage.setJing(ji.zhengJingPai,ji.fuJingPai);
            Debug.Log("手牌:" + name.Count);
            MJCardsManager._instance.addCard(DirectionEnum.Bottom, name[13]);
            name.RemoveAt(13);
            MJCardsManager._instance.startGame(name,name,name,name);
            MJCardsPile._instance.createCardPile();
            
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            // MJCardsManager._instance.pengCard(DirectionEnum.Top, "13");
            //MJPriticleManager.Instance.playAnim(AnimType.Gang, DirectionEnum.Top);
            // GlobalDataScript.isDrag = true;
            // MJCardsManager._instance.chiCard(DirectionEnum.Top, 6,7,5);
            //GameObject.Find("init").GetComponent<MyMahjongScript>().reSet();
            //MJScenesManager.Instance.loadSceneNotAnim(SceneName.MaJiang);
            MJCardsManager._instance.pengCard(DirectionEnum.Left,point+"",false);
         
            // WechatOperateScript._instance.shareAchievementToWeChat(PlatformType.WeChat);
            // MJCardsManager._instance.pengCard(DirectionEnum.Bottom, point + "", false);
            //MJUIManager._instance.loadingPage.Active();
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
            MJCardsManager._instance.gangCard(DirectionEnum.Left,point+"",false,false);
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
            //for (int i = 0; i < 4; i++)
            //{
            //    HupaiResponseItem h = new HupaiResponseItem();
            //    h.totalInfo = new TotalInfo();
            //    h.gangScore = 9;
            //    h.nickname = "nihao";
            //    h.totalScore = 77;
            //    h.totalInfo.scores = 30 + "";
            //    h.totalInfo.hu=""
            //    hupai.avatarList.Add(h);
            //    MJUIManager._instance.mJDanJIeSuan.setPlayerInfo(i, null, h);
            //}
            //MJCardsManager._instance.outCard(DirectionEnum.Top, "5");


            //List<string> name = new List<string>();
            //name.Add(32 + "");
            //for (int i = 0; i < 13; i++)
            //{
            //    name.Add((int)Random.Range(0, 17) + "");
            //}

            //MJCardsManager._instance.huCard(DirectionEnum.Top, name);
            //MJCardsManager._instance.outCard(DirectionEnum.Left,""+point);
            MJPlayerManager._instance.setHu(DirectionEnum.Bottom, true);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            MJCardsManager._instance.chiCard(DirectionEnum.Left, 2, 2, 2, false);
        }
    }

  
}  

