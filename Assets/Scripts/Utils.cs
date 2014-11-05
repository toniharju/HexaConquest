using UnityEngine;
using System.Collections;

public class Utils : MonoBehaviour {

	public static GameObject InstantiateLocal( GameObject parent, Object original, Vector3 position, Quaternion rotation, Vector3 scale ) {

		GameObject temp = Instantiate ( original, Vector3.zero, Quaternion.identity ) as GameObject;
		temp.transform.parent = parent.transform;
		temp.transform.localPosition = position;
		temp.transform.localRotation = rotation;
		temp.transform.localScale = scale;

		return temp;

	}

	public static GameObject InstantiateLocal( GameObject parent, string original, Vector3 position, Quaternion rotation, Vector3 scale ) {
		
		GameObject temp = Instantiate ( Resources.Load< GameObject >( original ), Vector3.zero, Quaternion.identity ) as GameObject;
		temp.transform.parent = parent.transform;
		temp.transform.localPosition = position;
		temp.transform.localRotation = rotation;
		temp.transform.localScale = scale;
		
		return temp;
		
	}

}
