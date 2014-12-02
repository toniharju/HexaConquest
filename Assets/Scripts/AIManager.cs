using UnityEngine;
using System.Collections;

public class AIManager : MonoBehaviour {

	private bool mIsFirstEncounter = true;
	private bool mIsFirstTurn = true;

	private GameObject mTown;
	private AILand mAILand;

	private int mTiles = 1;
	private int mMines = 0;
	private int[] mQueue = new int[ 3 ];
	private int[] mGarrison = new int[ 3 ];

	private int[,,] mArmy;

	private int previousTurn = 0;
	private int currentTurn = 0;

	private Map mMap;

	// Use this for initialization
	void Start () {

		if( GameObject.Find( "MapData" ) != null ) mMap = GameObject.Find( "MapData" ).GetComponent<Map>();

		mArmy = new int[ (int)mMap.Size.x, (int)mMap.Size.y, 3 ];

		mTown = GameObject.Find( "TownEnemy" );
		mAILand = mTown.GetComponent<AILand>();

	}
	
	// Update is called once per frame
	public void OnTurn () {

		previousTurn = currentTurn;
		currentTurn++;

		mGarrison[ 0 ] += mQueue[ 0 ];
		mGarrison[ 1 ] += mQueue[ 1 ];
		mGarrison[ 2 ] += mQueue[ 2 ];
		mQueue[ 0 ] = 0;
		mQueue[ 1 ] = 0;
		mQueue[ 2 ] = 0;
		
		//Check if AI has gold and act accordingly
		if( mAILand.GetGold() > 9 && mTown.transform.parent.GetComponent< Tile >().GetUnits().Count < 32 ) {

			int buy = ( int )( mAILand.GetGold() / 10 );
			mAILand.RemoveGold( buy * 10 );

			int x = (int)mTown.transform.parent.GetComponent<Tile>().GetPosition().x;
			int y = (int)mTown.transform.parent.GetComponent<Tile>().GetPosition().y;

			mArmy[ x, y, ( int )UnitType.Footman ] += buy;
			mQueue[ ( int )UnitType.Footman ] += buy;
			
			for( int i = 0; i < buy; i++ ) mTown.transform.parent.GetComponent<Tile>().AddFootman( 2 );

		}
		
		if( mGarrison[ 0 ] > 0 || mGarrison[ 1 ] > 0 || mGarrison[ 2 ] > 0 ) {

			if( mIsFirstTurn ) {
				
				//Choose waypoint path here later on
				Waypoint wp = mTown.transform.parent.FindChild( "Waypoint" ).GetComponent<Waypoint>();
				wp.MoveToNext( 1, 0, 0 );

				int x1 = ( int )mTown.transform.parent.GetComponent<Tile>().GetPosition().x;
				int y1 = ( int )mTown.transform.parent.GetComponent<Tile>().GetPosition().y;

				int x2 = ( int )wp.GetNext().GetComponent<Tile>().GetPosition().x;
				int y2 = ( int )wp.GetNext().GetComponent<Tile>().GetPosition().y;

				mArmy[ x1, y1, ( int )UnitType.Footman ]--;
				mArmy[ x2, y2, ( int )UnitType.Footman ]++;

			} else {



			}

		}

		if( mIsFirstTurn && currentTurn > 1 ) mIsFirstTurn = false;

	}

}
