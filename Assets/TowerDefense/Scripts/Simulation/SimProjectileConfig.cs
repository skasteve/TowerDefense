using UnityEngine;
using System.Collections;

public class SimProjectileConfig : SimObjectConfig {
	public SimUnitConfig.ETeam CollidesWithTeam = SimUnitConfig.ETeam.Enemy;
	public float Damage = 1.0f;
	public float Timeout = 10.0f;
}
