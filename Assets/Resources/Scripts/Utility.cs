using UnityEngine;
using System.Collections;

public static class Utility {
	public delegate void doThis();

	public static IEnumerator wait(float t, doThis d){
		yield return new WaitForSeconds(t);
		d ();
	}
}
