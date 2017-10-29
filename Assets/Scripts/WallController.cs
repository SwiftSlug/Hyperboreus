using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WallController : NetworkBehaviour
{
    public Vector3 currentRot;
    public bool placeStatus;

    public GameObject Wall_obj;
    public GameObject Stairs_obj;
    public GameObject Doorway_obj;
    public GameObject Door_obj;
    public GameObject Floor_obj;
    public GameObject Ceiling_obj;
    public GameObject Ramps_obj;
    public GameObject BarbedWireTrap_obj;

    public Material metal;
    public Material stone;
    public Material wood;


    public GameObject StructureToApplyMatTo;

    public int ActiveStructure = 0;

    // Use this for initialization
    void Start ()
    {
        currentRot.x = transform.rotation.x;
        currentRot.y = transform.rotation.y;
        currentRot.z = transform.rotation.z;

        //disable colliders for placement actor choices
        Stairs_obj.GetComponent<Collider>().enabled = false;
        Doorway_obj.GetComponent<Collider>().enabled = false;
        Door_obj.GetComponent<Collider>().enabled = false;
        Floor_obj.GetComponent<Collider>().enabled = false;
        Ceiling_obj.GetComponent<Collider>().enabled = false;
        Ramps_obj.GetComponent<Collider>().enabled = false;
        BarbedWireTrap_obj.GetComponent<Collider>().enabled = false;

        //disable renderer for all except Wall
        Stairs_obj.GetComponent<Renderer>().enabled = false;
        Doorway_obj.GetComponent<Renderer>().enabled = false;
        Door_obj.GetComponent<Renderer>().enabled = false;
        Floor_obj.GetComponent<Renderer>().enabled = false;
        Ceiling_obj.GetComponent<Renderer>().enabled = false;
        Ramps_obj.GetComponent<Renderer>().enabled = false;
        BarbedWireTrap_obj.GetComponent<Renderer>().enabled = false;

        SetMaterial(wood);
    }

    // Update is called once per frame
    void Update ()
    {
	    if (Input.GetKeyDown("space"))
        {
            placeStatus = true;
        }

        switch (ActiveStructure)
        {
            case 0:
                Wall_obj.GetComponent<Collider>().enabled = true;
                Wall_obj.GetComponent<Renderer>().enabled = true;

                BarbedWireTrap_obj.GetComponent<Collider>().enabled = false;
                BarbedWireTrap_obj.GetComponent<Renderer>().enabled = false;

                //StructureToApplyMatTo = Wall_obj;

                Debug.Log("Wall");
                break;

            case 1:
                Stairs_obj.GetComponent<Collider>().enabled = true;
                Stairs_obj.GetComponent<Renderer>().enabled = true;

                Wall_obj.GetComponent<Collider>().enabled = false;
                Wall_obj.GetComponent<Renderer>().enabled = false;

                //StructureToApplyMatTo = Stairs_obj;

                Debug.Log("Stairs");
                break;

            case 2:
                Doorway_obj.GetComponent<Collider>().enabled = true;
                Doorway_obj.GetComponent<Renderer>().enabled = true;

                Stairs_obj.GetComponent<Collider>().enabled = false;
                Stairs_obj.GetComponent<Renderer>().enabled = false;

               // StructureToApplyMatTo = Doorway_obj;

                Debug.Log("Doorway");
                break;

            case 3:
                Door_obj.GetComponent<Collider>().enabled = true;
                Door_obj.GetComponent<Renderer>().enabled = true;

                Doorway_obj.GetComponent<Collider>().enabled = false;
                Doorway_obj.GetComponent<Renderer>().enabled = false;

               // StructureToApplyMatTo = Door_obj;

                Debug.Log("Door");
                break;

            case 4:
                Floor_obj.GetComponent<Collider>().enabled = true;
                Floor_obj.GetComponent<Renderer>().enabled = true;

                Door_obj.GetComponent<Collider>().enabled = false;
                Door_obj.GetComponent<Renderer>().enabled = false;

               // StructureToApplyMatTo = Floor_obj;

                Debug.Log("Floor");
                break;

            case 5:
                Ceiling_obj.GetComponent<Collider>().enabled = true;
                Ceiling_obj.GetComponent<Renderer>().enabled = true;

                Floor_obj.GetComponent<Collider>().enabled = false;
                Floor_obj.GetComponent<Renderer>().enabled = false;

               // StructureToApplyMatTo = Ceiling_obj;

                Debug.Log("Ceiling");
                break;

            case 6:
                Ramps_obj.GetComponent<Collider>().enabled = true;
                Ramps_obj.GetComponent<Renderer>().enabled = true;

                Ceiling_obj.GetComponent<Collider>().enabled = false;
                Ceiling_obj.GetComponent<Renderer>().enabled = false;

               // StructureToApplyMatTo = Ramps_obj;

                Debug.Log("Ramps");
                break;

            case 7:
                BarbedWireTrap_obj.GetComponent<Collider>().enabled = true;
                BarbedWireTrap_obj.GetComponent<Renderer>().enabled = true;

                Ramps_obj.GetComponent<Collider>().enabled = false;
                Ramps_obj.GetComponent<Renderer>().enabled = false;

//              StructureToApplyMatTo = BarbedWireTrap_obj;

                Debug.Log("BarbedWire");
                break;
        }

    }

    public void SetMaterial(Material MaterialToApply)
    {
        //StructureToApplyMatTo.GetComponent<Renderer>().material = MaterialToApply;
        Wall_obj.GetComponent<Renderer>().material = MaterialToApply;
        Stairs_obj.GetComponent<Renderer>().material = MaterialToApply;
        Door_obj.GetComponent<Renderer>().material = MaterialToApply;
        Doorway_obj.GetComponent<Renderer>().material = MaterialToApply;
        Floor_obj.GetComponent<Renderer>().material = MaterialToApply;
        Ceiling_obj.GetComponent<Renderer>().material = MaterialToApply;
        Ramps_obj.GetComponent<Renderer>().material = MaterialToApply;
        BarbedWireTrap_obj.GetComponent<Renderer>().material = MaterialToApply;
    }
}
