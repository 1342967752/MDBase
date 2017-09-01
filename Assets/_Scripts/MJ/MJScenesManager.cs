using UnityEngine;
using UnityEngine.SceneManagement;

public class MJScenesManager : MonoBehaviour {
    private static MJScenesManager _instance;

    public static MJScenesManager Instance
    {
        get
        {
            if (_instance==null)
            {
                GameObject temp = new GameObject("SecnesManager");
                _instance = temp.AddComponent<MJScenesManager>();
            }
            return _instance;
        }
    }

    /// <summary>
    /// 直接加载场景
    /// </summary>
    /// <param name="name"></param>
    public void loadSceneNotAnim(string name)
    {
        if (SceneManager.GetActiveScene().name.Equals(name))
        {
            return;
        }

        if (name.Equals(SceneName.Login))
        {
            GlobalDataScript.reinitData();
        }
        SceneManager.LoadScene(name);
    }

    /// <summary>
    /// 获取当前界面名字
    /// </summary>
    /// <returns></returns>
    public string curSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}

public class SceneName
{
    public static string Login = "Login";
    public static string MainMenu = "MainMenu";
    public static string MaJiang = "MaJiang";
    public static string MaJiangRecord = "MaJiangRecord";
}