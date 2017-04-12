using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class WantedTextTool:MonoBehaviour  {
    private static WantedTextTool _instance = null;
    private Vector2 initPos= new Vector2(0, -250);//初始位置
    private Vector2 toPos=new Vector2(0,-180);//到达的位置
    private  List<ToolType> tips = new List<ToolType>();
    private List<GameObject> tipGb = new List<GameObject>();

	public static WantedTextTool Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject temp = new GameObject("TipTool");
                _instance = temp.AddComponent<WantedTextTool>();
            }

            return _instance;
        }
    }

    void Start()
    {
        StartCoroutine(startShow());
    }

    #region 文字提醒
    /// <summary>
    /// 显示文字
    /// </summary>
    /// <param name="tip">文字</param>
    private void showTip(string tip)
    { 
        GameObject temp = Resources.Load<GameObject>(MJPath.WantedTextPath);
        Image tipBg = temp.transform.FindChild("Bg").GetComponent<Image>();
        tipBg.rectTransform.sizeDelta = new Vector2(tip.Length * 25 < 200 ? 200 : tip.Length * 25, 150);
        tipBg.transform.FindChild("Text").GetComponent<Text>().text = tip;
        tipBg.transform.localPosition = initPos;
        GameObject parent = Instantiate(temp);
        parent.transform.FindChild("Bg").DOLocalMoveY(toPos.y, 1).OnComplete(complete);
        tipGb.Add(parent);
    }

   
    /// <summary>
    /// 显示tip
    /// </summary>
    /// <param name="tip"></param>
    /// <param name="pos">1.底部显示 2.中间显示</param>
    private void showTip(string tip, int pos)
    {
        if (pos == 0)
        {
            initPos = new Vector2(0, -250);
            toPos.y = -180;
            showTip(tip);
        }
        else if (pos == 1)
        {
            initPos = Vector2.zero;
            toPos.y = 70;
            showTip(tip);
        }
    }

    
    IEnumerator startShow()
    {
        while (true)
        {
            if (tips.Count>0)
            {
                showTip(tips[0].tip, tips[0].pos);
                tips.RemoveAt(0);
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void addTip(string tip,int pos)
    {
        ToolType temp = new ToolType();
        temp.tip = tip;
        temp.pos = pos;
        tips.Add(temp);
    }

   private void complete()
    {
        if (tipGb.Count>0)
        {
            Destroy(tipGb[0]);
            tipGb.RemoveAt(0);
        }
    }
    #endregion
}
