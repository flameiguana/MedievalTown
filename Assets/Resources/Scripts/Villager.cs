using UnityEngine;
using System.Collections;

public class Villager : MonoBehaviour {

	private Animator animator;
	[SerializeField] private float explodeSpeed = 10f;
	[SerializeField] private GameObject deathSplatter;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.tag == "Player")
		{
			animator.SetBool("Panic", true);
		}

	}

	void OnTriggerExit2D(Collider2D collider)
	{
		if(collider.tag == "Player")
		{
			animator.SetBool("Panic", false);
		}
	}

	void OnCollisionEnter2D(Collision2D collide){
		if (collide.relativeVelocity.magnitude > explodeSpeed) {
			explode();
		}
	}

	void explode(){
		Instantiate (deathSplatter, gameObject.transform.position, Quaternion.identity);
		Destroy (gameObject);
	}
}
