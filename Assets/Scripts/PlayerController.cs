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
    public bool AbleToInteractStruc = false;

    [SyncVar]
    public GameObject AssetToDestroy;

    [SyncVar]
    public GameObject AssetToLoot;

    [SyncVar]
    public GameObject StructureToInteract;

    public int tempStructure;

    Vector3 previousMousePos = new Vector3(0.0f, 0.0f, 0.0f);

    public float controllerDeadZone = 0.5f;

    Animator animator;

    Transform cam;

    Vector3 forward;

    Vector3 move;

    Vector3 moveInput;

    [SyncVar]
    float yValue;

    [SyncVar]
    float xValue;

    void Start()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        //Debug.Log("");
        //Vector3 facing = transform.forward;
        //Vector3 vel = transform.;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //facing.y = 0;
        //Debug.Log(facing);
        //Debug.Log(h);
        //Debug.Log(v);

        if (h == 0 & v == 0)
        {
            animator.SetBool("moving", false);
        }
        else
        {
            animator.SetBool("moving", true);
        }

        if (cam != null)
        {
            forward = Vector3.Scale(cam.up, new Vector3(1, 0, 1)).normalized;
            move = v * forward + h * cam.right;
        }
        else
        {
            move = v * Vector3.forward + h * Vector3.right;
        }

        if (move.magnitude > 1)
        {
            move.Normalize();
        }

        Move (move);
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
            AbleToDestroy = false;
            DamageAsset();
        }
        if (AbleToLoot == true)
        {
            AbleToLoot = false;
            LootableAmmo();
        }
    }

    public void StructureInteraction()
    {
        if (AbleToInteractStruc == true)
        {
            int tempMaterial;
            tempMaterial = StructureToInteract.GetComponent<BuildingController>().LocalNeededMaterial;
            tempStructure = StructureToInteract.GetComponent<BuildingController>().LocalNeededStructure;

            Debug.Log("Temp structure value: " + tempStructure);

            switch (tempStructure)
            {
                case 0:
                    StructureToInteract.GetComponent<BuildingController>().CmdDamage(500);
                    StructureToInteract = null;
                    AbleToInteractStruc = false;
                    break;
                case 1:
                    //Debug.Log("PlayerController: InteractWithGate");
                   // Debug.Log("Animating gate...");
                    break;
            }            
        }
    }

    [Command]
    public void CmdDestroyResource()
    {
        if (AssetToDestroy != null)
        {
            Destroy(AssetToDestroy, AssetToDestroy.GetComponent<LootAndDestroy>().audioSync.clipArray[1].length);
            AssetToDestroy.GetComponent<LootAndDestroy>().audioSync.ResetSound();
        }
    }

    public void AddHoritonzalMovement(float horizontal)
    {
        float xAxis = horizontal * Time.deltaTime * 6.0f;
        transform.Translate(Vector3.right * xAxis, Space.World);
    }

    public void AddVerticalMovement(float vertical)
    {
        float yAxis = vertical * Time.deltaTime * 6.0f;
        transform.Translate(Vector3.forward * yAxis, Space.World);
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

    private void OnTriggerEnter(Collider collidedAsset)
    {
        if (collidedAsset.gameObject.CompareTag("Structure"))
        {
            Debug.Log("you're colliding with a structure");
            AbleToInteractStruc = true;
            StructureToInteract = collidedAsset.gameObject;
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

    private void OnTriggerExit(Collider collidedAsset)
    {
        if (collidedAsset.gameObject.CompareTag("Structure"))
        {
            if (StructureToInteract != null)
            {
                Debug.Log("you're no longer colliding with a structure");
                AbleToInteractStruc = false;
                StructureToInteract = null;
            }
        }
    }

    public void DamageAsset()
    {
        if (AssetToDestroy != null)
        {
            AssetToDestroy.GetComponent<LootAndDestroy>().Interacting();
        }
    }

    public void LootableAmmo()
    {
        if (AssetToLoot != null)
        {
            AssetToLoot.GetComponent<LootAndDestroy>().Interacting();
        }
    }

    void Move(Vector3 move)
    {
        if (move.magnitude > 1)
        {
            move.Normalize();
        }

        this.moveInput = move;

        ConvertMovementInput();
        UpdateAnimator();
    }

    //converts movement input depending on player rotation for animations
    void ConvertMovementInput()
    {
        Vector3 localMove = transform.InverseTransformDirection(moveInput);
        xValue = localMove.x;
        yValue = localMove.z;
    }
    
    //updates animator on player movement animation values
    void UpdateAnimator()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        animator.SetFloat("velX", xValue, 0.1f, Time.deltaTime);
        animator.SetFloat("velY", yValue, 0.1f, Time.deltaTime);
    }
}
