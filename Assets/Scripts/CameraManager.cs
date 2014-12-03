using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

    public Vector2 ZoomClamp = new Vector2( 2, 5 );

    public Vector3 Position = new Vector3( -10, 5, 10 );
    public Vector3 Rotation = new Vector3( 45, 135, 0 );
    public Vector3 Velocity = new Vector3( 10, 10, 100 );

	// Use this for initialization
	void Start () {


        Camera.main.transform.position = Position;
        Camera.main.transform.rotation = Quaternion.Euler( Rotation );

	}
	
	// Update is called once per frame
	void Update () {

		float horizontal = -Input.GetAxis( "Horizontal" );
		float vertical = -Input.GetAxis( "Vertical" );

		if( horizontal != 0 || vertical != 0 ) {

			Camera.main.transform.localPosition = Camera.main.transform.localPosition + Quaternion.Euler( 0, -45, 0 ) * ( new Vector3( Velocity.x * horizontal * Time.deltaTime, 0, Velocity.y * vertical * Time.deltaTime ) );

		}

        if( Input.GetMouseButton( 2 ) ) {
            
            float dx = Input.GetAxisRaw( "Mouse X" );
            float dy = Input.GetAxisRaw( "Mouse Y" );

            Camera.main.transform.localPosition = Camera.main.transform.localPosition + Quaternion.Euler( 0, -45, 0 ) * ( new Vector3( Velocity.x * dx * Time.deltaTime, 0, Velocity.y * dy * Time.deltaTime ) );

        }

        float dz = Input.GetAxisRaw( "Mouse ScrollWheel" );
        if( dz != 0 ) {

            Vector3 temp = Camera.main.transform.localPosition + Camera.main.transform.forward * Velocity.z * dz * Time.deltaTime;

            if( temp.y > ZoomClamp.x && temp.y < ZoomClamp.y ) Camera.main.transform.localPosition = temp;

        }

		Vector3 pos = Camera.main.transform.localPosition;

		if( pos.z < 4 ) pos.z = 4;
		if( pos.x > -4 ) pos.x = -4;
		if( pos.z > 20 ) pos.z = 20;
		if( pos.x < -15 ) pos.x = -15;

		Camera.main.transform.localPosition = pos;

	}

}
