using UnityEngine;
using UnityEngine.SceneManagement;
using TinyTeam.UI;
using System.Collections;
using UnityEngine.Events;

public class MJUIManager : MonoBehaviour {
    public static MJUIManager _instance;


    //页面
    public MJDeskPage mJDeskPage;
    public MainMenuPage mainMenuPage;
    private EnterRoomPage _enterRoomPage;
    private MJQuitRoomPage _mJQuitRoomPage;
    private MJDanJIeSuan _mJDanJIeSuan;
    private MJZongJieSuan _mJZongJieSuan;
    private ReloginUIPage _reloginUIPage;

    public UnityAction<bool, GameObject> switchButtonListener;

    public MJDanJIeSuan mJDanJIeSuan
    {
        get
        {
            if (_mJDanJIeSuan==null||!_mJDanJIeSuan.isActive())
            {
                TTUIPage.ShowPage<MJDanJIeSuan>();
            }
            return _mJDanJIeSuan;
        }

        set
        {
            _mJDanJIeSuan = value;
        }
    }

    public MJQuitRoomPage mJQuitRoomPage
    {
        get
        {
            if (_mJQuitRoomPage==null||!_mJQuitRoomPage.isActive())
            {
                TTUIPage.ShowPage<MJQuitRoomPage>();
            }
            return _mJQuitRoomPage;
        }

        set
        {
            _mJQuitRoomPage = value;
        }
    }

    public MJZongJieSuan mJZongJieSuan
    {
        get
        {
            if (_mJZongJieSuan==null||!_mJZongJieSuan.isActive())
            {
                TTUIPage.ShowPage<MJZongJieSuan>();
            }
            return _mJZongJieSuan;
        }

        set
        {
            _mJZongJieSuan = value;
        }
    }

    public ReloginUIPage reloginUIPage
    {
        get
        {
            if (_reloginUIPage==null||!_reloginUIPage.isActive())
            {
                TTUIPage.ShowPage<ReloginUIPage>();
            }
            return _reloginUIPage;
        }

        set
        {
            _reloginUIPage = value;
        }
    }

    public EnterRoomPage enterRoomPage
    {
        get
        {
            if (_enterRoomPage == null||!_enterRoomPage.isActive())
            {
                TTUIPage.ShowPage<EnterRoomPage>();
            }
            return _enterRoomPage;
        }

        set
        {
            _enterRoomPage = value;
        }
    }


    void Awake()
    {
        if (_instance==null)
        {
            _instance = this;
        }
    } 

    void Start()
    {
        init();
        StartCoroutine(sendHeart());
    }

    private void init()
    {
        if (SceneManager.GetActiveScene().name.Equals(SceneName.MaJiang))
        {
            TTUIPage.ShowPage<MJDeskPage>();
        }else if (SceneManager.GetActiveScene().name.Equals(SceneName.Login))
        {
            TTUIPage.ShowPage<LoginPage>();
        }else if (SceneManager.GetActiveScene().name.Equals(SceneName.MainMenu))
        {
            TTUIPage.ShowPage<MainMenuPage>();
        }
    }

    /// <summary>
    /// 显示主界面
    /// </summary>
    public  void showMainMenuPage()
    {
        TTUIPage.ShowPage<MainMenuPage>();
    }

    public void closeLoginUI()
    {
        TTUIPage.ClosePage<LoginPage>();
    }

    /// <summary>
    /// 发送心跳
    /// </summary>
    /// <returns></returns>
    IEnumerator sendHeart()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            CustomSocket.getInstance().sendHeadData();
        }
    }
}
