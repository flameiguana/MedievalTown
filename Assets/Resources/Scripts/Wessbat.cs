using UnityEngine;
using System.Collections;

public class Wessbat : MonoBehaviour {
	private GameObject pickedUp = null; //object being held

	void Start () {
	
	}

	void Update () {
		if (Input.GetKeyUp (KeyCode.LeftShift)) {
			Drop ();
		}
		/*if (Input.GetKeyDown(KeyCode.S)) {
			Kill();
		}*/
		//control scheme goes here
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
	}

	void PickUp(GameObject target){		//Pick up the object
		if (pickedUp != null)
			return;
		else pickedUp = target;
		SpringJoint2D joint = target.AddComponent<SpringJoint2D> ();
		joint.connectedBody = rigidbody2D;
		joint.frequency = 20f;
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
			droppedObject.layer = LayerMask.NameToLayer ("Enemy");
		};
		if (droppedObject != null)
			StartCoroutine(Utility.wait (1f, d));
		pickedUp = null;
	}

	private void Kill() { // Kill the villager if one is currently held
		if (pickedUp.GetComponent<Villager>()){
			Drop ();
			pickedUp.GetComponent<Villager>().explode();
		}
	}
}
