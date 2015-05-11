﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Simulation {

	private const float LOCKSTEP = 0.33f;

	private List<SimUnitInstance> FriendlySimUnits;
	private List<SimUnitInstance> EnemySimUnits;

	private float TimeAccumulator = 0.0f;

	public float SimTime {
		get;
		private set;
	}

	public float SimTimeAlpha {
		get {
			return (SimTime + TimeAccumulator)/LOCKSTEP;
		}
	}

	public Simulation() {

	}

	public void AddUnit(SimUnit unittype, Vector3 startingposition, ISimUnitEventHandler EventHandler) {
		SimUnitInstance inst = new SimUnitInstance(this, unittype, startingposition, EventHandler);
		if(unittype.Team==SimUnit.ETeam.Friendly) {
			FriendlySimUnits.Add(inst);
		} else {
			EnemySimUnits.Add (inst);
		}
	}

	public void Update(float deltatime) {
		Simulate(deltatime);
	}

	public void Simulate(float deltatime) {

		foreach(SimUnitInstance inst in EnemySimUnits) {
			inst.NonDeterministicUpdate(deltatime);
		}
		
		foreach(SimUnitInstance inst in FriendlySimUnits) {
			inst.NonDeterministicUpdate(deltatime);
		}
		
		TimeAccumulator += deltatime;

		while(TimeAccumulator > 0.0f) {
			SimTime += LOCKSTEP;
			TimeAccumulator -= LOCKSTEP;

			foreach(SimUnitInstance inst in EnemySimUnits) {
				inst.Step(LOCKSTEP);
			}

			foreach(SimUnitInstance inst in FriendlySimUnits) {
				inst.Step(LOCKSTEP);
			}
		}

	}

}
