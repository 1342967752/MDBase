using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class MJPGCHAnim : MonoBehaviour {
    private UnityAction endUa = null;
    public bool isAutoDestory=true;//是否自动销毁

    /// <summary>
    /// 回弹动画
    /// </summary>
	public void playPunch()
    {
        StartCoroutine(delay());
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(0.6f);
        onComplete();
    }

    /// <summary>
    /// 回调事件
    /// </summary>
    private void onComplete()
    {
        if (endUa!=null)
        {
            endUa();
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// 设置动画结束动作
    /// </summary>
    /// <param name="ua"></param>
    public void setEndFun(UnityAction ua)
    {
        endUa = ua;
    }

}
