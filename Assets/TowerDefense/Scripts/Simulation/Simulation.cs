using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Simulation {

	private const float LOCKSTEP = 0.033f;

	private List<SimUnitInstance> FriendlySimUnits = new List<SimUnitInstance>();
	private List<SimUnitInstance> EnemySimUnits = new List<SimUnitInstance>();

	private float TimeAccumulator = 0.0f;

	public float SimTime {
		get;
		private set;
	}

	public float SimTimeAlpha {
		get {
			return (LOCKSTEP + TimeAccumulator)/LOCKSTEP;
		}
	}

	public IRandomNumberGenerator randGen {
		get;
		private set;
	}

	public BoundsOctree<IOctreeObject> Octtree {
		get;
		private set;
	}

	public Plane Goal {
		get;
		private set;
	}

	public Simulation(int seed, Plane goal) {
		Goal = goal;
		randGen = new MersenneTwister(seed);
		Octtree = new BoundsOctree<IOctreeObject>(1000.0f,Vector3.zero,10.0f,1);
	}
	
	public SimUnitInstance AddUnit(SimUnit unittype, Vector3 startingposition, ISimUnitEventHandler EventHandler) {
		SimUnitInstance inst = new SimUnitInstance(this, unittype, startingposition, EventHandler);
		if(unittype.Team==SimUnit.ETeam.Friendly) {
			FriendlySimUnits.Add(inst);
		} else {
			EnemySimUnits.Add (inst);
		}
		Octtree.Add(inst,inst.ObjectBounds());
		return inst;
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

			for(int i=EnemySimUnits.Count-1;i>=0;i--) {
				SimUnitInstance inst = EnemySimUnits[i];
				inst.Step(LOCKSTEP);
				if(inst.DeleteMe) {
					EnemySimUnits.Remove(inst);
					Octtree.Remove(inst);
				}
			}

			for(int i=FriendlySimUnits.Count-1;i>=0;i--) {
				SimUnitInstance inst = FriendlySimUnits[i];
				inst.Step(LOCKSTEP);
				if(inst.DeleteMe) {
					FriendlySimUnits.RemoveAt(i);
					Octtree.Remove (inst);
                }
            }
		}

	}

}

