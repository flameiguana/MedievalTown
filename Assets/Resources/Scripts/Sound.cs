using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sound : MonoBehaviour {

	public static AudioClip[] wessbat;
	public static AudioClip[] villager;
	// Use this for initialization
	void Start () {
		wessbat = Resources.LoadAll ("Audio/Wessbat") as AudioClip[];
		villager = Resources.LoadAll ("Audio/Villagers") as AudioClip[];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
