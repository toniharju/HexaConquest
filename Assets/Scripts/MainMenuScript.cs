using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

	public void NewGame ()	{

		Application.LoadLevel ("Main");

	}

	public void Credits () {

	}

	public void ExitGame () {

		Application.Quit ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
