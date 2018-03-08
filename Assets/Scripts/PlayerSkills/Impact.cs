using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Impact : NetworkBehaviour
{
    public Collider missileCollider;

    Rigidbody missileRB;

    public float blastRadius = 50;

    public float speed;

    public bool loser;

    public int damageMultiplier = 100;

    List<GameObject> enemiesHit;

    public bool sphereDrawDebug = true;       //Debug flag for drawing debug spheres

    float distanceFrom;

    float distanceMultiplier;

    float damageValue;

    public ParticleSystem explosion;

    // Use this for initialization
    void Start()
    {
        //missileCollider = transform.parent.GetComponent<Collider>();
        missileRB = transform.parent.GetComponent<Rigidbody>();

        missileRB.AddForce((-(transform.up)) * speed);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter()
    {
        Collider[] explosionHits = Physics.OverlapSphere(missileCollider.transform.position, blastRadius);

        for (int i = 0; i < explosionHits.Length; i++)
        {
            if (explosionHits[i].CompareTag("Enemy"))
            {
                distanceFrom = (missileCollider.transform.position - explosionHits[i].transform.position).magnitude;
                //Debug.Log(distanceFrom);

                distanceMultiplier = (1 - (distanceFrom / blastRadius));
                //Debug.Log(distanceMultiplier);

                damageValue = damageMultiplier * distanceMultiplier;
                //Debug.Log(damageValue);

                explosionHits[i].GetComponent<AIStats>().CmdDamage(Mathf.RoundToInt(damageValue));
                //Debug.Log(Mathf.RoundToInt(damageValue));
            }
        }

        //CmdExplode();
        //Debug.Log("explode");

        Destroy(transform.parent.gameObject);
        NetworkServer.Destroy(transform.parent.gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(missileCollider.transform.position, blastRadius);
    }

    [Command]
    public void CmdExplode()
    {
        if (isServer)
        {
            explosion.Play();
            RpcExplode();
        }
    }

    [ClientRpc]
    public void RpcExplode()
    {
        explosion.Play();
    }
}
