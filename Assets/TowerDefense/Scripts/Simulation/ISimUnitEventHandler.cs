using UnityEngine;
using System.Collections;
using System;

public interface ISimUnitEventHandler : ISimObjectEventHandler {

	void OnSimFireProjectile(SimUnitInstance sender, Vector3 impactlocation, float impacttime, SimUnitInstance impactunit);
	void OnSimExplode(SimUnitInstance sender);
	void OnSimDropBonus(SimUnitInstance sender, SimDrop drop);
	void OnSimReachedGoal(SimUnitInstance sender);
}
