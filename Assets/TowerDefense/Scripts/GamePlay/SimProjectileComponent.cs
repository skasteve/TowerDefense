using UnityEngine;
using System.Collections;
using System;

public class SimProjectileComponent : MonoBehaviour {

	public SimProjectileConfig ProjectileConfig;
	private SimProjectileInstance ProjectileInstance;

	void Start() {
		SetProjectileConfig(ProjectileConfig);
	}

	public void SetProjectileConfig(SimProjectileConfig config) {
		if(ProjectileInstance!=null) {
			throw new Exception("ProjectileInstance is already set!!!!");
		}

		ProjectileInstance = SimulationComponent.CurrentSim.AddProjectile(config, transform.position);
		ProjectileInstance.OnCollision += OnCollision;
		ProjectileInstance.OnDestroy += OnSimDestroy;
	}

	// Update is called once per frame
	void Update () {
		transform.position = ProjectileInstance.Position;
	}

	void OnCollision(SimProjectileInstance inst, SimProjectileInstance.OnCollisionEventArgs args) {
		gameObject.BroadcastMessage("SimOnExplode",SendMessageOptions.DontRequireReceiver);
		Destroy(gameObject);
	}

	void OnSimDestroy(object sender, EventArgs args) {
		Destroy(gameObject);
	}

}
