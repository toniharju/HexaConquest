using UnityEngine;
using System.Collections;

public class Mechanics : MonoBehaviour {

	// Use this for initialization


	//Boolean for turns	
	public bool activeTurn = false;

	//establish D4- and D6-dices
	public int d3 = Random.Range(1,3);
	public int d6 = Random.Range(1,6);
	public int hitDice;

	
	
	//Power
	public int meleePower;
	public int rangedPower;
	public int horsePower;
	
	//Healths
	public int meleeHealth = 10;
	public int rangedHealth = 5;
	public int horseHealth = 20;
	
	//Unit count
	public int meleeCount = 0;
	public int rangedCount = 0;
	public int horseCount = 0;
	public int totalCount = meleeCount + rangedCount + horseCount;
	public int maxCount = 16;

	void Start () {

		activeTurn = true;
		hitDice = Random.Range (1, totalCount);




		//Check that the tile's unit count isn't greater than the max unit count

		if (totalCount > maxCount) {
			/*Placeholder for A HUUUUUGE warning sign, that says
			 * "Fuck off, we are full!!!"
			 * 
			 * 
			 * */
				} else {
			/* Placeholder for succesful check
			 * 
			 * 
			 * */


				}

		//Power calculation for active turn (Attacker)
		if (activeTurn == true){

			meleePower = (10 + d3)-d6;
			rangedPower = (15 + d3)-d6;
			horsePower =(20 + d3)-d6;
		} 


	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
