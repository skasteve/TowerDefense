using UnityEngine;
using System.Collections;

public class SimProjectileSpawner : MonoBehaviour {

	public SimProjectileComponent ProjectileComponent;

	public void FireProjectile() {
		if(SimulationComponent.CurrentSim!=null) {
			Instantiate(ProjectileComponent, transform.position, transform.rotation);
		}
	}
}
