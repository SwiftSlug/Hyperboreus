using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerBuildingController : NetworkBehaviour
{
    public bool InbuildMode = false;
    public Transform PointToSpawnStructure;
    public GameObject PlayerReference;
    public GameObject StructureSpawnerRef;
    public GameObject TempStructureGuide;
    public GameObject NetworkSpawnedStructure;

    public AudioSource audioSource;
    public AudioClip clipBuildMode;
    public AudioClip clipCombatMode;
    public AudioClip clipBuildStructure;

    public bool blep = true;

    public int StructureNeeded = 0;
    public int MaterialNeeded = 0;
    public int RotationNeeded = 0;
    public float RotationToSet = 0;

    public void EnterOrExitBuildMode()
    {
        if (InbuildMode == true)
        {
            InbuildMode = false;

            audioSource.PlayOneShot(clipCombatMode, 1.0f);

            DestroyObject(TempStructureGuide);
            TempStructureGuide = null;
        }
        else
        {
            InbuildMode = true;

            audioSource.PlayOneShot(clipBuildMode, 1.0f);

            TempStructureGuide = Instantiate(StructureSpawnerRef, PointToSpawnStructure.position, PointToSpawnStructure.rotation);
            TempStructureGuide.transform.eulerAngles = new Vector3(0, RotationToSet, 0);
            TempStructureGuide.GetComponent<BuildingController>().LocalSetMaterialAndStructure(StructureNeeded, MaterialNeeded);
        }
    }

    [Command]
    void CmdPlaceStructure()
    {
        audioSource.PlayOneShot(clipBuildStructure, 1.0f);

        NetworkSpawnedStructure = (GameObject)Instantiate(TempStructureGuide, PointToSpawnStructure.position, TempStructureGuide.transform.rotation); //GetComponent<Transform>().rotation);
        NetworkServer.Spawn(NetworkSpawnedStructure);
        NetworkSpawnedStructure.GetComponent<BuildingController>().RpcSetMaterialAndStructure(StructureNeeded, MaterialNeeded);
        NetworkSpawnedStructure.GetComponent<BuildingController>().StructurePlaced = true;
    }

    [Command]
    void CmdRotateStructure(float Rotation)
    {
        TempStructureGuide.transform.eulerAngles = new Vector3(0, Rotation, 0);
    }
    void LocalRotateStructure(float Rotation)
    {
        TempStructureGuide.GetComponent<BuildingController>().LocalSetRotation(Rotation);
    }

    //[Command]
    //void CmdRotateStructure()
    //{
    //    TempStructureGuide.transform.Rotate(0, 10, 0);
    //}
    /* [Command]
     void CmdRotateStructure()
     {
             switch (RotationNeeded)
             {
                 case 0:
                     TempStructureGuide.transform.eulerAngles = new Vector3(0, 45, 0);
                     break;
                 case 1:
                     TempStructureGuide.transform.eulerAngles = new Vector3(0, 90, 0);
                     break;
                 case 2:
                     TempStructureGuide.transform.eulerAngles = new Vector3(0, 135, 0);
                     break;
                 case 3:
                     TempStructureGuide.transform.eulerAngles = new Vector3(0, 180, 0);
                     break;
                 case 4:
                     TempStructureGuide.transform.eulerAngles = new Vector3(0, 225, 0);
                     break;
                 case 5:
                     TempStructureGuide.transform.eulerAngles = new Vector3(0, 270, 0);
                     break;
                 case 6:
                     TempStructureGuide.transform.eulerAngles = new Vector3(0, 315, 0);
                 RotationNeeded = -1;
                     break;
             }
         RotationNeeded = RotationNeeded + 1;
     }*/
    /*void RotateStructure()
    {
        TempStructureGuide.transform.Rotate(0, 10, 0);
    }*/

    [Command]
    void CmdSelectStructure()
    {
        if (StructureNeeded == 2)
        {
            StructureNeeded = 0;
            //TempStructureGuide.GetComponent<BuildingController>().RpcSetMaterialAndStructure(StructureNeeded, MaterialNeeded);
        }
        else
        {
            StructureNeeded = StructureNeeded + 1;
            //TempStructureGuide.GetComponent<BuildingController>().RpcSetMaterialAndStructure(StructureNeeded, MaterialNeeded);
        }
    }
    [Command]
    void CmdSelectMaterial()
    {
        if (MaterialNeeded == 2)
            {
                MaterialNeeded = 0;
                //TempStructureGuide.GetComponent<BuildingController>().RpcSetMaterialAndStructure(StructureNeeded, MaterialNeeded);
               // LocalSetMaterialAndStructure(StructureNeeded, MaterialNeeded);
        }
        else
            {
                MaterialNeeded = MaterialNeeded + 1;
                //TempStructureGuide.GetComponent<BuildingController>().RpcSetMaterialAndStructure(StructureNeeded, MaterialNeeded);
            // LocalSetMaterialAndStructure(StructureNeeded, MaterialNeeded);
        }
    }

    void LocalSelectStructure()
    {
        if (StructureNeeded == 2)
        {
            StructureNeeded = 0;
            TempStructureGuide.GetComponent<BuildingController>().LocalSetMaterialAndStructure(StructureNeeded, MaterialNeeded);
        }
        else
        {
            StructureNeeded = StructureNeeded + 1;
            TempStructureGuide.GetComponent<BuildingController>().LocalSetMaterialAndStructure(StructureNeeded, MaterialNeeded);
        }
    }
    void LocalSelectMaterial()
    {
        if (MaterialNeeded == 2)
        {
            MaterialNeeded = 0;
            TempStructureGuide.GetComponent<BuildingController>().LocalSetMaterialAndStructure(StructureNeeded, MaterialNeeded);
            // LocalSetMaterialAndStructure(StructureNeeded, MaterialNeeded);
        }
        else
        {
            MaterialNeeded = MaterialNeeded + 1;
            TempStructureGuide.GetComponent<BuildingController>().LocalSetMaterialAndStructure(StructureNeeded, MaterialNeeded);
            // LocalSetMaterialAndStructure(StructureNeeded, MaterialNeeded);
        }
    }

    // Use this for initialization
    void Awake()
    {
        PlayerReference = gameObject;
        StructureSpawnerRef.GetComponent<BuildingController>().LinkedPlayer = PlayerReference;

        //Debug.Log("I'm not using the test script");

    }

    private void Start()
    {
        //LocalSelectStructure();
        //LocalSelectMaterial();
        //LocalRotateStructure(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (TempStructureGuide != null)
        {
            TempStructureGuide.transform.position = PointToSpawnStructure.position;
            
               // Debug.Log("Information for player: " + PlayerReference);
               // Debug.Log("Black blob location Before Placement: " + PointToSpawnStructure.position);
               // Debug.Log("Placement Ghost Location Before Placement: " + TempStructureGuide.transform.position);
        }
        /*
        if (Input.GetButton("BaseBuilding"))
        {
            EnterOrExitBuildMode();
            //Debug.Log("build mode on/off");
        }
        */
        /*
        if (Input.GetButton("BuildingPlace") && (InbuildMode == true))
        {
            switch (MaterialNeeded)
            {
                case 0:
                    if (gameObject.GetComponent<PlayerStats>().WoodInInventory >= 10)
                    {
                        CmdPlaceStructure();
                        gameObject.GetComponent<PlayerStats>().WoodInInventory = gameObject.GetComponent<PlayerStats>().WoodInInventory - 10;
                    }
                    break;
                case 1:
                    if (gameObject.GetComponent<PlayerStats>().StoneInInventory >= 10)
                    {
                        CmdPlaceStructure();
                        gameObject.GetComponent<PlayerStats>().StoneInInventory = gameObject.GetComponent<PlayerStats>().StoneInInventory - 10;
                    }
                    break;
                case 2:
                    if (gameObject.GetComponent<PlayerStats>().MetalInInventory >= 10)
                    {
                        CmdPlaceStructure();
                        gameObject.GetComponent<PlayerStats>().MetalInInventory = gameObject.GetComponent<PlayerStats>().MetalInInventory - 10;
                    }
                    break;
            }
        }
        */
        /*
        if (Input.GetKeyDown(("r")) && (InbuildMode == true))
        {
            //CmdRotateStructure();
            //RotateStructure();
            //CmdRotateStructure();
        }*/
        /*
        if (Input.GetButton("BuildingChangeRotation") && (InbuildMode == true))
        {
            if (RotationToSet == 360)
            {
                RotationToSet = 0;
                CmdRotateStructure(RotationToSet);
                LocalRotateStructure(RotationToSet);
            }
            else
            {
                RotationToSet = RotationToSet + 0.5f;
                CmdRotateStructure(RotationToSet);
                LocalRotateStructure(RotationToSet);
                Debug.Log("RotationValue: " + RotationToSet);
            }
        }
        */
        /*
        if (Input.GetButton("BuildingChangeStructure") && (InbuildMode == true))
        {
            CmdSelectStructure();
            LocalSelectStructure();
        }
        */
        /*
        if (Input.GetButton("BuildingChangeMaterial") && (InbuildMode == true))
        {
            CmdSelectMaterial();
            LocalSelectMaterial();
        }
        */
    }

    public void PlaceBuilding()
    {
        if ((InbuildMode == true))
        {
            switch (MaterialNeeded)
            {
                case 0:
                    if (gameObject.GetComponent<PlayerStats>().WoodInInventory >= 10)
                    {
                        CmdPlaceStructure();
                        gameObject.GetComponent<PlayerStats>().WoodInInventory = gameObject.GetComponent<PlayerStats>().WoodInInventory - 10;
                    }
                    break;
                case 1:
                    if (gameObject.GetComponent<PlayerStats>().StoneInInventory >= 10)
                    {
                        CmdPlaceStructure();
                        gameObject.GetComponent<PlayerStats>().StoneInInventory = gameObject.GetComponent<PlayerStats>().StoneInInventory - 10;
                    }
                    break;
                case 2:
                    if (gameObject.GetComponent<PlayerStats>().MetalInInventory >= 10)
                    {
                        CmdPlaceStructure();
                        gameObject.GetComponent<PlayerStats>().MetalInInventory = gameObject.GetComponent<PlayerStats>().MetalInInventory - 10;
                    }
                    break;
            }
        }
    }

    public void ChangeRotation()
    {
        if (InbuildMode == true)
        {
            if (RotationToSet == 360)
            {
                RotationToSet = 0;
                CmdRotateStructure(RotationToSet);
                LocalRotateStructure(RotationToSet);
            }
            else
            {
                RotationToSet = RotationToSet + 0.5f;
                CmdRotateStructure(RotationToSet);
                LocalRotateStructure(RotationToSet);
                //Debug.Log("RotationValue: " + RotationToSet);
            }
        }

    }

    public void ChangeStructure()
    {
        if(InbuildMode == true)
        {
            CmdSelectStructure();
            LocalSelectStructure();
        }
    }

    public void ChangeMaterial()
    {
        if(InbuildMode == true)
        {
            CmdSelectMaterial();
            LocalSelectMaterial();
        }
    }

}


























































