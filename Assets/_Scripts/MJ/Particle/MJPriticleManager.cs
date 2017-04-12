using UnityEngine;
using System.Collections;

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
            case AnimType.Gang:loadAnim("Gang",pos); break;
            case AnimType.Chi: loadAnim("Chi", pos); break;
            case AnimType.Peng: loadAnim("Peng", pos); break;
            case AnimType.Hu: loadAnim("Hu", pos); break;
            case AnimType.Zimo: loadAnim("ZiMo", pos); break;
            case AnimType.Duiju:loadAnimToGame("DuiJu");break;
        }
    }

    /// <summary>
    /// 加载动画,使用dotween
    /// </summary>
    /// <param name="path"></param>
    private void loadAnim(string name,string pos)
    {
        Transform parent = MJPlayerManager._instance.getTransfromByPos(pos);
        GameObject temp = Instantiate(Resources.Load<GameObject>(MJPath.MJParticlePath + name));
        temp.transform.SetParent(parent);
        temp.transform.localScale = Vector3.one;
        //设置位置
        switch (pos)
        {
            case DirectionEnum.Bottom: temp.transform.localPosition = MJPostion.bottonPGHCPos ;break;
            case DirectionEnum.Top: temp.transform.localPosition = MJPostion.topPGHCPos; break;
            case DirectionEnum.Right: temp.transform.localPosition = MJPostion.rightPGHCPos; break;
            case DirectionEnum.Left: temp.transform.localPosition = MJPostion.leftPGHCPos; break;
        }

        temp.GetComponent<MJPGCHAnim>().playPunch();
    }

    /// <summary>
    /// 加载动画到界面,使用animation
    /// </summary>
    /// <param name="name"></param>
    private void loadAnimToGame(string name)
    {
        GameObject temp = Instantiate(Resources.Load<GameObject>(MJPath.MJParticlePath + name));
        temp.transform.SetParent(GameObject.Find("UIRoot/FixedRoot").transform);
        temp.transform.localPosition = Vector3.zero;
        temp.transform.localScale = Vector3.one;
    }

    void OnDestory()
    {
        if (_instance!=null)
        {
            Destroy(gameObject);
            _instance = null;
        }
    }
}
