using UnityEngine;
using System.Collections;

public class ProjectileSpawner : MonoBehaviour {

	public ProjectileComponent[] projectiles;

	public void fireProjectile(UnitComponent.EventArgsFireProjectile args)
	{
		foreach(ProjectileComponent pc in projectiles)
		{
			ProjectileComponent newProjectile =  (ProjectileComponent)Instantiate(pc, transform.position, transform.rotation);
			newProjectile.FireProjectile(args);
		}
	}
}
