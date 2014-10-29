using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

	private TileManager mTileManager;
	private PlayerManager mPlayerManager;

	private int mCaptureProgress = 0;

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

			GameObject from = mTileManager.GetSelectedTile ();
			GameObject to = gameObject;

			int delta_x = (int)Mathf.Abs ( from.GetComponent< Position >().Location.x - to.GetComponent< Position >().Location.x );
			int delta_y = (int)Mathf.Abs ( from.GetComponent< Position >().Location.y - to.GetComponent< Position >().Location.y );

			if( Units.Count == 16 ) {

				//Le error, this tile is not available to move to, show it!
				mPlayerManager.SetState ( State.Wait );
				return;

			}

			if( mPlayerManager.GetStateParameters ().Equals ( "move_footman" ) ) {
				
				//Get these values from Mechanics
				if( ( delta_x == 1 || delta_y == 1 ) ) {

					if( Input.GetMouseButtonUp ( 0 ) ) {

						//Display arrow here
					
						Unit unit = new Unit();
						unit.SetUnitType ( UnitType.Footman );
						unit.SetOwner ( 1 );

						if( from.transform.FindChild( "OverlayFriendlyTown" ) ) {
					
							Units.Add ( unit );

							mPlayerManager.RemovePlayerUnit ( UnitType.Footman, 1 );

						} else {

							if( transform.FindChild ( "OverlayFriendlyTown" ) ) {

								mPlayerManager.SetState ( State.Wait );
								return;

							}

							List< Unit > unit_list = from.GetComponent< Tile >().Units;

							bool foundUnit = false;

							foreach( Unit u in unit_list ) {

								if( u.GetUnitType() == UnitType.Footman ) {

									foundUnit = true;
									unit_list.Remove ( u );
									break;

								}

							}

							if( !foundUnit ) {

								mPlayerManager.SetState ( State.Wait );
								return;

							}

							Units.Add ( unit );

						}

					}
					
				} else {
					
					//Display some indicator that this is a illegal move
					
				}
				
			} else if( mPlayerManager.GetStateParameters ().Equals ( "move_archer" ) ) {



			} else if( mPlayerManager.GetStateParameters ().Equals ( "move_lancer" ) ) {



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

	public void MoveUnitFrom( UnitType type, Vector2 position ) {

		/*int x = (int)position.x;
		int y = (int)position.y;
		Debug.Log (Mathf.Abs (transform.position.x - position.x));
		if ( Mathf.Abs (transform.position.x - position.x ) > 1 || Mathf.Abs ( transform.position.y - position.y ) > 1 || Units.Count == 16 ) return;

		Overlay overlay = TileManager.GetMapData ()[ x, y ].GetComponent< Overlay >();

		if( overlay != null && overlay.Model == OverlayType.FriendlyTown ) {

			Unit u = new Unit();
			u.SetOwner ( PlayerManager.GetTurn () );
			u.SetUnitType( type );

			Units.Add ( u );

		} else {

			if( Units.Count == 16 ) return;
			
			Unit u = new Unit();
			u.SetOwner ( PlayerManager.GetTurn () );
			u.SetUnitType( type );
			
			Units.Add ( u );

			int i = 0;
			foreach( Unit temp in TileManager.GetMapData ()[ x, y ].GetComponent< Tile >().Units ) {

				if( temp.GetUnitType () == type ) {

					TileManager.GetMapData ()[ x, y ].GetComponent< Tile >().Units.RemoveAt( i );

					return;

				}

				i++;

			}

		}*/

	}

}
