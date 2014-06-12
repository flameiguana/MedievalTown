using UnityEngine;
using System.Collections;

public class Villager : MonoBehaviour {

	private Animator animator;
	[SerializeField] private float explodeSpeed = 10f;
	[SerializeField] private GameObject deathSplatter;
	[SerializeField] private float walkSpeed = 1f;
	[SerializeField] private float runSpeed = 3f;
	[SerializeField] private SpriteRenderer bowRenderer;

	[SerializeField] private float panicRadius = 15f;
	[SerializeField] private float swordRadius = 5f;
	[SerializeField] private float bowRadius = 15f;

	[SerializeField] private SpriteRenderer swordVisual;
	[SerializeField] private float activeRadius = 10f;
	[SerializeField] private float swordCooldown = 2f;
	[SerializeField] private float bowCooldown = 3f;
	[SerializeField] private bool hasDoc = false;
	private GameObject wessbat, arrow, worksheet;
	private bool batNear = false;
	private float swordTime, bowTime;

	//Not sure if we want this here.
	public enum WeaponType
	{
		None,
		Bow,
		Sword
	}

	void OnDestroy(){
		Utility.kills++;
	}

	public WeaponType weaponType = WeaponType.None;

	void Awake () {
		animator = GetComponent<Animator>();
		wessbat = GameObject.Find("Wessbat");
		arrow = Resources.Load<GameObject>("Prefabs/Arrow");
		worksheet = Resources.Load<GameObject>("Prefabs/Document");
		swordTime = 0f;
		bowTime = 0f;
		if (weaponType == WeaponType.Sword)
			transform.Find("SwordVisual").gameObject.SetActive(true);
		else if (weaponType == WeaponType.Bow) {
			transform.Find("Bow").gameObject.SetActive(true);
			transform.Find("BowArms").gameObject.SetActive(true);
		}
	}

