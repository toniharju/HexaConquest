using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class Map : MonoBehaviour {

	private bool mMapDataLoaded = false;
	private GameObject[ , ] mMapData;

	private GameObject mCanvas;

    public Vector2 Size = new Vector2( 10, 10 );
	public GameObject MonetaryValueObj;

	// Use this for initialization
	void Start () {
		
		mMapData = new GameObject[ ( int )Size.x, ( int )Size.y ];

		mCanvas = GameObject.Find( "WorldCanvas" );

		int size = ( int )( Size.x * Size.y );
		int x = 0, y = 0;
		for( int i = 0; i < transform.childCount; i++ ) {

			if( i >= size ) break;

			GameObject child = transform.GetChild( i ).gameObject;
			child.GetComponent< Tile >().SetId( i );
			child.GetComponent< Tile >().SetPosition( new Vector2( x, y ) );
			
			//if( mCanvas.transform.childCount > 0 ) {



			//} else {

				/*GameObject value = Instantiate( MonetaryValueObj ) as GameObject;
				value.name = "MonetaryValue";
				value.transform.parent = mCanvas.transform;
				value.GetComponent<RectTransform>().position = new Vector3( child.transform.localPosition.x, 0.5f, child.transform.localPosition.z );
				value.GetComponent<RectTransform>().localRotation = Quaternion.Euler( -30, 35, -55 );

				if( !child.activeSelf || child.transform.childCount > 0 ) value.SetActive( false );*/
				

			//}

			if( child.transform.childCount > 0 && child.transform.GetChild( 0 ).tag == "Impassable" )
				child.tag = "Impassable";

			mMapData[ x, y ] = child;

			if( x == Size.x - 1 ) {

				x = 0;
				y++;

			} else {

				x++;

			}

		}

		mMapDataLoaded = true;

	}

	public GameObject[,] GetMapData() { return mMapData; }

	public bool IsMapDataLoaded() { return mMapDataLoaded; }
         
}
