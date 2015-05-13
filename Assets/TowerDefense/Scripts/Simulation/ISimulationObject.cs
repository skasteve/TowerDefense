using UnityEngine;
using System.Collections;

public interface ISimulationObject : IOctreeObject {
	Vector3 Position { get; }
	bool DeleteMe { get; }
	void Step(float deltatime);
}
