using UnityEngine;
using UnityEngine.Networking;

public class WeaponSwitch : NetworkBehaviour
{
    [SyncVar]
    public int currentWeapon = 0;

    public bool poop;

    [SyncVar]
    private int childNumber = 0;

    //private int maxWeaponNo = 3;

    private Transform weaponSwitchTransform;

    // Use this for initialization
    void Start()
    {
        weaponSwitchTransform = gameObject.transform.GetChild(2);
        SelectWeapon();
        //CmdUpdateWeapon();
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
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            CmdCurrentWeapon(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9) && transform.childCount >= 2)
        {
            CmdCurrentWeapon(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            CmdCurrentWeapon(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4)
        {
            CmdCurrentWeapon(3);
        }

        //If a different weapon has been selected, there will be a new value for it being different to the previous...
        //If previous weapon is not the same as the current weapon value...
        if (previousWeapon != currentWeapon)
        {
            //Select that new weapon.
            SelectWeapon();

            if(isServer)
            {
                RpcSelectWeapon();
            }
            else
            {
                CmdSelectWeapon();
            }
        }

        //CmdUpdateWeapon();
    }

    void SelectWeapon()
    {
        childNumber = 0;

        //for each child transform in this transform...
        foreach (Transform weapon in weaponSwitchTransform)
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

    //void SelectWeapon()
    //{

    //    if (!isServer)
    //    {
    //        return;
    //    }

    //    for (int i = 0; i == maxWeaponNo; i++)
    //    {
    //        if (i == currentWeapon)
    //        {
    //            weaponSwitchTransform.GetChild(currentWeapon).gameObject.SetActive(true);
    //        }
    //        else
    //        {
    //            weaponSwitchTransform.GetChild(i).gameObject.SetActive(false);
    //        }
    //    }
    //}

    [Command]
    void CmdSelectWeapon()
    {
        RpcSelectWeapon();
        SelectWeapon();
    }

    [ClientRpc]
    void RpcSelectWeapon()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        SelectWeapon();
    }

    [Command]
    void CmdCurrentWeapon(int x)
    {
        if (!isServer)
        {
            return;
        }

        currentWeapon = x;
    }
}
