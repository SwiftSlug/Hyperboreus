using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BarbedWireTrap : NetworkBehaviour {

    GameObject CollidedEnemy = null;
    float Timer = 0.0f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (CollidedEnemy != null)
        {
            if (Timer >= 1.0f)
            {
                CollidedEnemy.GetComponent<AIStats>().CmdDamage(5);
                Timer = 0.0f;
            }
            else
            {
                Timer = Timer + Time.deltaTime;
            }
        }
	}

    private void OnCollisionEnter(Collision collidedAsset)
    {
        if(collidedAsset.gameObject.CompareTag("Enemy") == true)
        {
            CollidedEnemy = collidedAsset.gameObject;
            CollidedEnemy.gameObject.GetComponent<StateController>().runSpeed = 1;
            CollidedEnemy.gameObject.GetComponent<StateController>().walkSpeed = 1;
        }
    }
    private void OnCollisionExit(Collision collidedAsset)
    {
        if (collidedAsset.gameObject.CompareTag("Enemy") == true)
        {
            CollidedEnemy.gameObject.GetComponent<StateController>().runSpeed = 1;
            CollidedEnemy.gameObject.GetComponent<StateController>().walkSpeed = 1;
            collidedAsset = null;
        }
    }
    void DestroyTrap()
    {
        CollidedEnemy.gameObject.GetComponent<StateController>().runSpeed = 1;
        CollidedEnemy.gameObject.GetComponent<StateController>().walkSpeed = 1;
        CollidedEnemy = null;
        Destroy(gameObject);
    }
}
