using UnityEngine;
using System.Collections;

public class Mechanics {

	private static TileManager mTileManager;

	//establish HitDice, D4-, D6- and healths + power
	private static int hitDice;
	private static int attackPower;
	private static int attackHealth;
	private static int attackTarget;
	private static int defendHealth;
	private static int unitCount;

	//Establish Arrays for unit's count, health and power
	private static int [] unitHealth = new int[3];
	private static int [] unitPower = new int[3];
	private static int [] unitCaptureRate = new int[3]; 
	private static int [] unitPrice = new int[3]; 
	private static int [] unitMovement = new int[3];



	public static void Initialize () {

		mTileManager = GameObject.Find ("Managers").GetComponent< TileManager > ();

		//Array for unitCount
		//unitCount = mTileManager.GetSelectedTile ().GetComponent< Tile >().Units.Count;

		//Establish dice for targets
		hitDice = Random.Range (0, unitCount);

		//Array for healths
		unitHealth [(int)UnitType.Footman] = 10;
		unitHealth [(int)UnitType.Archer] = 5;
		unitHealth [(int)UnitType.Lancer] = 20;

		//Array for power
		unitPower [(int) UnitType.Footman] = 10;
		unitPower [(int) UnitType.Archer] = 15;
		unitPower [(int) UnitType.Lancer] = 20;

		//Array for capture rates
		unitCaptureRate [(int) UnitType.Footman] = 20;
		unitCaptureRate [(int) UnitType.Archer] = 10;
		unitCaptureRate [(int) UnitType.Lancer] = 5;

		//Array for unit prices
		unitPrice [(int) UnitType.Footman] = 10;
		unitPrice [(int) UnitType.Archer] = 30;
		unitPrice [(int) UnitType.Lancer] = 50; 

		//Array for movement points
		unitMovement [(int) UnitType.Footman] = 1;
		unitMovement [(int) UnitType.Archer] = 1;
		unitMovement [(int) UnitType.Lancer] = 2;
	
	}

	public static int GetHealth( UnitType type ) { return unitHealth[ (int)type ]; }

	public static int GetAttackPower( UnitType type ) {
		
		return ( unitPower[ (int)type ] + Random.Range( 1, 4 ) ) - Random.Range( 1, 7 );
	
	}

	public static int GetCaptureRate( UnitType type ) { return unitCaptureRate[ ( int )type ]; }

	public static int GetPrice( UnitType type ) { return unitPrice[ ( int )type ]; }
	public static int GetMovement( UnitType type ) { return unitMovement[ ( int )type ]; }

}
