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
	void PlayUnitUpgraded(AudioUnitConfig auc);

	void PlayUnitReachedGoal(AudioUnitConfig auc);
	void PlayExplode(AudioUnitConfig auc);

	void PlayWeaponFire(AudioWeaponConfig proj);
	AudioSource PlayWeaponFireLoop(AudioWeaponConfig proj);
	void PlayWeaponImpact(AudioWeaponConfig proj);

	void PlayButtonSound();

	void StopSourceSafe(AudioSource source);
}
