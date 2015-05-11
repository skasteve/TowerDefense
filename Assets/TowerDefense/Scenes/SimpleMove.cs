using UnityEngine;
using System.Collections;

public class SimpleMove : MonoBehaviour {

	public float maxspeed = 10.0f;
	private float speed = 1.0f;
	
	// Use this for initialization
	void Start () {
		speed = maxspeed * Random.value;
	}
	
	// Update is called once per frame
	void Update () {
		float newz = transform.position.z - (Time.deltaTime * speed);
		if(newz<-10.0f) {
			newz = 10.0f;
			speed = maxspeed * Random.value;
		}
		transform.position = new Vector3(transform.position.x,transform.position.y,newz);

	}
}
