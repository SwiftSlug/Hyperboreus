using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestBuildingController : NetworkBehaviour
{
    //structure and material choice variables
    public int LocalStructureChoice = 0;
    public int LocalMaterialChoice = 0;
    // material variables
    public Material WallWoodMat;
    public Material WallStoneMat;
    public Material WallMetalMat;

    public Material FloorWoodMat;
    public Material FloorStoneMat;
    public Material FloorMetalMat;

    public Material DoorWoodMat;
    public Material DoorStoneMat;
    public Material DoorMetalMat;

    [SyncVar]
    public int hitPoints = 100;

    //Client Functions
    //ChooseStructureAndMaterial
    public void RpcChooseStructureAndMaterial(int StructureChoice, int MaterialChoice)
    {
        LocalStructureChoice = StructureChoice;
        LocalMaterialChoice = MaterialChoice;

        if (LocalStructureChoice == 0) // wall chosen
        {
            gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = true; // set wall visible
            gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
            gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = false;

            switch (LocalMaterialChoice)
            {
                case 0:
                    gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = true; //material = WallWoodMat; // S = 0, M = 0: Wooden Wall
                    gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
                    gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = false;
                    hitPoints = 20;
                    break;
                case 1:
                    gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false; // material = WallStoneMat; // S = 0, M = 1: Stone Wall
                    gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = true;
                    gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = false;
                    hitPoints = 40;
                    break;
                case 2:
                    gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false; // material = WallMetalMat; // S = 0, M = 2: Metal Wall
                    gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
                    gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = true;
                    hitPoints = 80;
                    break;
            }
        }
        else if (LocalStructureChoice == 1) // floor chosen
        {

            gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = true; //set floor to visible
            gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = false;

            switch (LocalMaterialChoice)
            {
                case 0:
                    gameObject.transform.GetChild(1).GetComponent<Renderer>().material = WallWoodMat; // S = 1, M = 0: Wooden Floor
                    hitPoints = 20;
                    break;
                case 1:
                    gameObject.transform.GetChild(1).GetComponent<Renderer>().material = WallStoneMat; // S = 1, M = 1: Stone Floor
                    hitPoints = 40;
                    break;
                case 2:
                    gameObject.transform.GetChild(1).GetComponent<Renderer>().material = WallMetalMat; // S = 1, M = 2: Metal Floor
                    hitPoints = 80;
                    break;
            }
        }
        else if (LocalStructureChoice == 2) // door chosen
        {

            gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
            gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = true; // set door to visible

            switch (LocalMaterialChoice)
            {
                case 0:
                    gameObject.transform.GetChild(2).GetComponent<Renderer>().material = WallWoodMat; // S = 2, M = 0: Wooden Door
                    hitPoints = 20;
                    break;
                case 1:
                    gameObject.transform.GetChild(2).GetComponent<Renderer>().material = WallStoneMat; // S = 2, M = 1: Stone Door
                    hitPoints = 40;
                    break;
                case 2:
                    gameObject.transform.GetChild(2).GetComponent<Renderer>().material = WallMetalMat; // S = 2, M = 2: Metal Door
                    hitPoints = 80;
                    break;
            }
        }
    }
    //Local Functions
    public void LocalChooseStructureAndMaterial(int StructureChoice, int MaterialChoice)
    {
        LocalStructureChoice = StructureChoice;
        LocalMaterialChoice = MaterialChoice;

        if (LocalStructureChoice == 0) // wall chosen
        {
            gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = true; // set wall visible
            gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
            gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = false;

            switch (LocalMaterialChoice)
            {
                /*case 0:
                    gameObject.transform.GetChild(0).GetComponent<Renderer>().material = WallWoodMat; // S = 0, M = 0: Wooden Wall
                    hitPoints = 20;
                    break;
                case 1:
                    gameObject.transform.GetChild(0).GetComponent<Renderer>().material = WallStoneMat; // S = 0, M = 1: Stone Wall
                    hitPoints = 40;
                    break;
                case 2:
                    gameObject.transform.GetChild(0).GetComponent<Renderer>().material = WallMetalMat; // S = 0, M = 2: Metal Wall
                    hitPoints = 80;
                    break;*/
                case 0:
                    gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = true; //material = WallWoodMat; // S = 0, M = 0: Wooden Wall
                    gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
                    gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = false;
                    hitPoints = 20;
                    break;
                case 1:
                    gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false; // material = WallStoneMat; // S = 0, M = 1: Stone Wall
                    gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = true;
                    gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = false;
                    hitPoints = 40;
                    break;
                case 2:
                    gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false; // material = WallMetalMat; // S = 0, M = 2: Metal Wall
                    gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
                    gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = true;
                    hitPoints = 80;
                    break;
            }
        }
        else if (LocalStructureChoice == 1) // floor chosen
        {

            gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = true; //set floor to visible
            gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = false;

            switch (LocalMaterialChoice)
            {
                case 0:
                    gameObject.transform.GetChild(1).GetComponent<Renderer>().material = WallWoodMat; // S = 1, M = 0: Wooden Floor
                    hitPoints = 20;
                    break;
                case 1:
                    gameObject.transform.GetChild(1).GetComponent<Renderer>().material = WallStoneMat; // S = 1, M = 1: Stone Floor
                    hitPoints = 40;
                    break;
                case 2:
                    gameObject.transform.GetChild(1).GetComponent<Renderer>().material = WallMetalMat; // S = 1, M = 2: Metal Floor
                    hitPoints = 80;
                    break;
            }
        }
        else if (LocalStructureChoice == 2) // door chosen
        {

            gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
            gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = true; // set door to visible

            switch (LocalMaterialChoice)
            {
                case 0:
                    gameObject.transform.GetChild(2).GetComponent<Renderer>().material = WallWoodMat; // S = 2, M = 0: Wooden Door
                    hitPoints = 20;
                    break;
                case 1:
                    gameObject.transform.GetChild(2).GetComponent<Renderer>().material = WallStoneMat; // S = 2, M = 1: Stone Door
                    hitPoints = 40;
                    break;
                case 2:
                    gameObject.transform.GetChild(2).GetComponent<Renderer>().material = WallMetalMat; // S = 2, M = 2: Metal Door
                    hitPoints = 80;
                    break;
            }
        }
    }

    [Command]
    public void CmdDamage(int amount)
    {
        if (!isServer)
        {
            return;
        }

        hitPoints -= amount;

        if (hitPoints <= 0)
        {
            //Debug.Log("Building Destroyed by AI Attack");
            //Network.Destroy(this.gameObject);
            Destroy(this.gameObject);
        }
    }


    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
