using System;
/// <summary>
///玩家每项内容 
/// </summary>
namespace AssemblyCSharp
{
	[Serializable]
	public class ZhanjiRoomDataItemVo
	{
		public int id;
		public int roomid;
		public string  content;
		public DateVo createtime;
		public ZhanjiRoomDataItemVo ()
		{
			
		}
	}
}

