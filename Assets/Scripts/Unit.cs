using UnityEngine;
using System.Collections;

public enum UnitType {
	
	Footman,
	Archer,
	Lancer
	
}

public class Unit {
	
	private UnitType unitType;
	private byte owner;

	public void SetOwner( byte o ) {

		owner = o;

	}

	public byte GetOwner() {

		return owner;

	}

	public void SetUnitType( UnitType type ) {

		unitType = type;

	}

	public UnitType GetUnitType() { return unitType; }

}
