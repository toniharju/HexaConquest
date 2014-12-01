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

		//Get values from mechanics
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
