using UnityEngine;
using System.Collections;

public enum TileType {

	Grass

}

public class Tile : MonoBehaviour {

	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;

	public TileType Model;

	public Vector2 Position;

	// Use this for initialization
	void Start () {
	
		transform.rotation = Quaternion.Euler ( 270, 180, 0 );
		transform.localScale = new Vector3( 100, 100, 100 );

		gameObject.AddComponent< MeshFilter >();
		gameObject.AddComponent< MeshRenderer >();

		meshFilter = GetComponent< MeshFilter >();
		meshRenderer = GetComponent< MeshRenderer >();
	
		switch( Model ) {

			case TileType.Grass:

				Mesh sharedMesh = Resources.Load< Mesh >( "Models/HexagonGrass" );
				Material[] sharedMaterials = new Material[2];
				sharedMaterials[0] = Resources.Load< Material >( "Materials/HexagonGrass-SidesMaterial" );
				sharedMaterials[1] = Resources.Load< Material >( "Materials/HexagonGrass-TopMaterial" );

				meshFilter.mesh = sharedMesh;
				meshRenderer.materials = sharedMaterials;

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
