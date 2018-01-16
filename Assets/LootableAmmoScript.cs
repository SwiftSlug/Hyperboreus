using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LootableAmmoScript : NetworkBehaviour
{

    public int AmmoType; // 0 = pistol, 1 = rifle, 2 = sniper, 3 = rocket ammo
    public int AmountToDrop;
    public int NeededHits;
    public int HitCounter;
    public GameObject PlayerLooting;
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
            switch (AmmoType)
            {
                case 0:
                    PlayerLooting.GetComponent<PlayerStats>().pistolAmmo = PlayerLooting.GetComponent<PlayerStats>().pistolAmmo + AmountToDrop;
                    RpcDestroyAsset();
                    break;
                case 1:
                    PlayerLooting.GetComponent<PlayerStats>().rifleAmmo = PlayerLooting.GetComponent<PlayerStats>().rifleAmmo + AmountToDrop;
                    RpcDestroyAsset();
                    break;
                case 2:
                    PlayerLooting.GetComponent<PlayerStats>().shotgunAmmo = PlayerLooting.GetComponent<PlayerStats>().shotgunAmmo + AmountToDrop;
                    RpcDestroyAsset();
                    break;
                case 3:
                    PlayerLooting.GetComponent<PlayerStats>().sniperAmmo = PlayerLooting.GetComponent<PlayerStats>().sniperAmmo + AmountToDrop;
                    RpcDestroyAsset();
                    break;
                case 4:
                    PlayerLooting.GetComponent<PlayerStats>().rocketAmmo = PlayerLooting.GetComponent<PlayerStats>().rocketAmmo + AmountToDrop;
                    RpcDestroyAsset();
                    break;
            }
        }
    }
}