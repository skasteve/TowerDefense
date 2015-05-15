using UnityEngine;
using System.Collections;

public class ProjectileComponent : MonoBehaviour {

	private float startTime;
	private float impactTime;
	private Transform target;
	private Vector3 startPos;
	private Quaternion startRotation;
	private Quaternion lookRotation;

	public GameObject mussleFlash;
	public GameObject impactEffect;

	public AudioWeaponConfig audioConfig;
	
	void Update()
	{
		if (target != null)
		{
			float lerpTime = (Time.time - startTime) / impactTime;
			transform.position = Vector3.Lerp(startPos, target.position, lerpTime);
			lookRotation = Quaternion.LookRotation(target.position);
			Quaternion slerp = Quaternion.Slerp (startRotation, lookRotation, lerpTime);
			transform.rotation = new Quaternion(slerp.x, slerp.y, transform.rotation.z, transform.rotation.w);
		}

		if (startTime != null && target == null)
			GameObject.Destroy(this.gameObject);
	}

	public void FireProjectile(UnitComponent.EventArgsFireProjectile args)
	{
		startTime = Time.time;
		impactTime = args.impactTime;
		target = args.targetObject;
		startPos = gameObject.transform.position;
		startRotation = transform.rotation;//Quaternion.LookRotation(target.position);
		StartCoroutine(TimeOut());

		AudioEngine.instance.PlayWeaponFire(audioConfig);
		GameObject mussleParticle = (GameObject)Instantiate(mussleFlash, this.transform.position, Quaternion.identity);
	}

	private IEnumerator TimeOut()
	{
		yield return new WaitForSeconds(impactTime);
		AudioEngine.instance.PlayWeaponImpact(audioConfig);
		GameObject impactParticle = (GameObject)Instantiate(impactEffect, this.transform.position, Quaternion.identity);
		GameObject.Destroy(this.gameObject);
	}
}
