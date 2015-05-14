using UnityEngine;
using System.Collections;
using System;

public interface ISimUnitEventHandler : ISimObjectEventHandler {

	void OnSimFireWeapon(SimUnitInstance sender, Vector3 impactlocation, float impacttime, SimUnitInstance impactunit);
	void OnSimExplode(SimUnitInstance sender);
	void OnSimDropBonus(SimUnitInstance sender, SimDropConfig drop);
	void OnSimReachedGoal(SimUnitInstance sender);
}
