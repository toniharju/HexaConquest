using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum State {

	Wait,
	Move

}

public class StateManager {

	private static State mState = State.Wait;
	private static List<string> mStateParameters = new List<string>();

	public static void SetState( State state ) { mState = state; }
	public static State GetState() { return mState; }

	public static void AddParameter( string param ) { mStateParameters.Add( param ); }
	public static List<string> GetParameters() { return mStateParameters; }

	public static void Clear() {

		mState = State.Wait;
		mStateParameters.Clear();

	}

}
