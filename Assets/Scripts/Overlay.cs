using UnityEngine;
using System.Collections;

public class Overlay : MonoBehaviour {

	public bool Clickable = false;

	// Update is called once per frame
	void Update () {
	
		if( transform.parent.gameObject.layer == 8 ) {
			gameObject.layer = 8;
		} else {
			gameObject.layer = 0;
		}

	}

}
