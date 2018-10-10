using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerStructureController : NetworkBehaviour
{
    //wall structure prefabs
    public GameObject woodenWall;
    public GameObject stoneWall;
    public GameObject metalWall;

    //gate structure prefabs
    public GameObject woodenGate;
    public GameObject stoneGate;
    public GameObject metalGate;

    //structure & material variables
    public int structureChoice;
    public int materialChoice;

    //variable to track if player is in build mode or not
    public bool inBuildmode = false;

    //ghost structure 
    public GameObject ghostStructure;
    public GameObject instantiatedGhostStructure;
    public GameObject serverGhostStructure;

    //point to spawn the ghost structure at.
    public Transform pointToSpawn;

	// Use this for initialization
	void Start ()
    {
		
	}	
	// Update is called once per frame
	void Update ()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (instantiatedGhostStructure != null)
        {
            instantiatedGhostStructure.transform.position = pointToSpawn.position;

        }
    }

    public void EnterExitBuildMode()
    {
        if (inBuildmode == false)
        {
            inBuildmode = true;
            SetActiveStructure(structureChoice, materialChoice);
        }
        else
        {
            inBuildmode = false;
            DestroyObject(ghostStructure);
            DestroyObject(instantiatedGhostStructure);
            ghostStructure = null;
            instantiatedGhostStructure = null;
        }
    }
    //Switches between structure types. such as walls and gates
    public void SwitchStructure()
    {
        if (structureChoice > 1)
        {
            structureChoice = 0;
            SetActiveStructure(structureChoice, materialChoice);
        }
        else
        {
            structureChoice++;
            SetActiveStructure(structureChoice, materialChoice);
        }
    }
    //Switches between wood stone and metal
    public void SwitchMaterial()
    {
        if (materialChoice > 1)
        {
            materialChoice = 0;
            SetActiveStructure(structureChoice, materialChoice);
        }
        else
        {
            materialChoice++;
            SetActiveStructure(structureChoice, materialChoice);
        }
    }
    //Switches active model based on structure and material choice
    public void SetActiveStructure(int structureChosen, int materialChosen)
    {
        switch(structureChosen)
        {
            //Walls
            case 0:
                switch(materialChosen)
                {
                    //Wood
                    case 0:
                        ghostStructure = woodenWall;
                        changeGhostStructure();
                        break;
                    //Stone
                    case 1:
                        ghostStructure = stoneWall;
                        changeGhostStructure();
                        break;
                    //Metal
                    case 2:
                        ghostStructure = metalWall;
                        changeGhostStructure();
                        break;
                }
                break;
            //Gates
            case 1:
                switch (materialChosen)
                {
                    //Wood
                    case 0:
                        ghostStructure = woodenGate;
                        changeGhostStructure();
                        break;
                    //Stone
                    case 1:
                        ghostStructure = stoneGate;
                        changeGhostStructure();
                        break;
                    //Metal
                    case 2:
                        ghostStructure = metalGate;
                        changeGhostStructure();
                        break;
                }
                break;
        }

    }
    //changes visible ghost structure
    public void changeGhostStructure()
    {
        instantiatedGhostStructure = (GameObject)Instantiate(ghostStructure, pointToSpawn.position, pointToSpawn.rotation);
    }
    //places structure on server
    [Command]
    public void CmdPlaceStructure()
    {
        serverGhostStructure = (GameObject)Instantiate(ghostStructure, pointToSpawn.position, pointToSpawn.rotation);
        NetworkServer.Spawn(serverGhostStructure);
        serverGhostStructure.GetComponent<StructureController>().RpcSetupSpawn();
    }
}
