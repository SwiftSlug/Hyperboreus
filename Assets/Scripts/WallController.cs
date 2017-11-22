using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WallController : NetworkBehaviour
{
    public Vector3 currentRot;
    public bool placeStatus;
    public bool firstPlace = true;

    public GameObject Wall_obj;
    public GameObject Stairs_obj;
    public GameObject Doorway_obj;
    public GameObject Door_obj;

    public Material metal;
    public Material stone;
    public Material wood;

    [ClientRpc]
    public void RpcSetMaterial(int MaterialToSet)
    {

        switch (MaterialToSet)
        {
            case 0:
                Wall_obj.GetComponent<Renderer>().material = wood;
                Stairs_obj.GetComponent<Renderer>().material = wood;
                Doorway_obj.GetComponent<Renderer>().material = wood;
                Door_obj.GetComponent<Renderer>().material = wood;
                break;
            case 1:
                Wall_obj.GetComponent<Renderer>().material = stone;
                Stairs_obj.GetComponent<Renderer>().material = stone;
                Doorway_obj.GetComponent<Renderer>().material = stone;
                Door_obj.GetComponent<Renderer>().material = stone;
                break;
            case 2:
                Wall_obj.GetComponent<Renderer>().material = metal;
                Stairs_obj.GetComponent<Renderer>().material = metal;
                Doorway_obj.GetComponent<Renderer>().material = metal;
                Door_obj.GetComponent<Renderer>().material = metal;
                break;
        }
    }

    [ClientRpc]
    public void RpcSetStructure(int StructureToSet)
    {
        switch (StructureToSet)
        {
            case 0:

                Door_obj.GetComponent<Collider>().enabled = false;
                Door_obj.GetComponent<Renderer>().enabled = false;
                Stairs_obj.GetComponent<Collider>().enabled = false;
                Stairs_obj.GetComponent<Renderer>().enabled = false;
                Doorway_obj.GetComponent<Collider>().enabled = false;
                Doorway_obj.GetComponent<Renderer>().enabled = false;

                Wall_obj.GetComponent<Collider>().enabled = true;
                Wall_obj.GetComponent<Renderer>().enabled = true;

                Debug.Log("Wall");
                break;

            case 1:

                Wall_obj.GetComponent<Collider>().enabled = false;
                Wall_obj.GetComponent<Renderer>().enabled = false;
                Door_obj.GetComponent<Collider>().enabled = false;
                Door_obj.GetComponent<Renderer>().enabled = false;
                Doorway_obj.GetComponent<Collider>().enabled = false;
                Doorway_obj.GetComponent<Renderer>().enabled = false;

                Stairs_obj.GetComponent<Collider>().enabled = true;
                Stairs_obj.GetComponent<Renderer>().enabled = true;

                Debug.Log("Stairs");
                break;

            case 2:

                Stairs_obj.GetComponent<Collider>().enabled = false;
                Stairs_obj.GetComponent<Renderer>().enabled = false;
                Door_obj.GetComponent<Collider>().enabled = false;
                Door_obj.GetComponent<Renderer>().enabled = false;
                Stairs_obj.GetComponent<Collider>().enabled = false;
                Stairs_obj.GetComponent<Renderer>().enabled = false;

                Doorway_obj.GetComponent<Collider>().enabled = true;
                Doorway_obj.GetComponent<Renderer>().enabled = true;

                Debug.Log("Doorway");
                break;

            case 3:

                Doorway_obj.GetComponent<Collider>().enabled = false;
                Doorway_obj.GetComponent<Renderer>().enabled = false;
                Stairs_obj.GetComponent<Collider>().enabled = false;
                Stairs_obj.GetComponent<Renderer>().enabled = false;
                Doorway_obj.GetComponent<Collider>().enabled = false;
                Doorway_obj.GetComponent<Renderer>().enabled = false;

                Door_obj.GetComponent<Collider>().enabled = true;
                Door_obj.GetComponent<Renderer>().enabled = true;

                Debug.Log("Doorway");
                break;
        }
    }

    // Use this for initialization
   void Start ()
    {
        currentRot.x = transform.rotation.x;
        currentRot.y = transform.rotation.y;
        currentRot.z = transform.rotation.z;

 //       if(firstPlace == true)
 //       {
            //disable colliders for placement actor choices
            Stairs_obj.GetComponent<Collider>().enabled = false;
            Doorway_obj.GetComponent<Collider>().enabled = false;
            Door_obj.GetComponent<Collider>().enabled = false;

            //disable renderer for all except Wall
            Stairs_obj.GetComponent<Renderer>().enabled = false;
            Doorway_obj.GetComponent<Renderer>().enabled = false;
            Door_obj.GetComponent<Renderer>().enabled = false;

            RpcSetMaterial(0);
//            firstPlace = false;
//       }
    }

    // Update is called once per frame
    void Update ()
    {
	    if (Input.GetKeyDown("space"))
        {
            placeStatus = true;
        }
    }
}
