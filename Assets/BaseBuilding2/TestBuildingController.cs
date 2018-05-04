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
                    gameObject.transform.GetChild(0).GetComponent<Renderer>().material = WallWoodMat; // S = 0, M = 0: Wooden Wall
                    break;
                case 1:
                    gameObject.transform.GetChild(0).GetComponent<Renderer>().material = WallStoneMat; // S = 0, M = 1: Stone Wall
                    break;
                case 2:
                    gameObject.transform.GetChild(0).GetComponent<Renderer>().material = WallMetalMat; // S = 0, M = 2: Metal Wall
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
                    break;
                case 1:
                    gameObject.transform.GetChild(1).GetComponent<Renderer>().material = WallStoneMat; // S = 1, M = 1: Stone Floor
                    break;
                case 2:
                    gameObject.transform.GetChild(1).GetComponent<Renderer>().material = WallMetalMat; // S = 1, M = 2: Metal Floor
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
                    break;
                case 1:
                    gameObject.transform.GetChild(2).GetComponent<Renderer>().material = WallStoneMat; // S = 2, M = 1: Stone Door
                    break;
                case 2:
                    gameObject.transform.GetChild(2).GetComponent<Renderer>().material = WallMetalMat; // S = 2, M = 2: Metal Door
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
                case 0:
                    gameObject.transform.GetChild(0).GetComponent<Renderer>().material = WallWoodMat; // S = 0, M = 0: Wooden Wall
                    break;
                case 1:
                    gameObject.transform.GetChild(0).GetComponent<Renderer>().material = WallStoneMat; // S = 0, M = 1: Stone Wall
                    break;
                case 2:
                    gameObject.transform.GetChild(0).GetComponent<Renderer>().material = WallMetalMat; // S = 0, M = 2: Metal Wall
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
                    break;
                case 1:
                    gameObject.transform.GetChild(1).GetComponent<Renderer>().material = WallStoneMat; // S = 1, M = 1: Stone Floor
                    break;
                case 2:
                    gameObject.transform.GetChild(1).GetComponent<Renderer>().material = WallMetalMat; // S = 1, M = 2: Metal Floor
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
                    break;
                case 1:
                    gameObject.transform.GetChild(2).GetComponent<Renderer>().material = WallStoneMat; // S = 2, M = 1: Stone Door
                    break;
                case 2:
                    gameObject.transform.GetChild(2).GetComponent<Renderer>().material = WallMetalMat; // S = 2, M = 2: Metal Door
                    break;
            }
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
