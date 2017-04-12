using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	[Serializable]
	public class HupaiResponseVo
	{
		public string type;//结束类型0是正常结束，1流局 2是中途解散
		public string allMas;//抓码
		public string currentScore;//uuid:分数,(本局)
		public List<int> validMas;
        public int huMode;//胡牌方式  胡牌方式 0-天胡 1-地胡 2-杠上开花 3-抢杠胡 4-自摸 5-放炮

        public int huType;//胡牌类型 返回胡牌类型 0-没有胡牌 1-南昌平胡 2-碰碰胡 3-七小对 4-十三烂 5-七星十三烂
                            //* 6-德国平胡 7-德国碰碰胡 8-德国七小对 9-德国十三烂
                                //* 10-精吊平胡 11-精吊德国平胡 12-精吊七小对 13-精吊德国七小对 14-精吊碰碰胡 15-精吊德国碰碰胡

        public List<HupaiResponseItem> avatarList;
	
	}
}