//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.Networking;
//using UnityEngine;

//public class PlayerBuildingController : NetworkBehaviour
//{
//    public bool InbuildMode = false;
//    public Transform PointToSpawnStructure;
//    public GameObject PlayerReference;
//    public GameObject StructureSpawnerRef;
//    public GameObject TempStructureGuide;
//    public GameObject NetworkStructureToSpawn;

//    public GameObject NetworkSpawnedStructure;

//    public int StructureNeeded = 0;
//    public int MaterialNeeded = 0;

//    [Client]
//    void EnterOrExitBuildMode()
//    {
//        if (InbuildMode == true)
//        {
//            InbuildMode = false;
//            DestroyObject(TempStructureGuide);
//            TempStructureGuide = null;
//        }
//        else
//        {
//            InbuildMode = true;
//            TempStructureGuide = Instantiate(StructureSpawnerRef, PointToSpawnStructure.position, PointToSpawnStructure.rotation);
//        }
//    }

//    /*[Client]
//    void ClientPlaceStructure()
//    {
//        Instantiate(StructureSpawnerRef, PointToSpawnStructure.position, TempStructureGuide.GetComponent<Transform>().rotation);
//    }
//    [Server]
//    void ServerPlaceStructure()
//    {
//    NetworkSpawnedStructure = Instantiate(StructureSpawnerRef, PointToSpawnStructure.position, TempStructureGuide.GetComponent<Transform>().rotation);
//    NetworkServer.Spawn(NetworkSpawnedStructure);
//    }*/

