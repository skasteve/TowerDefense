using UnityEngine;
using System.Collections;

public interface ISimUnitEventHandler {

	void OnFireProjectile(Vector3 impactlocation, float impacttime, SimUnitInstance impactunit);
	void OnExplode();
	void OnDropBonus(SimDrop drop);

}
