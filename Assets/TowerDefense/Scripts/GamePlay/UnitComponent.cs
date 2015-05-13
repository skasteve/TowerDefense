using UnityEngine;
using System.Collections;

public class UnitComponent : MonoBehaviour, ISimUnitEventHandler {

	private SimUnit simunit;
	public bool followsim = true;

	public GameObject placementArea;
	public GameObject attackArea;

	private SimUnitInstance _simunitinst;

	public void SetSimUnit(SimUnit inst) {
		if (_simunitinst == null && inst != null) {
			simunit = inst;
			_simunitinst = SimulationComponent.CurrentSim.AddUnit(simunit,transform.position, this);

			SetAreaVisuals(simunit);
		}
	}

	// Update is called once per frame
	void Update () {
		if (followsim && _simunitinst != null) {
			this.transform.position = _simunitinst.Position;
		}
	}

	public void SetAreaVisuals(SimUnit unit)
	{
		float placementAreaScale = unit.RadiusOfPlacement / 2;
		float attackAreaScale = unit.RadiusOfAffect / 2;

		placementArea.transform.localScale = new Vector3(placementAreaScale, placementAreaScale, placementAreaScale);
		attackArea.transform.localScale = new Vector3(attackAreaScale, attackAreaScale, attackAreaScale);
	}

	public void ShowAreas(bool show)
	{
		placementArea.SetActive(show);
		attackArea.SetActive(show);
	}

	#region ISimUnitEventHandler implementation

	public void OnSimFireProjectile (SimUnitInstance sender, Vector3 impactlocation, float impacttime, SimUnitInstance impactunit)
	{
		throw new System.NotImplementedException ();
	}

	public void OnSimExplode (SimUnitInstance sender)
	{
		gameObject.BroadcastMessage("SimOnExplode",SendMessageOptions.DontRequireReceiver);
	}

	public void OnSimDropBonus (SimUnitInstance sender, SimDrop drop)
	{
		gameObject.BroadcastMessage("SimOnDropBonus", drop, SendMessageOptions.DontRequireReceiver);
	}

	public void OnSimReachedGoal(SimUnitInstance sender) 
	{
		gameObject.BroadcastMessage("SimOnReachedGoal",SendMessageOptions.DontRequireReceiver);
	}

	public void OnSimDestroy(SimUnitInstance sender) 
	{
		Destroy(gameObject);
	}
	#endregion
}
