using UnityEngine;
using System.Collections;

public class SimMovement : ScriptableObject {
	
	public float MinSpeed = 1.0f;
	public float MaxSpeed = 2.0f;

	private class InstanceData {
		public float Speed = 1.0f;

		public InstanceData(Simulation sim, SimMovement simmovement) {
			Speed = simmovement.MinSpeed + (sim.randGen.NextFloat() * (simmovement.MaxSpeed - simmovement.MinSpeed));
		}

		public static InstanceData GetData(SimMovement simmove, SimUnitInstance instance) {
			if(instance.MovementData==null) {
				instance.MovementData = new InstanceData(instance.Sim, simmove);
			}
			return (InstanceData)instance.MovementData;
		}
	}

	public Vector3 CalculatePosition(SimUnitInstance instance, float deltatime) {
		InstanceData data = InstanceData.GetData(this, instance);

		return Vector3.forward * (deltatime * data.Speed * instance.Unit.Speed);
	}
}
