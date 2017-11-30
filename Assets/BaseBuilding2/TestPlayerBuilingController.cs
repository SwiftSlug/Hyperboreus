using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestPlayerBuilingController : NetworkBehaviour
{
    public bool InBuildMode = false;
    public Transform PointToSpawnStructure;
    public GameObject StructureSpawnerRef;
    public GameObject buildingTemplate;
    public GameObject NetworkSpawnedStructure;

    public int StructureChoice = 0;
    public int MaterialChoice = 0;
    public float RotationToSet = 0;

    void EnterOrExitBuildMode()
    {
        if (InBuildMode == true)
        {
            InBuildMode = false;
        }
        else if (InBuildMode == false)
        {
            InBuildMode = true;
            buildingTemplate = Instantiate(StructureSpawnerRef, PointToSpawnStructure.position, gameObject.transform.rotation); // spawn building tempate at the point to spawn structure orbs position and with the player's rotation.
            buildingTemplate.transform.eulerAngles = new Vector3(0, RotationToSet, 0);
        }
    }

    private void Awake()
    {
        Debug.Log("I'm using the test script");
    }

    //Server Functions
    //Place Structure
    [Command]
    void CmdSpawnStructure()
    {
        NetworkSpawnedStructure = (GameObject)Instantiate(buildingTemplate, buildingTemplate.transform.position, buildingTemplate.transform.rotation);
        NetworkServer.Spawn(NetworkSpawnedStructure);
        NetworkSpawnedStructure.GetComponent<TestBuildingController>().RpcChooseStructureAndMaterial(StructureChoice, MaterialChoice);
    }
    //Choose Structure
    [Command]
    void CmdChooseStructure(int StructureChoice)
    {
        buildingTemplate.GetComponent<BuildingController>().RpcSetMaterialAndStructure(StructureChoice, MaterialChoice);
    }
    //Choose Material
    [Command]
    void CmdChooseMaterial(int MaterialChoice)
    {
        buildingTemplate.GetComponent<BuildingController>().RpcSetMaterialAndStructure(StructureChoice, MaterialChoice);
    }

    //Local Functions
    //Choose Structure
    void LocalChooseStructure(int StructureChoice)
    {
        if (StructureChoice > 2)
        {
            StructureChoice = 0;
            buildingTemplate.GetComponent<TestBuildingController>().LocalChooseStructureAndMaterial(StructureChoice, MaterialChoice);
        }
        else
        {
            buildingTemplate.GetComponent<TestBuildingController>().LocalChooseStructureAndMaterial(StructureChoice, MaterialChoice);
            StructureChoice++;
        }

    }
    //Choose Material
    void LocalChooseMaterial(int MaterialChoice)
    {
        if (MaterialChoice > 2)
        {
            MaterialChoice = 0;
            buildingTemplate.GetComponent<TestBuildingController>().LocalChooseStructureAndMaterial(StructureChoice, MaterialChoice);
        }
        else
        {
            buildingTemplate.GetComponent<TestBuildingController>().LocalChooseStructureAndMaterial(StructureChoice, MaterialChoice);
            MaterialChoice++;
        }
    }
    //Change Rotation
    void LocalRotate()
    {
        buildingTemplate.transform.eulerAngles = new Vector3(0, RotationToSet, 0);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            return;
        }

        //call function to enter / exit build mode
        if (Input.GetKeyDown("b"))
        {
            EnterOrExitBuildMode();
        }

        if (Input.GetKey(KeyCode.R) && (InBuildMode == true))
        {
            LocalRotate();
        }

        if (Input.GetKeyDown("1") && (InBuildMode == true))
        {
            LocalChooseStructure(StructureChoice);
            CmdChooseStructure(StructureChoice);
        }

        if (Input.GetKeyDown("2") && (InBuildMode == true))
        {
            LocalChooseMaterial(MaterialChoice);
            CmdChooseMaterial(MaterialChoice);
        }
    }
}
