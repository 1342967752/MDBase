using TinyTeam.UI;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 工具类
/// </summary>
public class ToolsManager :MonoBehaviour {
    private static ToolsManager _instance;
   
    public static ToolsManager Instance
    {
        get
        {
            if (_instance==null)
            {
                GameObject temp = new GameObject();
                temp.name = "ToolsManager";
                _instance = temp.AddComponent<ToolsManager>();
            }
            return _instance;
        }
    }

    #region 弹窗提醒
    public WantedWindowPage wantedWindowPage;

    /// <summary>
    /// 显示一个按钮的窗口
    /// </summary>
    /// <param name="info">显示内容</param>
    /// <param name="midBtnText">中间按钮上显示文字</param>
    /// <param name="midUA">点击按钮跳转事件</param>
    public void showWantedWindow(string info, string midBtnText, UnityAction midUA)
    {
        TTUIPage.ShowPage<WantedWindowPage>();
        wantedWindowPage.showWantedWindow(info, midBtnText, midUA);
    }

    /// <summary>
    /// 显示两个按钮的窗体
    /// </summary>
    /// <param name="info">显示内容</param>
    /// <param name="leftBtnText">左边按钮显示文字</param>
    /// <param name="leftUA">左边按钮按下跳转事件</param>
    /// <param name="rightBtnText">右边按钮显示文字</param>
    /// <param name="rightUA">右边按钮按下跳转事件</param>
    public void showWantedWindow(string info, string leftBtnText, UnityAction leftUA, string rightBtnText, UnityAction rightUA)
    {
        TTUIPage.ShowPage<WantedWindowPage>();
        wantedWindowPage.showWantedWindow(info, leftBtnText, leftUA, rightBtnText, rightUA);
    }
    #endregion

}
