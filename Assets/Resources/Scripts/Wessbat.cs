using UnityEngine;
using System.Collections;

public class Wessbat : MonoBehaviour {
	[SerializeField] float invincibilityTimer = 1f;
	[SerializeField] int maxHealth = 10;
	private int curHealth, worksheets;
	private GameObject pickedUp = null; //object being held
	private float invincibility;
	[SerializeField] float weightScale = 25f;

	void Awake() {
		curHealth = maxHealth;
		worksheets = 0;
		invincibility = 0f;
	}

	void Start () {
	
	}

	void Update () {
		invincibility = Mathf.Max(invincibility - Time.deltaTime, 0f);
		rigidbody2D.gravityScale = 1f + transform.position.y / weightScale;
		if (Input.GetKeyUp (KeyCode.LeftShift)) {
			Drop ();
		}
		if (Input.GetKeyDown(KeyCode.S)) {
			Kill();
		}
		//control scheme goes here
	}

	// Using this garbage for now
	void OnGUI() {
		if (curHealth == 0) {
			KongregateAPI.instance.SubmitStats("Worksheets", worksheets);
			Application.LoadLevel (0);
		}
		GUI.Box(new Rect(0f, 0f, 110f, 30f), "Health: " + curHealth);
		GUI.Box(new Rect(Screen.width - 110f, 0f, 110f, 30f), "Worksheets: " + worksheets);
	}

	void OnCollisionStay2D(Collision2D target){
		switch (target.gameObject.layer) {
		case 8:							//Enemy layer
			if(Input.GetKey (KeyCode.LeftShift))
				PickUp (target.gameObject);
			break;
		default:
			break;
		}
		if (target.gameObject.tag == "Arrow")
			Destroy(target.gameObject);
	}

	void OnTriggerEnter2D(Collider2D target) {
		if (target.tag == "Worksheet") {
			worksheets++;
			Destroy(target.gameObject);
		} else if (target.tag == "Sword")
			Damage(1);
	}

	void PickUp(GameObject target){		//Pick up the object
		if (pickedUp != null)
			return;
		else pickedUp = target;
		SpringJoint2D joint = target.AddComponent<SpringJoint2D> ();
		joint.connectedBody = rigidbody2D;
		joint.frequency = 10f;
		joint.distance = 0.01f;
		joint.anchor = new Vector2 (0, 1);	//inline anchor, will need to specify per pickupable object in future
		target.transform.parent = gameObject.transform;
		target.layer = LayerMask.NameToLayer ("Picked Up Object");
	}

	void Drop(){						//Drop the object
		if (pickedUp == null)
			return;
		pickedUp.transform.parent = null;
		Destroy (pickedUp.GetComponent<SpringJoint2D> ());
		GameObject droppedObject = pickedUp;
		Utility.doThis d = () => {
			if (droppedObject != null)
				droppedObject.layer = LayerMask.NameToLayer ("Enemy");
		};
		if (droppedObject != null)
			StartCoroutine(Utility.wait (1f, d));
		pickedUp = null;
	}

	private void Kill() { // Kill the villager if one is currently held
		if (pickedUp){
			if(!pickedUp.GetComponent<Villager>())
				return;
			Villager killed = pickedUp.GetComponent<Villager>();
			Drop ();
			killed.explode();
		}
	}

	// Deal damage to Wessbat.
	public void Damage(int dmg) {
		if (invincibility <= 0f) {
			curHealth -= dmg;
			invincibility = invincibilityTimer;
			if (curHealth <= 0)
				Application.LoadLevel(Application.loadedLevel);
		}
	}
}
