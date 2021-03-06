using UnityEngine;
using UnityEditor;
using System.IO;

public static class ScriptableObjectUtility
{
	
	[MenuItem("TowerDefense/Simulation/Create Movement Type")]
	public static void CreateMovementType() {
		CreateAsset<SimMovement>();
	}
	
	[MenuItem("TowerDefense/Simulation/Create Weapon Type")]
	public static void CreateWeaponType() {
		CreateAsset<SimWeaponConfig>();
	}

	[MenuItem("TowerDefense/Simulation/Create Main Projectile Type")]
	public static void CreateProjectileType() {
		CreateAsset<SimProjectileConfig>();
	}

	[MenuItem("TowerDefense/Simulation/Create Unit Type")]
	public static void CreateUnitType() {
		CreateAsset<SimUnitConfig>();
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