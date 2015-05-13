using UnityEngine;
using System.Collections;
using System;

public class SimObject : ISimulationObject {

	public delegate void OnDestroyEventHandler(object sender, EventArgs e);

	public event OnDestroyEventHandler OnDestroy;
	
	private Vector3 position;
	private Vector3 oldposition;
	public Vector3 Position {
		get {
			//Interpolate the position from last to next
			return Vector3.Lerp(oldposition, position, Sim.SimTimeAlpha);
		}
		private set {
			oldposition = position;
			position = value;
			Sim.Octtree.Remove(this);
			Sim.Octtree.Add(this, ObjectBounds());
		}
	}
	
	public Simulation Sim {
		get;
		private set;
	}

	public bool Collides;

	private SimMovement Movement;

	public object MovementData = null;

	public bool DeleteMe {
		get;
		private set;
	}
	
	public virtual void Step(float deltatime) {
		if(!DeleteMe) {
			EvaluateMovement(deltatime);
		}
	}

	private void EvaluateMovement(float deltatime) {
		// Units lock step so only do movement updates here, and any deterministic logic
		if(Movement!=null) {
			Position = position + Movement.Integrate(this, deltatime);
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
