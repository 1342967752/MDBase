using System;
/****
 * 个人账户信息类 
 */ 
namespace AssemblyCSharp
{
	[Serializable]
	public class Account
	{
		public string openid;

		public string nickname;

		public string headicon;

		public string unionid;

		public string province;

		public string city;

		public int sex;

		public int roomcard;

		public int uuid;

	    public int prizecount;

		public int actualcard;
		public DateVo createtime;
		public int id;
		public DateVo lastlogintime;
		public int managerUpId;
		public int totalcard;
		public string status;
		public string isGame;
	}
}

