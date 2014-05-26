using UnityEngine;
using System.Collections;

public class Wessbat : MonoBehaviour {
	private GameObject pickedUp = null; //object being held

	void Start () {
	
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.F)) {
			Drop ();
		}
		//control scheme goes here
	}

	void OnCollisionStay2D(Collision2D target){
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
		SpringJoint2D joint = target.AddComponent<SpringJoint2D> ();
		joint.connectedBody = rigidbody2D;
		joint.frequency = 20f;
		joint.distance = 0.01f;
		joint.anchor = new Vector2 (0, 1);	//inline anchor, will need to specify per pickupable object in future
		target.transform.parent = gameObject.transform;
		//target.rigidbody2D.fixedAngle = true;
		//target.collider2D.enabled = false;
		pickedUp = target;
		target.layer = LayerMask.NameToLayer ("Picked Up Object");
	}

	void Drop(){						//Drop the object
		if (pickedUp == null)
			return;
		pickedUp.transform.parent = null;
		Destroy (pickedUp.GetComponent<SpringJoint2D> ());
		//pickedUp.rigidbody2D.fixedAngle = false;
		//pickedUp.collider2D.enabled = true;
		GameObject droppedObject = pickedUp;
		Utility.doThis d = () => {
			droppedObject.layer = LayerMask.NameToLayer ("Enemy");
		};
		StartCoroutine(Utility.wait (1f, d));
		pickedUp = null;
	}
}
