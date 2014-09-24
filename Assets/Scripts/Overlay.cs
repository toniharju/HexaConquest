using UnityEngine;
using System.Collections;

public enum OverlayType {

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

		overlayObject = new GameObject( "Overlay" );

		overlayObject.transform.position = GetComponent< Tile >().Position;;
		overlayObject.transform.rotation = Quaternion.Euler ( 270, 180, 0 );
		overlayObject.transform.localScale = new Vector3( 100, 100, 100 );

		meshFilter = overlayObject.GetComponent< MeshFilter >();
		meshRenderer = overlayObject.GetComponent< MeshRenderer >();
		meshCollider = overlayObject.GetComponent< MeshCollider >();
		
		Mesh sharedMesh = new Mesh();
		Material[] sharedMaterials = new Material[2];
		
		switch( Model ) {
			
		default:
		case OverlayType.Town:
			
			//sharedMesh = Resources.Load< Mesh >( "Models/Town" );
			//sharedMaterials[0] = Resources.Load< Material >( "Materials/HexagonGrass-SidesMaterial" );
			//sharedMaterials[1] = Resources.Load< Material >( "Materials/HexagonGrass-TopMaterial" );
			
			break;
			
		}
		
		meshFilter.mesh = sharedMesh;
		meshRenderer.materials = sharedMaterials;
		meshCollider.sharedMesh = sharedMesh;

	}
	
}
