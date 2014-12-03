using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class TileManager : MonoBehaviour {

	private Map mMap;

	private GameObject mCastlePanel;
	private GameObject mTilePanel;

    private GameObject[] mAddButton = new GameObject[ 3 ];
    private GameObject[] mDeployButton = new GameObject[ 3 ];
	private GameObject[] mMoveButton = new GameObject[ 3 ];
	private GameObject[] mQueueImage = new GameObject[ 3 ];

	private GameObject mHoverTile;
	private GameObject mSelectedTile;

	public GameObject Arrow;

	public Texture2D MainCursor;
	public Texture2D CrossCursor;

	// Use this for initialization
	void Start () {

		Mechanics.Initialize();

		mCastlePanel = GameObject.Find( "CastlePanel" );
		mTilePanel = GameObject.Find( "TilePanel" );

		mMoveButton[ 0 ] = GameObject.Find( "MoveFootman" );
		mMoveButton[ 1 ] = GameObject.Find( "MoveArcher" );
		mMoveButton[ 2 ] = GameObject.Find( "MoveLancer" );

        mAddButton[ 0 ] = GameObject.Find( "AddFootman" );
        mAddButton[ 1 ] = GameObject.Find( "AddArcher" );
        mAddButton[ 2 ] = GameObject.Find( "AddLancer" );

        mDeployButton[ 0 ] = GameObject.Find( "DeployFootman" );
        mDeployButton[ 1 ] = GameObject.Find( "DeployArcher" );
        mDeployButton[ 2 ] = GameObject.Find( "DeployLancer" );

		mQueueImage[ 0 ] = GameObject.Find( "QueueFootman" );
		mQueueImage[ 1 ] = GameObject.Find( "QueueArcher" );
		mQueueImage[ 2 ] = GameObject.Find( "QueueLancer" );

		mCastlePanel.SetActive( false );
		mTilePanel.SetActive( false );

		mMap = GameObject.Find( "MapData" ).GetComponent<Map>();

	}
	
	// Update is called once per frame
	void Update () {

		if( Input.GetKeyDown( KeyCode.Escape ) ) {

			Cursor.SetCursor( MainCursor, Vector2.zero, CursorMode.Auto );

			mSelectedTile = null;
			mCastlePanel.SetActive( false );
			mTilePanel.SetActive( false );
			StateManager.Clear();

			if( GameObject.Find( "Arrow" ) != null ) {

				Destroy( GameObject.Find( "Arrow" ) );

			}

		}

        if( !EventSystem.current.IsPointerOverGameObject() ) {

			RaycastHit hit;
            if( Physics.Raycast( Camera.main.ScreenPointToRay( Input.mousePosition ), out hit ) ) {

                if( hit.collider.gameObject.tag != "Impassable" ) {

					mHoverTile = hit.collider.gameObject;

                    if( Input.GetMouseButtonUp( 0 ) ) {

						Cursor.SetCursor( MainCursor, Vector2.zero, CursorMode.Auto );

						if( mSelectedTile != null && GameObject.Find( "Arrow" ) != null ) {

							Destroy( GameObject.Find( "Arrow" ) );

						}

                        mSelectedTile = hit.collider.gameObject;
						StateManager.Clear();

						if( mSelectedTile.transform.FindChild( "TownFriendly" ) != null ) {

							GameObject land = mSelectedTile.transform.FindChild( "TownFriendly" ).gameObject;

							mCastlePanel.SetActive( true );
							mTilePanel.SetActive( false );

							mAddButton[ 0 ].GetComponent<Button>().onClick.RemoveAllListeners();
							mAddButton[ 1 ].GetComponent<Button>().onClick.RemoveAllListeners();
							mAddButton[ 2 ].GetComponent<Button>().onClick.RemoveAllListeners();
							mAddButton[ 0 ].GetComponent<Button>().onClick.AddListener( () => mSelectedTile.GetComponent<Tile>().AddFootman( 1 ) );
							mAddButton[ 0 ].GetComponent<Button>().onClick.AddListener( () => land.GetComponent<Land>().AddFootman() );
                            mAddButton[ 1 ].GetComponent<Button>().onClick.AddListener( () => mSelectedTile.GetComponent<Tile>().AddArcher( 1 ) );
							mAddButton[ 1 ].GetComponent<Button>().onClick.AddListener( () => land.GetComponent<Land>().AddArcher() );
                            mAddButton[ 2 ].GetComponent<Button>().onClick.AddListener( () => mSelectedTile.GetComponent<Tile>().AddLancer( 1 ) );
							mAddButton[ 2 ].GetComponent<Button>().onClick.AddListener( () => land.GetComponent<Land>().AddLancer() );

							mDeployButton[ 0 ].GetComponent<Button>().onClick.RemoveAllListeners();
							mDeployButton[ 1 ].GetComponent<Button>().onClick.RemoveAllListeners();
							mDeployButton[ 2 ].GetComponent<Button>().onClick.RemoveAllListeners();
                            mDeployButton[ 0 ].GetComponent<Button>().onClick.AddListener( () => mSelectedTile.GetComponent<Tile>().MoveFootman() );
                            mDeployButton[ 1 ].GetComponent<Button>().onClick.AddListener( () => mSelectedTile.GetComponent<Tile>().MoveArcher() );
                            mDeployButton[ 2 ].GetComponent<Button>().onClick.AddListener( () => mSelectedTile.GetComponent<Tile>().MoveLancer() );

						} else {

							mCastlePanel.SetActive( false );
							mTilePanel.SetActive( true );

							mMoveButton[ 0 ].GetComponent<Button>().onClick.RemoveAllListeners();
							mMoveButton[ 1 ].GetComponent<Button>().onClick.RemoveAllListeners();
							mMoveButton[ 2 ].GetComponent<Button>().onClick.RemoveAllListeners();
							mMoveButton[ 0 ].GetComponent<Button>().onClick.AddListener( () => mSelectedTile.GetComponent<Tile>().MoveFootman() );
							mMoveButton[ 1 ].GetComponent<Button>().onClick.AddListener( () => mSelectedTile.GetComponent<Tile>().MoveArcher() );
							mMoveButton[ 2 ].GetComponent<Button>().onClick.AddListener( () => mSelectedTile.GetComponent<Tile>().MoveLancer() );

						}

                    }

                }

            } else {

				mHoverTile = null;

				Cursor.SetCursor( MainCursor, Vector2.zero, CursorMode.Auto );
				if( GameObject.Find( "Arrow" ) != null ) Destroy( GameObject.Find( "Arrow" ) );

				if( Input.GetMouseButtonUp( 0 ) ) {

					mSelectedTile = null;
					mCastlePanel.SetActive( false );
					mTilePanel.SetActive( false );
					StateManager.Clear();

				}

            }

			if( StateManager.GetState() == State.Move && mSelectedTile != null && mHoverTile != null ) {

				Vector3 delta = mSelectedTile.transform.position - mHoverTile.transform.position;
				int distance = Mathf.CeilToInt( delta.magnitude ) - 1;

				if( distance == 1 ) {
					
					if( mHoverTile.GetComponent<Tile>().IsLand() && mHoverTile.GetComponent<Tile>().GetUnits().Count + StateManager.GetParameters().Count > 32 ) {

						Cursor.SetCursor( CrossCursor, Vector2.zero, CursorMode.Auto );

						GameObject temp = GameObject.Find( "Arrow" );
						if( temp != null ) Destroy( temp );

					} else if( !mHoverTile.GetComponent<Tile>().IsLand() && mHoverTile.GetComponent<Tile>().GetUnits().Count + StateManager.GetParameters().Count > 16 ) {

						Cursor.SetCursor( CrossCursor, Vector2.zero, CursorMode.Auto );

						GameObject temp = GameObject.Find( "Arrow" );
						if( temp != null ) Destroy( temp );

					} else {

						Cursor.SetCursor( MainCursor, Vector2.zero, CursorMode.Auto );

						GameObject arrowTempParent;

						if( GameObject.Find( "Arrow" ) == null ) {

							arrowTempParent = new GameObject( "Arrow" );
							arrowTempParent.transform.position = mSelectedTile.transform.position;
							GameObject arrowTemp = Instantiate( Arrow ) as GameObject;
							arrowTemp.transform.parent = arrowTempParent.transform;
							arrowTemp.transform.localPosition = new Vector3( 0, 0.75f, 1.15f );

						} else {

							arrowTempParent = GameObject.Find( "Arrow" );

						}

						float angle = -Mathf.Atan2( delta.z, delta.x ) * Mathf.Rad2Deg + 270;
						arrowTempParent.transform.rotation = Quaternion.Euler( arrowTempParent.transform.rotation.x, angle, arrowTempParent.transform.rotation.z );

						if( mHoverTile.GetComponent<Tile>().GetUnits().Count > 0 && mHoverTile.GetComponent<Tile>().GetUnits()[ 0 ].GetUnitOwner() == 2 ) {

							Cursor.SetCursor( MainCursor, Vector2.zero, CursorMode.Auto );

							//Do attack here
							if( Input.GetMouseButtonUp( 1 ) ) {

								int enemyCount = mHoverTile.GetComponent<Tile>().GetUnits().Count;
								int friendlyCount = mSelectedTile.GetComponent<Tile>().GetUnits().Count;

								UnitType[] enemyType = new UnitType[ enemyCount ];
								int[] enemyHealth = new int[ enemyCount ];

								for( int i = 0; i < enemyCount; i++ ) {

									enemyType[ i ] = mHoverTile.GetComponent<Tile>().GetUnits()[ i ].GetUnitType();
									enemyHealth[ i ] = Mechanics.GetHealth( mHoverTile.GetComponent<Tile>().GetUnits()[ i ].GetUnitType() );

								}

								for( int i = 0; i < friendlyCount; i++ ) {

									int toAttack = Random.Range( 0, enemyCount );
									int attackPower = Mechanics.GetAttackPower( mSelectedTile.GetComponent<Tile>().GetUnits()[ i ].GetUnitType() );
									
									while( enemyHealth[ toAttack ] <= 0 ) toAttack = Random.Range( 0, enemyCount );

									enemyHealth[ toAttack ] -= attackPower;

								}

								for( int i = enemyCount - 1; i >= 0; i-- ) {

									if( enemyHealth[ i ] <= 0 ) {

										if( enemyType[ i ] == UnitType.Footman )
											mHoverTile.GetComponent<Tile>().RemoveFootman( true );
										else if( enemyType[ i ] == UnitType.Archer )
											mHoverTile.GetComponent<Tile>().RemoveArcher( true );
										else if( enemyType[ i ] == UnitType.Lancer )
											mHoverTile.GetComponent<Tile>().RemoveLancer( true );
										
										mHoverTile.GetComponent<Tile>().GetUnits().RemoveAt( i );

									}

								}

								mSelectedTile.GetComponent<Tile>().Disable();

								Destroy( arrowTempParent );
								StateManager.Clear();

							}

						} else {

							Cursor.SetCursor( MainCursor, Vector2.zero, CursorMode.Auto );

							if( Input.GetMouseButtonUp( 1 ) ) {

								foreach( string move in StateManager.GetParameters() ) {

									if( move == "move_footman" ) {

										mSelectedTile.GetComponent<Tile>().RemoveFootman();
										mHoverTile.GetComponent<Tile>().AddFootman( 1 );

									} else if( move == "move_archer" ) {

										mSelectedTile.GetComponent<Tile>().RemoveArcher();
										mHoverTile.GetComponent<Tile>().AddArcher( 1 );

									} else if( move == "move_lancer" ) {

										mSelectedTile.GetComponent<Tile>().RemoveLancer();
										mHoverTile.GetComponent<Tile>().AddLancer( 1 );

									}

								}

								Destroy( arrowTempParent );
								StateManager.Clear();

							}

						}

					}

				} else {

					Cursor.SetCursor( CrossCursor, Vector2.zero, CursorMode.Auto );

					GameObject temp = GameObject.Find( "Arrow" );
					if( temp != null ) Destroy( temp );

				}

			}

		} else {

			Cursor.SetCursor( MainCursor, Vector2.zero, CursorMode.Auto );

		}
		
        if( mSelectedTile != null ) {

			int footmanSelectCount = 0;
			int archerSelectCount = 0;
			int lancerSelectCount = 0;

			int footmanCount = mSelectedTile.GetComponent<Tile>().GetUnitCount()[ ( int )UnitType.Footman ];
			int archerCount = mSelectedTile.GetComponent<Tile>().GetUnitCount()[ ( int )UnitType.Archer ];
			int lancerCount = mSelectedTile.GetComponent<Tile>().GetUnitCount()[ ( int )UnitType.Lancer ];

			int owner = 1;
			bool isDisabled = mSelectedTile.GetComponent<Tile>().IsDisabled();

			if( footmanCount > 0 || archerCount > 0 || lancerCount > 0 ) owner = mSelectedTile.GetComponent<Tile>().GetUnits()[ 0 ].GetUnitOwner();

			if( StateManager.GetState() == State.Move && StateManager.GetParameters().Count > 0 ) {

				footmanSelectCount = StateManager.GetParameters().FindAll( delegate( string move ) { return move == "move_footman"; } ).Count;
				archerSelectCount = StateManager.GetParameters().FindAll( delegate( string move ) { return move == "move_archer"; } ).Count;
				lancerSelectCount = StateManager.GetParameters().FindAll( delegate( string move ) { return move == "move_lancer"; } ).Count;

			}

            if( mCastlePanel.activeSelf ) {

				if( isDisabled || owner == 2 || footmanCount == 0 || footmanSelectCount == footmanCount )
					mDeployButton[ 0 ].GetComponent<Button>().interactable = false;
				else
					mDeployButton[ 0 ].GetComponent<Button>().interactable = true;

				if( isDisabled || owner == 2 || archerCount == 0 || archerSelectCount == archerCount )
					mDeployButton[ 1 ].GetComponent<Button>().interactable = false;
				else
					mDeployButton[ 1 ].GetComponent<Button>().interactable = true;

				if( isDisabled || owner == 2 || lancerCount == 0 || lancerSelectCount == lancerCount )
					mDeployButton[ 2 ].GetComponent<Button>().interactable = false;
				else
					mDeployButton[ 2 ].GetComponent<Button>().interactable = true;

				if( owner == 1 && !isDisabled ) {

					mDeployButton[ 0 ].transform.FindChild( "UnitCount" ).GetComponent<Text>().text = footmanSelectCount + "/" + footmanCount;
					mDeployButton[ 1 ].transform.FindChild( "UnitCount" ).GetComponent<Text>().text = archerSelectCount + "/" + archerCount;
					mDeployButton[ 2 ].transform.FindChild( "UnitCount" ).GetComponent<Text>().text = lancerSelectCount + "/" + lancerCount;

				} else {

					mDeployButton[ 0 ].transform.FindChild( "UnitCount" ).GetComponent<Text>().text = "0/0";
					mDeployButton[ 1 ].transform.FindChild( "UnitCount" ).GetComponent<Text>().text = "0/0";
					mDeployButton[ 2 ].transform.FindChild( "UnitCount" ).GetComponent<Text>().text = "0/0";

				}

            } else if( mTilePanel.activeSelf ) {

				if( isDisabled || owner == 2 || footmanCount == 0 || footmanSelectCount == footmanCount )
					mMoveButton[ 0 ].GetComponent<Button>().interactable = false;
				else
					mMoveButton[ 0 ].GetComponent<Button>().interactable = true;

				if( isDisabled || owner == 2 || archerCount == 0 || archerSelectCount == archerCount )
					mMoveButton[ 1 ].GetComponent<Button>().interactable = false;
				else
					mMoveButton[ 1 ].GetComponent<Button>().interactable = true;

				if( isDisabled || owner == 2 || lancerCount == 0 || lancerSelectCount == lancerCount )
					mMoveButton[ 2 ].GetComponent<Button>().interactable = false;
				else
					mMoveButton[ 2 ].GetComponent<Button>().interactable = true;

				if( owner == 1 && !isDisabled ) {

					mMoveButton[ 0 ].transform.FindChild( "UnitCount" ).GetComponent<Text>().text = footmanSelectCount + "/" + footmanCount;
					mMoveButton[ 1 ].transform.FindChild( "UnitCount" ).GetComponent<Text>().text = archerSelectCount + "/" + archerCount;
					mMoveButton[ 2 ].transform.FindChild( "UnitCount" ).GetComponent<Text>().text = lancerSelectCount + "/" + lancerCount;

				} else {

					mMoveButton[ 0 ].transform.FindChild( "UnitCount" ).GetComponent<Text>().text = "0/0";
					mMoveButton[ 1 ].transform.FindChild( "UnitCount" ).GetComponent<Text>().text = "0/0";
					mMoveButton[ 2 ].transform.FindChild( "UnitCount" ).GetComponent<Text>().text = "0/0";

				}

            }

        }

	}

	public GameObject GetHoverTile() {

		return mHoverTile;

	}

	public GameObject GetSelectedTile() {

		return mSelectedTile;

	}

	public Map GetMap() {

		return mMap;

	}

	public GameObject[] GetAddButtons() { return mAddButton; }
	public GameObject[] GetDeployButtons() { return mDeployButton; }
	public GameObject[] GetMoveButtons() { return mMoveButton; }
	public GameObject[] GetQueueImages() { return mQueueImage; }

}
