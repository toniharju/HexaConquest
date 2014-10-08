using UnityEngine;
using System.Collections;

[RequireComponent( typeof( Camera ) )]
public class NoFog : MonoBehaviour {

	void OnPreRender() {
		
		RenderSettings.fog = false;
		
	}

	void OnPostRender() {

		RenderSettings.fog = true;

	}

}
