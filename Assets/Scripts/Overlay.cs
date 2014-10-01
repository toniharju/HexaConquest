using UnityEngine;
using System.Collections;

public enum OverlayType {

	Tree,
	Town

}

[ExecuteInEditMode]
public class Overlay : MonoBehaviour {

	private GameObject overlayObject;

	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;
	private MeshCollider meshCollider;

	public OverlayType Model;

	// Use this for initialization
	void Start () {

		if( transform.childCount != 0 ) return;

		overlayObject = new GameObject( "Overlay" );
		overlayObject.transform.parent = gameObject.transform;

		overlayObject.transform.position = transform.position;
		overlayObject.transform.rotation = Quaternion.Euler ( 270, 180, 0 );
		overlayObject.transform.localScale = new Vector3( 1, 1, 1 );

		overlayObject.AddComponent< MeshFilter >();
		overlayObject.AddComponent< MeshRenderer >();

		meshFilter = overlayObject.GetComponent< MeshFilter >();
		meshRenderer = overlayObject.GetComponent< MeshRenderer >();
		
		Mesh sharedMesh = new Mesh();
		Material[] sharedMaterials = new Material[2];
		
		switch( Model ) {
			
			default:
			case OverlayType.Town:
			
				//sharedMesh = Resources.Load< Mesh >( "Models/Town" );
				//sharedMaterials[0] = Resources.Load< Material >( "Materials/HexagonGrass-SidesMaterial" );
				//sharedMaterials[1] = Resources.Load< Material >( "Materials/HexagonGrass-TopMaterial" );
			
				break;

			case OverlayType.Tree:

				sharedMesh = Resources.Load< Mesh >( "Models/Tree" );
				sharedMaterials[0] = Resources.Load< Material >( "Materials/Tree-Bark" );
				sharedMaterials[1] = Resources.Load< Material >( "Materials/Tree-Leaves" );

				break;
			
		}
		
		meshFilter.mesh = sharedMesh;
		meshRenderer.materials = sharedMaterials;

	}
	
}
