using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {
	// Hearts stuff
	[SerializeField] float heartCD = 60f;
	public int totalHearts = 0;
	Queue<Heart> hearts;

	// Villager/building management
	int kills = 0; // Number of villagers killed by the Wessbat


	// Wessbat
	Wessbat wessbat;

	void Awake() {
		/*
		foreach (Heart heart in FindObjectsOfType<Heart>()) {
			hearts.Enqueue(heart);
			heart.cooldown = heartCD;
			if (heart.collider.enabled)
				totalHearts++;
		}

		wessbat = GameObject.Find("Wessbat").GetComponent<Wessbat>();
		 */
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (hearts.Peek().getTimer() <= 0f) {
			Heart front = hearts.Dequeue();
			if (wessbat.getHealth() < wessbat.getMax())
				front.activate();
			hearts.Enqueue(front);
		}
	}


}
