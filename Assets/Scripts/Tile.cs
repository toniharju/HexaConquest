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
	private bool isAILand = false;

	private List<Unit> mUnits = new List<Unit>();
	private List<Unit> mTempUnits = new List<Unit>();
    private int[] mUnitCount = new int[ 3 ];
	private int[] mTempUnitCount = new int[ 3 ];

	private GameObject mUnit;

	private Color[] mColors = {	new Color( 0.8f, 0.8f, 0.8f ),	//Normal
								new Color( 0, 1.0f, 0 ),		//Player
								new Color( 1.0f, 0, 0 ),		//AI
								new Color( 0, 0.7f, 0.8f ) };	//Selected

	private GameObject[] mNeighbour = new GameObject[6];

	private int captureProgress = 0;
	private int mId;
	private Vector2 mPosition = Vector2.zero;
	private TileManager mTileManager;
	private TurnManager mTurnManager;

	private bool mIsDisabled = false;
	private bool mHasRunOnce = false;

    public int GoldValue = 1;
    public TileOwner Owner;

	void Start() {

		if( GameObject.Find( "Managers" ) != null ) {

			mTileManager = GameObject.Find( "Managers" ).GetComponent<TileManager>();
			mTurnManager = GameObject.Find( "Managers" ).GetComponent<TurnManager>();

		}

		if( GetComponentInChildren<Land>() != null )
			isLand = true;
		else if( GetComponentInChildren<AILand>() != null )
			isAILand = true;
		else if( transform.FindChild( "Mine" ) != null )
			isMine = true;

		if( isLand )
			captureProgress = -200;
		else if( isAILand )
			captureProgress = 200;

	}

	void Update() {

		if( !mHasRunOnce ) {

			int i = 0;
			for( int dy = -1; dy < 2; dy++ ) {

				for( int dx = -1; dx < 2; dx++ ) {

					int x = ( int )mPosition.x + dx;
					int y = ( int )mPosition.y + dy;

					if( dx != 0 ) {
						if( x % 2 == 1 ) {
							if( dy == 1 ) continue;
						} else {
							if( dy == -1 ) continue;
						}
					}

					if( x > -1 && x < mTileManager.GetMap().Size.x && y > -1 && y < mTileManager.GetMap().Size.y ) {

						if( mTileManager.GetMap().GetMapData()[ x, y ] == gameObject ) continue;

						if( mTileManager.GetMap().GetMapData()[ x, y ] == null || !mTileManager.GetMap().GetMapData()[ x, y ].activeSelf ) {

							mNeighbour[ i ] = null;

						} else {
							
							mNeighbour[ i ] = mTileManager.GetMap().GetMapData()[ x, y ];

						}

					}

					i++;

				}

			}

			mHasRunOnce = true;

		}

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

	public void UpdateUnits( bool capture = true ) {

		if( mUnits.Count > 0 ) {

			if( !isLand && !isMine && !isAILand ) {

				GameObject units;

				if( transform.FindChild( "Units" ) != null ) {

					units = transform.FindChild( "Units" ).gameObject;

				} else {

					units = Instantiate( Resources.Load<Object>( "Prefabs/Units" ) ) as GameObject;
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

				Material[] changed = new Material[ 4 ];
				changed = units.renderer.materials;

				if( mUnits[ 0 ].GetUnitOwner() == 1 )
					changed[ 1 ] = Resources.Load<Material>( "Materials/ShieldFrontGreen" );
				else
					changed[ 1 ] = Resources.Load<Material>( "Materials/ShieldFrontRed" );

				units.renderer.materials = changed;

			} else {

				if( transform.FindChild( "Units" ) != null ) Destroy( transform.FindChild( "Units" ).gameObject );

			}

			if( capture ) {

				foreach( Unit unit in mUnits ) {

					if( mTurnManager.GetTurn() == Turn.Player && unit.GetUnitOwner() == 1 ) {

						captureProgress -= Mechanics.GetCaptureRate( unit.GetUnitType() );

					} else if( mTurnManager.GetTurn() == Turn.AI && unit.GetUnitOwner() == 2 ) {

						captureProgress += Mechanics.GetCaptureRate( unit.GetUnitType() );

					}

				}

			}

		} else {

			if( transform.FindChild( "Units" ) != null ) Destroy( transform.FindChild( "Units" ).gameObject );

		}

	}

	public void OnTurn() {

		mIsDisabled = false;

		if( mTempUnits.Count > 0 ) {

			bool clear = false;
			
			foreach( Unit unit in mTempUnits ) {
				
				if( ( mTurnManager.GetTurn() == Turn.Player && unit.GetUnitOwner() == 1 ) || ( mTurnManager.GetTurn() == Turn.AI && unit.GetUnitOwner() == 2 ) ) {
					
					clear = true;

					mUnits.Add( unit );
					mUnitCount[ ( int )unit.GetUnitType() ]++;

				} else {

					break;

				}

			}

			if( clear ) {

				mTempUnits.Clear();
				mTempUnitCount = new int[ 3 ];

			}

		}

		if( ( Owner == TileOwner.Player ) ||
			( mUnits.Count > 0 && mUnits[ 0 ].GetUnitOwner() == 1 ) ) {

			GameObject canvas = GameObject.Find( "WorldCanvas" );

			gameObject.layer = 8;
			for( int i = 0; i < 6; i++ ) {

				if( mNeighbour[ i ] == null ) continue;

				mNeighbour[ i ].layer = 8;

				int id = mNeighbour[ i ].GetComponent<Tile>().GetId();
				int childCount = mNeighbour[ i ].transform.childCount;
				string childName = "";
				TileOwner owner = mNeighbour[ i ].GetComponent<Tile>().GetOwner();

				if( childCount > 0 ) childName = mNeighbour[ i ].transform.GetChild( 0 ).name;

				if( mNeighbour[ i ] != null && owner != TileOwner.Player ) {

					if( childCount == 0 || childCount > 0 && childName != "Tree" )
						canvas.transform.GetChild( id ).gameObject.SetActive( true );

				}

			}

		}

		UpdateUnits();

		if( captureProgress <= -200 ) {

			captureProgress = -200;
			Owner = TileOwner.Player;

		} else if( captureProgress >= 200 ) {

			captureProgress = 200;
			Owner = TileOwner.AI;

		}

		if( Owner == TileOwner.Player && mTurnManager.GetTurn() == Turn.Player ) {

			GameObject.Find( "TownFriendly" ).GetComponent<Land>().AddGold( GoldValue );

		} else if( Owner == TileOwner.AI && mTurnManager.GetTurn() == Turn.AI ) {

			GameObject.Find( "TownEnemy" ).GetComponent<AILand>().AddGold( GoldValue );

		}

		if( ( Owner == TileOwner.Player ) ||
			( mUnits.Count > 0 && mUnits[ 0 ].GetUnitOwner() == 1 ) ) {

			GameObject canvas = GameObject.Find( "WorldCanvas" );

			gameObject.layer = 8;
			for( int i = 0; i < 6; i++ ) {

				if( mNeighbour[ i ] == null ) continue;

				mNeighbour[ i ].layer = 8;

				int id = mNeighbour[ i ].GetComponent<Tile>().GetId();
				int childCount = mNeighbour[ i ].transform.childCount;
				string childName = "";
				TileOwner owner = mNeighbour[ i ].GetComponent<Tile>().GetOwner();

				if( childCount > 0 ) childName = mNeighbour[ i ].transform.GetChild( 0 ).name;

				if( mNeighbour[ i ] != null && owner != TileOwner.Player ) {

					if( childCount == 0 || childCount > 0 && childName != "Tree" )
						canvas.transform.GetChild( id ).gameObject.SetActive( true );

				}

			}

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

	public void SetId( int id ) { mId = id; }
	public int GetId() { return mId; }

	public void SetOwner( TileOwner owner ) {

		Owner = owner;

	}

	public TileOwner GetOwner() {

		return Owner;

	}

	public void AddFootman( int owner, int task = 0 ) {

        if( isLand ) {

            if( mUnits.Count == 32 ) return;

        } else {

            if( mUnits.Count == 16 ) return;

        }
		
		mTempUnits.Add( new Unit( UnitType.Footman, owner, task ) );
		mTempUnitCount[ ( int )UnitType.Footman ]++;

	}

	public void AddArcher( int owner, int task = 0 ) {

        if( isLand ) {

            if( mUnits.Count == 32 ) return;

        } else {

            if( mUnits.Count == 16 ) return;

        }

		mTempUnits.Add( new Unit( UnitType.Archer, owner, task ) );
		mTempUnitCount[ ( int )UnitType.Archer ]++;

	}

	public void AddLancer( int owner, int task = 0 ) {

        if( isLand ) {

            if( mUnits.Count == 32 ) return;

        } else {

            if( mUnits.Count == 16 ) return;

        }

		mTempUnits.Add( new Unit( UnitType.Lancer, owner, task ) );
		mTempUnitCount[ ( int )UnitType.Lancer ]++;

	}

	public void MoveFootman() {

		StateManager.SetState( State.Move );
		StateManager.AddParameter( "move_footman" );
		
	}

	public void MoveArcher() {

		StateManager.SetState( State.Move );
		StateManager.AddParameter( "move_archer" );

	}

	public void MoveLancer() {

		StateManager.SetState( State.Move );
		StateManager.AddParameter( "move_lancer" );

	}

    public void RemoveFootman( bool countOnly = false ) {

		if( countOnly ) {

			mUnitCount[ ( int )UnitType.Footman ]--;

		} else {

			foreach( Unit unit in mUnits ) {

				if( unit.GetUnitType() == UnitType.Footman ) {

					mUnits.Remove( unit );
					mUnitCount[ ( int )UnitType.Footman ]--;
					break;

				}

			}

		}

    }

	public void RemoveArcher( bool countOnly = false ) {

		if( countOnly ) {

			mUnitCount[ ( int )UnitType.Archer ]--;

		} else {

			foreach( Unit unit in mUnits ) {

				if( unit.GetUnitType() == UnitType.Archer ) {

					mUnits.Remove( unit );
					mUnitCount[ ( int )UnitType.Archer ]--;
					break;

				}

			}

		}

	}

	public void RemoveLancer( bool countOnly = false ) {

		if( countOnly ) {

			mUnitCount[ ( int )UnitType.Lancer ]--;

		} else {

			foreach( Unit unit in mUnits ) {

				if( unit.GetUnitType() == UnitType.Lancer ) {

					mUnits.Remove( unit );
					mUnitCount[ ( int )UnitType.Lancer ]--;
					break;

				}

			}

		}

	}

	public void AddToCapture( int points ) { captureProgress += points; }

	public bool IsLand() { return isLand; }
	public bool IsAILand() { return isAILand; }
	public bool IsMine() { return isMine; }

	public List<Unit> GetUnits() { return mUnits; }
	public List<Unit> GetTempUnits() { return mTempUnits; }
	public int[] GetUnitCount() { return mUnitCount; }

	public bool IsDisabled() { return mIsDisabled; }
	public void Disable() { mIsDisabled = true; }

}
