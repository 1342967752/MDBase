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
    }

    void Start()
    {
        init();
    }

    private void init()
    {
        roomIdText = transform.FindChild("RoomId").GetComponent<TextMesh>();
        
        if (GlobalDataScript.reEnterRoomData != null)
        {
            //重新进入房间
            setRoomId(GlobalDataScript.reEnterRoomData.roomId);
            Debug.Log("重新进入房间");
        }
        else
        {
            //加入房间
            if (GlobalDataScript.roomJoinResponseData != null)
            {
                setRoomId(GlobalDataScript.roomJoinResponseData.roomId);
                Debug.Log("加入房间");
            }
            else//创建房间
            {
                setRoomId(GlobalDataScript.roomVo.roomId);
                Debug.Log("创建房间");
            }
        }

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
