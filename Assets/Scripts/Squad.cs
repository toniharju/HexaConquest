using UnityEngine;
using System.Collections;

public enum SquadType {
	
	Footman,
	Archer,
	Horseman
	
}

public class Squad : MonoBehaviour {

	private int squadSize;
	private SquadType squadType;

	// Use this for initialization
	void Start () {
	


	}
	
	// Update is called once per frame
	void Update () {
	


	}

	public void SetSquadType( SquadType type ) {

		squadType = type;

	}

	public SquadType GetSquadType() { return squadType; }

	public void SetSize( int size ) {

		squadSize = size;

	}

	public int GetSize() { return squadSize; }

}
