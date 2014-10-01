using UnityEngine;
using System.Collections;

public class Mechanics : MonoBehaviour {

	// Use this for initialization


	//Boolean for turns	
	public bool activeTurn = false;

	//establish D4- and D6-dices
	public int d4 = Random.Range(1,4);
	public int d6 = Random.Range(1,6);
	
	
	//Power
	public int meleePower;
	public int rangedPower;
	public int horsePower;
	public int attackPower;
	public int defencePower;
	
	//Healths
	public int meleeHealth = 10;
	public int rangedHealth = 5;
	public int horseHealth = 15;
	public int totalHealth;
	
	//Unit count
	public int meleeCount = 0;
	public int rangedCount = 0;
	public int horseCount = 0;

	void Start () {

		activeTurn = true;
	

		//Health + total health

		meleeHealth = meleeHealth * meleeCount;
		rangedHealth = rangedHealth * rangedCount;
		horseHealth = horseHealth * horseCount;
		totalHealth = meleeHealth + rangedHealth + horseHealth;



		//Power calculation for active turn (Attacker)
		if (activeTurn == true){

			meleePower = (10 + d4)*meleeCount;
			rangedPower = (15 + d4)*rangedCount;
			horsePower =( 20 + d4)*horseCount;
			attackPower = meleePower + rangedPower + horsePower;
		} 

		//Power calculation for non-active turn (Defender)
	else {

			meleePower = (10 + d6)*meleeCount;
			rangedPower = (15 + d6)*rangedCount;
			horsePower = (20 + d6)*horseCount;
			defencePower = meleePower + rangedPower + horsePower;
		}


	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
