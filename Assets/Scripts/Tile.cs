using UnityEngine;
using System.Collections;

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
	
	private int owner = 0;
	private int captureProgress = 0;

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

	}
	
	// Update is called once per frame
	void Update () {
	
		if (captureProgress <= -CaptureValue) {

			captureProgress = -CaptureValue;
			SetOwner (1);
			renderer.materials[0].color = new Color( 0.0f, 1.0f, 0.0f );
			renderer.materials[1].color = new Color( 0.0f, 1.0f, 0.0f );

		} else if (captureProgress >= CaptureValue) {

			captureProgress = CaptureValue;
			SetOwner (2);
			renderer.materials[0].color = new Color( 1.0f, 0.0f, 0.0f );
			renderer.materials[1].color = new Color( 1.0f, 0.0f, 0.0f );

		}

	}

	public void SetOwner( int value ) {

		owner = value;

	}

	public int GetOwner() {

		return owner;

	}

}
