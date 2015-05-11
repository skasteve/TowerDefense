using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimUnitInstance {

	private struct DamageDef {
		public float Amount;
		public float Delay;

		public DamageDef(float Amount, float Delay) {
			this.Amount = Amount;
			this.Delay = Delay;
		}
	}

	private Simulation Sim;

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

	private Vector3 startposition = Vector3.zero;
	private float starttime = 0.0f;
	private ISimUnitEventHandler eventhandler;

	public SimUnitInstance(Simulation sim, SimUnit unit, Vector3 startpos, ISimUnitEventHandler handler) {
		Sim = sim;
		Health = sim.health;
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

	private void EvaluateDamage(float deltatime) {
		if(Health<=0.0f) {
			ConditionalDropBonus();
			OnExplode();
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
}
