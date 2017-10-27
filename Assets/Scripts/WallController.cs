using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    public Vector3 currentRot;
    public bool placeStatus;

    public Transform Wall_obj;

	// Use this for initialization
	void Start ()
    {
        currentRot.x = transform.rotation.x;
        currentRot.y = transform.rotation.y;
        currentRot.z = transform.rotation.z;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown("space"))
        {
            placeStatus = true;
        }
	}
}
