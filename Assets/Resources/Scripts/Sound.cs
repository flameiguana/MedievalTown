using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sound : MonoBehaviour {

	public static Object[] wessbat;
	public static Object[] villager;
	// Use this for initialization
	void Start () {
		wessbat = Resources.LoadAll ("Audio/Wessbat");
		villager = Resources.LoadAll ("Audio/Villagers");
		// wessbat[0] roar
		// wessbat[1] worksheets
		// wessbat[2] wabing
		// villager[0] scream 1
		// villager[1] ready
		// villager[2] did you see
		// villager[3] sweet jesus
		// villager[4] run for your lives
		// villager[5] scream 2
		// villager[6] reporting for duty
		// villager[7] save the worksheets
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void play(string soundEvent, string other) {
		if (!audio.isPlaying) {
			AudioClip clip = (AudioClip)wessbat[0];
			if (soundEvent == "spawn") {
				if (other == "none")
					clip = (AudioClip)villager[1];
				else
					clip = (AudioClip)villager[6];
			} else if (soundEvent == "panic") {
				float random = Random.Range(0f, 1f);
				if (random < 0.167f)
					clip = (AudioClip)villager[0];
				else if (random < 0.333f)
					clip = (AudioClip)villager[2];
				else if (random < 0.5f)
					clip = (AudioClip)villager[3];
				else if (random < 0.667f)
					clip = (AudioClip)villager[4];
				else if (random < 0.833f)
					clip = (AudioClip)villager[5];
				else
					clip = (AudioClip)villager[7];
			} else if (soundEvent == "hurt") {
				clip = (AudioClip)wessbat[0];
			} else if (soundEvent == "worksheet") {
				clip = (AudioClip)wessbat[1];
			} else if (soundEvent == "heal") {
				clip = (AudioClip)wessbat[2];
			}
			audio.clip = clip;
			audio.Play();
		}
	}
}
