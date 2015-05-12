using UnityEngine;
using System.Collections;

public interface ISimUnitEventHandler {

	void OnFireProjectile(SimUnitInstance sender, Vector3 impactlocation, float impacttime, SimUnitInstance impactunit);
	void OnExplode(SimUnitInstance sender);
	void OnDropBonus(SimUnitInstance sender, SimDrop drop);

}
