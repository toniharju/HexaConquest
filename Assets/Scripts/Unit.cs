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

	private int mTask = 0;

	public Unit( UnitType type, int owner, int task = 0 ) {

		mType = type;
		mOwner = owner;
		mTask = task;

	}

	public void SetUnitType( UnitType type ) {

		mType = type;

	}

	public UnitType GetUnitType() { return mType; }

	public void SetUnitOwner( int owner ) { mOwner = owner; }
	public int GetUnitOwner() { return mOwner; }

	public void SetUnitTask( int task ) { mTask = task; }
	public int GetUnitTask() { return mTask; }

}
