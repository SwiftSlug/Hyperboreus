using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LootAndDestroy : NetworkBehaviour
{
    [SyncVar]
    public int DestroyOrLoot; //0 = Destroy, 1 = Loot

    //Shared Variables
    public GameObject PlayerDestroyingOrLooting = null;
    //looting variables
    [SyncVar]
    public int AmmoType = 0; // 0 = pistol, 1 = rifle, 2 = shotgun, 3 = sniper, 4 = rocket ammo
    [SyncVar]
    public int AmountOfAmmoToDrop = 0;
    //destroy variables
    [SyncVar]
    public int ResourceType = 0; //0 = wood, 1 = stone, 2 = metal
    [SyncVar]
    public int AmountOfResourceToDrop = 0;

    public bool blep = false;
    // Use this for initialization

    public AudioSync audioSync;

    void Start()
    {
        audioSync = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioSync>();
    }

    [Command]
    public void CmdDestroyObject() // destroy object on server
    {
        Destroy(this.gameObject, audioSync.clipArray[1].length);
        audioSync.ResetSound();
    }

    [ClientRpc]
    public void RpcDestroyObject() // destroy object on server
    {
        Destroy(this.gameObject, audioSync.clipArray[1].length);
        audioSync.ResetSound();

        CmdDestroyObject();
    }
    public void Interacting()
    {
        if (DestroyOrLoot == 0)
        {
            audioSync.PlaySound(this.gameObject, 1);

            switch (ResourceType)
            {
                case 0: // wood
                    PlayerDestroyingOrLooting.GetComponent<PlayerStats>().WoodInInventory = PlayerDestroyingOrLooting.GetComponent<PlayerStats>().WoodInInventory + AmountOfResourceToDrop;
                    PlayerDestroyingOrLooting.GetComponent<PlayerStats>().WoodText.text = "" + PlayerDestroyingOrLooting.GetComponent<PlayerStats>().WoodInInventory;
                    PlayerDestroyingOrLooting.GetComponent<PlayerStats>().WoodTextBackground.text = "" + PlayerDestroyingOrLooting.GetComponent<PlayerStats>().WoodInInventory;
                    PlayerDestroyingOrLooting.GetComponent<PlayerController>().CmdDestroyResource();
                    RpcDestroyObject();
                    break;
                case 1: //stone
                    PlayerDestroyingOrLooting.GetComponent<PlayerStats>().StoneInInventory = PlayerDestroyingOrLooting.GetComponent<PlayerStats>().StoneInInventory + AmountOfResourceToDrop;
                    PlayerDestroyingOrLooting.GetComponent<PlayerStats>().StoneText.text = "" + PlayerDestroyingOrLooting.GetComponent<PlayerStats>().StoneInInventory;
                    PlayerDestroyingOrLooting.GetComponent<PlayerStats>().StoneTextBackground.text = "" + PlayerDestroyingOrLooting.GetComponent<PlayerStats>().StoneInInventory;
                    PlayerDestroyingOrLooting.GetComponent<PlayerController>().CmdDestroyResource();
                    break;
                case 2: //metal
                    PlayerDestroyingOrLooting.GetComponent<PlayerStats>().MetalInInventory = PlayerDestroyingOrLooting.GetComponent<PlayerStats>().MetalInInventory + AmountOfResourceToDrop;
                    PlayerDestroyingOrLooting.GetComponent<PlayerStats>().MetalText.text = "" + PlayerDestroyingOrLooting.GetComponent<PlayerStats>().MetalInInventory;
                    PlayerDestroyingOrLooting.GetComponent<PlayerStats>().MetalTextBackground.text = "" + PlayerDestroyingOrLooting.GetComponent<PlayerStats>().MetalInInventory;
                    PlayerDestroyingOrLooting.GetComponent<PlayerController>().CmdDestroyResource();
                    break;
            }
        }
        else if (DestroyOrLoot == 1)
        {
            audioSync.PlaySound(this.gameObject, 0);

            switch (AmmoType)
            {
                case 0: //pistol
                    PlayerDestroyingOrLooting.GetComponent<PlayerStats>().pistolAmmo = PlayerDestroyingOrLooting.GetComponent<PlayerStats>().pistolAmmo + AmountOfAmmoToDrop; // give 
                    CmdDestroyObject();
                    break;
                case 1: //rifle
                    PlayerDestroyingOrLooting.GetComponent<PlayerStats>().rifleAmmo = PlayerDestroyingOrLooting.GetComponent<PlayerStats>().rifleAmmo + AmountOfAmmoToDrop;
                    CmdDestroyObject();
                    break;
                case 2: // shotgun
                    PlayerDestroyingOrLooting.GetComponent<PlayerStats>().shotgunAmmo = PlayerDestroyingOrLooting.GetComponent<PlayerStats>().shotgunAmmo + AmountOfAmmoToDrop;
                    CmdDestroyObject();
                    break;
                case 3: //sniper
                    PlayerDestroyingOrLooting.GetComponent<PlayerStats>().sniperAmmo = PlayerDestroyingOrLooting.GetComponent<PlayerStats>().sniperAmmo + AmountOfAmmoToDrop;
                    CmdDestroyObject();
                    break;
                case 4: //rocket launcher
                    PlayerDestroyingOrLooting.GetComponent<PlayerStats>().rocketAmmo = PlayerDestroyingOrLooting.GetComponent<PlayerStats>().rocketAmmo + AmountOfAmmoToDrop;
                    CmdDestroyObject();
                    break;
            }
        }

    }
}

