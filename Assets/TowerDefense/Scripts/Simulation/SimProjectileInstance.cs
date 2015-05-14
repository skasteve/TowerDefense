using UnityEngine;
using System.Collections;
using System;

public class SimProjectileInstance : SimObjectInstance {

	public class OnCollisionEventArgs : EventArgs {
		public SimUnitInstance UnitInstance { get; private set; }
		public OnCollisionEventArgs(SimUnitInstance unit) {
			this.UnitInstance = unit;
		}
	}

	public delegate void OnCollisionEventHandler(SimProjectileInstance projectile, OnCollisionEventArgs args);
	public event OnCollisionEventHandler OnCollision;

	public SimProjectileConfig ProjectileConfig { get; private set; }

	private float _starttime;

	public SimProjectileInstance(Simulation sim, SimProjectileConfig projectileconfig, Vector3 startpos) : base(sim,projectileconfig,startpos) {
		ProjectileConfig = projectileconfig;
		_starttime = sim.SimTime;
	}

	protected override void EvaluateStep(float deltatime) {
		if((Sim.SimTime - _starttime)>=ProjectileConfig.Timeout) {
			Destroy ();
		}
	}

	protected override void ObjectCollision(SimObjectInstance obj) {
		SimUnitInstance inst = obj as SimUnitInstance;
		if(inst!=null && (inst.UnitConfig.Team==ProjectileConfig.CollidesWithTeam || ProjectileConfig.CollidesWithTeam == SimUnitConfig.ETeam.Both)) {
			inst.DoDamage(ProjectileConfig.Damage,0.0f);
			if(OnCollision!=null) {
				OnCollision(this,new OnCollisionEventArgs(inst));
			}
			Destroy ();
		}
	}
}
