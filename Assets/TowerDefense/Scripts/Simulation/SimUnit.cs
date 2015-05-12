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

	public float PowerValue = 1.0f;

	public SimMovement Movement;
	public SimProjectile Projectile;

	public float Speed = 1.0f;
	public float Health = 100.0f;
	public float FireRate = 1.0f;
	public float RadiusOfCollision = 1.0f;
	public float RadiusOfPlacement = 10.0f;
	public float RadiusOfAffect = 10.0f;

	public float DropBonusPct = 1.0f;
	public SimDrop DropBonus;

	public int Cost = 1;

}