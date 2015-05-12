using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public static class MenuAudioEngine {
	[MenuItem("TowerDefense/Audio/Create Audio Engine")]
	public static void CreateAudioEngineConfig() {
		CreateAsset<AudioEngineConfig>();
	}

	[MenuItem("TowerDefense/Audio/Create Audio Unit", false, 10)]
	public static void CreateAudioUnitConfig() {
		CreateAsset<AudioUnitConfig>();
	}

	[MenuItem("TowerDefense/Audio/Create Audio Projectile", false, 10)]
	public static void CreateAudioProjectileConfig() {
		CreateAsset<AudioProjectileConfig>();
	}

	[MenuItem("TowerDefense/Audio/Create Audio Collection", false, 10)]
	public static void CreateAudioClipsCollection() {
		CreateAsset<AudioClipsCollection>();
	}

	/// <summary>
	//	This makes it easy to create, name and place unique new ScriptableObject asset files.
	/// </summary>
	public static void CreateAsset<T> () where T : ScriptableObject
	{
		T asset = ScriptableObject.CreateInstance<T> ();
		
		string path = AssetDatabase.GetAssetPath (Selection.activeObject);
		if (path == "") 
		{
			path = "Assets";
		} 
		else if (Path.GetExtension (path) != "") 
		{
			path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
		}
		
		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/New " + typeof(T).ToString() + ".asset");
		
		AssetDatabase.CreateAsset (asset, assetPathAndName);
		
		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = asset;
	}
}
