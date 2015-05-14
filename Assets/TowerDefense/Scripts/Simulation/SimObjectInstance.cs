using UnityEngine;
using System.Collections;
using System;

public abstract class SimObjectInstance : IOctreeObject {
	
	public delegate void OnDestroyEventHandler(object sender, EventArgs e);
	public event OnDestroyEventHandler OnDestroy;

	public SimObjectConfig ObjectConfig {
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

	public Bounds ObjectBounds {
		get {
			return new Bounds(simposition, Vector3.one * ObjectConfig.RadiusOfCollision);
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

	public SimObjectInstance(Simulation sim, SimObjectConfig simobject, Vector3 startpos) {
		Sim = sim;
		ObjectConfig = simobject;
		oldposition = startpos;
		simposition = startpos;
		UpdateOcttree();
	}

	private void UpdateOcttree() {
		if(ObjectConfig.AddToOcttree) {
			Sim.Octtree.Remove(this);
			Sim.Octtree.Add(this, ObjectBounds);
		}
	}

	public virtual void Step(float deltatime) {
		if(!DeleteMe) {
			EvaluateMovement(deltatime);
			EvaluateCollision();
			EvaluateStep(deltatime);
		}
	}

	private void EvaluateMovement(float deltatime) {
		// Units lock step so only do movement updates here, and any deterministic logic
		if(ObjectConfig.Movement!=null) {
			Position = simposition + ObjectConfig.Movement.Integrate(this, deltatime);
		}
	}

	private void EvaluateCollision() {
		if(ObjectConfig.EnableCollisionCheck) {
			IOctreeObject[] objs = Sim.Octtree.GetColliding(ObjectBounds);
			foreach(IOctreeObject obj in objs) {
				if(obj!=this) {
					SimObjectInstance inst = (SimObjectInstance)obj;
					if(inst!=null) {
						ObjectCollision(inst);
					}
				}
			}
		}
	}

	protected virtual void EvaluateStep(float deltatime) {}

	protected abstract void ObjectCollision(SimObjectInstance obj);

	public void Destroy() {
		if(DeleteMe==false) {
			DeleteMe = true;
			Sim.Octtree.Remove(this);
			if(OnDestroy!=null) {
				OnDestroy(this, new EventArgs());
			}
		}
	}
}
