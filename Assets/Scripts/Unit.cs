using UnityEngine;
using System.Collections;

public enum UnitType {
	
	Footman,
	Archer,
	Lancer
	
}

public class Unit {
	
	private UnitType mUnitType;
	private byte mOwner;

	public void SetOwner( byte owner ) {

		mOwner = owner;

	}

	public byte GetOwner() {

		return mOwner;

	}

	public void SetUnitType( UnitType unitType ) {

		mUnitType = unitType;

	}

	public UnitType GetUnitType() { return mUnitType; }

}
