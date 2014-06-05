using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	static Camera main;
	[SerializeField] float defaultZoom = 6f;
	[SerializeField] float maxZoom = 18f;
	[SerializeField] float ZoomYScalar = 2f;

	// Use this for initialization
	void Start () {
		main = Camera.main;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		main.orthographicSize = defaultZoom + transform.position.y / ZoomYScalar;
		if(main.orthographicSize > maxZoom)
			main.orthographicSize = maxZoom;
	}
}
