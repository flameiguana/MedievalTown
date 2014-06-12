using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {
	// Hearts stuff
	[SerializeField] float heartCD = 60f;
	public int totalHearts = 0;
	Queue<Heart> hearts;

	// Villager/building management
	int kills, worksheets = 0; // Number of villagers killed by the Wessbat
	private Building church, inn;
	private Building[] houses;

	// Wessbat and UI stuff
	Wessbat wessbat;
	UILabel worksheetsUI, killsUI;

	void Awake() {
		hearts = new Queue<Heart>();
		foreach (Heart heart in FindObjectsOfType<Heart>()) {
			hearts.Enqueue(heart);
			heart.cooldown = heartCD;
			if (heart.collider2D.enabled)
				totalHearts++;
		}
		houses = new Building[3];
		int houseNum = 0;
		foreach (Building bldg in GameObject.FindObjectsOfType<Building>()) {
			switch (bldg.name) {
				case "Church":
					church = bldg;
					break;
				case "Inn":
					inn = GameObject.Find("Inn").GetComponent<Building>();
					break;
				default:
					houses[houseNum] = bldg;
					houseNum++;
					break;
			}
		}
		wessbat = GameObject.Find("Wessbat").GetComponent<Wessbat>();
		killsUI = GameObject.Find("Peasant").transform.FindChild("Label").GetComponent<UILabel>();
		worksheetsUI = GameObject.Find("Worksheets").transform.FindChild("Label").GetComponent<UILabel>();
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		hearts.Peek().decreaseTimer(Time.deltaTime);
		if (hearts.Peek().getTimer() <= 0f) {
			Heart front = hearts.Dequeue();
			if (wessbat.getHealth() < wessbat.getMax())
				front.activate();
			hearts.Enqueue(front);
		}
	}

	public void updateKills() {
		kills++;
		killsUI.text = "x " + kills.ToString();
		if (kills % 5 == 0) {
			float churchRate = Mathf.Min(church.getSpawnRate() - 1f, 4f);
			float innRate = Mathf.Min(inn.getSpawnRate() - 1f, 4f);

			if (kills % 20 == 0) {
				float bldgWep = Mathf.Min(houses[0].getVillagerChance(1) + 0.1f, 0.5f);
				foreach(Building bldg in houses) {
					bldg.updateSpawnChance(0, Mathf.Max(bldg.getVillagerChance(0) - 0.2f, 0f));
					bldg.updateSpawnChance(1, bldgWep);
					bldg.updateSpawnChance(2, bldgWep);
				}
			}
		}
	}

	public void updateWorksheets() {
		worksheets++;
		worksheetsUI.text = "x " + worksheets.ToString();
	}
}