	void Start() {
		//Shows staic bow behind villager. Note that this bow is different from the one
		//in shoot animation, so it is disabled during that animation.
		if(weaponType == WeaponType.Bow)
			bowRenderer.enabled = true;
		else if(weaponType == WeaponType.Sword)
		{
			swordVisual.enabled = true;
		}
			
	}
	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetKeyDown(KeyCode.S) && swordTime <= 0f) {
			animator.SetTrigger("Swing");
			swordTime = swordCooldown;
		} else if (Input.GetKeyDown(KeyCode.B) && bowTime <= 0f) {
			animator.SetTrigger("Shoot");
			bowTime = bowCooldown;
		}*/

		//only play walking animation if vel greater than 0
		if(Mathf.Abs(rigidbody2D.velocity.x) >= 0.0001f)
			animator.SetBool("Walking", true);
		else
			animator.SetBool("Walking", false);
	}

	void FixedUpdate() {
		swordTime = Mathf.Max(swordTime - Time.deltaTime, 0f);
		bowTime = Mathf.Max(bowTime - Time.deltaTime, 0f);
		executeBehavior(Time.deltaTime); // Placeholder for AI
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.tag == "Player")
		{
			batNear = true;
			if (weaponType == WeaponType.None) {
				animator.SetBool("Panic", true);
				GameObject.Find("Wessbat").GetComponent<Sound>().play("panic", "ex");
			}
		}

	}

	void OnTriggerExit2D(Collider2D collider)
	{
		if(collider.tag == "Player")
		{
			animator.SetBool("Panic", false);
			batNear = false;
		} else if (collider.tag == "Nest") {
			if (transform.parent == null) {
				Destroy(gameObject);
			}
		}
	}

	// Weapon usage coroutine
	private delegate void weaponAnim(float length);

	private weaponAnim doAnim = (float length) => {

	};

	void OnCollisionEnter2D(Collision2D collide){
		if (Mathf.Abs (collide.relativeVelocity.magnitude) > explodeSpeed) {
			if (collide.gameObject.tag != "Arrow")
				explode();
		}
	}

	public void explode(){
		Instantiate (deathSplatter, gameObject.transform.position, Quaternion.identity);
		dropDoc();
		GameObject.Find("LevelManager").GetComponent<LevelManager>().updateKills();
		Destroy (gameObject);
	}

	private void executeBehavior(float timeDiff) {
		// Check if the villager is on the ground
		if (Physics2D.Raycast(transform.position, new Vector2(0, -1), collider2D.bounds.size.y * 0.7f, 1 << 11).collider != null) {
			// Check if Wessbat is near enough, and if so, perform an action according to the villager's weapon type.
			if (batNear) {
				switch (weaponType) {
					case WeaponType.None:
						// Panic
						if (wessbat.transform.position.x > transform.position.x) {
							transform.localEulerAngles = new Vector3(0f, 0f, 0f);
							rigidbody2D.velocity = new Vector2(-runSpeed, rigidbody2D.velocity.y);
						} else {
							transform.localEulerAngles = new Vector3(0f, 180f, 0f);
							rigidbody2D.velocity = new Vector2(runSpeed, rigidbody2D.velocity.y);
						}
						break;
					case WeaponType.Sword:
						// Face Wessbat, and swing sword if close enough
						if (wessbat.transform.position.x > transform.position.x) {
							transform.localEulerAngles = new Vector3(0f, 180f, 0f);
							rigidbody2D.velocity = Vector2.zero;
						} else {
							transform.localEulerAngles = new Vector3(0f, 0f, 0f);
							rigidbody2D.velocity = Vector2.zero;
						}
						if (Vector2.Distance(transform.position, wessbat.transform.position) <= swordRadius) {
							if (!animator.GetBool("Swing") && swordTime <= 0f) {
								transform.Find("Sword").gameObject.SetActive(true);
								animator.SetTrigger("Swing");
								swordVisual.enabled = false;
								swordTime = swordCooldown;
							}
						}
						break;
					case WeaponType.Bow:
						// Face and fire an arrow at Wessbat; does not check for distance because archers fire automatically if Wessbat is in range
						if (wessbat.transform.position.x > transform.position.x) {
							transform.localEulerAngles = new Vector3(0f, 180f, 0f);
							rigidbody2D.velocity = Vector2.zero;
						} else {
							transform.localEulerAngles = new Vector3(0f, 0f, 0f);
							rigidbody2D.velocity = Vector2.zero;
						}
						if (!animator.GetBool("Shoot") && bowTime <= 0f) {
							float ang = 180f / Mathf.PI * Mathf.Atan2(wessbat.transform.position.y - transform.position.y, wessbat.transform.position.x - transform.position.x);
							animator.SetTrigger("Shoot");
							GameObject newArrow = ((GameObject)GameObject.Instantiate(arrow, new Vector2(transform.position.x + 0.5f * renderer.bounds.size.x * Mathf.Cos(ang * Mathf.PI / 180f),
												    transform.position.y + 0.5f * renderer.bounds.size.y * Mathf.Sin(ang * Mathf.PI / 180f)), transform.rotation));
							newArrow.transform.localEulerAngles = new Vector3(0f, 0f, -90f + ang);
							newArrow.rigidbody2D.velocity = 15f * new Vector2(Mathf.Cos(ang * Mathf.PI / 180f), Mathf.Sin(ang * Mathf.PI / 180f));
							bowTime = bowCooldown;
						}
						break;
					default:
						break;
				};
			} else {
				float rand = Random.Range(0f, 1f);
				if (rand > 0.993f) {
					transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y + 180f, 0f);
				}
				float xVel = -walkSpeed * Mathf.Cos(transform.localEulerAngles.y * Mathf.PI / 180f);

				rigidbody2D.velocity = new Vector2(xVel, rigidbody2D.velocity.y);

			}
		}
	}

	private void dropDoc() {
		if (Random.Range(0f, 1f) > 0.667f)
			GameObject.Instantiate(worksheet);
	}

	public void setWeapon(WeaponType weapon) {
		weaponType = weapon;
		if (weapon == WeaponType.Sword){
			transform.Find("Sword").gameObject.SetActive(true);
			swordVisual.enabled = true;
		}
			
		else if (weapon == WeaponType.Bow) {
			transform.Find("Bow").gameObject.SetActive(true);
			transform.Find("BowArms").gameObject.SetActive(true);
		}
	}

	public void FinishedSwing()
	{
		swordVisual.enabled = true;
	}
}
