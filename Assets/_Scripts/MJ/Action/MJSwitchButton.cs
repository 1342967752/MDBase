using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class MJSwitchButton :MonoBehaviour,IPointerClickHandler
{
	private Transform OnObj;//onState transform
	private Transform OffObj;//offState transform

	public void init()
	{
		try {
			OnObj = this.transform.Find("OnObj");
			OffObj = this.transform.Find("OffObj");
     		changeState(isOn);
		} catch (Exception e) {
			Debug.Log("You must add two obj for this component！");
		}
	}
	private void changeState(bool state)
	{
		OnObj.gameObject.SetActive(state);
		OffObj.gameObject.SetActive(!state);
        //设置状态
	}

	[Serializable]
	public class SwitchEvent : UnityEvent<bool,GameObject>{

    }
	private SwitchEvent onClick = new SwitchEvent();
	private bool m_isOn;
	public bool isOn{
		get {
            //m_isOn = DateManger.Instance.getSoundState() == 0 ? true : false;
            return m_isOn; }
		set {
            m_isOn = value;
			changeState(m_isOn);
            onClick.Invoke(m_isOn, gameObject);
		}
	}
	void Start()
	{
		init();
		onClick.AddListener(MJUIManager._instance.switchButtonListener);
	}

	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
			return;
		isOn = !isOn;
       
    }
}

