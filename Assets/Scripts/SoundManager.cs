using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public AudioSource efxSource;
	// Use this for initialization
	public static SoundManager instance = null;
	// Update is called once per frame
	void Awake () {
		if(instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

	public void PlaySound(AudioClip clip) {
		efxSource.clip = clip;
		efxSource.Play();
	}
}
