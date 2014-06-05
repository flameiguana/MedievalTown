using UnityEngine;
using System.Collections;

public class ScaleCamera : MonoBehaviour {

	public Sprite background;
	public float width;
	// Use this for initialization
	void Start () {

		//Takes the height of background sprite in world units.
		float desiredHeightUnits = background.bounds.size.y;
		//adjust camera to match this
		camera.orthographicSize = desiredHeightUnits / 2.0f;
		width = camera.orthographicSize * 2f * camera.aspect;
		//print (width);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