//    void RotateStructure()
//    {
//        TempStructureGuide.transform.Rotate(0, 10, 0);
//    }

//    // Use this for initialization
//    void Awake()
//    {
//        PlayerReference = gameObject;
//        StructureSpawnerRef.GetComponent<BuildingController>().LinkedPlayer = PlayerReference;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (!isLocalPlayer)
//        {
//            return;
//        }

//        if (Input.GetKeyDown("b"))
//        {
//            /*if (InbuildMode == true)
//            {
//                InbuildMode = false;
//                DestroyObject(TempStructureGuide);
//                TempStructureGuide = null;
//            }
//            else
//            {
//                InbuildMode = true;
//                TempStructureGuide = Instantiate(StructureSpawnerRef, PointToSpawnStructure.position, PointToSpawnStructure.rotation);
//            }*/
//            EnterOrExitBuildMode();
//            Debug.Log("build mode on/off");
//        }
//        if (Input.GetKeyDown(("space")) && (InbuildMode == true))
//        {
//            //Instantiate(StructureSpawnerRef, PointToSpawnStructure.position, TempStructureGuide.GetComponent<Transform>().rotation);
//            //CmdPlaceStructure();
//            /*if (!isServer)
//            {
//                ClientPlaceStructure();
//            }
//            else
//            {
//                ServerPlaceStructure();
//            }*/


