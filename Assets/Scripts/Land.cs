using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Land : MonoBehaviour {

	private int mGold = 1;
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
		if( mGold < 1 )
			mTileManager.GetAddButtons()[ 0 ].GetComponent<Button>().interactable = false;
		else
			mTileManager.GetAddButtons()[ 0 ].GetComponent<Button>().interactable = true;

		if( mGold < 2 )
			mTileManager.GetAddButtons()[ 1 ].GetComponent<Button>().interactable = false;
		else
			mTileManager.GetAddButtons()[ 1 ].GetComponent<Button>().interactable = true;

		if( mGold < 3 )
			mTileManager.GetAddButtons()[ 2 ].GetComponent<Button>().interactable = false;
		else
			mTileManager.GetAddButtons()[ 2 ].GetComponent<Button>().interactable = true;

	}

	public void AddFootman() {

		mGold -= 1;

	}

	public void AddArcher() {

		mGold -= 2;

	}

	public void AddLancer() {

		mGold -= 3;

	}

	public void AddGold( int amount ) { mGold += amount; }

}
