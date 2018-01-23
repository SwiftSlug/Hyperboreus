using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trackingSphereScript : MonoBehaviour {

    Collider trackingCollider;

    //  Holds a reference to the player who fired the syringe
    public GameObject player;
    GameObject targetPlayer;
    Rigidbody rb;

    public float blep;

    public float trackingForce = 100.0f;

    // Use this for initialization
    void Start () {
        trackingCollider = GetComponent<Collider>();
        rb = transform.parent.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {

        if (targetPlayer)
        {
            //Debug.Log("Tracking Player");

            Vector3 vectorToTarget = targetPlayer.transform.position - transform.position;
            //vectorToTarget = Vector3.Scale(vectorToTarget, rb.velocity);

            //Vector3 vectorAddition = vectorToTarget * trackingForce;

            //Vector3 breakingForce;
            //breakingForce.x = rb.velocity.x * 0.05f;
            //breakingForce.y = rb.velocity.y * 0.05f;
            //breakingForce.z = rb.velocity.z * 0.05f;

            //Vector3 newVelocity = Vector3.Lerp(rb.velocity, vectorToTarget, 0.3f);

            //Vector3 newVelocity = Vector3.Lerp(transform.position, targetPlayer.transform.position, 0.3f);

            //rb.velocity = new Vector3(0, 0, 0);
            
            transform.parent.position = Vector3.Lerp(transform.position, targetPlayer.transform.position, 0.2f);

            //rb.velocity = newVelocity;




            //rb.AddForce(breakingForce);
            //rb.AddForce(vectorAddition);
        }

	}

    void OnTriggerEnter(Collider other)
    {

        //Debug.Log(other.tag);

        if (other.CompareTag("NetworkedPlayer") && (other.gameObject))
        {
            if (other.gameObject != player)
            {
                //Debug.Log("Tracking sphere hit player");
                transform.parent.GetComponent<CapsuleCollider>().enabled = false;
                targetPlayer = other.gameObject;
            }
        }


    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NetworkedPlayer") && (other.gameObject))
        {
            targetPlayer = null;
        }
    }

}
