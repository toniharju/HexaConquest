using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum State {

	Initialize,
	Wait,
	End

}

public class StateMachine {

	private static byte turn = 1;

	private static State state = State.Initialize;
	private static List< System.Action > actions = new List< System.Action >();

	public static void Update () {
	
		switch (state) {

		//Initialize
		case State.Initialize:

			TileManager.Initialize ();

			SetState ( State.Wait );
			break;

		//Wait For Action
		case State.Wait:

			TileManager.Update();

			if( Input.GetKeyDown ( KeyCode.Space ) ) SetState ( State.End );

			break;

		//Execute Actions and End Turn
		case State.End:

			foreach( System.Action a in actions ) {

				a.Invoke ();

			}

			SetState ( State.Wait );
			break;

		}

	}

	public static void SetState( State s ) {

		state = s;

	}

	public static void AddAction( System.Action a ) {

		actions.Add (a);

	}

}
