using UnityEngine;
using System.Collections;

public class Heart : MonoBehaviour {
	public float cooldown = 60f;
	float timer = 0f;
	LevelManager levelManager;

	void Awake() {
		levelManager = FindObjectOfType<LevelManager>();
	}

	// Use this for initialization
	void Start () {
		timer = cooldown;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public float getTimer() {
		return timer;
	}

	public void activate() {
		renderer.enabled = true;
		collider2D.enabled = true;
		timer = cooldown;
		levelManager.totalHearts++;
	}

	public void deactivate() {
		renderer.enabled = false;
		collider2D.enabled = false;
		levelManager.totalHearts--;
	}

	public void decreaseTimer(float amt) {
		timer = Mathf.Max(0f, timer - amt);
	}
}
