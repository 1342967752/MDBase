using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	[Serializable]
	public class RoomJoinResponseVo
	{
		public bool addWordCard;
		public bool hong;
		public int ma;
		public string name;
		public int roomId;
		public int roomType;
		public int roundNumber;
		public bool sevenDouble;
		public int xiaYu;
		public int ziMo;
		public int magnification;
		public List<AvatarVO> playerList;
        public int currentRound;
        public int id;
        public object endStatistics;

        //精牌
        public JingResponse jingPaiVo;

        //记录骰子
        public int pointOne=-1;
        public int pointTwo=-1;


        //public LastOperationVo lastOperationVo;
        public RoomJoinResponseVo()
        {
        }
    }
}