//        }

//        if (Input.GetKeyDown(("r")) && (InbuildMode == true))
//        {
//            //TempStructureGuide.transform.Rotate(0, 10, 0);
//            RotateStructure();
//        }

//        if (Input.GetKeyDown("1") && (InbuildMode == true))
//        {
//            if (StructureNeeded == 2)
//            {
//                StructureNeeded = 0;
//                TempStructureGuide.GetComponent<BuildingController>().SetMaterialAndStructure();
//                //Debug.Log("Structure Needed: " + StructureNeeded);
//            }
//            else
//            {
//                StructureNeeded = StructureNeeded + 1;
//                TempStructureGuide.GetComponent<BuildingController>().SetMaterialAndStructure();
//                //Debug.Log("Structure Needed: " + StructureNeeded);
//            }
//        }

//        if (Input.GetKeyDown("2") && (InbuildMode == true))
//        {
//            if (MaterialNeeded == 2)
//            {
//                MaterialNeeded = 0;
//                TempStructureGuide.GetComponent<BuildingController>().SetMaterialAndStructure();
//                //Debug.Log("Material Needed: " + MaterialNeeded);
//            }
//            else
//            {
//                MaterialNeeded = MaterialNeeded + 1;
//                TempStructureGuide.GetComponent<BuildingController>().SetMaterialAndStructure();
//                //Debug.Log("Material Needed: " + MaterialNeeded);
//            }
//        }

//        if (TempStructureGuide != null)
//        {
//            TempStructureGuide.transform.position = PointToSpawnStructure.position;
//        }
//    }
//}

