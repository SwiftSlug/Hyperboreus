using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Impact : NetworkBehaviour
{
    public Collider missileCollider;

    Rigidbody missileRB;

    public float blastRadius = 10;

    public float speed;

    public bool loser;

    List<GameObject> enemiesHit;

    public bool sphereDrawDebug = true;       //Debug flag for drawing debug spheres

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
        //Collider[] hitColliders = Physics.OverlapSphere(missileCollider., blastRadius);
        //ContactPoint contact = collision.contacts[0];
        //Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        //Vector3 pos = contact.point;
        //Instantiate(explosionPrefab, pos, rot);

        Debug.Log("explode");

        Destroy(transform.parent.gameObject);
        NetworkServer.Destroy(transform.parent.gameObject);
    }

    //[Command]
    //void CmdDestroyMissile(GameObject missile)
    //{
    //    NetworkServer.Destroy(missile);
    //    RpcDestroyMissile(missile);
    //}

    //[ClientRpc]
    //void RpcDestroyMissile(GameObject missile)
    //{
    //    Destroy(missile);
    //}
}
