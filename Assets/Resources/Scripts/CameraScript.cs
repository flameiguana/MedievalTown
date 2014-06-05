using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	static Camera main;
	[SerializeField] float defaultZoom = 8f;
	[SerializeField] float maxZoom = 16f;
	[SerializeField] float ZoomYScalar = 2f;

	float horizonLine = 7f;

	// Use this for initialization
	void Start () {
		main = Camera.main;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(transform.position.y > horizonLine)
		{
			main.orthographicSize = defaultZoom + (transform.position.y - horizonLine) / ZoomYScalar;

			if(main.orthographicSize > maxZoom)
				main.orthographicSize = maxZoom;
		}
		else main.orthographicSize = defaultZoom;
	}
}
