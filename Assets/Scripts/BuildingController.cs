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

    [SyncVar]
    public int Hitpoints;

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

    [Command]
    public void CmdDamage(int amount)
    {
        Debug.Log("BuildingController: CmdDamage");
        if (!isServer)
        {
            return;
        }

        Hitpoints -= amount;

        if (Hitpoints <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    [ClientRpc]
    public void RpcSetMaterialAndStructure(int StructureValue, int MaterialValue)
    {
        ChangeStructureOrMaterial(StructureValue, MaterialValue);
    }

    public void LocalSetMaterialAndStructure(int StructureValue, int MaterialValue)
    {
        ChangeStructureOrMaterial(StructureValue, MaterialValue);
    }

    public void LocalSetRotation(float Rotation)
    {
        gameObject.transform.eulerAngles = new Vector3(0, Rotation, 0);
    }

    void ChangeStructureOrMaterial(int StructureValue, int MaterialValue)
    {
        if (StructureValue == 0) //check if wall is needed
        {

            if (MaterialValue == 0) // check if wood is needed, if it is then disable renderers for non wood wall models
            {
                // set wooden wall visible
                gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = true;
                gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
                gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = false;
                gameObject.transform.GetChild(3).GetComponent<Renderer>().enabled = false;
                gameObject.transform.GetChild(4).GetComponent<Renderer>().enabled = false;
                Hitpoints = 100;
            }
            else if (MaterialValue == 1) // check if stone is needed, if it is then disable renderers for non stone wall models
            {
                // set stone wall visible
                gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
                gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = true;
                gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = false;
                gameObject.transform.GetChild(3).GetComponent<Renderer>().enabled = false;
                gameObject.transform.GetChild(4).GetComponent<Renderer>().enabled = false;
                Hitpoints = 200;
            }
            else if (MaterialValue == 2) // check if metal is needed, if it is then disable renderers for non metal wall models
            {
                // set metal wall visible
                gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
                gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
                gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = true;
                gameObject.transform.GetChild(3).GetComponent<Renderer>().enabled = false;
                gameObject.transform.GetChild(4).GetComponent<Renderer>().enabled = false;
                Hitpoints = 300;
            }
        }
        else if (StructureValue == 1) // //check if gate is needed
        {
            if (MaterialValue == 0)
            {
                // check if wood is needed, if it is then disable renderers for non wood gate models
                // set wooden gate visible
                gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
                gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
                gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = false;
                gameObject.transform.GetChild(3).GetComponent<Renderer>().enabled = true;
                gameObject.transform.GetChild(4).GetComponent<Renderer>().enabled = false;
            }
            else if (MaterialValue == 1)
            {
                // check if stone is needed, if it is then disable renderers for non stone gate models
                // set stone gate visible
                gameObject.transform.GetChild(0).GetComponent<Renderer>().enabled = false;
                gameObject.transform.GetChild(1).GetComponent<Renderer>().enabled = false;
                gameObject.transform.GetChild(2).GetComponent<Renderer>().enabled = false;
                gameObject.transform.GetChild(3).GetComponent<Renderer>().enabled = false;
                gameObject.transform.GetChild(4).GetComponent<Renderer>().enabled = true;
                // check if stone is needed, if it is then disable renderers for non stone gate models
            }
            else if (MaterialValue == 2)
            {
                Debug.Log("this would be ametal gate...");
                // check if metal is needed, if it is then disable renderers for non metal gate models
            }
        }
    }

}
