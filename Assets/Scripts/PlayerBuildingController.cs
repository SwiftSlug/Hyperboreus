using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBuildingController : NetworkBehaviour
{
    public bool InbuildMode = false;
    public Transform PointToSpawnStructure;
    public GameObject PlayerReference;
    public GameObject StructureSpawnerRef;
    public GameObject TempStructureGuide;
    public GameObject NetworkSpawnedStructure;

    public AudioSource audioSource;
    public AudioClip clipBuildMode;
    public AudioClip clipCombatMode;
    public AudioClip clipBuildStructure;

    //Images used for player HUD resources
    public Image WoodResourceColour;
    public Image WoodResourceNoColour;
    public Image StoneResourceColour;
    public Image StoneResourceNoColour;
    public Image MetalResourceColour;
    public Image MetalResourceNoColour;
    public Image WoodSelector;
    public Image StoneSelector;
    public Image MetalSelector;

    public bool blep = true;


    public int ServerStructureNeeded = 0;
    public int ServerMaterialNeeded = 0;
    private int localStructureNeeded = 0;
    private int localMaterialNeeded = 0;
    public int RotationNeeded = 0;
    public float RotationToSet = 0;

    public void EnterOrExitBuildMode()
    {
        if (InbuildMode == true)
        {
            InbuildMode = false;

            audioSource.PlayOneShot(clipCombatMode, 1.0f);

            WoodResourceColour.enabled = false;
            StoneResourceColour.enabled = false;
            MetalResourceColour.enabled = false;
            WoodResourceNoColour.enabled = true;
            StoneResourceNoColour.enabled = true;
            MetalResourceNoColour.enabled = true;
            WoodSelector.enabled = false;
            StoneSelector.enabled = false;
            MetalSelector.enabled = false;

            DestroyObject(TempStructureGuide);
            TempStructureGuide = null;
        }
        else
        {
            InbuildMode = true;

            if (localMaterialNeeded == 0)
            {
                WoodSelector.enabled = true;
                WoodResourceColour.enabled = true;
                WoodResourceNoColour.enabled = false;
            }
            else if (localMaterialNeeded == 1)
            {
                StoneSelector.enabled = true;
                StoneResourceColour.enabled = true;
                StoneResourceNoColour.enabled = false;
            }
            else if (localMaterialNeeded == 2)
            {
                MetalSelector.enabled = true;
                MetalResourceColour.enabled = true;
                MetalResourceNoColour.enabled = false;
            }

            audioSource.PlayOneShot(clipBuildMode, 1.0f);

            TempStructureGuide = Instantiate(StructureSpawnerRef, PointToSpawnStructure.position, PointToSpawnStructure.rotation);
            TempStructureGuide.transform.eulerAngles = new Vector3(0, RotationToSet, 0);
            TempStructureGuide.GetComponent<BuildingController>().LocalSetMaterialAndStructure(localStructureNeeded, localMaterialNeeded);
        }
    }

    [Command]
    void CmdPlaceStructure()
    {
        audioSource.PlayOneShot(clipBuildStructure, 1.0f);

        NetworkSpawnedStructure = (GameObject)Instantiate(TempStructureGuide, PointToSpawnStructure.position, TempStructureGuide.transform.rotation); //GetComponent<Transform>().rotation);
        NetworkServer.Spawn(NetworkSpawnedStructure);
        NetworkSpawnedStructure.GetComponent<BuildingController>().RpcSetMaterialAndStructure(ServerStructureNeeded, ServerMaterialNeeded);
        NetworkSpawnedStructure.GetComponent<BuildingController>().StructurePlaced = true;
    }

    [Command]
    void CmdRotateStructure(float Rotation)
    {
        TempStructureGuide.transform.eulerAngles = new Vector3(0, Rotation, 0);
    }

    void LocalRotateStructure(float Rotation)
    {
        TempStructureGuide.GetComponent<BuildingController>().LocalSetRotation(Rotation);
    }

    [Command]
    void CmdSelectStructure()
    {
        if (ServerStructureNeeded == 2)
        {
            ServerStructureNeeded = 0;
            //TempStructureGuide.GetComponent<BuildingController>().RpcSetMaterialAndStructure(StructureNeeded, MaterialNeeded);
        }
        else
        {
            ServerStructureNeeded = ServerStructureNeeded + 1;
            //TempStructureGuide.GetComponent<BuildingController>().RpcSetMaterialAndStructure(StructureNeeded, MaterialNeeded);
        }
    }

    [Command]
    void CmdSelectMaterial()
    {
        if (ServerMaterialNeeded == 2)
        {
            ServerMaterialNeeded = 0;
            //TempStructureGuide.GetComponent<BuildingController>().RpcSetMaterialAndStructure(StructureNeeded, MaterialNeeded);
            // LocalSetMaterialAndStructure(StructureNeeded, MaterialNeeded);


        }
        else
        {
            ServerMaterialNeeded = ServerMaterialNeeded + 1;
            //TempStructureGuide.GetComponent<BuildingController>().RpcSetMaterialAndStructure(StructureNeeded, MaterialNeeded);
            // LocalSetMaterialAndStructure(StructureNeeded, MaterialNeeded);
        }
    }

    void LocalSelectStructure()
    {
        if (localStructureNeeded == 2)
        {
            localStructureNeeded = 0;
            TempStructureGuide.GetComponent<BuildingController>().LocalSetMaterialAndStructure(localStructureNeeded, localMaterialNeeded);
        }
        else
        {
            localStructureNeeded = localStructureNeeded + 1;
            TempStructureGuide.GetComponent<BuildingController>().LocalSetMaterialAndStructure(localStructureNeeded, localMaterialNeeded);
        }
    }

    void LocalSelectMaterial()
    {
        if (localMaterialNeeded == 2)
        {
            localMaterialNeeded = 0;
            TempStructureGuide.GetComponent<BuildingController>().LocalSetMaterialAndStructure(localStructureNeeded, localMaterialNeeded);
            // LocalSetMaterialAndStructure(StructureNeeded, MaterialNeeded);

            WoodSelector.enabled = true;
            WoodResourceColour.enabled = true;
            WoodResourceNoColour.enabled = false;

            StoneSelector.enabled = false;
            StoneResourceColour.enabled = false;
            StoneResourceNoColour.enabled = true;
            MetalSelector.enabled = false;
            MetalResourceColour.enabled = false;
            MetalResourceNoColour.enabled = true;
        }
        else
        {
            localMaterialNeeded = localMaterialNeeded + 1;
            TempStructureGuide.GetComponent<BuildingController>().LocalSetMaterialAndStructure(localStructureNeeded, localMaterialNeeded);
            // LocalSetMaterialAndStructure(StructureNeeded, MaterialNeeded);

            if (localMaterialNeeded == 1)
            {
                StoneSelector.enabled = true;
                StoneResourceColour.enabled = true;
                StoneResourceNoColour.enabled = false;

                WoodSelector.enabled = false;
                WoodResourceColour.enabled = false;
                WoodResourceNoColour.enabled = true;
                MetalSelector.enabled = false;
                MetalResourceColour.enabled = false;
                MetalResourceNoColour.enabled = true;
            }
            else if (localMaterialNeeded == 2)
            {
                MetalSelector.enabled = true;
                MetalResourceColour.enabled = true;
                MetalResourceNoColour.enabled = false;

                WoodSelector.enabled = false;
                WoodResourceColour.enabled = false;
                WoodResourceNoColour.enabled = true;
                StoneSelector.enabled = false;
                StoneResourceColour.enabled = false;
                StoneResourceNoColour.enabled = true;
            }
        }
    }

    // Use this for initialization
    void Awake()
    {
        PlayerReference = gameObject;
        StructureSpawnerRef.GetComponent<BuildingController>().LinkedPlayer = PlayerReference;

        WoodResourceColour.enabled = false;
        StoneResourceColour.enabled = false;
        MetalResourceColour.enabled = false;
        WoodResourceNoColour.enabled = true;
        StoneResourceNoColour.enabled = true;
        MetalResourceNoColour.enabled = true;
        WoodSelector.enabled = false;
        StoneSelector.enabled = false;
        MetalSelector.enabled = false;

        //Debug.Log("I'm not using the test script");

    }

    private void Start()
    {
        //LocalSelectStructure();
        //LocalSelectMaterial();
        //LocalRotateStructure(0);

        Debug.Log("I'm working");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (TempStructureGuide != null)
        {
            TempStructureGuide.transform.position = PointToSpawnStructure.position;

        }
    }

    public void PlaceBuilding()
    {
        if ((InbuildMode == true))
        {
            switch (localMaterialNeeded)
            {
                case 0:
                    if (gameObject.GetComponent<PlayerStats>().WoodInInventory >= 10)
                    {
                        CmdPlaceStructure();
                        gameObject.GetComponent<PlayerStats>().WoodInInventory = gameObject.GetComponent<PlayerStats>().WoodInInventory - 10;
                        GetComponent<PlayerStats>().WoodText.text = "" + GetComponent<PlayerStats>().WoodInInventory;
                        GetComponent<PlayerStats>().WoodTextBackground.text = "" + GetComponent<PlayerStats>().WoodInInventory;
                    }
                    break;
                case 1:
                    if (gameObject.GetComponent<PlayerStats>().StoneInInventory >= 10)
                    {
                        CmdPlaceStructure();
                        gameObject.GetComponent<PlayerStats>().StoneInInventory = gameObject.GetComponent<PlayerStats>().StoneInInventory - 10;
                        GetComponent<PlayerStats>().StoneText.text = "" + GetComponent<PlayerStats>().StoneInInventory;
                        GetComponent<PlayerStats>().StoneTextBackground.text = "" + GetComponent<PlayerStats>().StoneInInventory;
                    }
                    break;
                case 2:
                    if (gameObject.GetComponent<PlayerStats>().MetalInInventory >= 10)
                    {
                        CmdPlaceStructure();
                        gameObject.GetComponent<PlayerStats>().MetalInInventory = gameObject.GetComponent<PlayerStats>().MetalInInventory - 10;
                        GetComponent<PlayerStats>().MetalText.text = "" + GetComponent<PlayerStats>().MetalInInventory;
                        GetComponent<PlayerStats>().MetalTextBackground.text = "" + GetComponent<PlayerStats>().MetalInInventory;
                    }
                    break;
            }
        }
    }

    public void ChangeRotation()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (InbuildMode == true)
        {
            if (RotationToSet == 360)
            {
                RotationToSet = 0;
                CmdRotateStructure(RotationToSet);
                LocalRotateStructure(RotationToSet);
            }
            else
            {
                RotationToSet = RotationToSet + 2.0f;
                CmdRotateStructure(RotationToSet);
                LocalRotateStructure(RotationToSet);
                //Debug.Log("RotationValue: " + RotationToSet);
            }
        }

    }

    public void ChangeStructure()
    {
        if (InbuildMode == true)
        {
            CmdSelectStructure();
            //LocalSelectStructure();
        }
    }

    public void ChangeMaterial()
    {
        if (InbuildMode == true)
        {
            CmdSelectMaterial();
            LocalSelectMaterial();
        }
    }

}

