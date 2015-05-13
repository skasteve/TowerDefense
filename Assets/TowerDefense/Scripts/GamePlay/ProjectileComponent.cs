using UnityEngine;
using System.Collections;

public class ProjectileComponent : MonoBehaviour {

	private float startTime;
	private float impactTime;
	private Transform targetPos;
	private Vector3 startPos;
	
	void Update()
	{
		if (targetPos != null)
		{
			float lerpTime = (Time.time - startTime) / impactTime;
			transform.position = Vector3.Lerp(startPos, targetPos.position, lerpTime);
		}
	}

	public void FireProjectile(UnitComponent.EventArgsFireProjectile args)
	{
		startTime = Time.time;
		impactTime = args.impactTime;
		targetPos = args.targetObject;
		startPos = gameObject.transform.position;
	}

	private IEnumerator TimeOut()
	{
		yield return new WaitForSeconds(impactTime);
		GameObject.Destroy(this.gameObject);
	}
}
