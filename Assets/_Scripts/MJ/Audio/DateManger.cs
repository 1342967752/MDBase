using UnityEngine;

public class DateManger : MonoBehaviour {
	private static DateManger _instance;
	public static DateManger Instance {
		get {
			if (_instance == null) {
				GameObject obj = new GameObject("DateManger");
				_instance = obj.AddComponent<DateManger>();
			}
			return _instance;
		}
	}
	public void setisLiaoTian(bool b)
	{
		if (b) {
			PlayerPrefs.SetInt("isLiaoTian", 1);
		} else {
			PlayerPrefs.SetInt("isLiaoTian", 0);
		}
	}
	public bool getisLiaoTian()
	{
		return PlayerPrefs.GetInt("isLiaoTian") == 0;
	}
	public void setisZhenDong(bool b)
	{
		if (b) {
			PlayerPrefs.SetInt("isZhenDong", 1);
		} else {
			PlayerPrefs.SetInt("isZhenDong", 0);
		}
	}
	public bool getisZhenDong()
	{
		return PlayerPrefs.GetInt("isZhenDong") == 0;
	}
	public void setisDuPai(bool b)
	{
		if (b) {
			PlayerPrefs.SetInt("isDuPai", 1);
		} else {
			PlayerPrefs.SetInt("isDuPai", 0);
		}
	}
	public bool getisDuPai()
	{
		return PlayerPrefs.GetInt("isDuPai") == 0;
	}
	/*****音效相关变量存储******/
	public void setBGMVolume(float volume){
		PlayerPrefs.SetFloat("bgmvolume",volume);
	}
	public float getBGMVolume(){
		return PlayerPrefs.GetFloat("bgmvolume", 1.0f);
	}
	public void setSoundVolume(float volume){
		PlayerPrefs.SetFloat("soundvolume", volume);
	}
	public float getSoundVolume(){
		return PlayerPrefs.GetFloat("soundvolume", 1.0f);
	}
	public void setBGMState(int state){
		PlayerPrefs.SetInt("bgmstate",state);
	}
	public int getBGMState(){
		return PlayerPrefs.GetInt("bgmstate",0);
	}
	public void setSoundState(int state){
		PlayerPrefs.SetInt("soundState",state);
	}
	public int getSoundState(){
		return PlayerPrefs.GetInt("soundState");
	}
}
