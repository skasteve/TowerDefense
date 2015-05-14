using UnityEngine;
using System.Collections;
using System;

public class SimObjectInstance : IOctreeObject {
	
	public delegate void OnDestroyEventHandler(object sender, EventArgs e);
	public event OnDestroyEventHandler OnDestroy;

	public SimObject SimObjectConfig {
		get;
		private set;
	}

	protected Vector3 simposition;
	private Vector3 oldposition;
	public Vector3 Position {
		get {
			//Interpolate the position from last to next
			return Vector3.Lerp(oldposition, simposition, Sim.SimTimeAlpha);
		}
		private set {
			oldposition = simposition;
			simposition = value;
			UpdateOcttree();
		}
	}

	private float _RadiusOfCollision;
	public float RadiusOfCollision {
		get {
			return _RadiusOfCollision;
		}
		set {
			_RadiusOfCollision = value;
			UpdateOcttree();
		}
	}

	public Bounds ObjectBounds {
		get {
			return new Bounds(simposition, Vector3.one * RadiusOfCollision);
		}
	}
	
	public Simulation Sim {
		get;
		private set;
	}

	public bool Collides;

	public object MovementData;

	public bool DeleteMe {
		get;
		private set;
	}

	public SimObjectInstance(Simulation sim, SimObject simobject, Vector3 startpos) {
		Sim = sim;
		SimObjectConfig = simobject;
		oldposition = startpos;
		simposition = startpos;
	}

	private void UpdateOcttree() {
		Sim.Octtree.Remove(this);
		Sim.Octtree.Add(this, ObjectBounds);
	}

	public virtual void Step(float deltatime) {
		if(!DeleteMe) {
			EvaluateMovement(deltatime);
		}
	}

	private void EvaluateMovement(float deltatime) {
		// Units lock step so only do movement updates here, and any deterministic logic
		if(SimObjectConfig.Movement!=null) {
			Position = simposition + SimObjectConfig.Movement.Integrate(this, deltatime);
		}
	}

	protected virtual void EvaluateStep(float deltatime) {
		// override to perform any other evaluations

	}

	public void Destroy() {
		if(DeleteMe==false) {
			DeleteMe = true;
			if(OnDestroy!=null) {
				OnDestroy(this, new EventArgs());
			}
		}
	}
}
