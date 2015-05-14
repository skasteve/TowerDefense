using UnityEngine;
using System.Collections;

public class ProjectileComponent : MonoBehaviour {

	private float startTime;
	private float impactTime;
	private Transform target;
	private Vector3 startPos;
	private Quaternion startRotation;
	private Quaternion lookRotation;

	public AudioWeaponConfig audioConfig;
	
	void Update()
	{
		if (target != null)
		{
			float lerpTime = (Time.time - startTime) / impactTime;
			transform.position = Vector3.Lerp(startPos, target.position, lerpTime);
			lookRotation = Quaternion.LookRotation(target.position);
			transform.rotation = Quaternion.Slerp (startRotation, lookRotation, lerpTime);
		}
	}

	public void FireProjectile(UnitComponent.EventArgsFireProjectile args)
	{
		startTime = Time.time;
		impactTime = args.impactTime;
		target = args.targetObject;
		startPos = gameObject.transform.position;
		startRotation = Quaternion.LookRotation(target.position);
		StartCoroutine(TimeOut());

		AudioEngine.instance.PlayWeaponFire(audioConfig);
	}

	private IEnumerator TimeOut()
	{
		yield return new WaitForSeconds(impactTime);
		AudioEngine.instance.PlayWeaponImpact(audioConfig);
		GameObject.Destroy(this.gameObject);
	}
}
