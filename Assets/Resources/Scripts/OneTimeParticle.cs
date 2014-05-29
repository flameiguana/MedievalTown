using UnityEngine;
using System.Collections;

public class OneTimeParticle : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(particleSystem.isStopped)
			Destroy (gameObject);
	}
}
