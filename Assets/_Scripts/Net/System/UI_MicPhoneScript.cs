using UnityEngine;
using System.Collections.Generic;

public class UI_MicPhoneScript : MonoBehaviour
{
    public float WholeTime=10f;
    public GameObject InputGameObject;
    private bool btnDown = false;
	
    void Start()
    {
        InputGameObject.SetActive(false);
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
			if (MJPlayerManager._instance.getPlayerByIndex(i).getPlayer().account.uuid != GlobalDataScript.loginResponseData.account.uuid) {
				userList.Add (MJPlayerManager._instance.getPlayerByIndex(i).getPlayer().account.uuid);
			}
		}
		return userList;
	}
}
