using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {

	public GameObject Next;
	public GameObject Previous;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool MoveToNext( int footmen, int archers, int lancers, int task ) {

		if( Next == null ) return false;

		int count = Next.GetComponent<Tile>().GetUnits().Count;
		
		if( Next.GetComponent<Tile>().IsAILand() && count < 32 && footmen + archers + lancers + count < 33 && transform.parent.GetComponent<Tile>().GetUnits().Count >= footmen + archers + lancers ) {

			for( int i = 0; i < footmen; i++ ) { Next.GetComponent<Tile>().AddFootman( 2, task ); transform.parent.GetComponent<Tile>().RemoveFootman(); }
			for( int i = 0; i < archers; i++ ) { Next.GetComponent<Tile>().AddArcher( 2, task ); transform.parent.GetComponent<Tile>().RemoveArcher(); }
			for( int i = 0; i < lancers; i++ ) { Next.GetComponent<Tile>().AddLancer( 2, task ); transform.parent.GetComponent<Tile>().RemoveLancer(); }

			return true;

		} else if( !Next.GetComponent<Tile>().IsAILand() && count < 16 && footmen + archers + lancers + count < 17 && transform.parent.GetComponent<Tile>().GetUnits().Count >= footmen + archers + lancers ) {

			for( int i = 0; i < footmen; i++ ) { Next.GetComponent<Tile>().AddFootman( 2, task ); transform.parent.GetComponent<Tile>().RemoveFootman(); }
			for( int i = 0; i < archers; i++ ) { Next.GetComponent<Tile>().AddArcher( 2, task ); transform.parent.GetComponent<Tile>().RemoveArcher(); }
			for( int i = 0; i < lancers; i++ ) { Next.GetComponent<Tile>().AddLancer( 2, task ); transform.parent.GetComponent<Tile>().RemoveLancer(); }

			return true;

		} else {

			return false;

		}

	}

	public bool MoveToPrevious( int footmen, int archers, int lancers, int task ) {

		if( Previous == null ) return false;

		int count = Previous.GetComponent<Tile>().GetUnits().Count;

		if( Previous.GetComponent<Tile>().IsAILand() && count < 32 && footmen + archers + lancers + count < 33 && transform.parent.GetComponent<Tile>().GetUnits().Count >= footmen + archers + lancers ) {

			for( int i = 0; i < footmen; i++ ) { Previous.GetComponent<Tile>().AddFootman( 2, task ); transform.parent.GetComponent<Tile>().RemoveFootman(); }
			for( int i = 0; i < archers; i++ ) { Previous.GetComponent<Tile>().AddArcher( 2, task ); transform.parent.GetComponent<Tile>().RemoveArcher(); }
			for( int i = 0; i < lancers; i++ ) { Previous.GetComponent<Tile>().AddLancer( 2, task ); transform.parent.GetComponent<Tile>().RemoveLancer(); }

			return true;

		} else if( !Previous.GetComponent<Tile>().IsAILand() && count < 16 && footmen + archers + lancers + count < 17 && transform.parent.GetComponent<Tile>().GetUnits().Count >= footmen + archers + lancers ) {

			for( int i = 0; i < footmen; i++ ) { Previous.GetComponent<Tile>().AddFootman( 2, task ); GetComponent<Tile>().RemoveFootman(); }
			for( int i = 0; i < archers; i++ ) { Previous.GetComponent<Tile>().AddArcher( 2, task ); GetComponent<Tile>().RemoveArcher(); }
			for( int i = 0; i < lancers; i++ ) { Previous.GetComponent<Tile>().AddLancer( 2, task ); GetComponent<Tile>().RemoveLancer(); }

			return true;

		} else {

			return false;

		}

	}

	public GameObject GetNext() { return Next; }
	public GameObject GetPrevious() { return Previous; }

}
