using UnityEngine;
using System.Collections;
using System;

namespace cn.sharesdk.unity3d 
{
	[Serializable]
	public class DevInfoSet

	{
		
		
		public WeChat wechat;
		public WeChatMoments wechatMoments; 
		

		#if UNITY_ANDROID

		#elif UNITY_IPHONE		
		public Copy copy;
		public YixinFavorites yixinFavorites;					//易信收藏，仅iOS端支持							[仅支持iOS端]
		public YixinSeries yixinSeries;							//iOS端易信系列, 可直接配置易信三个子平台			[仅支持iOS端]
		public WechatSeries wechatSeries;						//iOS端微信系列, 可直接配置微信三个子平台 		[仅支持iOS端]
		public QQSeries qqSeries;								//iOS端QQ系列,  可直接配置QQ系列两个子平台		[仅支持iOS端]
		public KakaoSeries kakaoSeries;							//iOS端Kakao系列, 可直接配置Kakao系列两个子平台	[仅支持iOS端]
		public EvernoteInternational evernoteInternational;		//iOS配置印象笔记国内版在Evernote中配置;国际版在EvernoteInternational中配置												 
		//安卓配置印象笔记国内与国际版直接在Evernote中配置														
		#endif

	}

	public class DevInfo 
	{	
		public bool Enable = true;
	}

	[Serializable]
	public class WeChat : DevInfo 
	{	
		#if UNITY_ANDROID
		public string SortId = "5";
		public const int type = (int) PlatformType.WeChat;
		public string AppId = "wx406ef08c0d6312b6";
		public string AppSecret = "418971c7a3c2fac8f995b0d374e278f6";

        public bool BypassApproval = true;
#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WeChat;
		public string app_id = "wx406ef08c0d6312b6";
		public string app_secret = "35acde159c85d09e8c17ff98a5fc1935";
#endif
    }

    [Serializable]
	public class WeChatMoments : DevInfo 
	{
		#if UNITY_ANDROID
		public string SortId = "6";
		public const int type = (int) PlatformType.WeChatMoments;
		public string AppId = "wx406ef08c0d6312b6";

        public string AppSecret = "418971c7a3c2fac8f995b0d374e278f6";

        public bool BypassApproval = false;
		#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WeChatMoments;
		public string app_id = "wx4868b35061f87885";
		public string app_secret = "64020361b8ec4c99936c0e3999a9f249";
		#endif
	}

}
