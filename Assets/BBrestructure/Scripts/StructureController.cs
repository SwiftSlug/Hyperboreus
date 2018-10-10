using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StructureController : NetworkBehaviour
{
    //Hitpoint value is set on a per prefab basis (Needs to be set on the base prefab to be set on all)
    public int hitpoints;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    [ClientRpc]
    public void RpcSetupSpawn()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    //Subtracts from hitpoints of the structure. If this reaches 0 or below then the structure is destroyed.
    [Command]
    public void CmdDamage(int amount)
    {
        if (!isServer)
        {
            return;
        }

        hitpoints -= amount;

        if (hitpoints <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
