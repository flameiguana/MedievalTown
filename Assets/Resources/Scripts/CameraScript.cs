using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	static Camera main;
	[SerializeField] float defaultZoom = 8f;
	[SerializeField] float maxZoom = 16f;
	[SerializeField] float ZoomYScalar = 2f;

	float horizonLine = 7f;
	[SerializeField] Transform target;
	// Use this for initialization
	void Start () {
		main = Camera.main;
	}
	[SerializeField]
	private Camera extraCamera;

	private Vector3 targetPosition;
	// Update is called once per frame
	void LateUpdate () {
		targetPosition = target.position;
		targetPosition.z = transform.position.z;
		transform.position = targetPosition;
		
		float zoom = CalculateZoom();
		main.orthographicSize = zoom;
		//TODO: Let standard camera zoom have some effect on background camera.
		//if(extraCamera != null)
			//extraCamera.orthographicSize = zoom;
	}

	float CalculateZoom()
	{
		float zoom = defaultZoom;
		if(transform.position.y > horizonLine)
		{
			zoom = defaultZoom + (transform.position.y - horizonLine) / ZoomYScalar;
			
			if(zoom > maxZoom)
				zoom = maxZoom;
		}
		return zoom;
	}
}
