using UnityEngine;
using System.Collections;

public enum UnitType {

	Footman,
	Archer,
	Lancer

}

public class Unit {

	private int mOwner;
	private UnitType mType;

	public Unit( UnitType type, int owner ) {

		mType = type;
		mOwner = owner;

	}

	public void SetUnitType( UnitType type ) {

		mType = type;

	}

	public UnitType GetUnitType() { return mType; }

	public void SetUnitOwner( int owner ) { mOwner = owner; }
	public int GetUnitOwner() { return mOwner; }

}
