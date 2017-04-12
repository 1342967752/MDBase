using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class BtnDelayActive : MonoBehaviour {
    public int delayTime = 3;//延迟时间
    private Button btn;
    void Start()
    {
       btn= transform.GetComponent<Button>();
       btn.onClick.AddListener(delayFun);
    }

    /// <summary>
    /// 延时函数
    /// </summary>
    private void delayFun()
    {
        StartCoroutine(delayActiveIE(delayTime));
    }

    /// <summary>
    /// 一定时间生效按钮
    /// </summary>
    /// <param name="btn"></param>
    /// <param name="time">时间</param>
    /// <returns></returns>
     IEnumerator delayActiveIE(float time)
    {
        if (btn.interactable)
        {
            btn.interactable = false;
            yield return new WaitForSeconds(time);
            btn.interactable = true;
        }

    }
}
