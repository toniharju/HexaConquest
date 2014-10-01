using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour {

	private GameObject current;
	private Color[] originalColor;

	// Use this for initialization
	void Start () {
	
		originalColor = new Color[2];

	}
	
	// Update is called once per frame
	void Update () {
	
		if ( current != null && Input.GetKeyDown ( KeyCode.Escape ) ) {

			current.renderer.materials[0].color = originalColor[0];
			current.renderer.materials[1].color = originalColor[1];

			current = null;

		}

		if ( Input.GetMouseButtonUp ( 0 ) ) {
			
			RaycastHit hit;
			
			if ( Physics.Raycast ( Camera.main.ScreenPointToRay ( Input.mousePosition ), out hit ) ) {

				if( current != null ) {
					current.renderer.materials[0].color = originalColor[0];
					current.renderer.materials[1].color = originalColor[1];
				}

				current = hit.transform.gameObject;

				originalColor[0] = current.renderer.materials[0].color;
				originalColor[1] = current.renderer.materials[1].color;

				current.renderer.materials[0].color = new Color( 0.0f, 0.74f, 0.95f );
				current.renderer.materials[1].color = new Color( 0.0f, 0.74f, 0.95f );
				
			} else {

				if ( current != null ) {
					
					current.renderer.materials[0].color = originalColor[0];
					current.renderer.materials[1].color = originalColor[1];
					
					current = null;
					
				}

			}
			
		}

	}

	public GameObject GetCurrent() {

		return current;

	}

}
