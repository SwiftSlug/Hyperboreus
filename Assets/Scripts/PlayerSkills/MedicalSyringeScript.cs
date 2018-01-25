using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MedicalSyringeScript : NetworkBehaviour {

    //Collider sphereCollider;
    //public Collider trackingCollider;


    public Collider physicsCollider;

    Rigidbody rb;

    //public bool blep;

    //  Holds a reference to the player who fired the syringe
    public GameObject player;

    public float speed;

    //public bool blep1;
    //public bool blep2;
    //public bool blep3;


    // Use this for initialization
    void Start () {
        //Debug.Log("Syringe Spawned !");

        //sphereCollider = GetComponent<SphereCollider>();
        physicsCollider = transform.parent.GetComponent<Collider>();


        rb = transform.parent.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed);
        Invoke("enableCollisions", 0.5f);
	}

    void enableCollisions()
    {
        if (physicsCollider)
        {
            physicsCollider.enabled = true;
        }
    }

    // Update is called once per frame
    void Update () {

	}

    void OnTriggerEnter(Collider other)
    {        

        if (other.CompareTag("NetworkedPlayer") && (other.gameObject))
        {
            //  Ensure that the syringe does not trigger off the firing player
            if (other.gameObject != player)
            {
                //Debug.Log("Hit Inner Layer, healing");
                other.GetComponent<PlayerStats>().CmdHeal(50);
                //this.enabled = false;

                Destroy(transform.parent.gameObject);
                NetworkServer.Destroy(transform.parent.gameObject);

                //CmdDestroySyringe(transform.parent.gameObject);
            }
        }        
        
    }
    [Command]
    void CmdDestroySyringe(GameObject syringe)
    {        
        NetworkServer.Destroy(syringe);
        RpcDestroySyringe(syringe);
    }
    [ClientRpc]
    void RpcDestroySyringe(GameObject syringe)
    {
        Destroy(syringe);
    }

}
