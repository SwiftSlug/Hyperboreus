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
   
    private int NeededMaterial;

    public GameObject SpawnedStructure;

    //public GameObject wall_obj;

    //public Transform Wall_obj;
    //public Transform Stairs_obj;
    //public Transform Doorway_obj;
    //public Transform Door_obj;
    //public Transform Floor_obj;
    //public Transform Ceiling_obj;
    //public Transform Ramps_obj;
    //public Transform BarbedWireTrap_obj;

    public int hitpoints = 100;
    public int structureSelector = 0;

    // Use this for initialization
    void Start ()
    {
        ////disable colliders for placement actor choices
        //Wall_obj.GetComponent<Collider>().enabled = false;
        //Stairs_obj.GetComponent<Collider>().enabled = false;
        //Doorway_obj.GetComponent<Collider>().enabled = false;
        //Door_obj.GetComponent<Collider>().enabled = false;
        //Floor_obj.GetComponent<Collider>().enabled = false;
        //Ceiling_obj.GetComponent<Collider>().enabled = false;
        //Ramps_obj.GetComponent<Collider>().enabled = false;
        //BarbedWireTrap_obj.GetComponent<Collider>().enabled = false;

        ////disable renderer for all except 
        //Stairs_obj.GetComponent<Renderer>().enabled = false;
        //Doorway_obj.GetComponent<Renderer>().enabled = false;
        //Door_obj.GetComponent<Renderer>().enabled = false;
        //Floor_obj.GetComponent<Renderer>().enabled = false;
        //Ceiling_obj.GetComponent<Renderer>().enabled = false;
        //Ramps_obj.GetComponent<Renderer>().enabled = false;
        //BarbedWireTrap_obj.GetComponent<Renderer>().enabled = false;
    }

    [Command]
    void CmdSpawnStructure()
    {
        StructurePrefab = (GameObject)Instantiate(StructurePrefab, Object.position, Object.rotation);
        //StructurePrefab.GetComponent<WallController>().SetMaterial(NeededMaterial);
        NetworkServer.Spawn(StructurePrefab);
    }

    /*[Command]
    void CmdSetMaterial(Material Mat)
    {
        StructurePrefab.GetComponent<WallController>().SetMaterial(Mat);

    }*/
	
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
            NeededMaterial = 0;
            //StructurePrefab.GetComponentInChildren<Renderer>().material = NeededMaterial;
            StructurePrefab.GetComponent<WallController>().RpcSetMaterial(NeededMaterial);
            hitpoints = 100;

            Debug.Log("ChangingToWood");
        }
        if (Input.GetKeyDown("2") && (StructurePrefab.GetComponent<WallController>().placeStatus != true))
        {
            NeededMaterial = 1;
            //StructurePrefab.GetComponentInChildren<Renderer>().material = NeededMaterial;
            StructurePrefab.GetComponent<WallController>().RpcSetMaterial(NeededMaterial);
            hitpoints = 200;

            Debug.Log("ChangingToStone");
        }
        if (Input.GetKeyDown("3") && (StructurePrefab.GetComponent<WallController>().placeStatus != true))
        {
            NeededMaterial = 2;
            StructurePrefab.GetComponent<WallController>().RpcSetMaterial(NeededMaterial);
            //StructurePrefab.GetComponentInChildren<Renderer>().material = NeededMaterial;
            hitpoints = 300;

            Debug.Log("ChangingToMetal");
        }

       //switch (structureSelector)
       // {
       //     case 0:
       //         Wall_obj.GetComponent<BoxCollider>().enabled = true;
       //         Wall_obj.GetComponent<Renderer>().enabled = true;

       //         BarbedWireTrap_obj.GetComponent<BoxCollider>().enabled = false;
       //         BarbedWireTrap_obj.GetComponent<Renderer>().enabled = false;

       //         //if (Input.GetKeyDown("1") && (placeStatus != true))
       //         //{
       //         //    cube_obj.GetComponent<Renderer>().material = wood;
       //         //    hitpoints = 100;
       //         //}
       //         //if (Input.GetKeyDown("2") && (placeStatus != true))
       //         //{
       //         //    cube_obj.GetComponent<Renderer>().material = stone;
       //         //    hitpoints = 150;
       //         //}
       //         //if (Input.GetKeyDown("3") && (placeStatus != true))
       //         //{
       //         //    cube_obj.GetComponent<Renderer>().material = metal;
       //         //    hitpoints = 200;
       //         //}
       //         break;

       //     case 1:
       //         Stairs_obj.GetComponent<BoxCollider>().enabled = true;
       //         Stairs_obj.GetComponent<Renderer>().enabled = true;

       //         Wall_obj.GetComponent<BoxCollider>().enabled = false;
       //         Wall_obj.GetComponent<Renderer>().enabled = false;
       //         break;

       //     case 2:
       //         Doorway_obj.GetComponent<BoxCollider>().enabled = true;
       //         Doorway_obj.GetComponent<Renderer>().enabled = true;

       //         Stairs_obj.GetComponent<BoxCollider>().enabled = false;
       //         Stairs_obj.GetComponent<Renderer>().enabled = false;
       //         break;

       //     case 3:
       //         Door_obj.GetComponent<BoxCollider>().enabled = true;
       //         Door_obj.GetComponent<Renderer>().enabled = true;

       //         Doorway_obj.GetComponent<BoxCollider>().enabled = false;
       //         Doorway_obj.GetComponent<Renderer>().enabled = false;
       //         break;

       //     case 4:
       //         Floor_obj.GetComponent<BoxCollider>().enabled = true;
       //         Floor_obj.GetComponent<Renderer>().enabled = true;

       //         Door_obj.GetComponent<BoxCollider>().enabled = false;
       //         Door_obj.GetComponent<Renderer>().enabled = false;
       //         break;

       //     case 5:
       //         Ceiling_obj.GetComponent<BoxCollider>().enabled = true;
       //         Ceiling_obj.GetComponent<Renderer>().enabled = true;

       //         Floor_obj.GetComponent<BoxCollider>().enabled = false;
       //         Floor_obj.GetComponent<Renderer>().enabled = false;
       //         break;

       //     case 6:
       //         Ramps_obj.GetComponent<BoxCollider>().enabled = true;
       //         Ramps_obj.GetComponent<Renderer>().enabled = true;

       //         Ceiling_obj.GetComponent<BoxCollider>().enabled = false;
       //         Ceiling_obj.GetComponent<Renderer>().enabled = false;
       //         break;

       //     case 7:
       //         BarbedWireTrap_obj.GetComponent<BoxCollider>().enabled = true;
       //         BarbedWireTrap_obj.GetComponent<Renderer>().enabled = true;

       //         Ramps_obj.GetComponent<BoxCollider>().enabled = false;
       //         Ramps_obj.GetComponent<Renderer>().enabled = false;
       //         break;
       // }

        if (Input.GetKeyDown("=") && (StructurePrefab.GetComponent<WallController>().placeStatus != true))
        {
            StructurePrefab.GetComponent<WallController>().ActiveStructure = structureSelector;
            structureSelector++;


            if (structureSelector == 8)
            {
                structureSelector = 0;
            }

            //Debug.Log(structureSelector);
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
