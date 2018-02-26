using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LootAndDestroy : NetworkBehaviour
{
    public int DestroyOrLoot; //0 = Destroy, 1 = Loot

    //Shared Variables
    public GameObject PlayerDestroyingOrLooting = null;
    //looting variables
    public int AmmoType = 0; // 0 = pistol, 1 = rifle, 2 = shotgun, 3 = sniper, 4 = rocket ammo
    public int AmountOfAmmoToDrop = 0;
    //destroy variables
    public int ResourceType = 0; //0 = wood, 1 = stone, 2 = metal
    public int AmountOfResourceToDrop = 0;
    // Use this for initialization

    public AudioSync audioSync;

    void Start()
    {
        audioSync = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioSync>();
    }

    [Command]
    public void CmdDestroy() // destroy object on server
    {
        NetworkServer.Destroy(gameObject);
        Destroy(gameObject);
    }

    public void Interacting()
    {
        switch (DestroyOrLoot)
        {
            case 0: //Destroy
                audioSync.PlaySound(this.gameObject, 1);

                switch (ResourceType)
                {
                    case 0: // wood
                        PlayerDestroyingOrLooting.GetComponent<PlayerStats>().WoodInInventory = PlayerDestroyingOrLooting.GetComponent<PlayerStats>().WoodInInventory + AmountOfResourceToDrop;
                        ResetAllValuesAndDestroy();
                        break;
                    case 1: //stone
                        PlayerDestroyingOrLooting.GetComponent<PlayerStats>().StoneInInventory = PlayerDestroyingOrLooting.GetComponent<PlayerStats>().StoneInInventory + AmountOfResourceToDrop;
                        ResetAllValuesAndDestroy();
                        break;
                    case 2: //metal
                        PlayerDestroyingOrLooting.GetComponent<PlayerStats>().MetalInInventory = PlayerDestroyingOrLooting.GetComponent<PlayerStats>().MetalInInventory + AmountOfResourceToDrop;
                        ResetAllValuesAndDestroy();
                        break;
                }
                break;
            case 1: //Loot
                audioSync.PlaySound(this.gameObject, 0);

                switch (AmmoType)
                {
                    case 0: //pistol
                        PlayerDestroyingOrLooting.GetComponent<PlayerStats>().pistolAmmo = PlayerDestroyingOrLooting.GetComponent<PlayerStats>().pistolAmmo + AmountOfAmmoToDrop; // give 
                        ResetAllValuesAndDestroy();
                        break;
                    case 1: //rifle
                        PlayerDestroyingOrLooting.GetComponent<PlayerStats>().rifleAmmo = PlayerDestroyingOrLooting.GetComponent<PlayerStats>().rifleAmmo + AmountOfAmmoToDrop;
                        ResetAllValuesAndDestroy();
                        break;
                    case 2: // shotgun
                        PlayerDestroyingOrLooting.GetComponent<PlayerStats>().shotgunAmmo = PlayerDestroyingOrLooting.GetComponent<PlayerStats>().shotgunAmmo + AmountOfAmmoToDrop;
                        ResetAllValuesAndDestroy();
                        break;
                    case 3: //sniper
                        PlayerDestroyingOrLooting.GetComponent<PlayerStats>().sniperAmmo = PlayerDestroyingOrLooting.GetComponent<PlayerStats>().sniperAmmo + AmountOfAmmoToDrop;
                        ResetAllValuesAndDestroy();
                        break;
                    case 4: //rocket launcher
                        PlayerDestroyingOrLooting.GetComponent<PlayerStats>().rocketAmmo = PlayerDestroyingOrLooting.GetComponent<PlayerStats>().rocketAmmo + AmountOfAmmoToDrop;
                        ResetAllValuesAndDestroy();
                        break;
                }
                break;
        }
    }

    public void ResetAllValuesAndDestroy()
    {
        //player reset
        PlayerDestroyingOrLooting.GetComponent<PlayerController>().ResetStats();
        //self destroy
        PlayerDestroyingOrLooting = null;

        //CmdDestroy();
    }
}
