using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class AudioEngine : MonoBehaviour, IAudioEngine {

	public static AudioEngine instance;

	public AudioEngineConfig EngineConfig;

	public bool ShowTestUI = true;

	private AudioSource AmbientSource;

	private IList<AudioSource> sourcepool;

	private Vector2 scrollPosition = Vector2.zero;

	void Awake()
	{
		instance = this;
		sourcepool = new List<AudioSource>();
	}

	public void PlayAmbientAudio() {
		PlayAmbientAudio (EngineConfig.ACCAmbientAudio);
	}

	private void PlayAmbientAudio(AudioClipsCollection acc) {
		StopSourceSafe(AmbientSource);
		AmbientSource = CreateSource(acc,true);
	}
	
	public void StopAmbientAudio() {
		StopSourceSafe(AmbientSource);
	}

	public void PlayAmbientEvent() {
		CreateSource (EngineConfig.ACCAmbientEvents, false);
	}

	public void PlayButtonSound() {
		CreateSource(EngineConfig.ACCButtonClicks, false);
	}

	public void PlayUnitPlaced(AudioUnitConfig auc) {
		CreateSource(auc.OnUnitPlaced, false);
	}

	public AudioSource PlayUnitPlacedLoop(AudioUnitConfig auc) {
		return CreateSource(auc.OnUnitPlacedLoop, true);
	}

	public void PlayUnitReachedGoal(AudioUnitConfig auc) {
		CreateSource (auc.OnReachedGoal, false);
	}
	
	public void PlayExplode(AudioUnitConfig auc) {
		CreateSource (auc.OnExplode, false);
	}

	public void PlayUnitUpgraded(AudioUnitConfig auc) {
		CreateSource (auc.OnUpgraded, false);
	}

	public void PlayMenuLoop() {
		PlayAmbientAudio(EngineConfig.ACCMenuLoop);
	}

	public void StopMenuLoop() {
		StopAmbientAudio();
	}

	public void PlayStart() {
		CreateSource (EngineConfig.ACCPlayStartGame, false);
	}

	public void PlayWin() {
		CreateSource(EngineConfig.ACCPlayWinGame, false);
	}

	public void PlayIncomingWave() {
		CreateSource(EngineConfig.ACCPlayIncomingWave, false);
	}

	public void PlayWeaponFire(AudioWeaponConfig proj) {
		CreateSource (proj.OnFire, false);
	}

	public AudioSource PlayWeaponFireLoop(AudioWeaponConfig proj) {
		return CreateSource (proj.OnFireLoop, true);
	}

	public void PlayWeaponImpact(AudioWeaponConfig proj) {
		CreateSource (proj.OnImpact, false);
	}

	public void PlayPickupCoin() {
		CreateSource(EngineConfig.ACCPickupCoin,false);
	}

	public AudioSource CreateSource(AudioClipsCollection audioclipscollection, bool looping) {
		return CreateSource (audioclipscollection.GetRandom(), looping, audioclipscollection.AudioMixer);
	}

	public void StopSourceSafe(AudioSource source) {
		if(source!=null) {
			source.Stop ();
		}
	}

	public AudioSource CreateSource(AudioClip clip, bool looping, AudioMixerGroup mixergroup=null) {
		AudioSource source = null;
		foreach(AudioSource s in sourcepool) {
			if(!s.isPlaying) {
				source =s;
				break;
			}
		}

		if(source==null) {
			source = gameObject.AddComponent<AudioSource>();
			sourcepool.Add (source);
		}

		source.volume = 1.0f;
		source.clip =  clip;
		source.loop = looping;
		source.outputAudioMixerGroup = mixergroup;
		source.Play();
		return source;
	}

	private void RemoveFromPool(AudioSource source) {
		if(sourcepool.Contains(source)) {
			sourcepool.Remove(source);
		}
	}

	private void ReturnToPool(AudioSource source) {
		sourcepool.Add (source);
	}

	void OnGUI() {
		AudioSource tmpsource=null;

		if(!ShowTestUI)
			return;

		scrollPosition = GUILayout.BeginScrollView(scrollPosition);
		GUILayout.BeginHorizontal();
		if(GUILayout.Button ("Play Button Click")) {
			PlayButtonSound();
		}
		if(GUILayout.Button ("Play Pickup Coin")) {
			PlayButtonSound();
		}

		if(GUILayout.Button ("Play Menu Loop")) {
			PlayMenuLoop();
		}
		if(GUILayout.Button ("Stop Menu Loop")) {
			StopMenuLoop();
		}
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		if(GUILayout.Button ("Play Ambient Audio")) {
			PlayAmbientAudio();
		}
		if(GUILayout.Button ("Stop Ambient Audio")) {
			StopAmbientAudio();
		}

		if(GUILayout.Button ("Play Ambient Event")) {
			PlayAmbientEvent();
		}
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		if(GUILayout.Button ("Play Start")) {
			PlayStart();
		}
		if(GUILayout.Button ("Play Win")) {
			PlayWin();
		}
		
		if(GUILayout.Button ("Play Incoming Wave")) {
			PlayIncomingWave();
		}
		GUILayout.EndHorizontal();
		

		foreach(AudioUnitConfig auc in EngineConfig.UnitConfigs) {
			GUILayout.Label(" ");
			GUILayout.Label("Unit " + auc.ToString());
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			{
				GUILayout.BeginVertical();

				GUILayout.BeginHorizontal();
				if(GUILayout.Button("Play Upgraded")) {
					PlayUnitUpgraded(auc);
				}
				if(GUILayout.Button ("Play Explode")) {
					PlayExplode(auc);
				}
				if(GUILayout.Button ("Play Unit Placed")) {
					PlayUnitPlaced(auc);
				}
				if(GUILayout.Button ("Play Unit Reached Goal")) {
					PlayUnitReachedGoal(auc);
				}
				if(GUILayout.Button ("Play Unit Placed Looping")) {
					StopSourceSafe(tmpsource);
					tmpsource = PlayUnitPlacedLoop(auc);
				}
				if(GUILayout.Button ("Stop Unit Placed Looping")) {
					StopSourceSafe(tmpsource);
				}
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.BeginVertical();
				GUILayout.EndVertical();
				GUILayout.EndHorizontal();

				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();
		}

		foreach(AudioWeaponConfig proj in EngineConfig.WeaponConfigs) {
			GUILayout.Label(" ");
			GUILayout.Label("Weapon " + proj.ToString());
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			{
				GUILayout.BeginVertical();
				
				GUILayout.BeginHorizontal();
				if(GUILayout.Button ("Play Fire")) {
					PlayWeaponFire(proj);
				}
				if(GUILayout.Button ("Play Fire Loop")) {
					StopSourceSafe(tmpsource);
					tmpsource = PlayWeaponFireLoop(proj);
				}
				if(GUILayout.Button ("Play Weapon Impact")) {
					PlayWeaponImpact(proj);
				}
				if(GUILayout.Button ("Stop Fire Loop")) {
					StopSourceSafe(tmpsource);
				}
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				GUILayout.BeginVertical();
				GUILayout.EndVertical();
				GUILayout.EndHorizontal();
				
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();
		}

		GUILayout.EndScrollView();
	}
}
