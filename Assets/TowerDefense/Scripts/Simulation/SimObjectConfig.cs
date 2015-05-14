using UnityEngine;
using System.Collections;

public class SimObjectConfig : ScriptableObject {
	public SimMovement Movement;
	public float RadiusOfCollision = 1.0f;
	public bool EnableCollisionCheck=false;
	public bool AddToOcttree = false;
}
