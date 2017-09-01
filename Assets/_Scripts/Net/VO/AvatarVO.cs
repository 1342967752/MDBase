using System;
/**
 * 登录接口返回数据封装
 * 
 */ 
namespace AssemblyCSharp
{
	public class AvatarVO
	{
		public Account account;

		public bool isOnLine;
		public bool isReady;
		public bool main;
		public int roomId;
		public int[] chupais;//出牌
		public int commonCards;//剩余牌数
		public int[][] paiArray;
		public HupaiResponseItem huReturnObjectVO;//胡牌才有数据，登录过程不管
		public  int scores;//分数
		public string IP;
        public int huType;
        public int queType;
        public int winTimes;
        public int drawTimes;
        public int loseTimes;
        public string locationName;//位置信息
        


        public bool hasMopaiChupai;


        public void resetData(){
			//isOnLine = false;
			isReady = false;
			main = false;
			roomId = 0;


            chupais = null;
            commonCards = 0;
            paiArray = null;
            huReturnObjectVO = null;
		}
	}
}

