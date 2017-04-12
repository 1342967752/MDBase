using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	[Serializable]

	public class HupaiResponseItem
	{
		public int[]paiArray;
		public TotalInfo totalInfo;
		public string huType;
		public string nickname;
		public int gangScore;//杠分
		public int totalScore;//总分
		public int uuid;
		private List<int> maPonits;
        public int huScore;//胡的分数
        public int jingScore;
		

		public void setMaPoints(List<int> mas){
			maPonits = mas;
		}
		public List<int> getMaPoints(){
			return  maPonits;
		}
	}
}

