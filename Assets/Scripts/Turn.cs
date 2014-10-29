using UnityEngine;
using System.Collections;

public class Turn {

	public const byte Player = 0;
	public const byte AI = 1;

	private static int mTurnCount = 0;
	private static byte mTurn = Player;

	public static void SetTurn( byte turn ) {

		mTurn = turn;
		mTurnCount++;

	}

	public static int GetTurnCount() { return mTurnCount; }
	public static byte GetTurn() { return mTurn; }

	public static bool IsPlayer() {

		if( mTurn == Player ) return true;
		return false;

	}

	public static bool IsAI() {

		if( mTurn == AI ) return true;
		return false;

	}

}
