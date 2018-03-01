using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponSwap : NetworkBehaviour
{
    public bool blep = true;
    public bool blep2 = true;
    public bool blep3 = true;
    public bool blep4 = true;

    public GameObject[] weaponSlots;

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

    public void changeWeapon(int weaponNumber)
    {
        if (weaponSlots[weaponNumber])
        {
            //Debug.Log("Swapping to weapon 1");
            CmdChangeWeapon(weaponNumber);
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
        var weaponShootingScript = GetComponent<WeaponShooting>();

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