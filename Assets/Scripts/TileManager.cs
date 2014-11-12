using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileManager : MonoBehaviour {

	private bool mRunOnce = false;

	private PlayerManager mPlayerManager;

	private GameObject mHoverTile;
	private GameObject mSelectedTile;

	private GameObject[,] mTiles = null;
	
	public Vector2 MapSize = new Vector2( 10, 10 );

	public Texture2D MainCursor;
	public Texture2D ImpassableCursor;

	public GameObject TileFootmanNumberText;
	public GameObject MoveFootmanButton;
	public GameObject MoveArcherButton;
	public GameObject MoveLancerButton;

	// Use this for initialization
	void Start () {
	
		mPlayerManager = GetComponent< PlayerManager >();

		mTiles = new GameObject[ (int)MapSize.x, (int)MapSize.y ];

	}
	
	// Update is called once per frame
	void Update () {

		if( !Turn.IsPlayer () ) return;

		if( !mRunOnce ) {

			GameObject[] tiles = GameObject.FindGameObjectsWithTag ( "Tile" );
		
			foreach( GameObject tile in tiles ) {
			
				int x = (int)tile.GetComponent< Position >().Location.x;
				int y = (int)tile.GetComponent< Position >().Location.y;
				
				mTiles[ x, y ] = tile;

			}

			foreach( GameObject tile in tiles ) {

				tile.GetComponent< Tile >().UpdateFog ();
				
			}

			mRunOnce = true;

		}



		if( !EventSystem.current.IsPointerOverGameObject () ) {

			RaycastHit hit;

			if ( Physics.Raycast ( Camera.main.ScreenPointToRay ( Input.mousePosition ), out hit ) ) {

				if( mHoverTile == null ) mHoverTile = hit.transform.gameObject;
				if( mHoverTile != null ) {

					if( !mHoverTile.Equals ( hit.transform.gameObject ) ) mHoverTile = hit.transform.gameObject;

					mHoverTile.GetComponent< Tile >().OnHover();

				}

				if( Input.GetMouseButtonUp ( 0 ) ) {

					if( mSelectedTile != null ) {

						if( mSelectedTile.transform.FindChild ( "Arrow" ) != null ) Destroy ( mSelectedTile.transform.FindChild ( "Arrow" ).gameObject );
						mSelectedTile.GetComponent< Tile >().OnDeselect ();

					}

					mSelectedTile = hit.transform.gameObject;
					mSelectedTile.GetComponent< Tile > ().OnSelect ();

					if( mSelectedTile.transform.childCount > 0 ) {

						if( mSelectedTile.transform.GetChild ( 0 ).GetComponent< Overlay >().Clickable ) {

							mPlayerManager.SetState ( State.Wait );

						} else {

							mSelectedTile = null;

						}

					} else {

						mPlayerManager.SetState ( State.Wait );

					}

				} else if( Input.GetMouseButtonUp ( 1 ) && mPlayerManager.GetState () == State.SelectTile ) {

					if( mSelectedTile.transform.FindChild ( "Arrow" ) != null ) Destroy ( mSelectedTile.transform.FindChild ( "Arrow" ).gameObject );

					mPlayerManager.SetState ( State.Wait );

					Cursor.SetCursor ( GetMainCursor (), Vector2.zero, CursorMode.Auto );

				}
			
			} else {

				mHoverTile = null;

				if( mSelectedTile != null && Input.GetMouseButtonUp ( 0 ) ) {

					mSelectedTile.GetComponent< Tile >().OnDeselect ();
					mSelectedTile = null;

				}

			}

		}

		if( mSelectedTile != null && ( Input.GetKeyDown ( KeyCode.Escape ) || Input.GetKeyDown ( KeyCode.Space ) ) ) {
			
			if( mPlayerManager.GetState () == State.SelectTile ) {
				
				if( mSelectedTile.transform.FindChild ( "Arrow" ) != null ) Destroy ( mSelectedTile.transform.FindChild ( "Arrow" ).gameObject );
				
				ResetSelectedTile ();
				mPlayerManager.SetState ( State.Wait );
				
			} else {
				
				ResetSelectedTile ();
				
			}
			
		}

		if (mSelectedTile != null) { 

			int count = mSelectedTile.GetComponent< Tile >().Units.Count;

			TileFootmanNumberText.GetComponent< Text >().text = count.ToString ();

		}

	}

	public Texture2D GetMainCursor() { return MainCursor; }
	public Texture2D GetImpassableCursor() { return ImpassableCursor; }

	public void ResetSelectedTile() {

		if( mSelectedTile == null ) return;

		mSelectedTile.GetComponent< Tile >().OnDeselect ();
		mSelectedTile = null;

	}

	public GameObject GetSelectedTile() {

		return mSelectedTile;

	}

	public GameObject GetHoverTile() {

		return mHoverTile;

	}

	public GameObject[,] GetTiles() {
		
		return mTiles;
		
	}

}
