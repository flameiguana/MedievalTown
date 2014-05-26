using UnityEngine;
using System.Collections;

public class Wessbat : MonoBehaviour {
	private GameObject pickedUp = null; //object being held
	[SerializeField] private Vector3 objectDrift = new Vector3(0f,-0.5f,0f);

	void Start () {
	
	}
	

	void Update () {
		if (pickedUp) {
			pickedUp.transform.localPosition = Vector3.zero + objectDrift;
		}

		if (Input.GetKeyDown (KeyCode.F)) {
			Drop ();
		}
		//control scheme goes here
	}

	void OnCollisionEnter2D(Collision2D target){
		switch (target.gameObject.layer) {
		case 8:							//Enemy layer
			if(Input.GetKey (KeyCode.E))
				PickUp (target.gameObject);
			break;
		default:
			break;
		}
	}

	void PickUp(GameObject target){		//Pick up the object
		if (pickedUp != null)
			return;
		target.transform.parent = gameObject.transform;
		target.rigidbody2D.fixedAngle = true;
		target.collider2D.enabled = false;
		pickedUp = target;
	}

	void Drop(){						//Drop the object
		if (pickedUp == null)
			return;
		pickedUp.transform.parent = null;
		pickedUp.rigidbody2D.fixedAngle = false;
		pickedUp.collider2D.enabled = true;
		pickedUp = null;
	}
}
