using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {
	[SerializeField] private float spawnRate = 20f; // The rate (in seconds) at which a new villager spawns from the building.
	[SerializeField] private float[] villagerProbabilities = {0.7f, 0.2f, 0.1f, 0f}; // The probability of each of the 3 types of villager spawning. The first is
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
			spawnVillager();
		}
	}

	// Spawn a villager from the building
	public void spawnVillager() {
		float villagerSpawn = Random.Range(0f, 1f);
		Villager.WeaponType weapon;
		//GameObject.Instantiate(villager, new Vector2(transform.localPosition.x + spawnX, transform.localPosition.y + spawnY), transform.rotation);
		if (villagerSpawn > villagerProbabilities[0] + villagerProbabilities[1] + villagerProbabilities[2])
			weapon = Villager.WeaponType.None; // Until Catapult is in the game
		else if (villagerSpawn > villagerProbabilities[0] + villagerProbabilities[1])
			weapon = Villager.WeaponType.Bow;
		else if (villagerSpawn > villagerProbabilities[0])
			weapon = Villager.WeaponType.Sword;
		else
			weapon = Villager.WeaponType.None;
		((GameObject) GameObject.Instantiate(villager, new Vector2(transform.localPosition.x + spawnX, transform.localPosition.y + spawnY), transform.rotation)).GetComponent<Villager>().setWeapon(weapon);

		spawnCD = spawnRate;
	}
}
