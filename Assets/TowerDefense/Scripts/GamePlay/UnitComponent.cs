using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitComponent : MonoBehaviour, ISimUnitEventHandler {

	private SimUnit simunit;
	public bool followsim = true;

	public GameObject placementArea;
	public GameObject attackArea;

	private SimUnitInstance _simunitinst;

	private static IDictionary<SimUnitInstance, Transform> _unitMapping = new Dictionary<SimUnitInstance, Transform>();

	public void SetSimUnit(SimUnit inst) {
		if (_simunitinst == null && inst != null) {
			simunit = inst;
			_simunitinst = SimulationComponent.CurrentSim.AddUnit(simunit,transform.position, this);
			_unitMapping.Add (_simunitinst, this.transform);

			SetAreaVisuals(simunit);
		}
	}

	void OnDestroy()
	{
		if (_simunitinst != null)
			_unitMapping.Remove(_simunitinst);
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

	public class EventArgsFireProjectile{
		public Vector3 impactLocation;
		public float impactTime;
		public Transform targetObject;
		public Transform sourceObject;

		public EventArgsFireProjectile(Vector3 impactLocation, float impactTime, Transform targetObject, Transform sourceObject)
		{
			this.impactLocation = impactLocation;
			this.impactTime = impactTime;
			this.targetObject = targetObject;
			this.sourceObject = sourceObject;
		}
	}

	public void OnSimFireProjectile (SimUnitInstance sender, Vector3 impactlocation, float impacttime, SimUnitInstance impactunit)
	{
		Transform target = null;

		_unitMapping.TryGetValue(impactunit, out target);

		EventArgsFireProjectile args = new EventArgsFireProjectile(impactlocation, impacttime, target, this.transform); 
		ProjectileSpawner[] spawners = GetComponentsInChildren<ProjectileSpawner>();

		foreach(ProjectileSpawner sp in spawners)
		{
			sp.fireProjectile(args);
		}
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
