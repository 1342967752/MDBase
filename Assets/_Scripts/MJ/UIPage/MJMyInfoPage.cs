using UnityEngine;
using TinyTeam.UI;

public class MJMyInfoPage : TTUIPage {

	public MJMyInfoPage() : base(UIType.Fixed, UIMode.HideOther, UICollider.None)
    {
        uiPath = MJPath.MJMyInfoPagePath;
    }

    public override void Awake(GameObject go)
    {
        
    }

    public override void Refresh()
    {
        
    }
}
