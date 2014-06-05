using UnityEngine;
using System.Collections.Generic;

public class ParallaxSet : MonoBehaviour {

	[SerializeField]
	private List<ParallaxLayer> layers = new List<ParallaxLayer>();

	public Transform FollowTarget;
	private Vector3 oldFollowPosition;

	void Update()
	{
		ScrollLayers(FollowTarget.position - oldFollowPosition);
		oldFollowPosition = FollowTarget.position;
	}

	private void ScrollLayers(Vector3 moveDelta)
	{
		foreach(ParallaxLayer layer in layers)
		{
			layer.Scroll(moveDelta);
		}
	}
}
