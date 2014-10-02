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

		if( transform.childCount > 0 ) DestroyImmediate ( transform.FindChild ( "Overlay" ).gameObject );

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
		Material[] sharedMaterials;
		
		switch( Model ) {
			
			default:
			case OverlayType.Town:

				sharedMaterials = new Material[3];
			
				sharedMesh = Resources.Load< Mesh >( "Models/Town" );
				sharedMaterials[0] = Resources.Load< Material >( "Materials/Town-House" );
				sharedMaterials[1] = Resources.Load< Material >( "Materials/Town-Castle" );
				sharedMaterials[2] = Resources.Load< Material >( "Materials/Town-CastleDoors" );
			
				break;

			case OverlayType.Tree:

				sharedMaterials = new Material[2];

				sharedMesh = Resources.Load< Mesh >( "Models/Tree" );
				sharedMaterials[0] = Resources.Load< Material >( "Materials/Tree-Bark" );
				sharedMaterials[1] = Resources.Load< Material >( "Materials/Tree-Leaves" );

				break;
			
		}
		
		meshFilter.mesh = sharedMesh;
		meshRenderer.materials = sharedMaterials;

	}
	
}
