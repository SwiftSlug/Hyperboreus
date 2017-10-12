using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
	Vector3 playerOrientation;
	public Camera myCamera;

	//Update is called once per frame
	void Update()
	{
		if (!this.isLocalPlayer)
		{
			myCamera.enabled = false;
			return;
		}

		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit mouseRayHit;

		if (Physics.Raycast(mouseRay, out mouseRayHit, 100))
		{
			playerOrientation = mouseRayHit.point;
		}

		Vector3 playerDirection = playerOrientation - transform.position;
		playerDirection.y = 0;

		var xAxis = Input.GetAxis("Horizontal") * 0.1f;
		var yAxis = Input.GetAxis("Vertical") * 0.1f;

		transform.LookAt(transform.position + playerDirection, Vector3.up);
		transform.Translate(Vector3.forward * yAxis, Space.World);
		transform.Translate(Vector3.right * xAxis, Space.World);
	}
}