using UnityEngine;
using System.Collections;

public class InterfaceManager : MonoBehaviour {

	private GameObject mFootmanCountText;

	private GameObject mCastleInfoPanel;
	private GameObject mTileInfoPanel;
	private TileManager mTileManager;

	// Use this for initialization
	public void Start () {
	
		mTileManager = GetComponent< TileManager >();

		mCastleInfoPanel = GameObject.Find ( "CastleInfoPanel" );
		mCastleInfoPanel.SetActive ( false );

		mTileInfoPanel = GameObject.Find ( "TileInfoPanel" );
		mTileInfoPanel.SetActive ( false );

	}
	
	// Update is called once per frame
	public void Update () {
	
		if( !Turn.IsPlayer () ) return;

		GameObject selected_object = mTileManager.GetSelectedTile ();

		if ( selected_object != null ) {

			if( selected_object.transform.FindChild ( "OverlayFriendlyTown" ) != null ) {
				
				mCastleInfoPanel.SetActive ( true );
				mTileInfoPanel.SetActive ( false );
				
			} else {
				
				mCastleInfoPanel.SetActive ( false );
				mTileInfoPanel.SetActive ( true );
				
			}

			if( Input.GetKeyDown ( KeyCode.Escape ) || Input.GetKeyDown ( KeyCode.Space ) ) {
				
				mTileInfoPanel.SetActive ( false );
				mCastleInfoPanel.SetActive ( false );
				
			}

		} else {

			mTileInfoPanel.SetActive ( false );
			mCastleInfoPanel.SetActive ( false );

		}

	}

}
