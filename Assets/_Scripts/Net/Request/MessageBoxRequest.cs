using System;

namespace AssemblyCSharp
{
	public class MessageBoxRequest : ClientRequest
	{
		public MessageBoxRequest (MesssageType type,int codeIndex,int uuid)//0 是发消息 1.是发表情 2.是发动画
		{
			headCode = APIS.MessageBox_Request;
			
            switch ((int)type)
            {
                case 0:; break;
                case 1:
                    messageContent = ((int)type) + "|" + codeIndex + "|" + uuid;
                    break;
                case 2:
                    messageContent = ((int)type) + "|" + codeIndex + "|" + uuid; ; break;
                case 3:
                    messageContent = ((int)type) + "|" + GlobalDataScript.loginResponseData.account.uuid + "|" + codeIndex + "|"+ uuid;break;
            }
        }
	}

    public  enum MesssageType:int
    {
        None,
        Msg,
        Emoji,
        Anim
    }
}

