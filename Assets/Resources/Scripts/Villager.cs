using UnityEngine;
using System.Collections;

public class Villager : MonoBehaviour {

	private Animator animator;
	[SerializeField] private float explodeSpeed = 10f;

	[SerializeField] private GameObject deathSplatter;
	[SerializeField] private float walkSpeed = 1f;
	[SerializeField] private float runSpeed = 3f;
	[SerializeField] private SpriteRenderer bowRenderer;

	//Not sure if we want this here.
	public enum WeaponType
	{
		None,
		Bow,
		Sword
	}

	public WeaponType weaponType;

	void Awake () {
		animator = GetComponent<Animator>();
	}

	void Start() {
		//Shows staic bow behind villager. Note that this bow is different from the one
		//in shoot animation, so it is disabled during that animation.
		if(weaponType == WeaponType.Bow)
			bowRenderer.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.S))
			animator.SetTrigger("Swing");
		else if(Input.GetKeyDown(KeyCode.B))
			animator.SetTrigger("Shoot");
	}

	void FixedUpdate() {
		executeBehavior(Time.deltaTime); // Placeholder for AI
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
		} else if (collider.tag == "Nest") {
			if (transform.parent == null) {
				Destroy(gameObject);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D collide){
		if (Mathf.Abs (collide.relativeVelocity.y) > explodeSpeed) {
			explode();
		}
	}

	public void explode(){
		Instantiate (deathSplatter, gameObject.transform.position, Quaternion.identity);
		Destroy (gameObject);
	}

	private void executeBehavior(float timeDiff) {
		if(Physics2D.Raycast(transform.position, new Vector2(0,-1), collider2D.bounds.size.y * 0.7f, 1 << 11).collider != null){
			if (animator.GetBool("Panic")) {
				if (GameObject.Find("Wessbat").transform.position.x > transform.position.x) {
					transform.localEulerAngles = new Vector3(0f, 0f, 0f);
					rigidbody2D.velocity = new Vector2(-runSpeed, rigidbody2D.velocity.y);
				} else {
					transform.localEulerAngles = new Vector3(0f, 180f, 0f);
					rigidbody2D.velocity = new Vector2(runSpeed, rigidbody2D.velocity.y);
				}
			} else {
				float rand = Random.Range(0f, 1f);
				if (rand > 0.993f) {
					transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y + 180f, 0f);
				}
				rigidbody2D.velocity = new Vector2(-walkSpeed, rigidbody2D.velocity.y);
			}
		}
	}
}
