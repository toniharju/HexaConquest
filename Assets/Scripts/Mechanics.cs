using UnityEngine;
using System.Collections;

public class Mechanics : MonoBehaviour {



	//Boolean for turns	
	public bool activeTurn = false;

	//establish D4- and D6-dices
	public int d3 = Random.Range(1,3);
	public int d6 = Random.Range(1,6);
	public int hitDice;
	
	//Array for healths 

	public int [] unitHealth = new int[3];
	public int [] unitPower = new int[3];

	void Start () {

		activeTurn = true;

		hitDice = Random.Range (0, 15);//Replace 15 with the army's lenght / total count

		unitHealth [(int)UnitType.Footman] = 10;
		unitHealth [(int)UnitType.Archer] = 5;
		unitHealth [(int)UnitType.Lancer] = 20;


		unitPower [(int) UnitType.Footman] = (10 + d3)-d6;
		unitPower [(int) UnitType.Archer] = (15 + d3)-d6;
		unitPower [(int) UnitType.Lancer] = (20 + d3)-d6;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
