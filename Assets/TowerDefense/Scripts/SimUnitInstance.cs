using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimUnitInstance {

	private class DamageDef {
		public float Amount;
		public float Delay;

		public DamageDef(float Amount, float Delay) {
			this.Amount = Amount;
			this.Delay = Delay;
		}
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

	public void NonDeterministicUpdate(float deltatime) {
		// Do any (non-deterministic) processing you want here that can happen multiple times a frame.
	}

	public void Step(float deltatime) {
		EvaluateAttack(deltatime);
		EvaluateMovement(deltatime);
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

		if(Health<=0.0f) {
			ConditionalDropBonus();
			OnExplode();
		}
	}

	private void ConditionalDropBonus() {
		// Determine if we should drop a bonus when killed.
		if(Sim.randGen.NextFloat() < Unit.DropBonusPct) {
			OnDropBonus(Unit.DropBonus);
		}
	}

	private void EvaluateAttack(float deltatime) {

	}

	private void EvaluateMovement(float deltatime) {
		// Units lock step so only do movement updates here, and any deterministic logic
		if(Unit.Movement!=null) {
			Position = Unit.Movement.CalculatePosition(this, deltatime);
		}
	}
	 
	private void OnExplode() {
		if(eventhandler!=null) {
			eventhandler.OnExplode();
		}
	}

	private void OnDropBonus(SimDrop bonus) {
		if(eventhandler!=null) {
			eventhandler.OnDropBonus(bonus);
		}
	}
}
