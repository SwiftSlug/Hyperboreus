using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Impact : NetworkBehaviour
{
    public Collider missileCollider;    // Reference for impact collider

    public Rigidbody missileRB;         // Reference for missile rigid body

    public float blastRadius = 100;     // Value for missile explosion blast radius

    public float speed;                 // Speed that the missile go towards the ground

    public int damageMultiplier = 100;  // How much damage is dealt if an enemy is hit directly

    List<GameObject> enemiesHit;        // List to contain all enemies within the missile blast radius

    public bool sphereDrawDebug = true; // Debug flag for drawing debug spheres

    float distanceFrom;                 // Distance that the enemy is from the centre of the explosion

    float distanceMultiplier;           // Multiplier value that reduces damage based on enemies distance from the centre of blast

    float damageValue;                  // Final damagen value dealt to enemy after calculations

    public ParticleSystem explosion;    // Particle system for initial explosion

    // Use this for initialization
    void Start()
    {
        //missileCollider = transform.parent.GetComponent<Collider>();

        //
        missileRB = transform.parent.GetComponent<Rigidbody>();

        // Add force value to send missile down at the given speed value
        missileRB.AddForce((-(transform.up)) * speed);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //
    void OnTriggerEnter()
    {
        // Find all hit within explosion blast radius from the centre of the missile collider
        Collider[] explosionHits = Physics.OverlapSphere(missileCollider.transform.position, blastRadius);

        // Check through each collider hit by overlap sphere
        for (int i = 0; i < explosionHits.Length; i++)
        {
            // For each collider, check if it's an enemy
            if (explosionHits[i].CompareTag("Enemy"))
            {
                // Calculate the distance of the enemy from the explosion centre
                // distanceFrom = position of missile - enemy position 
                // Magnitude to get float value from vector3 positions
                distanceFrom = (missileCollider.transform.position - explosionHits[i].transform.position).magnitude;
                //Debug.Log(distanceFrom);

                // Calculate value between 1 and 0 (1 being at the centre of the explosion and 0 farthest away)
                // distanceMultiplier = 1 - (distance from blast radius / size of blast radius)
                // e.g. 1 - (20 / 100) == 1 - 0.2 == 0.8
                distanceMultiplier = (1 - (distanceFrom / blastRadius));
                //Debug.Log(distanceMultiplier);

                // Final damage value calculation betweem the damageMultiplier and distanceMultiplier, to get a percentage of the core damage value
                // e.g. damageValue = 100 * 0.8 = 80
                damageValue = damageMultiplier * distanceMultiplier;
                //Debug.Log(damageValue);

                // Damage the above value to enemy
                // Damage value to enemy must be int value so round from float
                explosionHits[i].GetComponent<AIStats>().CmdDamage(Mathf.RoundToInt(damageValue));
                //Debug.Log(Mathf.RoundToInt(damageValue));
            }
        }

        //CmdExplode();
        //Debug.Log("explode");

        // Destroy missile object
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
