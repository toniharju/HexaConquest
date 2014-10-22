using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

	private int captureProgress = 0;

	public List< Unit > Units = new List< Unit >();

	public int GoldValue = 1;
	public int CaptureValue = 200;
	public byte Owner = 0;

	// Use this for initialization
	void Start () {

		TileManager.InitializeMap ();

		//TileManager.GetMapData() [ (int)Position.x, (int)Position.y ] = this.gameObject;
		//TileManager.GetOwnerData() [ (int)Position.x, (int)Position.y ] = Owner;

	}

	void Update() {

		if ( !this.Equals ( TileManager.GetCurrent () ) ) return;

		Overlay overlay = GetComponent< Overlay > ();

		if ( overlay != null ) {

			if( overlay.Model == OverlayType.FriendlyTown ) {



			}

		}

	}

	private void UpdateFog() {

		int x = (int)Position.x;
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
			
		}

	}

	public void OnTurnUpdate() {
	
		if( ( Owner == 0 || Owner != PlayerManager.GetTurn () ) && Units.Count > 0 ) {

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

		Transform unit = transform.FindChild ( "Unit" );
		
		if( Units.Count != 0 && unit == null ) {
			
			GameObject unit_object = new GameObject( "Unit" );
			
			unit_object.layer = 8;
			
			unit_object.AddComponent< MeshFilter >();
			MeshRenderer mesh_renderer = unit_object.AddComponent< MeshRenderer >();
			mesh_renderer.sharedMaterials = new Material[4];
			
			unit_object.transform.parent = transform;
			
			unit_object.transform.localPosition = Vector3.zero;
			unit_object.transform.localRotation = Quaternion.Euler ( 0, 0, 0 );
			unit_object.transform.localScale = new Vector3( 1, 1, 1 );
			
			unit = unit_object.transform;
			
		}
		
		if( Units.Count > 0 && Units.Count < 6 ) {
			
			Material[] material = new Material[4];
			material[0] = Resources.Load< Material >( "Materials/ShieldSidesAndBack" );
			material[1] = Resources.Load< Material >( "Materials/ShieldFront" );
			material[2] = Resources.Load< Material >( "Materials/Stand" );
			material[3] = Resources.Load< Material >( "Materials/Helmet" );
			
			unit.GetComponent< MeshFilter >().sharedMesh = Resources.Load< Mesh >( "Models/Unit3" );
			unit.GetComponent< MeshRenderer >().sharedMaterials = material;
			
		} else if( Units.Count > 5 && Units.Count < 11 ) {
			
			Material[] material = new Material[4];
			material[0] = Resources.Load< Material >( "Materials/ShieldSidesAndBack" );
			material[1] = Resources.Load< Material >( "Materials/ShieldFront" );
			material[2] = Resources.Load< Material >( "Materials/Stand" );
			material[3] = Resources.Load< Material >( "Materials/Helmet" );
			
			unit.GetComponent< MeshFilter >().sharedMesh = Resources.Load< Mesh >( "Models/Unit6" );
			unit.GetComponent< MeshRenderer >().sharedMaterials = material;
			
		} else if( Units.Count > 10 && Units.Count < 17 ) {
			
			Material[] material = new Material[4];
			material[0] = Resources.Load< Material >( "Materials/ShieldSidesAndBack" );
			material[1] = Resources.Load< Material >( "Materials/ShieldFront" );
			material[2] = Resources.Load< Material >( "Materials/Stand" );
			material[3] = Resources.Load< Material >( "Materials/Helmet" );
			
			unit.GetComponent< MeshFilter >().sharedMesh = Resources.Load< Mesh >( "Models/Unit9" );
			unit.GetComponent< MeshRenderer >().sharedMaterials = material;
			
		} else {
			
			if( unit != null ) DestroyImmediate ( unit.gameObject );
			
		}

		if( Owner == 0 ) {
			renderer.material.color = new Color( 0.8f, 0.8f, 0.8f );
		} else if( Owner == 1 ) {
			renderer.material.color = new Color( 0.0f, 1.0f, 0.0f );
		} else if( Owner == 2 ) {
			renderer.material.color = new Color( 1.0f, 0.0f, 0.0f );
		}

		UpdateFog ();

	}

	public List< Unit > GetUnits() {

		return Units;

	}

	public void MoveUnitFrom( UnitType type, Vector2 position ) {

		int x = (int)position.x;
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

		}

	}

}
