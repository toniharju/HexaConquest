using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {

	private byte turn = 1;

	private static int[] units = new int[3];

	public Vector2 FriendlyTownLocation;

	// Use this for initialization
	void Start () {
	


	}
	
	// Update is called once per frame
	void Update () {

		if( Input.GetKeyDown ( KeyCode.Space ) && turn == 1 ) {

			int mapW = (int)TileManager.MapSize.x;
			int mapH = (int)TileManager.MapSize.y;
			
			for( int y = 0; y < mapH; y++ ) {
				
				for( int x = 0; x < mapW; x++ ) {
					
					TileManager.GetMapData ()[ x, y ].layer = 0;
					
				}
				
			}

			for( int y = 0; y < mapH; y++ ) {

				for( int x = 0; x < mapW; x++ ) {
				
					List< Unit > tile_units = TileManager.GetMapData ()[ x, y ].GetComponent< Tile >().GetUnits();

					if( tile_units.Count > 0 ) {



					}

					TileManager.GetMapData ()[ x, y ].GetComponent< Tile >().OnTurnUpdate ();

				}

			}

			turn = 2;

		} else if( turn == 2 ) {

			turn = 1;

		}

	}

	public void AddFootmanSquad() {

		units[ (int)UnitType.Footman ]++;

	}

	public static int GetAllUnits( UnitType type ) {

		return units[ (int)type ];

	}

	public byte GetTurn() { return turn; }

	public bool IsPlayerTurn() { return turn == 1; }
	public bool IsAITurn() { return turn == 2; }

}
