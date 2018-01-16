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
    public GameObject PlayerDestroying;
    public GameObject NetworkSpawnedAsset;
    public bool Spawned = false;


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


    void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown("*"))
        {
            RpcDestroyAsset();
        }
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
                    RpcDestroyAsset();
                    break;
                case 1:
                    PlayerDestroying.GetComponent<PlayerStats>().StoneInInventory = PlayerDestroying.GetComponent<PlayerStats>().StoneInInventory + AmountToDrop;
                    RpcDestroyAsset();
                    break;
                case 2:
                    PlayerDestroying.GetComponent<PlayerStats>().MetalInInventory = PlayerDestroying.GetComponent<PlayerStats>().MetalInInventory + AmountToDrop;
                    RpcDestroyAsset();
                    break;
            }
        }
    }
}