using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum TileOwner {

	None = 0,
	Player,
	AI

}

public class Tile : MonoBehaviour {

    private bool isLand = false;
	private bool isMine = false;

	private List<Unit> mUnits = new List<Unit>();
	private List<Unit> mTempUnits = new List<Unit>();
    private int[] mUnitCount = new int[ 3 ];
	private int[] mTempUnitCount = new int[ 3 ];

	private GameObject mUnit;

	private Color[] mColors = {	new Color( 0.8f, 0.8f, 0.8f ),	//Normal
								new Color( 0, 1.0f, 0 ),		//Player
								new Color( 1.0f, 0, 0 ),		//AI
								new Color( 0, 0.7f, 0.8f ) };	//Selected

	private Vector2 mPosition = Vector2.zero;
	private TileManager mTileManager;
	private TurnManager mTurnManager;

    public int GoldValue = 1;
    public TileOwner Owner;

	void Start() {

		if( GameObject.Find( "Managers" ) != null ) {

			mTileManager = GameObject.Find( "Managers" ).GetComponent<TileManager>();
			mTurnManager = GameObject.Find( "Managers" ).GetComponent<TurnManager>();

		}

		if( GetComponentInChildren<Land>() != null )
			isLand = true;
		else if( transform.FindChild( "Mine" ) != null )
			isMine = true;

	}

	void Update() {

		if( mTileManager.GetSelectedTile() == gameObject ) {

			renderer.material.color = mColors[ 3 ];

		} else {

			renderer.material.color = mColors[ ( int )Owner ];

		}

		if( isLand ) {

			mTileManager.GetQueueImages()[ 0 ].transform.FindChild( "Count" ).GetComponent<Text>().text = mTempUnitCount[ ( int )UnitType.Footman ].ToString();
			mTileManager.GetQueueImages()[ 1 ].transform.FindChild( "Count" ).GetComponent<Text>().text = mTempUnitCount[ ( int )UnitType.Archer ].ToString();
			mTileManager.GetQueueImages()[ 2 ].transform.FindChild( "Count" ).GetComponent<Text>().text = mTempUnitCount[ ( int )UnitType.Lancer ].ToString();

		}

	}

	public void OnTurn() {

		if( mTempUnits.Count > 0 ) {

			foreach( Unit unit in mTempUnits ) {

				mUnits.Add( unit );
				mUnitCount[ (int)unit.GetUnitType() ]++;

			}

			mTempUnits.Clear();
			mTempUnitCount = new int[ 3 ];

		}

		if( !isLand && !isMine && mUnits.Count > 0 ) {

			GameObject units;

			if( transform.FindChild( "Units" ) != null ) {

				units = transform.FindChild( "Units" ).gameObject;

			} else {

				units = Instantiate( Resources.Load< Object >( "Prefabs/Units" ) ) as GameObject;
				units.name = "Units";
				units.transform.parent = transform;
				units.transform.localPosition = Vector3.zero;

			}

			if( mUnits.Count > 11 ) {

				units.GetComponent<MeshFilter>().mesh = Resources.Load<Mesh>( "Models/Unit9" );

			} else if( mUnits.Count > 5 ) {

				units.GetComponent<MeshFilter>().mesh = Resources.Load<Mesh>( "Models/Unit6" );

			} else {

				units.GetComponent<MeshFilter>().mesh = Resources.Load<Mesh>( "Models/Unit3" );

			}

		} else {

			if( transform.FindChild( "Units" ) != null ) Destroy( transform.FindChild( "Units" ).gameObject );

		}

		if( Owner == TileOwner.Player && mTurnManager.GetTurn() == Turn.Player ) {

			GameObject.Find( "TownFriendly" ).GetComponent<Land>().AddGold( GoldValue );

		}

	}

	public void SetPosition( Vector2 position ) {

		mPosition = position;

		if( position.x % 2 == 0 ) {

			transform.localPosition = new Vector3( position.x * -1.5f, 0, position.y * 2 );

		} else {

			transform.localPosition = new Vector3( position.x * -1.5f, 0, 1 + position.y * 2 );

		}

	}

	public Vector2 GetPosition() {

		return mPosition;

	}

	public void SetOwner( TileOwner owner ) {

		Owner = owner;

	}

	public TileOwner GetOwner() {

		return Owner;

	}

	public void AddFootman( int owner ) {

        if( isLand ) {

            if( mUnits.Count == 32 ) return;

        } else {

            if( mUnits.Count == 16 ) return;

        }

		mTempUnits.Add( new Unit( UnitType.Footman, owner ) );
		mTempUnitCount[ ( int )UnitType.Footman ]++;

	}

	public void AddArcher( int owner ) {

        if( isLand ) {

            if( mUnits.Count == 100 ) return;

        } else {

            if( mUnits.Count == 16 ) return;

        }

		mTempUnits.Add( new Unit( UnitType.Archer, owner ) );
		mTempUnitCount[ ( int )UnitType.Archer ]++;

	}

	public void AddLancer( int owner ) {

        if( isLand ) {

            if( mUnits.Count == 100 ) return;

        } else {

            if( mUnits.Count == 16 ) return;

        }

		mTempUnits.Add( new Unit( UnitType.Lancer, owner ) );
		mTempUnitCount[ ( int )UnitType.Lancer ]++;

	}

	public void MoveFootman() {

		StateManager.SetState( State.Move );
		StateManager.AddParameter( "move_footman" );
		
	}

	public void MoveArcher() {



	}

	public void MoveLancer() {



	}

    public void RemoveFootman() {

        foreach( Unit unit in mUnits ) {

            if( unit.GetUnitType() == UnitType.Footman ) {

                mUnits.Remove( unit );
                mUnitCount[ ( int )UnitType.Footman ]--;
                break;

            }

        }

    }

	public int[] GetUnitCount() { return mUnitCount; }

}
