using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SimUnitInstance : SimObjectInstance {
	
	private class DamageDef {
		public float Amount;
		public float Delay;

		public DamageDef(float Amount, float Delay) {
			this.Amount = Amount;
			this.Delay = Delay;
		}
	}

	public float Health {
		get;
		private set;
	}

	public SimUnitConfig Unit {
		get;
		private set;
	}

	private float nextFireTime = 0.0f;

	private ISimUnitEventHandler eventhandler;

	private List<DamageDef> DamageList = new List<DamageDef>();


	public SimUnitInstance(Simulation sim, SimUnitConfig unit, Vector3 startpos, ISimUnitEventHandler handler) : base(sim, unit, startpos) {
		Health = unit.Health;
		Unit = unit;
		eventhandler = handler;
		this.OnDestroy += handler.OnDestroyEventHandler;
	}
	
	public void NonDeterministicUpdate(float deltatime) {
		// Do any (non-deterministic) processing you want here that can happen multiple times a frame.
	}

	public override void Step(float deltatime) {
		if(Health<=0.0f) {
			ConditionalDropBonus();
			OnExplode();
			Destroy();
		} else if(Sim.Goal.GetSide(simposition)==false) {
			OnReachedGoal();
			Destroy();
		} else {
			base.Step(deltatime);
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
		if(Unit.WeaponConfig!=null && Sim.SimTime>=nextFireTime && Unit.FireRate>0.0f) {
			float closestdist = 0.0f;
			float closestdisttogoal = 0.0f;
			SimUnitInstance closest = null;
			IOctreeObject[] objs = Sim.Octtree.GetColliding(new Bounds(simposition, Vector3.one * Unit.RadiusOfAffect));
			foreach(IOctreeObject obj in objs) {
				SimUnitInstance inst = (SimUnitInstance)obj;
				if(inst!=null && inst != this && inst.Unit.Team != this.Unit.Team) {
					float disttoobject = Vector3.Distance(this.simposition, inst.simposition);
					if(disttoobject<=Unit.RadiusOfAffect) {
						float dist = Sim.Goal.GetDistanceToPoint(inst.simposition);
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
				float impacttime = closestdist / Unit.WeaponConfig.Speed;
				closest.DoDamage(Unit.WeaponConfig.DamageAmount, impacttime);
				OnFireWeapon(closest.simposition, impacttime, closest);
				nextFireTime = Sim.SimTime + 1.0f/Unit.FireRate;
			}
		}
	}

	private void OnExplode() {
		if(eventhandler!=null) {
			eventhandler.OnSimExplode(this);
		}
	}

	private void OnDropBonus(SimDropConfig bonus) {
		if(eventhandler!=null) {
			eventhandler.OnSimDropBonus(this,bonus);
		}
	}

	private void OnFireWeapon(Vector3 impactlocation, float impacttime, SimUnitInstance impactunit) {
		if(eventhandler!=null) {
			eventhandler.OnSimFireWeapon(this,impactlocation, impacttime, impactunit);
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
}
