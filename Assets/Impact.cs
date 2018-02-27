using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Impact : NetworkBehaviour
{
    public Collider missileCollider;

    Rigidbody missileRB;

    public float speed;

    // Use this for initialization
    void Start()
    {
        missileCollider = transform.parent.GetComponent<Collider>();
        missileRB = transform.parent.GetComponent<Rigidbody>();

        missileRB.AddForce((-(transform.up)) * speed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime, Space.World);
    }

    void OnTriggerEnter()
    {


        if (Physics.SphereCast(p1, 10, transform.forward, out hit, 10))
        {
            distanceToObstacle = hit.distance;
        }

        Destroy(transform.parent.gameObject);
        NetworkServer.Destroy(transform.parent.gameObject);
    }

    [Command]
    void CmdDestroyMissile(GameObject missile)
    {
        NetworkServer.Destroy(missile);
        RpcDestroyMissile(missile);
    }

    [ClientRpc]
    void RpcDestroyMissile(GameObject missile)
    {
        Destroy(missile);
    }
}
