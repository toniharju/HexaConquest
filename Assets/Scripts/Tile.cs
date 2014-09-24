using UnityEngine;
using System.Collections;

public enum TileType {

	Grass

}

[ExecuteInEditMode]
[RequireComponent( typeof( MeshFilter ), typeof( MeshRenderer ), typeof( MeshCollider ) )]
public class Tile : MonoBehaviour {

	private bool selected = false;

	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;
	private MeshCollider meshCollider;

	public TileType Model;

	public Vector2 Position;

	// Use this for initialization
	void Start () {
	
		transform.rotation = Quaternion.Euler ( 270, 180, 0 );
		transform.localScale = new Vector3( 100, 100, 100 );

		meshFilter = GetComponent< MeshFilter >();
		meshRenderer = GetComponent< MeshRenderer >();
		meshCollider = GetComponent< MeshCollider > ();
	
		switch( Model ) {

			case TileType.Grass:

				Mesh sharedMesh = Resources.Load< Mesh >( "Models/HexagonGrass" );
				Material[] sharedMaterials = new Material[2];
				sharedMaterials[0] = Resources.Load< Material >( "Materials/HexagonGrass-SidesMaterial" );
				sharedMaterials[1] = Resources.Load< Material >( "Materials/HexagonGrass-TopMaterial" );

				meshFilter.mesh = sharedMesh;
				meshRenderer.materials = sharedMaterials;
				meshCollider.sharedMesh = sharedMesh;

				break;

		}

		if( Position.x % 2 == 1 ) 
			transform.position = new Vector3( Position.x * 1.5f, 0, -1 * Position.y - 1 - Position.y );
		else
			transform.position = new Vector3( Position.x * 1.5f, 0, -1 * Position.y * 2 );

	}
	
	// Update is called once per frame
	void Update () {
	


	}

}
