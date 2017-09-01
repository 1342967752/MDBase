using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;
using System.Collections.Generic;
/// <summary>
/// 进入房间拨号盘
/// </summary>
public class EnterRoomPage : TTUIPage {
    public EnterRoomPage() : base(UIType.Fixed, UIMode.HideOther, UICollider.None)
	{
        uiPath = MyPath.EnterRoomPagePath;
    }
    private List<Transform> inputLabelList=new List<Transform>();
    private InputField roomNumberInput;
    public const int ROOMNUMBERLENGTH = 6;
    public override void Awake(GameObject go)
    {
        MJUIManager._instance.enterRoomPage = this;
        findView();
        setBtnClickListener();
    }

    public override void Active()
    {
        gameObject.SetActive(true);
        cleanNum();
    }

    /// <summary>
    /// 清除数字
    /// </summary>
    private void cleanNum()
    {
        for (int i = 0; i < inputLabelList.Count; i++)
        {
            inputLabelList[i].GetComponent<Text>().text = "";
        }
        inputLength = 0;
    }

    /// <summary>
    /// 设置按钮监听
    /// </summary>
    private void setBtnClickListener()
    {
        this.transform.Find("Bg/BtnBack").GetComponent<Button>().onClick.AddListener(() =>
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
            cleanNum();
        });

        this.transform.Find("KeyBord/deleteBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            if (inputLength > 0)
                inputLength--;
            inputLabelList[inputLength].GetComponent<Text>().text = "";
        });

        this.transform.Find("BtnConfuseCreate").GetComponent<Button>().onClick.AddListener(() =>
        {
            if (inputLength == 6)
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
    int inputLength = 0; 
    private void inputNum(int num)
    {
        if (inputLength==6)
        {
            return;
        }

        if (inputLength<6)
        {
            inputLabelList[inputLength].GetComponent<Text>().text = num + "";
            inputLength++;
        }

        if (inputLength == 6)
        {
            transform.GetComponent<EnterRoomScript>().sureRoomNumber();
            MJUIManager._instance.loadingPage.Active();
        }        
    }

    /// <summary>
    /// 获取输入的数字
    /// </summary>
    /// <returns></returns>
    public string getNumber()
    {
        string input = "";
        for(int i = 0; i < inputLength; i++)
        {
            input += inputLabelList[i].GetComponent<Text>().text;
        }
        return input;
    }

    /// <summary>
    /// 找到view
    /// </summary>
    private void findView()
    {
        Debug.Log(ROOMNUMBERLENGTH);
        inputLabelList.Clear();
        inputLength = 0;
        for (int i = 0; i < ROOMNUMBERLENGTH; i++)
        {
            string name = "InputField/Input" + i+ "/Text";
            Debug.Log(name);
            inputLabelList.Add(transform.Find(name));
        }
        
    }

  
}
