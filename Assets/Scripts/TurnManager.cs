using UnityEngine;
using System.Collections;

public enum Turn {

	Player = 1,
	AI

}

public class TurnManager : MonoBehaviour {

	private Turn mTurn = Turn.Player;

	private Map mMap;

	// Use this for initialization
	void Start () {

		if( GameObject.Find( "MapData" ) != null ) {

			mMap = GameObject.Find( "MapData" ).GetComponent<Map>();

		}

	}
	
	// Update is called once per frame
	void Update () {

		if( mTurn == Turn.Player && Input.GetKeyUp( KeyCode.Space ) ) {

			int width = ( int )mMap.Size.x;
			int height = ( int )mMap.Size.y;
			
			for( int y = 0; y < height; y++ ) {

				for( int x = 0; x < width; x++ ) {

					mMap.GetMapData()[ x, y ].GetComponent<Tile>().OnTurn();

				}

			}

			StateManager.Clear();
			
			mTurn = Turn.AI;

		} else {

			mTurn = Turn.Player;

		}

	}

	public Turn GetTurn() { return mTurn; }

}
