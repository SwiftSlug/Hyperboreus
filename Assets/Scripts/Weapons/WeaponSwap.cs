using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponSwap : NetworkBehaviour
{

    public GameObject[] weaponSlots;

    //public bool belp;
    //public bool belp1;

    // Update is called once per frame
    void Update() {

        if (!isLocalPlayer)
        {
            return;
        }

        if (Input.GetKeyDown("1"))
        {
            //Debug.Log("1");
            if (weaponSlots[0])
            {
                //Debug.Log("Swapping to weapon 1");
                CmdChangeWeapon(0);                
            }
        }

        if (Input.GetKeyDown("2"))
        {
            //Debug.Log("2");
            if (weaponSlots[1])
            {
                //Debug.Log("Swapping to weapon 2");
                CmdChangeWeapon(1);
            }
        }

        if (Input.GetKeyDown("3"))
        {
            if (weaponSlots[2])
            {
                //Debug.Log("Swapping to weapon 3");
                CmdChangeWeapon(2);
            }
        }

        if (Input.GetKeyDown("4"))
        {
            if (weaponSlots[3])
            {
                //Debug.Log("Swapping to weapon 4");
                CmdChangeWeapon(3);
            }
        }
        if (Input.GetKeyDown("j"))
        {
            GetComponent<WeaponShooting>().equippedWeapon.SetActive(false);
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