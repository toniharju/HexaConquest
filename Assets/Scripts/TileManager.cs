using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour {

	private GameObject current;

	// Use this for initialization
	void Start () {
	


	}
	
	// Update is called once per frame
	void Update () {
	
		if ( Input.GetMouseButtonUp ( 0 ) ) {
			
			RaycastHit hit;
			
			if ( Physics.Raycast ( Camera.main.ScreenPointToRay ( Input.mousePosition ), out hit ) ) {

				if( current != null )
					current.renderer.material.color = new Color( 0, 0, 0 );

				current = hit.transform.gameObject;

				current.renderer.material.color = new Color( 0, 255, 0 );
				
			}
			
		}

	}

}
