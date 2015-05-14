using UnityEngine;
using UnityEditor;
using System.IO;

public class SimUnitConfig : SimObjectConfig
{
	public enum ETeam {
		Friendly,
		Enemy
	};

	public ETeam Team = ETeam.Friendly;

	public float PowerValue = 1.0f;
	public int MinWave = 1;
	public float spawnChance = .2f;

	public SimWeaponConfig WeaponConfig;

	public float Health = 100.0f;
	public float FireRate = 1.0f;
	public float RadiusOfPlacement = 10.0f;
	public float RadiusOfAffect = 10.0f;

	public float DropBonusPct = 1.0f;
	public SimDropConfig DropBonus;

	public AudioUnitConfig AudioConfig;
	public int Cost = 1;

	public GameObject UnitPrefab;
}
