using UnityEngine;
using System.Collections;

public class SimulationComponent : MonoBehaviour, ISimUnitEventHandler {

	public Transform goal;

	public SimUnit friendlydef;
	public SimUnit enemydef;


	private Simulation sim;
	private SimUnitInstance friendlyunit;
	private SimUnitInstance enemyunit;

	// Use this for initialization
	void StartSim (int seed) {
		if(sim==null) {
			Plane goalplane = new Plane(goal.rotation * Vector3.forward, goal.position);
			sim = new Simulation(seed,goalplane);		
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(sim!=null) {
			sim.Update(Time.deltaTime);
		}
	}

	void OnGUI() {
		if(sim==null && GUILayout.Button("StartSim")) {
			StartSim(0);
		}
		if(sim!=null && GUILayout.Button ("StopSim")) {
			sim = null;
		}
		if(GUILayout.Button ("Add Friendly Unit")) {
			friendlyunit = sim.AddUnit(friendlydef, Vector3.zero, this);
		}
		if(GUILayout.Button ("Add Enemy Unit")) {
			enemyunit = sim.AddUnit(enemydef, Vector3.forward * 20.f, this);
		}
	}


	#region ISimUnitEventHandler implementation
	public void OnFireProjectile (SimUnitInstance sender, Vector3 impactlocation, float impacttime, SimUnitInstance impactunit)
	{
		throw new System.NotImplementedException ();
	}
	public void OnExplode (SimUnitInstance sender )
	{
		throw new System.NotImplementedException ();
	}
	public void OnDropBonus (SimUnitInstance sender, SimDrop drop)
	{
		throw new System.NotImplementedException ();
	}
	#endregion
}
