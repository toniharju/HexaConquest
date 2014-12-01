using UnityEngine;
using System.Collections;

public class AILand : MonoBehaviour {

	private int mGold = 10;
	private int mGoldIncome = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddGold( int gold ) { mGold += gold; }
	public void RemoveGold( int gold ) { mGold -= gold; }
	public int GetGold() { return mGold; }

	public void SetGoldIncome( int income ) { mGoldIncome = income; }
	public int GetGoldIncome() { return mGoldIncome; }

}
