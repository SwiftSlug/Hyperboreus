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

    public int damageValue = 150;

    List<GameObject> enemiesHit;

    public bool sphereDrawDebug = true;       //Debug flag for drawing debug spheres

    float distanceFrom;

    float distanceMultiplier;

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
        Collider[] explosionHits = Physics.OverlapSphere(missileCollider.transform.position, blastRadius);

        for (int i = 0; i < explosionHits.Length; i++)
        {
            if (explosionHits[i].CompareTag("Enemy"))
            {
                distanceFrom = (missileCollider.transform.position - explosionHits[i].transform.position).magnitude;
                distanceMultiplier = 1 - (distanceFrom / blastRadius);
                explosionHits[i].GetComponent<AIStats>().CmdDamage(damageValue * Mathf.RoundToInt(distanceMultiplier));
            }
        }

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
