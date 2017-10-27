using UnityEngine;
using UnityEngine.Networking;

public class WeaponSwitch : NetworkBehaviour
{
    [SyncVar]
    public int currentWeapon = 0;
    public int childNumber;

    private Transform weaponSwtitchObj;

    // Use this for initialization
    void Start()
    {
        weaponSwtitchObj = this.gameObject.transform.GetChild(2);
        CmdSelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        //int for previous weapon number set to current weapon so there is always a current weapon value.
        int previousWeapon = currentWeapon;

        //Key presses for weapon switching 1-4.
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            currentWeapon = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            currentWeapon = 2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4)
        {
            currentWeapon = 3;
        }

        //If a different weapon has been selected, there will be a new value for it being different to the previous...
        //If previous weapon is not the same as the current weapon value...
        if (previousWeapon != currentWeapon)
        {
            //Select that new weapon.
            CmdSelectWeapon();
        }
    }

    [Command]
    void CmdSelectWeapon()
    {
        childNumber = 0;
        //for each child transform in this transform...
        foreach (Transform weapon in weaponSwtitchObj)
        {
            //Sets the current weapon to active..
            if (childNumber == currentWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            //Disables any other weapon.
            else
            {
                weapon.gameObject.SetActive(false);
            }
            //increment i to check through each weapon.
            childNumber++;
        }

    }

    //[ClientRpc]
    //void RpcsetWeaponActive(childNumber)
    //{
    //    weaponSwtitchObj.GetChild().gameObject.SetActive(true);
    //}

    //[ClientRpc]
    //void RpcsetWeaponNotActive(childNumber)
    //{
    //    weaponSwtitchObj.GetChild().gameObject.SetActive(false);
    //}
}
