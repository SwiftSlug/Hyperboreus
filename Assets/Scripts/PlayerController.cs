using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public bool blep = true;
    Vector3 playerOrientation; //Player Orientation
    [SyncVar]
    public bool AbleToDestroy = false;
    [SyncVar]
    public bool AbleToLoot = false;
    public GameObject AssetToDestroy;
    public GameObject AssetToLoot;
    public bool test = false;

    //Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition); //Cast a ray from the main camera relative to the screen position

        RaycastHit mouseRayHit; //Reference to the ray hit - used in calculating player orientation

        if (Physics.Raycast(mouseRay, out mouseRayHit, 100)) //If the Ray has hit an object
        {
            playerOrientation = mouseRayHit.point; //Make our potential player orientation equal to the location we hit with the ray cast.
        }

        Vector3 playerDirection = playerOrientation - transform.position; //Save a vector based on our players position and the saved orientation
        playerDirection.y = 0; //Get rid of our y portion of the vector because we don't need it

        float xAxis = Input.GetAxis("Horizontal") * Time.deltaTime * 6.0f; //Get the input horizontally and save it. "W,S,Up,Down,Joystick Up,Joystick Down"
        float yAxis = Input.GetAxis("Vertical") * Time.deltaTime * 6.0f; //Get the vertical input and and save it. "A,D,Left,Right,Joystick Left, Joystick Right"

        

        transform.LookAt(transform.position + playerDirection, Vector3.up); //Look at the mouse position in screen space
        transform.Translate(Vector3.forward * yAxis, Space.World); //Move horizontally within world space instead of local
        transform.Translate(Vector3.right * xAxis, Space.World); //Move vertically within world space instead of local

        //  Controller Aiming

        Vector3 controllerAimingDirection = new Vector3(Input.GetAxis("ControllerLookX"), 0, Input.GetAxis("ControllerLookY"));
        //Debug.Log(controllerAimingDirection);

        if(controllerAimingDirection.magnitude > 0)
        {
            // Controller stick in use
            transform.LookAt(transform.position + (controllerAimingDirection * 10));
            //Debug.Log("Controller Direction Set");
        }




        if (Input.GetButton("Interact"))
        {
            if (AbleToDestroy == true)
            {
                CmdDamageAsset();
            }
            if (AbleToLoot == true)
            {
               CmdLootableAmmo();
            }
        }
    }

    private void OnCollisionEnter(Collision collidedAsset)
    {
        //destroyable asset start collision
        if (collidedAsset.gameObject.CompareTag("DestructibleScenery"))
        {
            AbleToDestroy = true;
            AssetToDestroy = collidedAsset.gameObject;
            AssetToDestroy.GetComponent<LootAndDestroy>().PlayerDestroyingOrLooting = gameObject;
        }
        //lootable asset start collision
        if (collidedAsset.gameObject.CompareTag("LootableContainer"))
        {
            AbleToLoot = true;
            AssetToLoot = collidedAsset.gameObject;
            AssetToLoot.GetComponent<LootAndDestroy>().PlayerDestroyingOrLooting = gameObject;

        }
    }
    private void OnCollisionExit(Collision collidedAsset)
    {
        //destroyable asset end collision
        if (collidedAsset.gameObject.CompareTag("DestructibleScenery"))
        {
            AssetToDestroy.GetComponent<LootAndDestroy>().PlayerDestroyingOrLooting = null;
            AbleToDestroy = false;
            AssetToDestroy = null;
        }
        //lootable asset end collision
        if (collidedAsset.gameObject.CompareTag("LootableContainer"))
        {
            AssetToLoot.GetComponent<LootAndDestroy>().PlayerDestroyingOrLooting = null;
            AbleToLoot = false;
            AssetToLoot = null;
        }
    }
    [Command]
    public void CmdDamageAsset()
    {
        //AssetToDestroy.GetComponent<LootAndDestroy>().PlayerDestroyingOrLooting = gameObject;
        AssetToDestroy.GetComponent<LootAndDestroy>().Interacting();
    }

    [Command]
    public void CmdLootableAmmo()
    {
       // AssetToLoot.GetComponent<LootAndDestroy>().PlayerDestroyingOrLooting = gameObject;
        AssetToLoot.GetComponent<LootAndDestroy>().Interacting();
    }

    public void ResetStats()
    {
        AbleToLoot = false;
        AbleToDestroy = false;
        AssetToLoot = null;
        AssetToDestroy = null;
    }
}
