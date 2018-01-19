using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalSyringeScript : MonoBehaviour {

    Collider sphereCollider;
    public Collider trackingCollider;

    Rigidbody rb;

    //  Holds a reference to the player who fired the syringe
    public GameObject player;

    public float speed = 600.0f;

	// Use this for initialization
	void Start () {
        //Debug.Log("Syringe Spawned !");

        sphereCollider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * speed);
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>() )
        {
            Debug.Log("Hit sphereCollider");
        }
        if (other.GetComponent<Collider>() == trackingCollider)
        {
            Debug.Log("Hit tracking collider");
        }

        if (other.CompareTag("NetworkedPlayer") && (other.gameObject))
        {            
            if (other.gameObject != player)
            {
                other.GetComponent<PlayerStats>().CmdHeal(50);
                Debug.Log("Blep");
            }
        }
        
    }
}
