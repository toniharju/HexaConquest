using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Position : MonoBehaviour {

	public Vector2 Location = new Vector2( 0, 0 );

	// Use this for initialization
	void Update () {
	
		if( Location.x % 2 == 1 )
			transform.position = new Vector3( Location.x * 1.5f, 0, -1 * Location.y - 1 - Location.y );
		else
			transform.position = new Vector3( Location.x * 1.5f, 0, -1 * Location.y * 2 );

	}

}
