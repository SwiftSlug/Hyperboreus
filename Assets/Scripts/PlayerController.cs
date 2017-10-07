using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
	//Use this for initialization
	void Start()
	{
		attachCamera();
	}
	
	//Update is called once per frame
	[Client]
	void Update()
	{
		if(!this.isLocalPlayer)
		{
			return;
		}

		var xAxis = Input.GetAxis("Horizontal") * 1.5f;
		var yAxis = Input.GetAxis("Vertical") * 0.1f;

		transform.Rotate(0, xAxis, 0);
		transform.Translate(0, 0, yAxis);
	}

	void attachCamera()
	{
		if(this.isLocalPlayer)
		{
			GameObject.FindGameObjectWithTag("MainCamera").transform.parent = this.transform; //Attach the MainCamera as a child to the player
			GameObject.FindGameObjectWithTag("MainCamera").transform.position = transform.Find("DummyCam").position; //Transform the position of the MainCamera to the DummyCam
		}
	}
}
