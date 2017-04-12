using UnityEngine;
using System.Collections;
/// <summary>
/// 获取ip
/// </summary>
public class GetIp : MonoBehaviour {

    private string ip = "";
    private bool isGet = false;

	void Update()
    {
        if (!isGet)
        {
            StartCoroutine(GET(APIS.getIpAddress));
            isGet = true;
        }
    }


    IEnumerator GET(string url)
    {
        WWW www;
        float mProgress=0;
        www = new WWW(url);
        yield return www;

        mProgress = www.progress;
        if (www.error != null)
        {
            Debug.Log(www.error);
            MJUIManager._instance.reloginUIPage.setInfo("连接服务器失败!");
            MJUIManager._instance.reloginUIPage.setOnClick(reLogin); 
        }
        else
        {
            ip = www.text;
            APIS.socketUrl = ip;

            //连接socket
            CustomSocket.getInstance().Connect();
            ChatSocket.getInstance().Connect();
            Debug.Log("服务器地址:"+www.text);
        }
    }

    public void reLogin()
    {
        isGet = false;
    }
}
