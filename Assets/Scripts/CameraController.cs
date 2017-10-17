using UnityEngine;
using UnityEngine.Networking;

public class CameraController : NetworkBehaviour
{
    public GameObject myCamera; //Camera Reference
    Vector3 heightOffset; //Camera height from player
    Vector3 verticalOffset; //Camera vertical alignment from player

    void Start ()
    {
		if(isLocalPlayer)
        {
            myCamera = GameObject.FindGameObjectWithTag("MainCamera"); //Set a reference to the player
            heightOffset = new Vector3(0.0f, 12.0f, 0.0f); //Give our camera a certain height to offset - could be exposed to editor
            verticalOffset = new Vector3(0.0f, 0.0f, -2.0f); //Give our camera a certain vertical offset - could be exposed to editor
        }
	}

    //Called after the scene has been updated
    void LateUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        //Make sure our camera is following the player, smoothly move it.
        myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, this.transform.position + heightOffset + verticalOffset, Time.deltaTime * 2.0f); 
    }
}
