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

	private GameObject[] squads = new GameObject[ 3 ];

	public TileType Model;
	public Vector2 Position;
	public int GoldValue = 1;
	public int CaptureValue = 200;

	// Use this for initialization
	void Start () {

		TileManager.InitializeMap ();

		transform.rotation = Quaternion.Euler ( 270, 180, 0 );
		transform.localScale = new Vector3( 100, 100, 100 );

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

		meshFilter.mesh = sharedMesh;
		meshRenderer.materials = sharedMaterials;
		meshCollider.sharedMesh = sharedMesh;

		if( Position.x % 2 == 1 )
			transform.position = new Vector3( Position.x * 1.5f, 0, -1 * Position.y - 1 - Position.y );
		else
			transform.position = new Vector3( Position.x * 1.5f, 0, -1 * Position.y * 2 );

		TileManager.GetMapData() [ (int)Position.x, (int)Position.y ] = this.gameObject;
		TileManager.GetOwnerData() [ (int)Position.x, (int)Position.y ] = 0;

	}

	public void OnTurnUpdate() {

		if (captureProgress <= -CaptureValue) {
			
			captureProgress = -CaptureValue;
			TileManager.GetOwnerData() [ (int)Position.x, (int)Position.y ] = 1;
			renderer.materials[0].color = new Color( 0.0f, 1.0f, 0.0f );
			renderer.materials[1].color = new Color( 0.0f, 1.0f, 0.0f );
			
		} else if (captureProgress >= CaptureValue) {
			
			captureProgress = CaptureValue;
			TileManager.GetOwnerData() [ (int)Position.x, (int)Position.y ] = 2;
			renderer.materials[0].color = new Color( 1.0f, 0.0f, 0.0f );
			renderer.materials[1].color = new Color( 1.0f, 0.0f, 0.0f );
			
		}

		if( TileManager.GetOwnerData ()[ (int)Position.x, (int)Position.y ] == 1 ) {

			

		}

	}

	public void AddSquad( SquadType type, int size ) {

		if( squads[ (int)type ] != null ) return;

		GameObject temp = Instantiate ( Resources.Load ( "Prefabs/FootmanSquad" ) ) as GameObject;
		Squad squad = temp.AddComponent< Squad >();

		temp.transform.parent = gameObject.transform;

		squads[ (int)type ] = temp;

	}

	public void MoveSquad( SquadType type, Vector2 to ) {



	}

	public void SetSquadSize( SquadType type, int size ) {

		//squad[ type ] = size;

	}

}
