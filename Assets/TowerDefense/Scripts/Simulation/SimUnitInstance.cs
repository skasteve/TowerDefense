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

	public SimUnitConfig UnitConfig {
		get;
		private set;
	}

	private float nextFireTime = 0.0f;

	private ISimUnitEventHandler eventhandler;

	private List<DamageDef> DamageList = new List<DamageDef>();


	public SimUnitInstance(Simulation sim, SimUnitConfig unit, Vector3 startpos, ISimUnitEventHandler handler) : base(sim, unit, startpos) {
		Health = unit.Health;
		UnitConfig = unit;
		eventhandler = handler;
		this.OnDestroy += handler.OnDestroyEventHandler;
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
		if(Sim.randGen.NextFloat() < UnitConfig.DropBonusPct) {
			OnDropBonus(UnitConfig.DropBonus);
		}
	}

	private void EvaluateAttack(float deltatime) {
		if(UnitConfig.WeaponConfig!=null && Sim.SimTime>=nextFireTime && UnitConfig.FireRate>0.0f) {
			float closestdist = 0.0f;
			float closestdisttogoal = 0.0f;
			SimUnitInstance closest = null;
			IOctreeObject[] objs = Sim.Octtree.GetColliding(new Bounds(simposition, Vector3.one * UnitConfig.RadiusOfAffect));
			foreach(IOctreeObject obj in objs) {
				SimUnitInstance inst = obj as SimUnitInstance;
				if(inst!=null && inst != this && inst.UnitConfig.Team != this.UnitConfig.Team) {
					float disttoobject = Vector3.Distance(this.simposition, inst.simposition);
					if(disttoobject<=UnitConfig.RadiusOfAffect) {
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
				float impacttime = closestdist / UnitConfig.WeaponConfig.Speed;
				closest.DoDamage(UnitConfig.WeaponConfig.DamageAmount, impacttime);
				OnFireWeapon(closest.simposition, impacttime, closest);
				nextFireTime = Sim.SimTime + 1.0f/UnitConfig.FireRate;
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

	#region implemented abstract members of SimObjectInstance


	protected override void ObjectCollision (SimObjectInstance obj)
	{
		throw new NotImplementedException ();
	}


	#endregion

}
