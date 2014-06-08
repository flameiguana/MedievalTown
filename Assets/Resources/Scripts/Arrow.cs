using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D target) {
		if (target.gameObject.tag == "Player") {
			target.gameObject.GetComponent<Wessbat>().Damage(1);
			Instantiate(Resources.Load<GameObject>("Prefabs/bloodsplatter"), target.contacts[0].point, Quaternion.identity);
		}
		Destroy(gameObject);
	}
}
