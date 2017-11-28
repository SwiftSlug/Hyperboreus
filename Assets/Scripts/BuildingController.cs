using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BuildingController : NetworkBehaviour
{
    public GameObject LinkedPlayer;
    public GameObject PlayerStructureGuide;

    public int LocalNeededStructure = 0;
    public int LocalNeededMaterial = 0;

    public Material WallWoodMat;
    public Material WallStoneMat;
    public Material WallMetalMat;

    public Material FloorWoodMat;
    public Material FloorStoneMat;
    public Material FloorMetalMat;

    public Material DoorWoodMat;
    public Material DoorStoneMat;
    public Material DoorMetalMat;

    public bool StructurePlaced = false;

    // Use this for initialization
    void Start()
    {
        //RpcSetMaterialAndStructure();
        Debug.Log("I've been spawned! My linked player is: " + LinkedPlayer);
    }

    // Update is called once per frame
    void Update()
    {
    }
    [ClientRpc]
    public void RpcSetMaterialAndStructure(int StructureValue, int MaterialValue)
    {
        //LocalNeededStructure = LinkedPlayer.GetComponent<PlayerBuildingController>().StructureNeeded;
        //LocalNeededMaterial = LinkedPlayer.GetComponent<PlayerBuildingController>().MaterialNeeded;

        if (StructureValue == 0) //if (LocalNeededStructure == 0)
        {
            gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = true;

            gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
            gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = false;
            if (MaterialValue == 0) //if (LocalNeededMaterial == 0)
            {
                gameObject.transform.GetChild(0).GetComponent<Renderer>().material = WallWoodMat;
            }
            else if (MaterialValue == 1) // else if (LocalNeededMaterial == 1)
            {
                gameObject.transform.GetChild(0).GetComponent<Renderer>().material = WallStoneMat;
            }
            else if (MaterialValue == 2) //else if (LocalNeededMaterial == 2)
            {
                gameObject.transform.GetChild(0).GetComponent<Renderer>().material = WallMetalMat;
            }
        }
        else if (StructureValue == 1) //else if (LocalNeededStructure == 1)
        {
            gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = true;

            gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = false;
            if (MaterialValue == 0) //if (LocalNeededMaterial == 0)
            {
                gameObject.transform.GetChild(1).GetComponent<Renderer>().material = FloorWoodMat;
            }
            else if (MaterialValue == 1) //            else if (LocalNeededMaterial == 1)
            {
                gameObject.transform.GetChild(1).GetComponent<Renderer>().material = FloorStoneMat;
            }
            else if (MaterialValue == 2) //            else if (LocalNeededMaterial == 2)

            {
                gameObject.transform.GetChild(1).GetComponent<Renderer>().material = FloorMetalMat;
            }
        }
        else if (StructureValue == 2) //else if (LocalNeededStructure == 2)
        {
            gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = true;

            gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
            if (MaterialValue == 0) //             if (LocalNeededMaterial == 0)

            {
                gameObject.transform.GetChild(2).GetComponent<Renderer>().material = DoorWoodMat;
            }
            else if (MaterialValue == 1) //            else if (LocalNeededMaterial == 1)
            {
                gameObject.transform.GetChild(2).GetComponent<Renderer>().material = DoorStoneMat;
            }
            else if (MaterialValue == 2) //            else if (LocalNeededMaterial == 2)
            {
                gameObject.transform.GetChild(2).GetComponent<Renderer>().material = DoorMetalMat;
            }
        }
    }

    public void LocalSetMaterialAndStructure(int StructureValue, int MaterialValue)
    {
        //LocalNeededStructure = LinkedPlayer.GetComponent<PlayerBuildingController>().StructureNeeded;
        //LocalNeededMaterial = LinkedPlayer.GetComponent<PlayerBuildingController>().MaterialNeeded;

        if (StructureValue == 0) //if (LocalNeededStructure == 0)
        {
            gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = true;

            gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
            gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = false;
            if (MaterialValue == 0) //if (LocalNeededMaterial == 0)
            {
                gameObject.transform.GetChild(0).GetComponent<Renderer>().material = WallWoodMat;
            }
            else if (MaterialValue == 1) // else if (LocalNeededMaterial == 1)
            {
                gameObject.transform.GetChild(0).GetComponent<Renderer>().material = WallStoneMat;
            }
            else if (MaterialValue == 2) //else if (LocalNeededMaterial == 2)
            {
                gameObject.transform.GetChild(0).GetComponent<Renderer>().material = WallMetalMat;
            }
        }
        else if (StructureValue == 1) //else if (LocalNeededStructure == 1)
        {
            gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = true;

            gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = false;
            if (MaterialValue == 0) //if (LocalNeededMaterial == 0)
            {
                gameObject.transform.GetChild(1).GetComponent<Renderer>().material = FloorWoodMat;
            }
            else if (MaterialValue == 1) //            else if (LocalNeededMaterial == 1)
            {
                gameObject.transform.GetChild(1).GetComponent<Renderer>().material = FloorStoneMat;
            }
            else if (MaterialValue == 2) //            else if (LocalNeededMaterial == 2)

            {
                gameObject.transform.GetChild(1).GetComponent<Renderer>().material = FloorMetalMat;
            }
        }
        else if (StructureValue == 2) //else if (LocalNeededStructure == 2)
        {
            gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = true;

            gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
            if (MaterialValue == 0) //             if (LocalNeededMaterial == 0)

            {
                gameObject.transform.GetChild(2).GetComponent<Renderer>().material = DoorWoodMat;
            }
            else if (MaterialValue == 1) //            else if (LocalNeededMaterial == 1)
            {
                gameObject.transform.GetChild(2).GetComponent<Renderer>().material = DoorStoneMat;
            }
            else if (MaterialValue == 2) //            else if (LocalNeededMaterial == 2)
            {
                gameObject.transform.GetChild(2).GetComponent<Renderer>().material = DoorMetalMat;
            }
        }
    }

    public void LocalSetRotation(float Rotation)
    {
        gameObject.transform.eulerAngles = new Vector3(0, Rotation, 0);
    }
}

