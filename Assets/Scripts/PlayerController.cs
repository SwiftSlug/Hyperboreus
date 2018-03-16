using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    Vector3 playerOrientation; //Player Orientation

    [SyncVar]
    public bool AbleToDestroy = false;

    [SyncVar]
    public bool AbleToLoot = false;

    [SyncVar]
    public GameObject AssetToDestroy;

    [SyncVar]
    public GameObject AssetToLoot;

    Vector3 previousMousePos = new Vector3(0.0f, 0.0f, 0.0f);

    public float controllerDeadZone = 0.5f;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        //Debug.Log("");
        Vector3 facing = transform.forward;
        //Vector3 vel = transform.;
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        facing.y = 0;
        Debug.Log(facing);
        //Debug.Log(h);
        //Debug.Log(v);

        // if facing up 
            //if horizontal = 1 then right

    }

    //Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        /*
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition); //Cast a ray from the main camera relative to the screen position

        RaycastHit mouseRayHit; //Reference to the ray hit - used in calculating player orientation

        if (Physics.Raycast(mouseRay, out mouseRayHit, 100)) //If the Ray has hit an object
        {
            playerOrientation = mouseRayHit.point; //Make our potential player orientation equal to the location we hit with the ray cast.
        }

        Vector3 playerDirection = playerOrientation - transform.position; //Save a vector based on our players position and the saved orientation
        playerDirection.y = 0; //Get rid of our y portion of the vector because we don't need it

        

        if (Input.mousePosition != previousMousePos)
        {
            //  Only apply mouse movement if mouse has moved since last frame
            transform.LookAt(transform.position + playerDirection, Vector3.up); //Look at the mouse position in screen space
        }

        //  Save mouse position for next position update check
        previousMousePos = Input.mousePosition;
        */

        /*
        float xAxis = Input.GetAxis("Horizontal") * Time.deltaTime * 6.0f; //Get the input horizontally and save it. "W,S,Up,Down,Joystick Up,Joystick Down"
        float yAxis = Input.GetAxis("Vertical") * Time.deltaTime * 6.0f; //Get the vertical input and and save it. "A,D,Left,Right,Joystick Left, Joystick Right"

        //  Apply character movement
        transform.Translate(Vector3.forward * yAxis, Space.World); //Move horizontally within world space instead of local
        transform.Translate(Vector3.right * xAxis, Space.World); //Move vertically within world space instead of local
        */


        //  Controller Aiming
        /*
        Vector3 controllerAimingDirection = new Vector3(Input.GetAxis("ControllerLookX"), 0, Input.GetAxis("ControllerLookY"));

        if(controllerAimingDirection.magnitude > controllerDeadZone)
        {
            // Controller stick in use
            transform.LookAt(transform.position + (controllerAimingDirection * 1000));
        }
        */
        /*
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
        */
    }

    public void MouseAim()
    {

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition); //Cast a ray from the main camera relative to the screen position

        RaycastHit mouseRayHit; //Reference to the ray hit - used in calculating player orientation

        if (Physics.Raycast(mouseRay, out mouseRayHit, 100)) //If the Ray has hit an object
        {
            playerOrientation = mouseRayHit.point; //Make our potential player orientation equal to the location we hit with the ray cast.
        }

        Vector3 playerDirection = playerOrientation - transform.position; //Save a vector based on our players position and the saved orientation
        playerDirection.y = 0; //Get rid of our y portion of the vector because we don't need it



        if (Input.mousePosition != previousMousePos)
        {
            //  Only apply mouse movement if mouse has moved since last frame
            transform.LookAt(transform.position + playerDirection, Vector3.up); //Look at the mouse position in screen space
        }

        //  Save mouse position for next position update check
        previousMousePos = Input.mousePosition;

    }
    
    public void ControllerAiming()
    {
        Vector3 controllerAimingDirection = new Vector3(Input.GetAxis("ControllerLookX"), 0, Input.GetAxis("ControllerLookY"));

        if (controllerAimingDirection.magnitude > controllerDeadZone)
        {
            // Controller stick in use
            transform.LookAt(transform.position + (controllerAimingDirection * 1000));
        }
    }

    public void LootObject()
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

    public void AddHoritonzalMovement(float horizontal)
    {
        float xAxis = horizontal * Time.deltaTime * 6.0f;
        transform.Translate(Vector3.right * xAxis, Space.World);
        AnimatorH(xAxis);
    }

    public void AddVerticalMovement(float vertical)
    {
        float yAxis = vertical * Time.deltaTime * 6.0f;
        transform.Translate(Vector3.forward * yAxis, Space.World);
        AnimatorV(yAxis);
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
            if (AssetToDestroy != null)
            {
                AssetToDestroy.GetComponent<LootAndDestroy>().PlayerDestroyingOrLooting = null;
                AbleToDestroy = false;
                AssetToDestroy = null;
            }
        }
        //lootable asset end collision
        if (collidedAsset.gameObject.CompareTag("LootableContainer"))
        {
            if (AssetToLoot != null)
            {
                AssetToLoot.GetComponent<LootAndDestroy>().PlayerDestroyingOrLooting = null;
                AbleToLoot = false;
                AssetToLoot = null;
            }
        }
    }

    void AnimatorH(float horizontal)
    {
        bool left = horizontal < 0;
        animator.SetBool("left", left);
        //Debug.Log("left");

        bool right = horizontal > 0;
        animator.SetBool("right", right);
        //Debug.Log("right");
    }

    void AnimatorV(float vertical)
    {
        bool forward = vertical > 0;
        animator.SetBool("forward", forward);
        //Debug.Log("forward");

        bool backward = vertical < 0;
        animator.SetBool("backward", backward);
        //Debug.Log("backward");
    }

    [Command]
    public void CmdDamageAsset()
    {
        //AssetToDestroy.GetComponent<LootAndDestroy>().PlayerDestroyingOrLooting = gameObject;
        if (AssetToDestroy != null)
        {
            AssetToDestroy.GetComponent<LootAndDestroy>().Interacting();
        }
    }

    [Command]
    public void CmdLootableAmmo()
    {
        //AssetToLoot.GetComponent<LootAndDestroy>().PlayerDestroyingOrLooting = gameObject;
        if (AssetToLoot != null)
        {
            AssetToLoot.GetComponent<LootAndDestroy>().Interacting();
        }
    }

    [Command]
    public void CmdResetStats()
    {
        AbleToLoot = false;
        AbleToDestroy = false;
        AssetToLoot = null;
        AssetToDestroy = null;
        RpcResetStats();
    }

    [ClientRpc]
    public void RpcResetStats()
    {
        AbleToLoot = false;
        AbleToDestroy = false;
        AssetToLoot = null;
        AssetToDestroy = null;
    }
}
