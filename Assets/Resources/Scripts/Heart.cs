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
	
	}
	
	// Update is called once per frame
	void Update () {
		timer = Mathf.Max(0f, timer - Time.deltaTime);
	}

	public float getTimer() {
		return timer;
	}

	public void activate() {
		renderer.enabled = true;
		collider.enabled = true;
		timer = 0f;
		levelManager.totalHearts++;
	}

	public void deactivate() {
		renderer.enabled = false;
		collider.enabled = false;
		timer = cooldown;
		levelManager.totalHearts--;
	}
}
