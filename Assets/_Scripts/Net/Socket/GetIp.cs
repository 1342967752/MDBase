using UnityEngine;
using System.Collections;
/// <summary>
/// 获取ip
/// </summary>
public class GetIp : MonoBehaviour {

    private string ip = "";

    void Start()
    {
        CustomSocket.getInstance().Connect();
        ChatSocket.getInstance().Connect();
    }

    //void FixedUpdate()
    //{
    //    if (!GlobalDataScript.isGetIP && !CustomSocket.getInstance().isConnected)
    //    {
    //        GameTools.instance.loadText(APIS.getIpAddress, getIpCallBack);
    //        GlobalDataScript.isGetIP = true;
    //    }
    //}


    int time = 0;
    IEnumerator checkeConnect()
    {
        while (true)
        {
            if (CustomSocket.getInstance().isConnected)
            {
                break;
            }
            time++;
            yield return new WaitForSeconds(0.2f);
            if (time > 10)
            {
                time = 0;
                MJUIManager._instance.reloginUIPage.setInfo("连接服务器失败!");
                MJUIManager._instance.reloginUIPage.setOnClick(reLogin);
                break;
            }
        }
    }
    /// <summary>
    /// 重新登录获取信息
    /// </summary>
    public void reLogin()
    {
        GlobalDataScript.isGetIP = false;
        MJUIManager._instance.loginPage.loginErrorCallBack();
        Debug.Log("重新登录");
    }

    private void getIpCallBack(HttpLoadModel model)
    {
       if (model.error!=null)
       {
            MJUIManager._instance.loadingPage.close();
            MJUIManager._instance.reloginUIPage.setInfo("拉取服务器信息失败!");
            MJUIManager._instance.reloginUIPage.setOnClick(reLogin);
        }
        else
        {
            ip = model.text;
            APIS.socketUrl = ip;//设置ip
            //APIS.chatSocketUrl = ip;
            //连接socket
            CustomSocket.getInstance().Connect();
            ChatSocket.getInstance().Connect();
            MJUIManager._instance.loadingPage.close();
            StartCoroutine(checkeConnect());
        }
    }
}
