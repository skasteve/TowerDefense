﻿using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioClipsCollection : ScriptableObject {
	public AudioMixerGroup AudioMixer;
	public AudioClip[] AudioClips;

	public AudioClip GetRandom() {
		int index = Random.Range(0,AudioClips.Length);
		if(index==AudioClips.Length) {
			index = AudioClips.Length - 1;
		}
		return AudioClips[index];
	}
}
