using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BaseBuildingPlayerScript : NetworkBehaviour
{

    public Transform Object;
    public GameObject StructurePrefab;

    public Material metal;
    public Material stone;
    public Material wood;

    /*public Transform cube_obj;
    public Transform Stairs_obj;
    public Transform Doorway_obj;
    public Transform Door_obj;
    public Transform Floor_obj;
    public Transform Ceiling_obj;
    public Transform Ramps_obj;
    public Transform BarbedWireTrap_obj;*/

    // Use this for initialization
    void Start () {
		
	}

    [Command]
    void CmdSpawnStructure()
    {
        StructurePrefab = (GameObject)Instantiate(StructurePrefab, Object.position, Object.rotation);
        NetworkServer.Spawn(StructurePrefab);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!isLocalPlayer)
        {
            return;
        }

		if (Input.GetKeyDown ("b"))
        {
            CmdSpawnStructure();

            Debug.Log("build mode engaged!!!");
        }

        if (Input.GetKeyDown("1") && (StructurePrefab.GetComponent<WallController>().placeStatus != true))
        {
            StructurePrefab.GetComponentInChildren<Renderer>().material = wood;

            Debug.Log("ChangingToWood");
        }
        if (Input.GetKeyDown("2") && (StructurePrefab.GetComponent<WallController>().placeStatus != true))
        {
            StructurePrefab.GetComponentInChildren<Renderer>().material = stone;

            Debug.Log("ChangingToStone");
        }
        if (Input.GetKeyDown("3") && (StructurePrefab.GetComponent<WallController>().placeStatus != true))
        {
            StructurePrefab.GetComponentInChildren<Renderer>().material = metal;

            Debug.Log("ChangingToMetal");
        }
    }

    private void OnTriggerEnter(Collider CollidedAsset)    
    {
        BoxCollider boxColliderComponent = GetComponent<BoxCollider>();

       if (CollidedAsset.CompareTag("GridCollider"))
       {
            Debug.Log("ColliderHit!");
            if (StructurePrefab.GetComponent<WallController>().placeStatus != true)
            {
                StructurePrefab.GetComponent<Transform>().position = new Vector3(CollidedAsset.transform.position.x, CollidedAsset.transform.position.y, CollidedAsset.transform.position.z);
            }
       }
    }
}
