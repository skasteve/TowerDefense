using UnityEngine;
using System.Collections;

public class PlaneFlight : MonoBehaviour {
	public float wobblespeed = 2.0f;
	public float wobbleamount = 1.0f;
	public float maxrolldegrees = 45.0f;
	public float maxrollspeed = 1.0f;

	private Vector3 randomVect;
	private Vector3 startPos;

	private float newwoobletime;
	private float alphafactor = 2.0f;

	// Update is called once per frame
	void Update () {
		if(Time.time>=newwoobletime) {
			NewWobble();
		}
		Vector3 newpos = Vector3.Lerp(transform.position, transform.parent.position + randomVect, 0.01f * alphafactor);
		Vector3 deltaPos = newpos - transform.position;

		//Calc roll to recover
		float roll = Mathf.Clamp((-deltaPos.x/Time.deltaTime)/maxrollspeed*maxrolldegrees,-maxrolldegrees,maxrolldegrees);
		transform.localRotation = Quaternion.Lerp(transform.localRotation,Quaternion.Euler(new Vector3(0.0f,0.0f,roll)), 0.1f);
		transform.position = newpos;
	}

	void NewWobble() {
		randomVect = Random.onUnitSphere * wobbleamount;
		randomVect.z = 0.0f;
		float dist = Vector3.Distance(transform.localPosition, randomVect);
		alphafactor = dist/wobblespeed;
		newwoobletime = Time.time + (alphafactor);
	}
}
