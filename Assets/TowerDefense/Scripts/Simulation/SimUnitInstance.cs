using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SimUnitInstance : IOctreeObject {
	
	private class DamageDef {
		public float Amount;
		public float Delay;

		public DamageDef(float Amount, float Delay) {
			this.Amount = Amount;
			this.Delay = Delay;
		}
	}

	public bool DeleteMe {
		get;
		private set;
	}

	public Simulation Sim {
		get;
		private set;
	}

	public float Health {
		get;
		private set;
	}

	public SimUnit Unit {
		get;
		private set;
	}

	public object MovementData = null;

	private Vector3 position;
	private Vector3 oldposition;
	private float lasttime;
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

	private ISimUnitEventHandler eventhandler;


	private List<DamageDef> DamageList = new List<DamageDef>();


	public SimUnitInstance(Simulation sim, SimUnit unit, Vector3 startpos, ISimUnitEventHandler handler) {
		Sim = sim;
		Health = unit.Health;
		oldposition = startpos;
		position = startpos;
		Unit = unit;
		eventhandler = handler;
	}

	public Bounds ObjectBounds() {
		return new Bounds(position, Vector3.one * Unit.RadiusOfCollision);
	}
	
	public void NonDeterministicUpdate(float deltatime) {
		// Do any (non-deterministic) processing you want here that can happen multiple times a frame.
	}

	public void Step(float deltatime) {
		if(Health<=0.0f) {
			ConditionalDropBonus();
			OnExplode();
			OnDestroy();
		} else if(Sim.Goal.GetSide(position)==false) {
			OnReachedGoal();
			OnDestroy();
		} else {
			EvaluateMovement(deltatime);
			EvaluateDamage(deltatime);
			if(Health>0.0f) {
				EvaluateAttack(deltatime);
			}
		}
	}

	public void DoDamage(float Amount, float Delay) {
		DamageList.Add(new DamageDef(Amount,Delay));
	}

	private void EvaluateDamage(float deltatime) {
		//Reverse iterate and remove any damage from the damage list.
		for(int i=DamageList.Count-1;i>=0;i--) {
			DamageDef dd = DamageList[i];
			dd.Delay -= deltatime;
			if(dd.Delay<=0.0f) {
				Health -= dd.Amount;
				DamageList.RemoveAt(i);
			}
		}
	}

	private void ConditionalDropBonus() {
		// Determine if we should drop a bonus when killed.
		if(Sim.randGen.NextFloat() < Unit.DropBonusPct) {
			OnDropBonus(Unit.DropBonus);
		}
	}

	private void EvaluateAttack(float deltatime) {
		if(Unit.Projectile!=null) {
			float closestdist = 0.0f;
			float closestdisttogoal = 0.0f;
			SimUnitInstance closest = null;
			IOctreeObject[] objs = Sim.Octtree.GetColliding(new Bounds(position, Vector3.one * Unit.RadiusOfAffect));
			foreach(IOctreeObject obj in objs) {
				SimUnitInstance inst = (SimUnitInstance)obj;
				if(inst!=null && inst != this && inst.Unit.Team != this.Unit.Team) {
					float disttoobject = Vector3.Distance(this.position, inst.position);
					if(disttoobject<=Unit.RadiusOfAffect) {
						float dist = Sim.Goal.GetDistanceToPoint(inst.position);
						if(closest==null || dist < closestdisttogoal) {
							closest = inst;
							closestdisttogoal = dist;
							closestdist = disttoobject;
						} 
					}
				}
			}	      
			if(closest!=null) {
				//Attack closest to goal
				float impacttime = closestdist / Unit.Projectile.Speed;
				closest.DoDamage(Unit.Projectile.DamageAmount, impacttime);
				OnFireProjectile(closest.position, impacttime, closest);
			}
		}
	}

	private void EvaluateMovement(float deltatime) {
		// Units lock step so only do movement updates here, and any deterministic logic
		if(Unit.Movement!=null) {
			Position = position + Unit.Movement.Integrate(this, deltatime);
		}
	}

	private void OnExplode() {
		if(eventhandler!=null) {
			eventhandler.OnSimExplode(this);
		}
	}

	private void OnDropBonus(SimDrop bonus) {
		if(eventhandler!=null) {
			eventhandler.OnSimDropBonus(this,bonus);
		}
	}

	private void OnFireProjectile(Vector3 impactlocation, float impacttime, SimUnitInstance impactunit) {
		if(eventhandler!=null) {
			eventhandler.OnSimFireProjectile(this,impactlocation, impacttime, impactunit);
		}
	}

	private void OnReachedGoal() {
		try {
			if(eventhandler!=null) {
				eventhandler.OnSimReachedGoal(this);
			}
		} catch(Exception e) {
			Debug.LogException(e);
		}
	}

	private void OnDestroy() {
		if(DeleteMe==false) {
			DeleteMe = true;
			if(eventhandler!=null) {
				eventhandler.OnSimDestroy (this);
			}
		}
	}

	public void Destroy() {
		OnDestroy();
	}
}
