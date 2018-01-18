using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DestructibleAttributes : NetworkBehaviour
{

    public int MaterialType; // 0 = wood, 1 = stone, 2 = metal
    public int AmountToDrop;
    public int NeededHits;
    public int HitCounter;
    public bool ResourceUsed = false;
    public GameObject PlayerDestroying;

    [ClientRpc]
    void RpcDestroyAsset()
    {
        Network.Destroy(gameObject);
        Destroy(gameObject);
    }

    [Command]
    void CmdDestroyAsset()
    {
        NetworkServer.Destroy(gameObject);
        Destroy(gameObject);
        RpcDestroyAsset();
    }

    public void LocalDestroyAsset()
    {
        CmdDestroyAsset();
    }

    void Start()
    {
    }

    private void Update()
    {
    }

    public void HitCountIncreaseAndCheck()
    {
        HitCounter = HitCounter + 1;
        if (HitCounter >= NeededHits)
        {
            switch (MaterialType)
            {
                case 0:
                    PlayerDestroying.GetComponent<PlayerStats>().WoodInInventory = PlayerDestroying.GetComponent<PlayerStats>().WoodInInventory + AmountToDrop;
                    PlayerDestroying.GetComponent<PlayerController>().AbleToDestroy = false;
                    PlayerDestroying.GetComponent<PlayerController>().AssetToDestroy = null;
                    ResourceUsed = true;
                    //CmdDestroyAsset();
                    break;
                case 1:
                    PlayerDestroying.GetComponent<PlayerStats>().StoneInInventory = PlayerDestroying.GetComponent<PlayerStats>().StoneInInventory + AmountToDrop;
                    PlayerDestroying.GetComponent<PlayerController>().AbleToDestroy = false;
                    PlayerDestroying.GetComponent<PlayerController>().AssetToDestroy = null;
                    ResourceUsed = true;
                    //CmdDestroyAsset();
                    break;
                case 2:
                    PlayerDestroying.GetComponent<PlayerStats>().MetalInInventory = PlayerDestroying.GetComponent<PlayerStats>().MetalInInventory + AmountToDrop;
                    PlayerDestroying.GetComponent<PlayerController>().AbleToDestroy = false;
                    PlayerDestroying.GetComponent<PlayerController>().AssetToDestroy = null;
                    ResourceUsed = true;
                    //CmdDestroyAsset();
                    break;
            }
        }
    }
}