using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Map : MonoBehaviour {

	private bool mMapDataLoaded = false;
	private GameObject[ , ] mMapData;

    public Vector2 Size = new Vector2( 10, 10 );

	// Use this for initialization
	void Start () {

		mMapData = new GameObject[ ( int )Size.x, ( int )Size.y ];

		int size = ( int )( Size.x * Size.y );
		int x = 0, y = 0;
		for( int i = 0; i < transform.childCount; i++ ) {

			if( i >= size ) break;

			GameObject child = transform.GetChild( i ).gameObject;
			child.GetComponent< Tile >().SetPosition( new Vector2( x, y ) );

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
