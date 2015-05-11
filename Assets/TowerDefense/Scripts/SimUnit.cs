using UnityEngine;
using UnityEditor;
using System.IO;

public class SimUnit : ScriptableObject
{
	public enum ETeam {
		Friendly,
		Enemy
	};

	public ETeam Team = ETeam.Friendly;

	public SimMovement Movement;
	public SimProjectile Projectile;

	public float Speed = 1.0f;
	public float Health = 100.0f;
	public float FireRate = 1.0f;
	public float AreaOfPlacement = 10.0f;
	public float AreaOfAffect = 10.0f;

	public float DropBonusPct = 1.0f;
	public SimDrop DropBonus;

}