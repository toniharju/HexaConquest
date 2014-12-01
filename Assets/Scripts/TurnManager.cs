using UnityEngine;
using UnityEngine.UI;
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

					mMap.GetMapData()[ x, y ].layer = 0;
					GameObject.Find( "WorldCanvas" ).transform.GetChild( mMap.GetMapData()[ x, y ].GetComponent<Tile>().GetId() ).gameObject.SetActive( false );

				}

			}

			Land land = GameObject.Find( "TownFriendly" ).GetComponent<Land>();
			int goldIncome = land.GetGold();

			for( int y = 0; y < height; y++ ) {

				for( int x = 0; x < width; x++ ) {

					mMap.GetMapData()[ x, y ].GetComponent<Tile>().OnTurn();

				}

			}

			goldIncome = land.GetGold() - goldIncome;
			land.SetGoldIncome( goldIncome );

			StateManager.Clear();
			
			mTurn = Turn.AI;

			AILand ailand = GameObject.Find( "TownEnemy" ).GetComponent<AILand>();
			goldIncome = land.GetGold();

			for( int y = 0; y < height; y++ ) {

				for( int x = 0; x < width; x++ ) {

					mMap.GetMapData()[ x, y ].GetComponent<Tile>().OnTurn();

				}

			}

			goldIncome = ailand.GetGold() - goldIncome;
			ailand.SetGoldIncome( goldIncome );

			GameObject.Find( "Managers" ).GetComponent<AIManager>().OnTurn();

			mTurn = Turn.Player;

		}

	}

	public Turn GetTurn() { return mTurn; }

}
