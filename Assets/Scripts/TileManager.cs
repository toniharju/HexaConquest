using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour {

	private GameObject current;
	private Color originalColor;

	public static Vector2 MapSize = new Vector2( 10, 10 );
	private static GameObject[,] mapData;
	private static int[,] ownerData;

	// Use this for initialization
	void Start () {
	


	}
	
	// Update is called once per frame
	void Update () {
	
		if ( current != null && Input.GetKeyDown ( KeyCode.Escape ) ) {

			current.renderer.material.color = originalColor;

			current = null;

		}

		if ( Input.GetMouseButtonUp ( 0 ) ) {
			
			RaycastHit hit;
			
			if ( Physics.Raycast ( Camera.main.ScreenPointToRay ( Input.mousePosition ), out hit ) ) {

				if( current != null ) {
					current.renderer.material.color = originalColor;
				}

				current = hit.transform.gameObject;

				if( current.GetComponent< Overlay >() != null ) {

					if( current.GetComponent< Overlay >().Model == OverlayType.Tree ) return;

				}

				originalColor = current.renderer.material.color;

				current.renderer.material.color = new Color( 0.0f, 0.74f, 0.95f );
				
			} else {

				if ( current != null ) {
					
					current.renderer.material.color = originalColor;

					current = null;
					
				}

			}
			
		}

	}

	public GameObject GetCurrent() {

		return current;

	}

	public static void InitializeMap() {

		if( mapData == null )
			mapData = new GameObject[ (int)MapSize.x, (int)MapSize.y ];

		if( ownerData == null )
			ownerData = new int[ (int)MapSize.x, (int)MapSize.y ];

	}

	public static GameObject[,] GetMapData() {
		
		return mapData;
		
	}

	public static int[,] GetOwnerData() {

		return ownerData;

	}

}
