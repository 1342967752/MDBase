using AssemblyCSharp;
using LitJson;

public class LoginRequest : ClientRequest
{

    public LoginRequest(string data)
    {
        headCode = APIS.LOGIN_REQUEST;

        if (data == null)
        {
            LoginVo loginvo = new LoginVo();

            loginvo.openId = "123";


            loginvo.nickName = "123";
            loginvo.headIcon = "imgicon";
            loginvo.unionid = "123";
            loginvo.province = "21sfsd";
            loginvo.city = "afafsdf";
            loginvo.sex = 1;
            //loginvo.IP = GlobalDataScript.getInstance().getIpAddress();
            data = JsonMapper.ToJson(loginvo);

            GlobalDataScript.loginVo = loginvo;
            GlobalDataScript.loginResponseData = new AvatarVO();
            GlobalDataScript.loginResponseData.account = new Account();
            GlobalDataScript.loginResponseData.account.city = loginvo.city;
            GlobalDataScript.loginResponseData.account.openid = loginvo.openId;
            GlobalDataScript.loginResponseData.account.nickname = loginvo.nickName;
            GlobalDataScript.loginResponseData.account.headicon = loginvo.headIcon;
            GlobalDataScript.loginResponseData.account.unionid = loginvo.city;
            GlobalDataScript.loginResponseData.account.sex = loginvo.sex;
            GlobalDataScript.loginResponseData.IP = loginvo.IP;
        }
        messageContent = data;
    }



    //退出登录
    public LoginRequest()
    {
        headCode = APIS.QUITE_LOGIN;
        if (GlobalDataScript.loginResponseData != null)
        {
            messageContent = GlobalDataScript.loginResponseData.account.uuid + "";
        }
    }


}

