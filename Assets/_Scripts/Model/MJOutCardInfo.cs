using UnityEngine;

public class MJOutCardInfo {

    public int cardPoint=0;
    public string pos = "";
    public GameObject outCardGB = null;

    public void setOutInfo(GameObject outCard,string pos)
    {
        this.pos = pos;
        outCardGB = outCard;
        cardPoint = int.Parse(outCard.name);
    }
}
