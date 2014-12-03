using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIManager : MonoBehaviour {

	private bool mIsFirstEncounter = true;
	private bool mIsFirstTurn = true;

	private GameObject mTown;
	private AILand mAILand;

	private int mTiles = 1;
	private int mMines = 0;
	private int mScouts = 1;
	private int[] mQueue = new int[ 3 ];
	private int[] mGarrison = new int[ 3 ];

	private int mPreviousTurn = 0;
	private int mCurrentTurn = 0;

	private int mNextTask = 0;

	private Map mMap;

	public GameObject[] WaypointPath = new GameObject[ 3 ];

	// Use this for initialization
	void Start () {

		if( GameObject.Find( "MapData" ) != null ) mMap = GameObject.Find( "MapData" ).GetComponent<Map>();

		mTown = GameObject.Find( "TownEnemy" );
		mAILand = mTown.GetComponent<AILand>();

	}
	
	// Update is called once per frame
	public void OnTurn () {

		mPreviousTurn = mCurrentTurn;
		mCurrentTurn++;

		mGarrison[ 0 ] += mQueue[ 0 ];
		mGarrison[ 1 ] += mQueue[ 1 ];
		mGarrison[ 2 ] += mQueue[ 2 ];
		mQueue[ 0 ] = 0;
		mQueue[ 1 ] = 0;
		mQueue[ 2 ] = 0;
		
		//Check if AI has gold and act accordingly
		if( mAILand.GetGold() >= Mechanics.GetPrice( UnitType.Footman ) && mTown.transform.parent.GetComponent< Tile >().GetUnits().Count < 32 ) {
			
			int buy = ( int )( mAILand.GetGold() / Mechanics.GetPrice( UnitType.Footman ) );
			mAILand.RemoveGold( buy * Mechanics.GetPrice( UnitType.Footman ) );

			int x = (int)mTown.transform.parent.GetComponent<Tile>().GetPosition().x;
			int y = (int)mTown.transform.parent.GetComponent<Tile>().GetPosition().y;

			mQueue[ ( int )UnitType.Footman ] = buy;

			if( mCurrentTurn == 1 ) {

				mNextTask = 1;

			} else {

				if( Random.Range( 1, 4 ) == 1 ) {

					mNextTask = 3;

				} else {

					mNextTask = ( int )Random.Range( 1, 3 );
					if( mNextTask == 1 ) mScouts++;
					if( mScouts > 1 ) {

						while( mNextTask == 1 ) mNextTask = Random.Range( 1, 3 );

					}

				}

			}
			
			for( int i = 0; i < buy; i++ ) mTown.transform.parent.GetComponent<Tile>().AddFootman( 2, mNextTask );
			
		}
		
		if( mGarrison[ 0 ] > 0 || mGarrison[ 1 ] > 0 || mGarrison[ 2 ] > 0 ) {

			int x1 = ( int )mTown.transform.parent.GetComponent<Tile>().GetPosition().x;
			int y1 = ( int )mTown.transform.parent.GetComponent<Tile>().GetPosition().y;

			int count = mTown.transform.parent.GetComponent<Tile>().GetUnits().Count;

			if( mCurrentTurn == 2 ) {

				Waypoint wp = mTown.transform.parent.GetChild( Random.Range( 1, 4 ) ).GetComponent<Waypoint>();
				wp.MoveToNext( 1, 0, 0, mNextTask );

				int x2 = ( int )wp.GetNext().GetComponent<Tile>().GetPosition().x;
				int y2 = ( int )wp.GetNext().GetComponent<Tile>().GetPosition().y;

				mGarrison[ 0 ]--;

			} else {

				for( int i = count - 1; i >= 0; i-- ) {

					if( mTown.transform.parent.GetComponent<Tile>().GetUnits()[ i ].GetUnitTask() == 3 ) {

						continue;

					} else if( mTown.transform.parent.GetComponent<Tile>().GetUnits()[ i ].GetUnitTask() == 2 ) {

						Waypoint wp = mTown.transform.parent.GetChild( Random.Range( 1, 4 ) ).GetComponent<Waypoint>();
						wp.MoveToNext( 1, 0, 0, mNextTask );

						int x2 = ( int )wp.GetNext().GetComponent<Tile>().GetPosition().x;
						int y2 = ( int )wp.GetNext().GetComponent<Tile>().GetPosition().y;

						mGarrison[ 0 ]--;

					} else if( mTown.transform.parent.GetComponent<Tile>().GetUnits()[ i ].GetUnitTask() == 1 ) {

						Waypoint wp = mTown.transform.parent.GetChild( Random.Range( 1, 4 ) ).GetComponent<Waypoint>();
						wp.MoveToNext( 1, 0, 0, mNextTask );

						int x2 = ( int )wp.GetNext().GetComponent<Tile>().GetPosition().x;
						int y2 = ( int )wp.GetNext().GetComponent<Tile>().GetPosition().y;

						mGarrison[ 0 ]--;

					}

				}

			}

		}

		if( mCurrentTurn > 2 ) {

			GameObject mapData = GameObject.Find( "MapData" );
			int count = mapData.transform.childCount;
			for( int i = 0; i < count; i++ ) {

				GameObject current = mapData.transform.GetChild( i ).gameObject;
				if( !current.activeSelf ) continue;

				int x1 = ( int )current.GetComponent<Tile>().GetPosition().x;
				int y1 = ( int )current.GetComponent<Tile>().GetPosition().y;

				for( int j = current.GetComponent<Tile>().GetUnits().Count - 1; j >= 0; j-- ) {

					if( current.GetComponent<Tile>().GetUnits()[ j ].GetUnitOwner() == 2 ) {

						//Defend
						if( current.GetComponent<Tile>().GetUnits()[ j ].GetUnitTask() == 3 ) {
							continue;
						//Attack
						} else if( current.GetComponent<Tile>().GetUnits()[ j ].GetUnitTask() == 2 ) {

							Waypoint wp = current.transform.FindChild( "Waypoint" ).GetComponent<Waypoint>();

							GameObject next = wp.GetNext();

							//Attack
							bool attacked = false;
							for( int dy = -1; dy < 2; dy++ ) {

								for( int dx = -1; dx < 2; dx++ ) {

									int x = ( int )x1 + dx;
									int y = ( int )y1 + dy;

									if( dx != 0 ) {
										if( x % 2 == 1 ) {
											if( dy == 1 ) continue;
										} else {
											if( dy == -1 ) continue;
										}
									}

									if( x > -1 && x < mMap.Size.x && y > -1 && y < mMap.Size.y ) {

										GameObject neighbour = mMap.GetMapData()[ x, y ];

										if( neighbour.GetComponent<Tile>().GetUnits().Count > 0 && neighbour.GetComponent<Tile>().GetUnits()[ 0 ].GetUnitOwner() == 1 ) {

											attacked = true;

											int enemyCount = neighbour.GetComponent<Tile>().GetUnits().Count;
											int friendlyCount = current.GetComponent<Tile>().GetUnits().Count;

											UnitType[] enemyType = new UnitType[ enemyCount ];
											int[] enemyHealth = new int[ enemyCount ];

											for( int k = 0; k < enemyCount; k++ ) {

												enemyType[ k ] = neighbour.GetComponent<Tile>().GetUnits()[ k ].GetUnitType();
												enemyHealth[ k ] = Mechanics.GetHealth( neighbour.GetComponent<Tile>().GetUnits()[ k ].GetUnitType() );

											}

											for( int k = 0; k < friendlyCount; k++ ) {

												bool allDead = false;

												for( int l = enemyCount - 1; l >= 0; l-- ) {

													allDead = true;

													if( enemyHealth[ l ] > 0 ) {

														allDead = false;
														break;

													}

												}

												if( allDead ) break;

												int toAttack = Random.Range( 0, enemyCount );
												int attackPower = Mechanics.GetAttackPower( current.GetComponent<Tile>().GetUnits()[ k ].GetUnitType() );

												while( enemyHealth[ toAttack ] <= 0 ) toAttack = Random.Range( 0, enemyCount );

												enemyHealth[ toAttack ] -= attackPower;

											}

											for( int k = enemyCount - 1; k >= 0; k-- ) {

												if( enemyHealth[ k ] <= 0 ) {

													if( enemyType[ k ] == UnitType.Footman )
														neighbour.GetComponent<Tile>().RemoveFootman( true );
													else if( enemyType[ k ] == UnitType.Archer )
														neighbour.GetComponent<Tile>().RemoveArcher( true );
													else if( enemyType[ k ] == UnitType.Lancer )
														neighbour.GetComponent<Tile>().RemoveLancer( true );

													neighbour.GetComponent<Tile>().GetUnits().RemoveAt( k );

												}

											}

										}

									}

								}

							}

							if( attacked ) continue;

							if( next.transform.FindChild( "TownFriendly" ) != null ) {

								for( int k = 0; k < current.GetComponent<Tile>().GetUnits().Count; k++ ) {

									next.GetComponent<Tile>().AddToCapture( Mechanics.GetCaptureRate( current.GetComponent<Tile>().GetUnits()[ k ].GetUnitType() ) );

								}

							} else {

								wp.MoveToNext( 1, 0, 0, current.GetComponent<Tile>().GetUnits()[ j ].GetUnitTask() );

								int x2 = ( int )wp.GetNext().GetComponent<Tile>().GetPosition().x;
								int y2 = ( int )wp.GetNext().GetComponent<Tile>().GetPosition().y;

							}

						//Scout & Conquer
						} else if( current.GetComponent<Tile>().GetUnits()[ j ].GetUnitTask() == 1 ) {

							if( current.GetComponent<Tile>().GetOwner() == TileOwner.AI ) {

								GameObject[] neighbours = new GameObject[ 6 ];

								int c = 0;
								for( int dy = -1; dy < 2; dy++ ) {

									for( int dx = -1; dx < 2; dx++ ) {

										int x = ( int )x1 + dx;
										int y = ( int )y1 + dy;

										if( dx != 0 ) {
											if( x % 2 == 1 ) {
												if( dy == 1 ) continue;
											} else {
												if( dy == -1 ) continue;
											}
										}

										if( x > -1 && x < mMap.Size.x && y > -1 && y < mMap.Size.y ) {

											if( mMap.GetMapData()[ x, y ] == null ) neighbours[ c ] = null;
											if( mMap.GetMapData()[ x, y ] == current ) continue;

											neighbours[ c ] = mMap.GetMapData()[ x, y ];
											c++;

										}

									}

								}

								int r = Random.Range( 0, 5 );
								bool found = false;

								bool[] inspected = new bool[ 6 ];
								bool[] dangerous = new bool[ 6 ];
								bool[] blocked = new bool[ 6 ];

								//Fix this sometimes
								int loopBlock = 0;
								while( !found ) {

									if( neighbours[r] == null || !neighbours[r].activeSelf || neighbours[ r ].GetComponent<Tile>().IsLand() || neighbours[ r ].transform.FindChild( "Tree" ) ) {

										inspected[ r ] = true;
										blocked[ r ] = true;
										r = Random.Range( 0, 5 );

									} else if( neighbours[ r ].GetComponent<Tile>().GetOwner() != TileOwner.AI ) {

										if( neighbours[ r ].GetComponent<Tile>().GetUnits().Count > 0 && neighbours[ r ].GetComponent<Tile>().GetUnits()[ 0 ].GetUnitOwner() == 1 ) {

											inspected[ r ] = true;
											dangerous[ r ] = true;
											blocked[ r ] = true;
											r = Random.Range( 0, 5 );

										} else {

											UnitType t = current.GetComponent<Tile>().GetUnits()[ j ].GetUnitType();
											if( t == UnitType.Footman ) {

												neighbours[ r ].GetComponent<Tile>().AddFootman( 2, 1 );
												current.GetComponent<Tile>().RemoveFootman();

											} else if( t == UnitType.Archer ) {

												neighbours[ r ].GetComponent<Tile>().AddArcher( 2, 1 );
												current.GetComponent<Tile>().RemoveArcher();

											} else if( t == UnitType.Lancer ) {

												neighbours[ r ].GetComponent<Tile>().AddLancer( 2, 1 );
												current.GetComponent<Tile>().RemoveLancer();

											}

											found = true;
											break;

										}

									} else {

										inspected[ r ] = true;
										r = Random.Range( 0, 5 );
										
									}

									int inspectedCount = 0;
									int blockedCount = 0;
									for( int k = 0; k < 6; k++ ) {

										if( inspected[ k ] == true ) inspectedCount++;
										if( blocked[ k ] == true ) blockedCount++;

									}
									
									if( inspectedCount == 6 - blockedCount ) {
										
										r = Random.Range( 0, 5 );
										while( dangerous[ r ] || blocked[ r ] ) r = Random.Range( 0, 5 );

										UnitType t = current.GetComponent<Tile>().GetUnits()[ j ].GetUnitType();
										if( t == UnitType.Footman ) {

											neighbours[ r ].GetComponent<Tile>().AddFootman( 2, 1 );
											current.GetComponent<Tile>().RemoveFootman();

										} else if( t == UnitType.Archer ) {

											neighbours[ r ].GetComponent<Tile>().AddArcher( 2, 1 );
											current.GetComponent<Tile>().RemoveArcher();

										} else if( t == UnitType.Lancer ) {

											neighbours[ r ].GetComponent<Tile>().AddLancer( 2, 1 );
											current.GetComponent<Tile>().RemoveLancer();

										}

										found = true;
										break;

									}

								}

								if( loopBlock == 100 ) break;
								loopBlock++;

							} else {

								continue;

							}

						}

					}

				}

			}

		}

	}

}
