using UnityEngine;

public class CameraTest : MonoBehaviour {
	//Players correctly obeserve the same camera, however the transform does not set correctly for each individual.
	public GameObject Player; //Changed this aswell
	Vector3 heightOffset;
	Vector3 verticalOffset;

	// Use this for initialization
	void Start ()
	{
		//Player = GameObject.FindGameObjectWithTag("NetworkedPlayer");//Potential network problem here because it only finds the one with the tag
		heightOffset = new Vector3(0.0f, 12.0f, 0.0f);
		verticalOffset = new Vector3(0.0f, 0.0f, -2.0f);
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		this.transform.position = (Player.transform.position + heightOffset + verticalOffset);
	}
}