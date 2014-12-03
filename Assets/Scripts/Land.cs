using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Land : MonoBehaviour {

	private int mGold = 10;
	private int mGoldIncome = 1;

	private TileManager mTileManager;

	// Use this for initialization
	void Start () {
		
		if( GameObject.Find( "Managers" ) != null ) {

			mTileManager = GameObject.Find( "Managers" ).GetComponent<TileManager>();

		}

	}
	
	// Update is called once per frame
	void Update () {

		if( transform.parent.GetComponent<Tile>().GetUnits().Count + transform.parent.GetComponent<Tile>().GetTempUnits().Count < 32 ) {

			if( mGold < Mechanics.GetPrice( UnitType.Footman ) )
				mTileManager.GetAddButtons()[ 0 ].GetComponent<Button>().interactable = false;
			else
				mTileManager.GetAddButtons()[ 0 ].GetComponent<Button>().interactable = true;

			if( mGold < Mechanics.GetPrice( UnitType.Archer ) )
				mTileManager.GetAddButtons()[ 1 ].GetComponent<Button>().interactable = false;
			else
				mTileManager.GetAddButtons()[ 1 ].GetComponent<Button>().interactable = true;

			if( mGold < Mechanics.GetPrice( UnitType.Lancer ) )
				mTileManager.GetAddButtons()[ 2 ].GetComponent<Button>().interactable = false;
			else
				mTileManager.GetAddButtons()[ 2 ].GetComponent<Button>().interactable = true;

		} else {

			mTileManager.GetAddButtons()[ 0 ].GetComponent<Button>().interactable = false;
			mTileManager.GetAddButtons()[ 1 ].GetComponent<Button>().interactable = false;
			mTileManager.GetAddButtons()[ 2 ].GetComponent<Button>().interactable = false;

		}

		mTileManager.GetTurnText()[ 0 ].GetComponent<Text>().text = "/turn: " + mGoldIncome;
		mTileManager.GetTurnText()[ 1 ].GetComponent<Text>().text = "total: " + mGold;

	}

	public void AddFootman() {

		mGold -= Mechanics.GetPrice( UnitType.Footman );

	}

	public void AddArcher() {

		mGold -= Mechanics.GetPrice( UnitType.Archer );

	}

	public void AddLancer() {

		mGold -= Mechanics.GetPrice( UnitType.Lancer );

	}

	public void AddGold( int amount ) { mGold += amount; }
	public int GetGold() { return mGold; }

	public void SetGoldIncome( int i ) { mGoldIncome = i; }
	public int GetGoldIncome() { return mGoldIncome; }

}
