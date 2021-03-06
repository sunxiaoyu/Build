﻿using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;


namespace Assets.Editor.Scripts.Build
{
	class BuildXcode
	{
		private string productName = "⚓巅峰战舰";
		private string bundleIdentifier = "com.games.DFZJ.AppStore";
		private string infoDictionaryVersion = "6.0";
		private string shortVersionString = "1.0.0";   //Version
		private string bundleVersion = "1.0.0";        //Build
		
		[PostProcessBuild]
		public static void OnPostprocessBuild(BuildTarget BuildTarget, string path)
		{
			if (BuildTarget == BuildTarget.iPhone)
			{
				ProcessProject(path);
				ProcessPlist(path);

			}
		}

		private static void ProcessProject(string path)
		{
			string projPath = PBXProject.GetPBXProjectPath(path);
			PBXProject proj = new PBXProject();
			proj.ReadFromString(File.ReadAllText(projPath));
			
			//获取当前项目名字  
			string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
			
			//对所有的编译配置设置选项  
			//                proj.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
			proj.SetBuildProperty(target, "PRODUCT_BUNDLE_IDENTIFIER", "com.games.DFZJ.AppStore");
			proj.SetBuildProperty(target, "OTHER_LDFLAGS", "-ObjC -licucore");
			proj.SetBuildProperty(target, "PRODUCT_NAME", "⚓巅峰战舰");
			
			//添加依赖库
			proj.AddFrameworkToProject(target, "libiPhone-lib.a", false);
			proj.AddFrameworkToProject(target, "libc++.1.tbd", false);
			proj.AddFrameworkToProject(target, "libsqlite3.tbd", false);
			proj.AddFrameworkToProject(target, "ImageIO.framework", false);
			proj.AddFrameworkToProject(target, "WebKit.framework", false);
			
			//设置签名  
			//                proj.SetBuildProperty(target, "CODE_SIGN_IDENTITY", "iPhone Distribution: _______________");
			//                proj.SetBuildProperty(target, "PROVISIONING_PROFILE", "********-****-****-****-************");
			
			//保存工程  
			proj.WriteToFile(projPath);
		}

		private static void ProcessPlist(string path)
		{
			string plistPath = path + "/Info.plist";
			PlistDocument plist = new PlistDocument();
			plist.ReadFromString(File.ReadAllText(plistPath));
			PlistElementDict rootDict = plist.root;
			
			//PlayerSettings.bundleIdentifier
			rootDict.SetString("CFBundleName", "${PRODUCT_NAME}");
			rootDict.SetString("CFBundleDisplayName", "${PRODUCT_NAME}");
			rootDict.SetString("CFBundleIdentifier", "$(PRODUCT_BUNDLE_IDENTIFIER)");
			rootDict.SetString("CFBundleDevelopmentRegion", "China");
			rootDict.SetString("CFBundleInfoDictionaryVersion", "6.0");
			rootDict.SetString("CFBundleShortVersionString", "1.0.0");
			rootDict.SetString("CFBundleVersion", "1.0.0");
			
			//NSAppTransportSecurity
			var transportSecurity = rootDict.CreateDict("NSAppTransportSecurity");
			transportSecurity.SetBoolean("NSAllowsArbitraryLoads", true);
			
			//UISupportedInterfaceOrientations
			var orientation = rootDict.CreateArray("UISupportedInterfaceOrientations");
			orientation.AddString("UIInterfaceOrientationLandscapeRight");
			orientation.AddString("UIInterfaceOrientationLandscapeLeft");
			
			//直播所需要的声明，iOS10必须  
			rootDict.SetString("NSContactsUsageDescription", "App需要您的同意,才能访问通讯录");
			rootDict.SetString("NSCalendarsUsageDescription", "App需要您的同意,才能访问日历");
			rootDict.SetString("NSCameraUsageDescription", "App需要您的同意,才能使用相机");
			rootDict.SetString("NSLocationUsageDescription", "App需要您的同意,才能访问位置");
			rootDict.SetString("NSLocationWhenInUseUsageDescription", "App需要您的同意,才能在使用期间访问位置");
			rootDict.SetString("NSLocationAlwaysUsageDescription", "App需要您的同意,才能始终访问位置");
			rootDict.SetString("NSMicrophoneUsageDescription", "App需要您的同意,才能使用麦克风");
			rootDict.SetString("NSPhotoLibraryUsageDescription", "App需要您的同意,才能访问相册");
			
			// LSApplicationQueriesSchemes
			var queriesSchemes = rootDict.CreateArray("LSApplicationQueriesSchemes");
			queriesSchemes.AddString("mqqapi");
			queriesSchemes.AddString("mqqopensdkapiV4");
			queriesSchemes.AddString("mqqopensdkapiV3");
			queriesSchemes.AddString("mqqopensdkapiV2");
			queriesSchemes.AddString("mqqapiwallet");
			queriesSchemes.AddString("mqqwpa");
			queriesSchemes.AddString("mqqbrowser");
			queriesSchemes.AddString("weixin");
			queriesSchemes.AddString("wechat");
			queriesSchemes.AddString("mqqOpensdkSSoLogin");
			queriesSchemes.AddString("mqzone");
			queriesSchemes.AddString("sinaweibo");
			queriesSchemes.AddString("sinaweibohd");
			queriesSchemes.AddString("sinaweibo");
			queriesSchemes.AddString("weibosdk");
			queriesSchemes.AddString("weibosdk2.5");
			queriesSchemes.AddString("mqq");
			queriesSchemes.AddString("mqzoneopensdk");
			
			//CFBundleURLTypes
			var urlTypes = rootDict.CreateArray("CFBundleURLTypes");
			
			//weibo
			var dictWeibo = urlTypes.AddDict();
			dictWeibo.SetString("CFBundleTypeRole", "Editor");
			dictWeibo.SetString("CFBundleURLName", "com.weibo");
			var dictWeiboDic = dictWeibo.CreateArray("CFBundleURLSchemes");
			dictWeiboDic.AddString("wb639535745");
			
			//weixin
			var dictWeiXin = urlTypes.AddDict();
			dictWeiXin.SetString("CFBundleTypeRole", "Editor");
			dictWeiXin.SetString("CFBundleURLName", "");
			var dictWeiXinDic = dictWeiXin.CreateArray("CFBundleURLSchemes");
			dictWeiXinDic.AddString("wxb80d3faba4d24a1c");
			
			//qq
			var dictQQ1 = urlTypes.AddDict();
			dictQQ1.SetString("CFBundleTypeRole", "Editor");
			dictQQ1.SetString("CFBundleURLName", "");
			var dictQQDic1 = dictQQ1.CreateArray("CFBundleURLSchemes");
			dictQQDic1.AddString("tencent1105220627");
			
			var dictQQ2 = urlTypes.AddDict();
			dictQQ2.SetString("CFBundleTypeRole", "Editor");
			dictQQ2.SetString("CFBundleURLName", "");
			var dictQQDic2 = dictQQ2.CreateArray("CFBundleURLSchemes");
			dictQQDic2.AddString("QQ1105220627");
			
			// 保存plist  
			plist.WriteToFile(plistPath);
		}
	}
}