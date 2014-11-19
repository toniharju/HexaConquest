using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

	private GameObject mArrow = null;

	private TileManager mTileManager;
	private PlayerManager mPlayerManager;

	private int mCaptureProgress = 0;

	public List< Unit > TempUnits = new List< Unit >();
	public List< Unit > Units = new List< Unit >();

	public int GoldValue = 1;
	public int CaptureValue = 200;
	public byte Owner = 0;

	// Use this for initialization
	void Start () {

		mTileManager = GameObject.Find ( "Manager" ).GetComponent< TileManager >();
		mPlayerManager = GameObject.Find ( "Manager" ).GetComponent< PlayerManager >();

	}

	void Update() {

		if (mTileManager.GetSelectedTile () == gameObject && mPlayerManager.GetState () == State.Wait) {

			renderer.material.color = new Color( 0.0f, 0.74f, 0.95f );

		} else {

			switch (Owner) {
				
				default:
				case 0:
					renderer.material.color = new Color (0.8f, 0.8f, 0.8f);
					break;
				
				case 1:
					renderer.material.color = new Color (0, 1, 0);
					break;
				
				case 2:
					renderer.material.color = new Color (1, 0, 0);
					break;
				
			}

		}

	}

	public void OnSelect() {



	}

	public void OnDeselect() {


		
	}
	
	public void OnHover() {

		if( mPlayerManager.GetState () == State.SelectTile ) {

			Cursor.SetCursor( mTileManager.GetMainCursor (), Vector2.zero, CursorMode.ForceSoftware );

			bool allowed = true;

			GameObject from = mTileManager.GetSelectedTile ();

			int delta_x = (int)( GetComponent< Position >().Location.x - from.GetComponent< Position >().Location.x );
			int delta_y = (int)( GetComponent< Position >().Location.y - from.GetComponent< Position >().Location.y );

			if( GetComponent< Position >().Location.x % 2 == 1 ) {
				if( delta_x != 0 && delta_y == 1 ) { delta_x = 10; delta_y = 10; }
			} else {
				if( delta_x != 0 && delta_y == -1 ) { delta_x = 10; delta_y = 10; }
			}

			delta_x = (int)Mathf.Abs ( delta_x );
			delta_y = (int)Mathf.Abs ( delta_y );

			if( Units.Count + TempUnits.Count >= 16 ) {

				Cursor.SetCursor( mTileManager.GetImpassableCursor (), Vector2.zero, CursorMode.ForceSoftware );
				allowed = false;

			}

			UnitType type = (UnitType)mPlayerManager.GetStateParameters ().Peek ();
			int delta_max = 0;

			if( type == UnitType.Footman ) {
				
				delta_max = 1;

			} else if( type == UnitType.Archer ) {

				delta_max = 1;

			} else if( type == UnitType.Lancer ) {

				delta_max = 1;

			}

			if( ( delta_x == 0 && delta_y == 0 ) || transform.childCount > 0 ) {

				if( !( transform.FindChild ( "Unit3" ) || transform.FindChild ( "Unit6" ) || transform.FindChild ( "Unit9" ) ) || from == gameObject ) {

					Cursor.SetCursor( mTileManager.GetImpassableCursor (), Vector2.zero, CursorMode.ForceSoftware );
					allowed = false;

				}

			}

			if( ( delta_x < delta_max + 1 ) && ( delta_y < delta_max + 1 ) && allowed ) {

				if( from.transform.FindChild ( "Arrow" ) == null ) {
					mArrow = new GameObject( "Arrow" );
					mArrow.transform.parent = from.transform;
					mArrow.transform.localPosition = Vector3.zero;
					mArrow.transform.localRotation = Quaternion.identity;
					mArrow.transform.localScale = new Vector3( 1, 1, 1 );
					GameObject temp = Utils.InstantiateLocal ( mArrow, "Prefabs/ShortArrow", new Vector3( 0, -0.01f, 0.006f ),
					                        											   Quaternion.Euler ( 0, 180, 180 ),
					                        											   new Vector3( 0.005f, 0.008f, 1 ) );
					temp.name = "Short";
					temp.layer = 8;
				} else {
					mArrow = from.transform.FindChild ( "Arrow" ).gameObject;
				}

				Vector3 delta = transform.position - from.transform.position;
				float angle = Mathf.Atan2 ( delta.z, delta.x ) * Mathf.Rad2Deg;
				mArrow.transform.localRotation = Quaternion.Euler ( 0, 0, 270 - angle );

				if( Input.GetMouseButtonUp ( 1 ) ) {

					if( mArrow != null ) Destroy( mArrow );
					Cursor.SetCursor( mTileManager.GetMainCursor (), Vector2.zero, CursorMode.ForceSoftware );

					if( !allowed ) {

						mPlayerManager.SetState ( State.Wait );
						return;

					}

					foreach( UnitType temp_type in mPlayerManager.GetStateParameters () ) {

						Unit unit = new Unit();
						unit.SetUnitType ( temp_type );
						unit.SetOwner ( 1 );
					
						if( from.transform.FindChild( "OverlayFriendlyTown" ) ) {
						
							TempUnits.Add ( unit );
							mPlayerManager.RemovePlayerUnit ( type, 1 );
						
						} else {
						
							List< Unit > unit_list = from.GetComponent< Tile >().Units;
						
							bool foundUnit = false;
						
							foreach( Unit u in unit_list ) {
							
								if( u.GetUnitType() == type ) {
								
									foundUnit = true;
									unit_list.Remove ( u );
									break;
								
								}
							
							}
						
							if( !foundUnit ) {
							
								mPlayerManager.SetState ( State.Wait );
								return;
							
							}
						
							TempUnits.Add ( unit );

						}

					}

					mPlayerManager.ClearStateParameters();
					
				}
				
			} else {

				if( from.transform.FindChild ( "Arrow" ) != null ) Destroy( from.transform.FindChild ( "Arrow" ).gameObject );

				Cursor.SetCursor( mTileManager.GetImpassableCursor (), Vector2.zero, CursorMode.ForceSoftware );
				
			}
			
		}

	}
	
	public void UpdateFog() {

		int x = (int)GetComponent< Position >().Location.x;
		int y = (int)GetComponent< Position >().Location.y;

		if( transform.FindChild ( "OverlayFriendlyTown" ) != null ||
		    Owner == 1 ||
		    Units.Count > 0 ) {

			if( Units.Count > 0 ) {

				bool notOwn = true;

				foreach( Unit u in Units ) {

					if( u.GetOwner () == 1 ) {

						notOwn = false;
						break;

					}

				}

				if( notOwn ) return;

			}

			for( int ox = -1; ox < 2; ox++ ) {
				
				for( int oy = -1; oy < 2; oy++ ) {

					int dx = x + ox;
					int dy = y + oy;

					if( dx > -1 && dx < mTileManager.MapSize.x &&
					    dy > -1 && dy < mTileManager.MapSize.y && mTileManager.GetTiles () [ dx, dy ] != null && mTileManager.GetTiles()[ dx, dy ].layer != 8 ) {

						if( x % 2 == 1 ) {
							if( ox != 0 && oy == -1 ) continue;
						} else {
							if( ox != 0 && oy == 1 ) continue;
						}

						mTileManager.GetTiles ()[ dx, dy ].layer = 8;
						
					}
					
				}
				
			}
			
		}

	}

	public void OnTurn() {

		//Update Capture
		if ( Units.Count > 0 ) {
			
			if( Units[0].GetOwner () == 1 ) {
				
				//Get multiplier from Mechanics
				mCaptureProgress -= Units.Count * 200;
				
			} else if( Units[0].GetOwner () == 2 ) {
				
				//Get multiplier from Mechanics
				mCaptureProgress += Units.Count * 200;
				
			}
			
		}

		if ( TempUnits.Count > 0 ) {

			foreach( Unit u in TempUnits ) {

				Units.Add ( u );

			}

			TempUnits.Clear ();

		}

		if (mCaptureProgress <= -CaptureValue) {
		
			mCaptureProgress = -CaptureValue;
			Owner = 1;

		} else if (mCaptureProgress >= CaptureValue) {

			mCaptureProgress = CaptureValue;
			Owner = 2;

		}

		//Update Gold
		if ( Owner == 1 ) {

			mPlayerManager.SetPlayerGold ( mPlayerManager.GetPlayerGold () + GoldValue );

		} else if ( Owner == 2 ) {

			mPlayerManager.SetAIGold ( mPlayerManager.GetAIGold () + GoldValue );

		}

		if( Units.Count > 0 && Units.Count < 6 && transform.FindChild ( "Unit3" ) == null ) {
			
			if( transform.FindChild ( "Unit6" ) != null ) Destroy ( transform.FindChild ( "Unit6" ).gameObject );
			if( transform.FindChild ( "Unit9" ) != null ) Destroy ( transform.FindChild ( "Unit9" ).gameObject );
			GameObject temp = Utils.InstantiateLocal ( gameObject, "Prefabs/Units/Unit3", Vector3.zero, Quaternion.identity, new Vector3( 1, 1, 1 ) );
			temp.name = "Unit3";
			
		} else if( Units.Count > 5 && Units.Count < 11 && transform.FindChild ( "Unit6" ) == null ) {
			
			if( transform.FindChild ( "Unit3" ) != null ) Destroy ( transform.FindChild ( "Unit3" ).gameObject );
			if( transform.FindChild ( "Unit9" ) != null ) Destroy ( transform.FindChild ( "Unit9" ).gameObject );
			GameObject temp = Utils.InstantiateLocal ( gameObject, "Prefabs/Units/Unit6", Vector3.zero, Quaternion.identity, new Vector3( 1, 1, 1 ) );
			temp.name = "Unit6";
			
		} else if( Units.Count > 10 && Units.Count < 17 && transform.FindChild ( "Unit9" ) == null ) {
			
			if( transform.FindChild ( "Unit3" ) != null ) Destroy ( transform.FindChild ( "Unit3" ).gameObject );
			if( transform.FindChild ( "Unit6" ) != null ) Destroy ( transform.FindChild ( "Unit6" ).gameObject );
			GameObject temp = Utils.InstantiateLocal ( gameObject, "Prefabs/Units/Unit9", Vector3.zero, Quaternion.identity, new Vector3( 1, 1, 1 ) );
			temp.name = "Unit9";
			
		} else if( Units.Count == 0 ) {

			if( transform.FindChild ( "Unit3" ) != null ) Destroy ( transform.FindChild ( "Unit3" ).gameObject );
			if( transform.FindChild ( "Unit6" ) != null ) Destroy ( transform.FindChild ( "Unit6" ).gameObject );
			if( transform.FindChild ( "Unit9" ) != null ) Destroy ( transform.FindChild ( "Unit9" ).gameObject );
			
		}

		UpdateFog ();
		
	}

	public List< Unit > GetUnits() {

		return Units;

	}

}
