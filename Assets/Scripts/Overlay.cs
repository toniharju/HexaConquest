using UnityEngine;
using System.Collections;

public class Overlay : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		gameObject.layer = transform.parent.gameObject.layer;

	}

}
