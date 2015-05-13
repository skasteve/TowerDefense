using UnityEngine;
using System.Collections;

public class AudioUnitConfig : ScriptableObject {
	public AudioClipsCollection OnUnitPlaced;
	public AudioClipsCollection OnUnitPlacedLoop;
	public AudioClipsCollection OnExplode;
	public AudioClipsCollection OnReachedGoal;
	public AudioClipsCollection OnUpgraded;
}
