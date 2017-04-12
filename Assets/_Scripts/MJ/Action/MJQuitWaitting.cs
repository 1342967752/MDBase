using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MJQuitWaitting : MonoBehaviour {
    private Text text=null;

    //开始等待
	public void OnEnable()
    {
        if (text == null)
        {
            text = transform.GetComponent<Text>();
        }
        text.text = "选择中";
        StartCoroutine(waitting());
    }

    IEnumerator waitting()
    {
        while (true)
        {
            yield  return new WaitForSeconds(1);
            Debug.Log("开始等待退出");
            if (text.text.Equals("选择中..."))
            {
                text.text = "选择中";
            }
            else
            {
                text.text += ".";
            }
            
        }
    }
}
