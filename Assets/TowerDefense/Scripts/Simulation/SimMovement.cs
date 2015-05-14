using UnityEngine;
using System.Collections;

public class SimMovement : ScriptableObject {
	
	public float MinSpeed = 1.0f;
	public float MaxSpeed = 2.0f;
	public Vector3 WobbleFrequency;
	public Vector3 WobbleAmplitute;
	
	private class InstanceData {
		public float Speed = 1.0f;
		public Vector3 WobbleRandom = Vector3.zero;
		public Vector3 WobbleOffset = Vector3.zero;

		public InstanceData(Simulation sim, SimMovement simmovement) {
			Speed = simmovement.MinSpeed + (sim.randGen.NextFloat() * (simmovement.MaxSpeed - simmovement.MinSpeed));
			WobbleRandom.x = sim.randGen.NextFloat();
			WobbleRandom.y = sim.randGen.NextFloat();
			WobbleRandom.z = sim.randGen.NextFloat();
			WobbleRandom = WobbleRandom * Mathf.PI * 2.0f;
		}

		public static InstanceData GetData(SimMovement simmove, SimObjectInstance instance) {
			if(instance.MovementData==null) {
				instance.MovementData = new InstanceData(instance.Sim, simmove);
			}
			return (InstanceData)instance.MovementData;
		}
	}

	public Vector3 Integrate(SimObjectInstance instance, float deltatime) {
		InstanceData data = InstanceData.GetData(this, instance);

		if(data.Speed==0.0f) {
			return Vector3.zero;
		}

		Vector3 delta = instance.Sim.Goal.normal * (deltatime * data.Speed) * -1.0f;

		if(WobbleFrequency != Vector3.zero && WobbleAmplitute != Vector3.zero) {
			data.WobbleOffset.x = Mathf.Sin(data.WobbleRandom.x * (WobbleFrequency.x * instance.Sim.SimTime)) * WobbleAmplitute.x;
			data.WobbleOffset.y = Mathf.Sin(data.WobbleRandom.y * (WobbleFrequency.x * instance.Sim.SimTime)) * WobbleAmplitute.y;
			data.WobbleOffset.z = Mathf.Sin(data.WobbleRandom.z * (WobbleFrequency.x * instance.Sim.SimTime)) * WobbleAmplitute.z;
			delta = delta + data.WobbleOffset;
		}

		return delta;
	}
}
