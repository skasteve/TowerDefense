using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioEngineConfig : ScriptableObject {
	public AudioClipsCollection ACCMenuLoop;
	public AudioClipsCollection ACCAmbientAudio;
	public AudioClipsCollection ACCAmbientEvents;
	public AudioClipsCollection ACCButtonClicks;
	public AudioClipsCollection ACCPlayIncomingWave;
	public AudioClipsCollection ACCPlayStartGame;
	public AudioClipsCollection ACCPlayWinGame;

	public List<AudioUnitConfig> UnitConfigs;
	public List<AudioProjectileConfig> WeaponConfigs;
}
