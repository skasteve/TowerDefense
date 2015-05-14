using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Simulation {

	private const float LOCKSTEP = 0.033f;

	private List<SimObjectInstance> FriendlySimUnits = new List<SimObjectInstance>();
	private List<SimObjectInstance> EnemySimUnits = new List<SimObjectInstance>();
	private List<SimObjectInstance> Projectiles = new List<SimObjectInstance>();

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

	public SimProjectileInstance AddProjectile(SimProjectileConfig config, Vector3 startingposition) {
		SimProjectileInstance inst = new SimProjectileInstance(this, config, startingposition);
		Projectiles.Add (inst);
		return inst;
	}

	public SimUnitInstance AddUnit(SimUnitConfig unittype, Vector3 startingposition, ISimUnitEventHandler EventHandler) {
		SimUnitInstance inst = new SimUnitInstance(this, unittype, startingposition, EventHandler);
		if(unittype.Team==SimUnitConfig.ETeam.Friendly) {
			FriendlySimUnits.Add(inst);
		} else {
			EnemySimUnits.Add (inst);
		}
		return inst;
	}

	public void Update(float deltatime) {
		Simulate(deltatime);
	}

	public void Simulate(float deltatime) {

		TimeAccumulator += deltatime;

		while(TimeAccumulator > 0.0f) {
			SimTime += LOCKSTEP;
			TimeAccumulator -= LOCKSTEP;

			Step (EnemySimUnits);
			Step (FriendlySimUnits);
			Step (Projectiles);
		}
	}

	private void Step(IList<SimObjectInstance> objs) {
		for(int i=objs.Count-1;i>=0;i--) {
			SimObjectInstance inst = objs[i];
			inst.Step(LOCKSTEP);
			if(inst.DeleteMe) {
				objs.RemoveAt(i);
			}
		}
	}

}

