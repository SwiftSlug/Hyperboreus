using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalSyringeScript : MonoBehaviour {

    Collider sphereCollider;
    Rigidbody rb;

    public float speed = 600.0f;

	// Use this for initialization
	void Start () {
        Debug.Log("Syringe Spawned !");

        sphereCollider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * speed);
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter(Collider other)
    {
        if (other.GetType() == typeof(NetworkPlayer))
        {
            Debug.Log("Blep");
        }

        
        //Destroy(other.gameObject);
    }

}
