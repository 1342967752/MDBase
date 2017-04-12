

using UnityEngine;

public class MJPostion  {

    //出牌的角度
    public static Vector3 outCardEulerAngles=new Vector3(-90,180,0);
    //手牌角度
    public static Vector3 handCardEulerAngles = Vector3.up * 180;
    //牌堆牌角度
    public static Vector3 pileCardEulerAngles = Vector3.left * -90;
    //庄的图片位置
    public static Vector3 bankerImgPos = new Vector3(25, 25, 0);
    //暗杠角度
    public static Vector3 anGangAngles = Vector3.right * 90;

    //玩家碰杠胡等动画显示位置
    public static Vector3 bottonPGHCPos = new Vector3(550,200,0);
    public static Vector3 rightPGHCPos = new Vector3(-200, 0, 0);
    public static Vector3 leftPGHCPos = new Vector3(200, 0, 0);
    public static Vector3 topPGHCPos = new Vector3(-440, -50, 0);

    public static Vector3 jingAngles = new Vector3(0, 180, 0);

}
