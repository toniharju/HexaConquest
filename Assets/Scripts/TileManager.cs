using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TileManager {

	private static GameObject castleInfoPanel;
	private static GameObject tileInfoPanel;

	private static GameObject current;
	private static Color originalColor;

	public static Vector2 MapSize = new Vector2( 10, 10 );
	private static GameObject[,] mapData;
	private static byte[,] ownerData;

	// Use this for initialization
	public static void Initialize () {
	
		castleInfoPanel = GameObject.Find ( "CastleInfoPanel" );
		castleInfoPanel.SetActive ( false );

		tileInfoPanel = GameObject.Find ( "TileInfoPanel" );
		tileInfoPanel.SetActive ( false );

	}
	
	// Update is called once per frame
	public static void Update () {
	
		if ( current != null && ( Input.GetKeyDown ( KeyCode.Escape ) || Input.GetKeyDown ( KeyCode.Space ) ) ) {

			tileInfoPanel.SetActive ( false );
			castleInfoPanel.SetActive ( false );

			current.renderer.material.color = originalColor;
			current = null;

		}

		if ( Input.GetMouseButtonUp ( 0 ) && !EventSystem.current.IsPointerOverGameObject () ) {
			
			RaycastHit hit;
			
			if ( Physics.Raycast ( Camera.main.ScreenPointToRay ( Input.mousePosition ), out hit ) ) {

				if( current != null ) {
					current.renderer.material.color = originalColor;
				}

				current = hit.transform.gameObject;

				if( current.GetComponent< Overlay >() != null && current.GetComponent< Overlay >().Model == OverlayType.FriendlyTown ) {

					tileInfoPanel.SetActive ( false );
					castleInfoPanel.SetActive ( true );

				} else if( current.GetComponent< Overlay >() != null && current.GetComponent< Overlay >().Model == OverlayType.Tree ) {

					tileInfoPanel.SetActive ( false );
					castleInfoPanel.SetActive ( false );

				} else {

					tileInfoPanel.SetActive ( true );
					castleInfoPanel.SetActive ( false );
					
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

	public static GameObject GetCurrent() {

		return current;

	}

	public static void InitializeMap() {

		if( mapData == null )
			mapData = new GameObject[ (int)MapSize.x, (int)MapSize.y ];

		if( ownerData == null )
			ownerData = new byte[ (int)MapSize.x, (int)MapSize.y ];

	}

	public static GameObject[,] GetMapData() {
		
		return mapData;
		
	}

	public static byte[,] GetOwnerData() {

		return ownerData;

	}

}
