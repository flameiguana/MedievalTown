using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {
	[SerializeField] private float spawnRate = 20f; // The rate (in seconds) at which a new villager spawns from the building.
	[SerializeField] private float[] villagerProbabilities = {0.7f, 0.3f, 0f, 0f}; // The probability of each of the 3 types of villager spawning. The first is
																			 // an unarmed villager, second is a villager with a sword, third is a villager
																			 // with a bow, and fourth is a catapult.
	[SerializeField] private float documentProbability = 0.2f;
	[SerializeField] private float spawnX = 0f; // The local X positon where villagers will be spawned from.
	[SerializeField] private float spawnY = 0f; // The local Y positon where villagers will be spawned from.
	private float spawnCD; // The cooldown on spawning a new villager.
	private GameObject villager; // Unarmed villager prefab
	//private GameObject swordVillager; // Sword villager prefab
	//private GameObject bowVillager; // Bow villager prefab
	//private GameObject catapultVillager; // Catapult villager prefab

	// Use this for initialization
	void Start () {
		spawnCD = spawnRate;
		villager = Resources.Load("Prefabs/male-peasant") as GameObject;
		//swordVillager = Resources.Load("Prefabs/male-peasant") as GameObject;
		//bowVillager = Resources.Load("Prefabs/male-peasant") as GameObject;
		//catapultVillager = Resources.Load("Prefabs/male-peasant") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {
		spawnCD = Mathf.Max(0f, spawnCD - Time.deltaTime);
		if (spawnCD <= 0f) {
			GameObject.Instantiate(villager, new Vector2(transform.localPosition.x + spawnX, transform.localPosition.y + spawnY), transform.rotation);
			spawnCD = spawnRate;
		}
	}
}
