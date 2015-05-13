using UnityEngine;
using System.Collections;

public class SimulationComponent : MonoBehaviour {

	public Transform goal;

	public SimUnit friendlydef;
	public SimUnit enemydef;

	public GameObject testUnit;

	public bool ShowDebugUI = false;

	public static Simulation CurrentSim {
		get;
		private set;
	}

	// Use this for initialization
	public void StartSim (int seed) {
		if(CurrentSim==null) {
			Plane goalplane = new Plane(goal.rotation * Vector3.forward, goal.position);
			CurrentSim = new Simulation(seed,goalplane);		
		}
	}

	public void AddUnit(GameObject go, SimUnit unit) {
		UnitComponent ucomp = go.AddComponent<UnitComponent>();
		ucomp.SetSimUnit(unit);
	}

	// Update is called once per frame
	void Update () {
		if(CurrentSim!=null) {
			CurrentSim.Update(Time.deltaTime);
		}
	}

	void OnGUI() {
		if(ShowDebugUI) {
			if(CurrentSim==null && GUILayout.Button("StartSim")) {
				StartSim(0);
			}
			if(CurrentSim!=null && GUILayout.Button ("StopSim")) {
				CurrentSim = null;
			}
			if(CurrentSim!=null && GUILayout.Button ("Add Friendly Unit")) {
				GameObject inst = (GameObject)Instantiate(testUnit,Random.insideUnitSphere * Random.Range(1,50),Quaternion.identity);
				AddUnit(inst,friendlydef);
			}
			if(CurrentSim!=null && GUILayout.Button ("Add Enemy Unit")) {
				GameObject inst = (GameObject)Instantiate(testUnit,Random.insideUnitSphere * Random.Range(1,50),Quaternion.identity);
				AddUnit(inst,enemydef);
			}
		}
	}
}
