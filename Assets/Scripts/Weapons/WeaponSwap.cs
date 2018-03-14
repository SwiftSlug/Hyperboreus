using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponSwap : NetworkBehaviour
{
    public GameObject[] weaponSlots;

    public WeaponShooting weaponShootingScript;

    private void Start()
    {
        weaponShootingScript = GetComponent<WeaponShooting>();
    }

    // Update is called once per frame
    void Update() {

        if (!isLocalPlayer)
        {
            return;
        }

        // Keyboard bindings for buttons ----------------------
        /*
        if ( (Input.GetButton("Weapon1")) || (Input.GetAxis("Weapon1") > 0) )
        {
            //Debug.Log("1");
            if (weaponSlots[0])
            {
                //Debug.Log("Swapping to weapon 1");
                CmdChangeWeapon(0);                
            }
        }
        */
        /*
        if ( (Input.GetButton("Weapon2")) || (Input.GetAxis("Weapon2") > 0) )
        {
            //Debug.Log("2");
            if (weaponSlots[1])
            {
                //Debug.Log("Swapping to weapon 2");
                CmdChangeWeapon(1);
            }
        }
        */
        /*
        if ( (Input.GetButton("Weapon3")) || (Input.GetAxis("Weapon3") > 0) )
        {
            if (weaponSlots[2])
            {
                //Debug.Log("Swapping to weapon 3");
                CmdChangeWeapon(2);
            }
        }
        */
        /*
        if ((Input.GetButton("Weapon4")) || (Input.GetAxis("Weapon4") > 0))
        {
            if (weaponSlots[3])
            {
                //Debug.Log("Swapping to weapon 4");
                CmdChangeWeapon(3);
            }
        }
        */
        /*
        if (Input.GetKeyDown("j"))
        {
            GetComponent<WeaponShooting>().equippedWeapon.SetActive(false);
        }
        */
    }

    public void ChangeWeapon(int weaponNumber)
    {            
        //  Only run if the selected weapon slot has a weapon in it
        if (weaponSlots[weaponNumber] != null)
        {
            //  Check if the player already has a weapon equipped
            if(weaponShootingScript.selectedWeapon)
            {
                //  Player has a weapon equipped so check for reloading
                if (weaponShootingScript.selectedWeapon.reloading != true)
                {
                    //  Player not reloading, continue with weapon change
                    CmdChangeWeapon(weaponNumber);
                }
            }
            //  No weapon equipped so skip the reload check
            else
            {                
                CmdChangeWeapon(weaponNumber);
            }
            
        }
    }


    [Command]
    void CmdChangeWeapon(int weaponSlot)
    {
        RpcUpdateClients(weaponSlot);
    }

    [ClientRpc]
    void RpcUpdateClients(int weaponSlot)
    {
        //  Get reference to weapon script
        //var weaponShootingScript = GetComponent<WeaponShooting>();

        //  Hide current weapon
        if (weaponShootingScript.equippedWeapon)
        {
            weaponShootingScript.equippedWeapon.SetActive(false);
            //weaponShootingScript.equippedWeapon = null;
        }
        //  Show new weapon
        weaponShootingScript.equippedWeapon = weaponSlots[weaponSlot];
        weaponShootingScript.equippedWeapon.SetActive(true);
        weaponShootingScript.shootInit();
    }

}