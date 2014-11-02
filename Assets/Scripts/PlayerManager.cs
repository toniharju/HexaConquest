﻿using UnityEngine;
using System.Collections;

public enum State {

	Wait,
	SelectTile

}

public class PlayerManager : MonoBehaviour {

	private TileManager mTileManager;

	private State mState = State.Wait;
	private string mStateParameters;

	private int mPlayerGold = 0;
	private int[] mPlayerArmy = new int[3];
	
	private int mAIGold = 0;
	private int[] mAIArmy = new int[3];

	// Use this for initialization
	void Start () {
		
		mTileManager = GameObject.Find ( "Manager" ).GetComponent< TileManager >();

	}
	
	// Update is called once per frame
	void Update () {

		if( Input.GetKeyDown ( KeyCode.Space ) ) {
			
			int width = (int) GameObject.Find ( "Manager" ).GetComponent< TileManager >().MapSize.x;
			int height = (int) GameObject.Find ( "Manager" ).GetComponent< TileManager >().MapSize.y;
			
			if( Turn.IsPlayer () ) {

				for( int y = 0; y < height; y++ ) {
					
					for( int x = 0; x < width; x++ ) {
						
						mTileManager.GetTiles() [ x, y ].layer = 0;
						
					}
					
				}

				for( int y = 0; y < height; y++ ) {
					
					for( int x = 0; x < width; x++ ) {
						
						mTileManager.GetTiles() [ x, y ].GetComponent< Tile >().OnTurn ();
						
					}
					
				}

				mTileManager.ResetSelectedTile ();
				Turn.SetTurn ( Turn.AI );
				
			} else {
				
				Turn.SetTurn ( Turn.Player );
				
			}
			
		}
		
	}

	public void AddFootman() {
		
		if( Turn.IsPlayer () ) {
			
			mPlayerArmy[ (int)UnitType.Footman ]++;
			
		} else {
			
			mAIArmy[ (int)UnitType.Footman ]++;
			
		}
		
	}
	
	public void AddArcher() {
		
		if( Turn.IsPlayer () ) {
			
			mPlayerArmy[ (int)UnitType.Archer ]++;
			
		} else {
			
			mAIArmy[ (int)UnitType.Archer ]++;
			
		}
		
	}
	
	public void AddLancer() {
		
		if( Turn.IsPlayer () ) {
			
			mPlayerArmy[ (int)UnitType.Lancer ]++;
			
		} else {
			
			mAIArmy[ (int)UnitType.Lancer ]++;
			
		}
		
	}
	
	public void MoveFootman() {

		if( Turn.IsPlayer () ) {

			if( mTileManager.GetSelectedTile () != null ) {

				if( mTileManager.GetSelectedTile ().transform.FindChild ( "OverlayFriendlyTown" ) != null ) {

					if( mPlayerArmy[ (int)UnitType.Footman ] > 0 ) {
						
						SetState( State.SelectTile );
						SetStateParameters ( "move_footman" );
						
					}

				} else {

					if( mTileManager.GetSelectedTile ().GetComponent< Tile >().Units.Count > 0 ) {

						SetState ( State.SelectTile );
						SetStateParameters ( "move_footman" );

					}

				}

			}
			
		} else {

		}

	}

	public void SetStateParameters( string parameters ) { mStateParameters = parameters; }
	public string GetStateParameters() { return mStateParameters; }

	public void SetState( State state ) { mState = state; }
	public State GetState() { return mState; }

	public void RemovePlayerUnit( UnitType type, int ammount ) {

		if( mPlayerArmy[ (int)type ] > 0 ) mPlayerArmy[ (int)type ] -= ammount;

	}

	public void SetPlayerGold( int gold ) { mPlayerGold = gold; }
	public int GetPlayerGold() { return mPlayerGold; }
	
	public void SetAIGold( int gold ) { mAIGold = gold; }
	public int GetAIGold() { return mAIGold; }
	
	public int[] GetPlayerArmy() { return mPlayerArmy; }
	
	public int[] GetAIArmy() { return mAIArmy; }
	
}
