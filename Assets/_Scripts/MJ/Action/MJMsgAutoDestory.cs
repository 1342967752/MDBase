using UnityEngine;
using System.Collections;
/// <summary>
/// 消息自动摧毁
/// </summary>
public class MJMsgAutoDestory : MonoBehaviour {
    public float time = 3;

    public void begin()
    {
        StartCoroutine(delayToDestory());
    }

	IEnumerator delayToDestory()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
