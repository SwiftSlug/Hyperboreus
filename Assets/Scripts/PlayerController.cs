using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
	Vector3 playerOrientation; //Player Orientation

	//Update is called once per frame
	void Update()
	{
		if (!isLocalPlayer)
		{
			return;
		}

		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition); //Cast a ray from the main camera relative to the screen position

		RaycastHit mouseRayHit; //Reference to the ray hit - used in calculating player orientation

		if (Physics.Raycast(mouseRay, out mouseRayHit, 100)) //If the Ray has hit an object
		{
			playerOrientation = mouseRayHit.point; //Make our potential player orientation equal to the location we hit with the ray cast.
		}

		Vector3 playerDirection = playerOrientation - transform.position; //Save a vector based on our players position and the saved orientation
		playerDirection.y = 0; //Get rid of our y portion of the vector because we don't need it

		float xAxis = Input.GetAxis("Horizontal") * Time.deltaTime * 6.0f; //Get the input horizontally and save it. "W,S,Up,Down,Joystick Up,Joystick Down"
		float yAxis = Input.GetAxis("Vertical") * Time.deltaTime * 6.0f; //Get the vertical input and and save it. "A,D,Left,Right,Joystick Left, Joystick Right"

		transform.LookAt(transform.position + playerDirection, Vector3.up); //Look at the mouse position in screen space
		transform.Translate(Vector3.forward * yAxis, Space.World); //Move horizontally within world space instead of local
		transform.Translate(Vector3.right * xAxis, Space.World); //Move vertically within world space instead of local
	}
}