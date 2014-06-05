using UnityEngine;
using System.Collections.Generic;

public class ParallaxLayer : MonoBehaviour {
	[SerializeField]
	float parralaxFactorX = .01f;
	[SerializeField]
	float parralaxFactorY = .01f;

	[SerializeField]
	private List<MeshFilter> meshFilters = new List<MeshFilter>();

	//TODO: I should probably make an interface instead of doing enums.
	public enum Type{
		UseTexture, UseSprite
	}

	public Type type; 

	Vector2 currentOffset;
	public void Scroll (Vector3 moveDelta) {

		currentOffset.x = moveDelta.x * parralaxFactorX;

		//Sort of hacky. There is no guarantee that the y offset wont go past a certain point yet
		currentOffset.y = parralaxFactorY * moveDelta.y;

		if(type == Type.UseTexture){
		
			foreach(MeshFilter meshFilter in meshFilters)
			{
				currentOffset.x = currentOffset.x / meshFilter.renderer.bounds.size.x;
				currentOffset.y = currentOffset.y / meshFilter.renderer.bounds.size.y;
				OffsetTexture(currentOffset, meshFilter.mesh);
			}
		}

		else if(type == Type.UseSprite){
				MovePosition(currentOffset);
		}
	}

	//tex coords are [0..1]
	private void OffsetTexture(Vector2 offset, Mesh mesh)
	{
		Vector2[] uv =  mesh.uv;
		for(int i = 0; i < uv.Length; i++)
		{
			uv[i] += offset;
		}
		
		mesh.uv = uv;
	}	

	private void MovePosition(Vector3 offset)
	{
		Vector3 newPosition = transform.localPosition - offset;
		transform.localPosition = newPosition;
	}
}

//might need this
public static class Helper {
	
	public static float RemapFloat (float value, float from1, float to1, float from2, float to2) {
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}
	
}


