using UnityEngine;

interface IAudioEngine {
	void PlayMenuLoop();
	void StopMenuLoop();

	void PlayAmbientAudio();
	void StopAmbientAudio();
	void PlayAmbientEvent();

	void PlayStart();
	void PlayWin();
	void PlayIncomingWave();
	void PlayPickupCoin();

	void PlayUnitPlaced(AudioUnitConfig auc);
	AudioSource PlayUnitPlacedLoop(AudioUnitConfig auc); 

	void PlayUnitReachedGoal(AudioUnitConfig auc);
	void PlayExplode(AudioUnitConfig auc);

	void PlayProjectileFire(AudioProjectileConfig proj);
	AudioSource PlayProjectileFireLoop(AudioProjectileConfig proj);
	void PlayProjectileImpact(AudioProjectileConfig proj);

	void PlayButtonSound();

	void StopSourceSafe(AudioSource source);
}
