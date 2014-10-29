using UnityEngine;
using System.Collections;

public class Mechanics : MonoBehaviour {

	private TileManager mTileManager;

	//Boolean for turns	
	public bool activeTurn = false;

	//establish HitDice, D4-, D6- and healths + power
	public int d3 = Random.Range(1,3);
	public int d6 = Random.Range(1,6);
	public int hitDice;
	public int attackPower;
	public int attackHealth;
	public int attackTarget;
	public int defendHealth;
	public int unitCount;

	//Establish Arrays for unit's count, health and power
	public int [] unitHealth = new int[3];
	public int [] unitPower = new int[3];



	void Start () {

		mTileManager = GameObject.Find ("Manager").GetComponent< TileManager > ();

		activeTurn = true;

		//Array for unitCount
		unitCount = mTileManager.GetSelectedTile ().GetComponent< Tile >().Units.Count;

		//Establish dice for targets
		hitDice = Random.Range (0, unitCount);

		//Array for healths
		unitHealth [(int)UnitType.Footman] = 10;
		unitHealth [(int)UnitType.Archer] = 5;
		unitHealth [(int)UnitType.Lancer] = 20;

		//Array for power
		unitPower [(int) UnitType.Footman] = (10 + d3)-d6;
		unitPower [(int) UnitType.Archer] = (15 + d3)-d6;
		unitPower [(int) UnitType.Lancer] = (20 + d3)-d6;


		//Damage calculation and targeting
		attackTarget = hitDice;
		attackHealth = GetType (UnitType);
		attackPower = GetType (UnitType);
		defendHealth = GetType (UnitType);
			
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
