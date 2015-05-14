using UnityEngine;
using System.Collections;

public class TerrainMover : MonoBehaviour {

	private const float MOVEMENT_SPEED = -0.05f;

	void Update()
	{
		Vector3 currentPos = this.transform.localPosition;
		this.transform.localPosition = new Vector3(currentPos.x, currentPos.y, currentPos.z + MOVEMENT_SPEED);
	}
}
