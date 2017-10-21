using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{

    public int currentWeapon = 0;

    // Use this for initialization
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
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
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        //for each child transform in this transform...
        foreach (Transform weapon in transform)
        {
            //Sets the current weapon to active..
            if (i == currentWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            //Disables any other weapon.
            else
            {
                weapon.gameObject.SetActive(false);
            }
            //increment i to check through each weapon.
            i++;
        }
    }
}
