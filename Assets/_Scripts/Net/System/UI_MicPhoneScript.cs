using UnityEngine;
using System.Collections.Generic;

public class UI_MicPhoneScript : MonoBehaviour
{
    public float WholeTime=10f;
    public GameObject InputGameObject;
    private bool btnDown = false;
    public static bool isCanUse = true;//语音按钮是否可用
	
    void Start()
    {
        InputGameObject.SetActive(false);
        isCanUse = GlobalDataScript.getTalkingCanUse();
    }

    void FixedUpdate(){
		if (btnDown)
		{
			WholeTime -= Time.deltaTime;
			if (WholeTime <= 0)
			{
				OnPointerUp ();
			}
		}
	}

    public void OnPointerDown()
    {
        if (!isCanUse)
        {
            WantedTextTool.Instance.addTip("语音不可用",1);
            return;
        }

        Debug.Log("语音按钮按下");
        if (MJPlayerManager._instance.getPlayerListCount() >1) {
			btnDown = true;
            InputGameObject.SetActive(true);
            MicroPhoneInput.getInstance ().StartRecord (getUserList ());
		}else{
			WantedTextTool.Instance.addTip ("房间里只有你一个人，不能发送语音",2);
		}
    }

    public void OnPointerUp()
    {
        if (!isCanUse)
        {
            return;
        }

        Debug.Log("语音按钮松开");
        MicroPhoneInput.getInstance ().StopRecord ();
		if (btnDown) {
			btnDown = false;
            InputGameObject.SetActive(false);
            WholeTime = 10;
			if (MJPlayerManager._instance.getPlayerListCount()> 1) {
				MicroPhoneInput.getInstance ().StopRecord ();
				//myScript.myselfSoundActionPlay ();
			} else {
				
			}
		}
    }

	private List<int> getUserList(){
		List<int> userList = new List<int> ();
		for(int i=0;i< MJPlayerManager._instance.getPlayerListCount(); i++){
			if (MJPlayerManager._instance.getPlayerByIndex(i).getPlayer().account.uuid!= GlobalDataScript.loginResponseData.account.uuid) {
				userList.Add (MJPlayerManager._instance.getPlayerByIndex(i).getPlayer().account.uuid);
                Debug.Log("接收语音玩家(uuid):" + MJPlayerManager._instance.getPlayerByIndex(i).getPlayer().account.uuid);
            }
		}
        
		return userList;
	}
}
