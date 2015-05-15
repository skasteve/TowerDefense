using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UnitComponent : MonoBehaviour, ISimUnitEventHandler {

	public SimUnitConfig simunit;
	public bool followsim = true;

	public UnitButton unitButton;
	public GameObject placementArea;
	public GameObject attackArea;
	public GameObject destructionEffect;

	public Action onSimDestroy;

	public delegate void SimExplodeAction(SimUnitConfig simUnit);
	public static event SimExplodeAction onSimExplode;

	private SimUnitInstance _simunitinst;

	private static IDictionary<SimUnitInstance, Transform> _unitMapping = new Dictionary<SimUnitInstance, Transform>();

	public void SetSimUnit(SimUnitConfig inst) {
		if (_simunitinst == null && inst != null) {
			simunit = inst;
			_simunitinst = SimulationComponent.CurrentSim.AddUnit(simunit,transform.position, this);
			_unitMapping.Add (_simunitinst, this.transform);

			SetAreaVisuals(simunit);
		}
		else if (_simunitinst != null && inst == null)
		{
			SimulationComponent.CurrentSim.RemoveUnit(_simunitinst);
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

	public void SetAreaVisuals(SimUnitConfig unit)
	{
		float placementAreaScale = unit.RadiusOfPlacement;
		float attackAreaScale = unit.RadiusOfAffect;

		placementArea.transform.localScale = new Vector3(placementAreaScale, .1f, placementAreaScale);
		attackArea.transform.localScale = new Vector3(attackAreaScale, .1f, attackAreaScale);
	}

	public void ShowAreas(bool show)
	{
		placementArea.SetActive(show);
		attackArea.SetActive(show);
	}

	public void EnableUnitButton()
	{
		unitButton.enabled = true;
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

	public void OnSimFireWeapon (SimUnitInstance sender, Vector3 impactlocation, float impacttime, SimUnitInstance impactunit)
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

		if (onSimExplode != null)
			onSimExplode(simunit);
	}

	public void OnSimDropBonus (SimUnitInstance sender, SimDropConfig drop)
	{
		gameObject.BroadcastMessage("SimOnDropBonus", drop, SendMessageOptions.DontRequireReceiver);
	}

	public void OnSimReachedGoal(SimUnitInstance sender) 
	{
		gameObject.BroadcastMessage("SimOnReachedGoal",SendMessageOptions.DontRequireReceiver);
	}

	public void OnDestroyEventHandler(object sender, EventArgs e)
	{
		if (onSimDestroy != null)
			onSimDestroy();

		Destroy(gameObject);

		if (destructionEffect != null)
		{
			AudioEngine.instance.PlayExplode(AudioEngine.instance.EngineConfig.UnitConfigs[1]);
			Instantiate(destructionEffect, this.transform.position, Quaternion.identity);
			CameraShake.instance.Shake();
		}
	}
	#endregion
}
