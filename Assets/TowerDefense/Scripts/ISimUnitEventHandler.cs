using UnityEngine;
using System.Collections;

public interface ISimUnitEventHandler {

	void OnFireProjectile(Vector3 tolocation, SimUnit atunit);
	void OnExplode();
	void OnDrop(SimDrop drop);

}
