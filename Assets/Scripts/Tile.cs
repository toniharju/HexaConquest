using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TileType {

	Grass,
	Snow

}

[ExecuteInEditMode]
[RequireComponent( typeof( MeshFilter ), typeof( MeshRenderer ), typeof( MeshCollider ) )]
public class Tile : MonoBehaviour {

	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;
	private MeshCollider meshCollider;

	private int captureProgress = 0;

	public List< Unit > Units = new List< Unit >();

	public TileType Model;
	public Vector2 Position;
	public int GoldValue = 1;
	public int CaptureValue = 200;
	public byte Owner = 0;

	// Use this for initialization
	void Start () {

		TileManager.InitializeMap ();

		transform.rotation = Quaternion.Euler ( 270, 180, 0 );
		transform.localScale = new Vector3( 100, 100, 100 );

		gameObject.isStatic = true;
		renderer.castShadows = false;

		meshFilter = GetComponent< MeshFilter >();
		meshRenderer = GetComponent< MeshRenderer >();
		meshCollider = GetComponent< MeshCollider >();
	
		Mesh sharedMesh;
		Material[] sharedMaterials = new Material[2];

		switch( Model ) {

			default:
			case TileType.Grass:

				sharedMesh = Resources.Load< Mesh >( "Models/HexagonGrass" );
				sharedMaterials[0] = Resources.Load< Material >( "Materials/HexagonGrass-SidesMaterial" );
				sharedMaterials[1] = Resources.Load< Material >( "Materials/HexagonGrass-TopMaterial" );

				break;

			case TileType.Snow:

				sharedMesh = Resources.Load< Mesh >( "Models/HexagonSnow" );
				sharedMaterials[0] = Resources.Load< Material >( "Materials/HexagonSnow-SidesMaterial" );
				sharedMaterials[1] = Resources.Load< Material >( "Materials/HexagonSnow-TopMaterial" );

				break;

		}

		meshFilter.sharedMesh = sharedMesh;
		meshRenderer.sharedMaterials = sharedMaterials;
		meshCollider.sharedMesh = sharedMesh;

		if( Position.x % 2 == 1 )
			transform.position = new Vector3( Position.x * 1.5f, 0, -1 * Position.y - 1 - Position.y );
		else
			transform.position = new Vector3( Position.x * 1.5f, 0, -1 * Position.y * 2 );

		TileManager.GetMapData() [ (int)Position.x, (int)Position.y ] = this.gameObject;
		TileManager.GetOwnerData() [ (int)Position.x, (int)Position.y ] = Owner;

	}

	private void UpdateFog() {

		int x = (int)Position.x;
		int y = (int)Position.y;

		if( GetComponent< Overlay >() != null && GetComponent< Overlay >().Model == OverlayType.FriendlyTown ) {

			for( int ox = -1; ox < 2; ox++ ) {
				
				for( int oy = -1; oy < 2; oy++ ) {

					int dx = x + ox;
					int dy = y + oy;

					if( dx > -1 && dx < TileManager.MapSize.x &&
					    dy > -1 && dy < TileManager.MapSize.y && TileManager.GetMapData()[ dx, dy ].layer != 8 ) {

						if( x % 2 == 1 ) {
							if( ox != 0 && oy == -1 ) continue;
						} else {
							if( ox != 0 && oy == 1 ) continue;
						}

						TileManager.GetMapData ()[ dx, dy ].layer = 8;
						
					}
					
				}
				
			}
			
		}

	}

	public void OnTurnUpdate() {

		if (captureProgress <= -CaptureValue) {
			
			captureProgress = -CaptureValue;
			Owner = 1;
			TileManager.GetOwnerData() [ (int)Position.x, (int)Position.y ] = 1;
			renderer.materials[0].color = new Color( 0.0f, 1.0f, 0.0f );
			renderer.materials[1].color = new Color( 0.0f, 1.0f, 0.0f );
			
		} else if (captureProgress >= CaptureValue) {
			
			captureProgress = CaptureValue;
			Owner = 2;
			TileManager.GetOwnerData() [ (int)Position.x, (int)Position.y ] = 2;
			renderer.materials[0].color = new Color( 1.0f, 0.0f, 0.0f );
			renderer.materials[1].color = new Color( 1.0f, 0.0f, 0.0f );
			
		}

		UpdateFog ();

	}

	public List< Unit > GetUnits() {

		return Units;

	}

	public void MoveUnitFrom( UnitType type, Vector2 position ) {

		int x = (int)position.x;
		int y = (int)position.y;

		Overlay overlay = TileManager.GetMapData ()[ x, y ].GetComponent< Overlay >();

		if( overlay != null && overlay.Model == OverlayType.FriendlyTown ) {



			Unit u = new Unit();
			u.SetOwner ( GameObject.Find ( "Manager" ).GetComponent< PlayerManager >().GetTurn () );
			u.SetUnitType( type );

			Units.Add ( u );

		} else {



		}

	}

}
