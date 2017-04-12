using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;
/// <summary>
/// 进入房间拨号盘
/// </summary>
public class EnterRoomPage : TTUIPage {
    public EnterRoomPage() : base(UIType.Fixed, UIMode.HideOther, UICollider.None)
	{
        uiPath = MJPath.EnterRoomPagePath;
    }
    private InputField roomNumberInput;
    public override void Awake(GameObject go)
    {
        MJUIManager._instance.enterRoomPage = this;
        findView();
        setBtnClickListener();
    }

    /// <summary>
    /// 设置按钮监听
    /// </summary>
    private void setBtnClickListener()
    {
        this.transform.Find("BtnBack").GetComponent<Button>().onClick.AddListener(() =>
        {
            Hide();
        });
        this.transform.Find("KeyBord/0").GetComponent<Button>().onClick.AddListener(() =>
        {
            inputNum(0);
        });
        this.transform.Find("KeyBord/1").GetComponent<Button>().onClick.AddListener(() =>
        {
            inputNum(1);
        });
        this.transform.Find("KeyBord/2").GetComponent<Button>().onClick.AddListener(() =>
        {
            inputNum(2);
        });
        this.transform.Find("KeyBord/3").GetComponent<Button>().onClick.AddListener(() =>
        {
            inputNum(3);
        });
        this.transform.Find("KeyBord/4").GetComponent<Button>().onClick.AddListener(() =>
        {
            inputNum(4);
        });
        this.transform.Find("KeyBord/5").GetComponent<Button>().onClick.AddListener(() =>
        {
            inputNum(5);
        });
        this.transform.Find("KeyBord/6").GetComponent<Button>().onClick.AddListener(() =>
        {
            inputNum(6);
        });
        this.transform.Find("KeyBord/7").GetComponent<Button>().onClick.AddListener(() =>
        {
            inputNum(7);
        });
        this.transform.Find("KeyBord/8").GetComponent<Button>().onClick.AddListener(() =>
        {
            inputNum(8);
        });
        this.transform.Find("KeyBord/9").GetComponent<Button>().onClick.AddListener(() =>
        {
            inputNum(9);
        });
        this.transform.Find("KeyBord/initBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            roomNumberInput.text = "";
        });

        this.transform.Find("KeyBord/deleteBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            string str = roomNumberInput.text;
            
            try
            {
                str = str.Remove(str.Length - 1);
            }
            catch (System.Exception e)
            {
                WantedTextTool.Instance.addTip("请先输入", 0);
            }
            roomNumberInput.text = str;
        });

        this.transform.Find("BtnConfuseCreate").GetComponent<Button>().onClick.AddListener(() =>
        {
            if (roomNumberInput.text.Length==6)
            {
                transform.GetComponent<EnterRoomScript>().sureRoomNumber();
            }
            else
            {
                WantedTextTool.Instance.addTip("请输入六位房间号", 0);
            }
        });
    }

    /// <summary>
    /// 检测数字是否超出范围
    /// </summary>
    private void inputNum(int num)
    {
        if (roomNumberInput.text.Length>=6)
        {
            Debug.Log("超出");
            return;
        }
        else
        {
            roomNumberInput.text += num;
        }
    }

    /// <summary>
    /// 获取输入的数字
    /// </summary>
    /// <returns></returns>
    public string getNumber()
    {
        return roomNumberInput.text;
    }

    /// <summary>
    /// 找到view
    /// </summary>
    private void findView()
    {
        roomNumberInput = transform.Find("InputRoomNumber").GetComponent<InputField>();
    }
}
