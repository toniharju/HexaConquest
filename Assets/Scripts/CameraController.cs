using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float Velocity = 10;
	public float ZoomVelocity = 100;

	private Vector3 clickMousePosition;

	// Use this for initialization
	void Start () {
	
		transform.position = new Vector3( 0, 5, 0 );
		transform.rotation = Quaternion.Euler ( 45, 315, 0 );

	}
	
	// Update is called once per frame
	void LateUpdate () {
	
		float horizontal = Input.GetAxis ( "Horizontal" );
		float vertical = Input.GetAxis ( "Vertical" );
		float scroll = Input.GetAxis ( "Mouse ScrollWheel" );

		if( vertical != 0 ) {

			transform.localPosition = transform.localPosition + ( Quaternion.Euler( 0, -45, 0 ) * new Vector3( 0, 0, vertical * Time.deltaTime * Velocity ) );

		}

		if( horizontal != 0 ) {
			
			transform.localPosition = transform.localPosition + ( Quaternion.Euler( 0, -45, 0 ) * new Vector3( horizontal * Time.deltaTime * Velocity, 0 ) );
			
		}

		if( scroll != 0 ) {

			transform.Translate ( 0, 0, scroll * Time.deltaTime * ZoomVelocity );

		}

	}

}
