using UnityEngine;
/// <summary>
/// 房间信息
/// </summary>
public class MJRoomInfo : MonoBehaviour {
    public static MJRoomInfo _instance;

    private TextMesh roomIdText;//房间号

    void Awake()
    {
        _instance = this;
        init();
    }

    private void init()
    {
        roomIdText = transform.FindChild("RoomId").GetComponent<TextMesh>();
    }

    /// <summary>
    /// 设置房间号
    /// </summary>
    /// <param name="id"></param>
    public void setRoomId(int id)
    {
        roomIdText.text = "房间号:" + id;
        Debug.Log(roomIdText.text);
    }
    
}
