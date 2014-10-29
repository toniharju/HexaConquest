using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

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

	public void OnSelect() {

		if( mPlayerManager.GetState () == State.Wait ) {
				
			renderer.material.color = new Color( 0.0f, 0.74f, 0.95f );
			
		}

	}

	public void OnDeselect() {

		switch( Owner ) {
			
			default:
			case 0:
				renderer.material.color = new Color( 0.8f, 0.8f, 0.8f );
				break;
			
			case 1:
				renderer.material.color = new Color( 0, 1, 0 );
				break;
			
			case 2:
				renderer.material.color = new Color( 1, 0, 0 );
				break;
			
		}
		
	}
	
	public void OnHover() {

		if( mPlayerManager.GetState () == State.SelectTile ) {

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

			if( Units.Count == 16 ) {

				//Le error, this tile is not available to move to, show it!
				mPlayerManager.SetState ( State.Wait );
				return;

			}

			UnitType type = UnitType.Footman;
			int delta_max = 0;

			if( mPlayerManager.GetStateParameters ().Equals ( "move_footman" ) ) {
				
				delta_max = 1;
				type = UnitType.Footman;

			} else if( mPlayerManager.GetStateParameters ().Equals ( "move_archer" ) ) {

				delta_max = 1;
				type = UnitType.Archer;

			} else if( mPlayerManager.GetStateParameters ().Equals ( "move_lancer" ) ) {

				delta_max = 1;
				type = UnitType.Lancer;

			}

			if( ( delta_x == 0 && delta_y == 0 ) || transform.FindChild( "OverlayFriendlyTown" ) != null ) {

				allowed = false;

			}

			//Get these values from Mechanics
			if( ( delta_x < delta_max + 1 ) && ( delta_y < delta_max + 1 ) ) {
			
				GameObject arrow = null;

				if( from.transform.FindChild ( "Arrow" ) == null ) {
					arrow = new GameObject( "Arrow" );
					arrow.transform.parent = from.transform;
					arrow.transform.localPosition = Vector3.zero;
					arrow.transform.localRotation = Quaternion.identity;
					arrow.transform.localScale = new Vector3( 1, 1, 1 );
					GameObject temp = Instantiate ( Resources.Load< GameObject >( "Prefabs/ShortArrow" ) ) as GameObject;
					temp.name = "Short";
					temp.transform.parent = arrow.transform;
					temp.transform.localPosition = new Vector3( 0, -0.01f, 0.006f );
					temp.transform.localRotation = Quaternion.Euler ( 0, 180, 180 );
					temp.transform.localScale = new Vector3( 0.005f, 0.008f, 1 );
				} else {
					arrow = from.transform.FindChild ( "Arrow" ).gameObject;
				}

				Vector3 delta = transform.position - from.transform.position;
				float angle = Mathf.Atan2 ( delta.z, delta.x ) * Mathf.Rad2Deg;
				arrow.transform.localRotation = Quaternion.Euler ( 0, 0, 270 - angle );

				if( Input.GetMouseButtonUp ( 1 ) && allowed ) {

					if( arrow != null ) Destroy( arrow );

					Unit unit = new Unit();
					unit.SetUnitType ( type );
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
				
			} else {
				
				//Display some indicator that this is a illegal move
				
			}
			
		}

	}
	
	private void UpdateFog() {

		/*int x = (int)Position.x;
		int y = (int)Position.y;

		if( ( GetComponent< Overlay >() != null && GetComponent< Overlay >().Model == OverlayType.FriendlyTown ) ||
		    Owner == 1 ||
		    Units.Count > 0 ) {

			for( int ox = -1; ox < 2; ox++ ) {
				
				for( int oy = -1; oy < 2; oy++ ) {

					int dx = x + ox;
					int dy = y + oy;

					if( dx > -1 && dx < TileManager.MapSize.x &&
					    dy > -1 && dy < TileManager.MapSize.y && TileManager.GetMapData()[ dx, dy ].layer != 8 ) {

						if( x % 2 == 1 ) {
							if( ox != 0 && oy == -1 ) continue;
						} else {
							if( ox != 0 && oy == 1 ) continue;
						}

						TileManager.GetMapData ()[ dx, dy ].layer = 8;
						
					}
					
				}
				
			}
			
		}*/

	}

	public void OnTurn() {

		if ( TempUnits.Count > 0 ) {

			foreach( Unit u in TempUnits ) {

				Units.Add ( u );

			}

			TempUnits.Clear ();

		}

		if( transform.FindChild ( "OverlayFriendlyTown" ) ) {



		}

		if( Units.Count > 0 && Units.Count < 6 ) {
			
			if( transform.FindChild ( "Units" ) != null ) Destroy ( transform.FindChild ( "Units" ).gameObject );
			GameObject temp = Instantiate ( Resources.Load< GameObject >( "Prefabs/Units/Unit3" ) ) as GameObject;
			temp.name = "Units";
			temp.transform.parent = gameObject.transform;
			temp.transform.localPosition = Vector3.zero;
			temp.transform.localRotation = Quaternion.identity;
			temp.transform.localScale = new Vector3( 1, 1, 1 );
			
		} else if( Units.Count > 5 && Units.Count < 11 ) {
			
			if( transform.FindChild ( "Units" ) != null ) Destroy ( transform.FindChild ( "Units" ).gameObject );
			GameObject temp = Instantiate ( Resources.Load< GameObject >( "Prefabs/Units/Unit6" ) ) as GameObject;
			temp.name = "Units";
			temp.transform.parent = gameObject.transform;
			temp.transform.localPosition = Vector3.zero;
			temp.transform.localRotation = Quaternion.identity;
			temp.transform.localScale = new Vector3( 1, 1, 1 );
			
		} else if( Units.Count > 10 && Units.Count < 17 ) {
			
			if( transform.FindChild ( "Units" ) != null ) Destroy ( transform.FindChild ( "Units" ).gameObject );
			GameObject temp = Instantiate ( Resources.Load< GameObject >( "Prefabs/Units/Unit9" ) ) as GameObject;
			temp.name = "Units";
			temp.transform.parent = gameObject.transform;
			temp.transform.localPosition = Vector3.zero;
			temp.transform.localRotation = Quaternion.identity;
			temp.transform.localScale = new Vector3( 1, 1, 1 );
			
		} else {
			
			if( transform.FindChild ( "Units" ) != null ) Destroy ( transform.FindChild ( "Units" ).gameObject );
			
		}
		
	}

	public void OnTurnUpdate() {
	
		/*if( ( Owner == 0 || Owner != PlayerManager.GetTurn () ) && Units.Count > 0 ) {

			if( PlayerManager.GetTurn () == 1 ) {

				captureProgress -= 200;

			} else {

			}

			if (captureProgress <= -CaptureValue) {
				
				captureProgress = -CaptureValue;
				Owner = 1;
				TileManager.GetOwnerData() [ (int)Position.x, (int)Position.y ] = 1;
				
			} else if (captureProgress >= CaptureValue) {
				
				captureProgress = CaptureValue;
				Owner = 2;
				TileManager.GetOwnerData() [ (int)Position.x, (int)Position.y ] = 2;
				
			}

		}

		if( Owner == 0 ) {
			renderer.material.color = new Color( 0.8f, 0.8f, 0.8f );
		} else if( Owner == 1 ) {
			renderer.material.color = new Color( 0.0f, 1.0f, 0.0f );
		} else if( Owner == 2 ) {
			renderer.material.color = new Color( 1.0f, 0.0f, 0.0f );
		}

		UpdateFog ();*/

	}

	public List< Unit > GetUnits() {

		return Units;

	}

	public void MoveUnit( UnitType type, Vector2 position ) {



	}

}
