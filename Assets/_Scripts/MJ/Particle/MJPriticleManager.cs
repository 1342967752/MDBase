using UnityEngine;
using UnityEngine.Events;

public enum AnimType
{
    None,
    Gang,
    Peng,
    Chi,
    Hu,
    Zimo,
    Duiju
}
/// <summary>
/// 
/// 粒子特效管理器
/// </summary>
public class MJPriticleManager : MonoBehaviour {

    private static MJPriticleManager _instance;

    public static MJPriticleManager Instance
    {
        get
        {
            if (_instance==null)
            {
                GameObject temp = new GameObject("MJPriticleManager");
                _instance = temp.AddComponent<MJPriticleManager>();
            }
            return _instance;
        }
    }

    public UnityAction gangCallBack;//杠回调
    public UnityAction huCallBack;//胡回调

    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="type"></param>
    /// <param name=""></param>
    public void playAnim(AnimType type,string pos)
    {
        switch (type)
        {
            case AnimType.None:break;
            case AnimType.Gang:loadAnim("Gang",pos,gangCallBack); break;
            case AnimType.Chi: loadAnim("Chi", pos,null); break;
            case AnimType.Peng: loadAnim("Peng", pos,null); break;
            case AnimType.Hu: loadAnim("Hu", pos,huCallBack); break;
            case AnimType.Zimo: loadAnim("ZiMo", pos, huCallBack); break;
            case AnimType.Duiju:loadAnimToGame("DuiJu");break;
        }
    }

    /// <summary>
    /// 加载动画（碰杠胡）
    /// </summary>
    /// <param name="path"></param>
    /// <param name="pos">座位</param>
    /// <param name="ua">动画结束执行函数</param>
    private void loadAnim(string name,string pos,UnityAction ua)
    {
        Transform parent = MJPlayerManager._instance.getTransfromByPos(pos);
        GameObject temp = Instantiate(Resources.Load<GameObject>(MyPath.MJParticlePath + name));
        temp.transform.SetParent(parent);
        temp.transform.localScale = Vector3.one;
        //设置位置
        switch (pos)
        {
            case DirectionEnum.Bottom: temp.transform.localPosition = MyPostion.bottonPGHCPos ;break;
            case DirectionEnum.Top: temp.transform.localPosition = MyPostion.topPGHCPos; break;
            case DirectionEnum.Right: temp.transform.localPosition = MyPostion.rightPGHCPos; break;
            case DirectionEnum.Left: temp.transform.localPosition = MyPostion.leftPGHCPos; break;
        }

        temp.GetComponent<MJPGCHAnim>().playPunch();
        temp.GetComponent<MJPGCHAnim>().setEndFun(ua);
    }

    /// <summary>
    /// 加载动画到界面,使用animation
    /// </summary>
    /// <param name="name"></param>
    private void loadAnimToGame(string name)
    {
        GameObject temp = Instantiate(Resources.Load<GameObject>(MyPath.MJParticlePath + name));
        temp.transform.SetParent(GameObject.Find("UIRoot/FixedRoot").transform);
        temp.transform.localPosition = Vector3.zero;
        temp.transform.localScale = Vector3.one;
    }

    void OnDestory()
    {
        if (_instance!=null)
        {
            Destroy(_instance);
            _instance = null;
        }
    }
}
