using UnityEngine;
using System.Collections;

public class Villager : MonoBehaviour {

	private Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		animator.SetBool("Panic", true);
	}

	void OnTriggerExit2D()
	{

	}
}
