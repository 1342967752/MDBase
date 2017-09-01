using System;

namespace AssemblyCSharp
{
	[Serializable]
	public class TotalInfo
	{
		public string gang;//杠的牌 例如 1,2,3
		public string peng;
		public string chi;
		public string hu;//uuid:胡牌点数:胡牌类型
		public string genzhuang;
        public string scores;
        public string jing;//分数+冲关（0,1）+霸王（0,1）
    }
}

