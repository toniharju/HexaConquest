using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	private int turn = 1;

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
				
					TileManager.GetMapData ()[ x, y ].GetComponent< Tile >().OnTurnUpdate ();

				}

			}

			turn = 2;

		} else if( turn == 2 ) {

			turn = 1;

		}

	}

	public bool IsPlayerTurn() { return turn == 1; }
	public bool IsAITurn() { return turn == 2; }

}
