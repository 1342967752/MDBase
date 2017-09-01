using System;

namespace AssemblyCSharp
{
	[Serializable]
	public class RoomCreateVo
	{
		public  bool hong;
		public int ma;
		public int roomId;
		public int roomType;//1南昌；2、划水；3、长沙
		/**局数**/
		public int roundNumber;
		public bool sevenDouble;
		public int ziMo;//1：自摸胡；2、抢杠胡
		public int xiaYu;
		public string name;
		public bool addWordCard;
		public int magnification;

        //回放时使用
        public int currentRound;//单前局数
        public int id;//回放id
        public JingResponse jingPaiVo;//精牌vo
        //色子点数
        public int pointOne;
        public int pointTwo;
    }
}

