using UnityEngine;
using System.Collections;
using System;

public class SimProjectileComponent : MonoBehaviour {
	
	public SimProjectileConfig ProjectileConfig;

	public GameObject mussleFlash;
	public GameObject impactEffect;

	public AudioWeaponConfig audioConfig;

	private SimProjectileInstance ProjectileInstance;

	void Start() {
		SetProjectileConfig(ProjectileConfig);
	}

	public void SetProjectileConfig(SimProjectileConfig config) {
		if(ProjectileInstance!=null) {
			throw new Exception("ProjectileInstance is already set!!!!");
		}

		AudioEngine.instance.PlayWeaponFire(audioConfig);
		Instantiate(mussleFlash, this.transform.position, Quaternion.identity);

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
		AudioEngine.instance.PlayWeaponImpact(audioConfig);
		Instantiate(impactEffect, this.transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

	void OnSimDestroy(object sender, EventArgs args) {
		Destroy(gameObject);
	}

}
