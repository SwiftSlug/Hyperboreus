﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class NetworkUpdate : NetworkBehaviour {

    [SyncVar]
    public Vector3 realPosition = Vector3.zero;
    [SyncVar]
    Quaternion realRotation;

    NavMeshAgent navMesh;
    public float updateInterval;

	// Use this for initialization
	void Start () {
        navMesh = GetComponent<NavMeshAgent>();

        //realPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        if (isLocalPlayer)
        {

            //realPosition = navMesh.destination;
            Debug.Log("Is local player");
            updateInterval += Time.deltaTime;
            if (updateInterval > 0.11f) // 9 times per second
            {
                updateInterval = 0;
                Debug.Log("Update should run");
                CmdSync(transform.position, transform.rotation);

            }
            
        }
        else
        {
            Debug.Log("Not local");
            transform.position = Vector3.Lerp(transform.position, realPosition, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.1f);
        }
                
    }

    [Command]
    void CmdSync(Vector3 position, Quaternion rotation)
    {

        //Debug.Log("RealPosition set on server");

        realPosition = position;
        realRotation = rotation;
        //transform.position = position;
        //transform.rotation = rotation;
    }
}
