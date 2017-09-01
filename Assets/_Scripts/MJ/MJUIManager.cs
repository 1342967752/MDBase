using UnityEngine;
using UnityEngine.SceneManagement;
using TinyTeam.UI;
using System.Collections;
using UnityEngine.Events;

public class MJUIManager : MonoBehaviour {
    public static MJUIManager _instance;


    //页面
    private LoginPage _loginPage;
    public MJDeskPage mJDeskPage;
    public MainMenuPage mainMenuPage;
    private EnterRoomPage _enterRoomPage;
    private MJQuitRoomPage _mJQuitRoomPage;
    private MJDanJIeSuan _mJDanJIeSuan;
    private MJZongJieSuan _mJZongJieSuan;
    private ReloginUIPage _reloginUIPage;
    private ReturnLoginWantedPage _returnLoginWantedPage;
    private LoadingPage _loadingPage;
    public ExitGamePage _exitGamePage;
    private MJMyInfoPage _mJMyInfoPage;
    private UIZhanJi _uIZhanJi;
    private UIZhanJiXiangQing _uIZhanJiXiangQing;
    private AdPage _adPage;
    private BackWindow _backWindow;

    public UnityAction<bool, GameObject> switchButtonListener;

    public LoginPage loginPage
    {
        get
        {
            if (_loginPage == null || !_loginPage.isActive())
            {
                TTUIPage.ShowPage<LoginPage>();
            }
            return _loginPage;
        }

        set
        {
            _loginPage = value;
        }
    }

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

    public ReturnLoginWantedPage returnLoginWantedPage
    {
        get
        {
            if (_returnLoginWantedPage==null||!_returnLoginWantedPage.isActive())
            {
                TTUIPage.ShowPage<ReturnLoginWantedPage>();
            }
            return _returnLoginWantedPage;
        }

        set
        {
            _returnLoginWantedPage = value;
        }
    }

    public MJMyInfoPage mJMyInfoPage
    {
        get
        {
            if (_mJMyInfoPage==null||!_mJMyInfoPage.isActive())
            {
                TTUIPage.ShowPage<MJMyInfoPage>();
            }
            return _mJMyInfoPage;
        }

        set
        {
            _mJMyInfoPage = value;
        }
    }

    public UIZhanJi uiZhanJi
    {
        get
        {
            if (_uIZhanJi==null||!_uIZhanJi.isActive())
            {
                TTUIPage.ShowPage<UIZhanJi>();
            }
            return _uIZhanJi;
        }

        set
        {
            _uIZhanJi = value;
        }
    }

    public LoadingPage loadingPage
    {
        get
        {
            if (_loadingPage==null)
            {
                TTUIPage.ShowPage<LoadingPage>();
            }
            return _loadingPage;
        }

        set
        {
            _loadingPage = value;
        }
    }
    
    public UIZhanJiXiangQing uIZhanJiXiangQing
    {
        get
        {
            if (_uIZhanJiXiangQing==null||!_uIZhanJiXiangQing.isActive())
            {
                TTUIPage.ShowPage<UIZhanJiXiangQing>();
            }
            return _uIZhanJiXiangQing;
        }

        set
        {
            _uIZhanJiXiangQing = value;
        }
    }

    public AdPage adPage
    {
        get
        {
            if (_adPage == null || !_adPage.isActive())
            {
                TTUIPage.ShowPage<AdPage>();
            }
            return _adPage;
        }

        set
        {
            _adPage = value;
        }
    }

    public BackWindow backWindow
    {
        get
        {
            if (_backWindow==null||!_backWindow.isActive())
            {
                TTUIPage.ShowPage<BackWindow>();
            }
            return _backWindow;
        }

        set
        {
            _backWindow = value;
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_exitGamePage==null||!_exitGamePage.isActive())
            {
                TTUIPage.ShowPage<ExitGamePage>();
            }
            else
            {
                TTUIPage.ClosePage<ExitGamePage>();
            }
               
        }
    }

    private void init()
    {
        if (SceneManager.GetActiveScene().name.Equals(SceneName.MaJiang)||SceneManager.GetActiveScene().name.Equals(SceneName.MaJiangRecord))
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
