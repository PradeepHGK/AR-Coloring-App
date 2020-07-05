using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CreateAssetBundles
{
	[MenuItem("Assets/Build AssetBundles")]

	static void BuildAllAssetBundles()
	{
		string assetBundleDirectory = "Assets/Assetbundle/";

		if(!Directory.Exists(assetBundleDirectory))
		{
			Directory.CreateDirectory(assetBundleDirectory);
		}

		//AssetImporter.GetAtPath(assetBundleDirectory).SetAssetBundleNameAndVariant("Volume1" + DateTime.Today.Minute, "");
		BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.UncompressedAssetBundle, EditorUserBuildSettings.activeBuildTarget);

		#if UNITY_ANDROID
		BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
		#endif


		#if UNITY_IOS
		BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.iOS);
		#endif

		AssetDatabase.Refresh ();
	}
}