using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    //  This class handles all of the player input for the players within the game
    //  It detects the input from the player and then calls the corresponding functionality
    //  from the desired scripts attached to the player

    /*
    public bool blep = true;
    public bool blep1 = true;
    public bool blep2 = true;
    public bool blep3 = true;
    public bool blep4 = true;
    */

    WeaponShooting weaponShootingScript;
    WeaponSwap weaponSwapScript;
    PlayerSkills playerSkillScript;
    PlayerController playerControllerScript;


	// Use this for initialization
	void Start () {
        weaponShootingScript = GetComponent<WeaponShooting>();
        weaponSwapScript = GetComponent<WeaponSwap>();
        playerSkillScript = GetComponent<PlayerSkills>();
        playerControllerScript = GetComponent<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {

        //  Movement -----------------------------------------------------------

        //  Horizontal Input
        if (Input.GetAxis("Horizontal") != 0)
        {
            playerControllerScript.AddHoritonzalMovement(Input.GetAxis("Horizontal"));
        }

        //  Vertical Input
        if (Input.GetAxis("Vertical") != 0)
        {
            playerControllerScript.AddVerticalMovement(Input.GetAxis("Vertical"));
        }


        //  Aiming -------------------------------------------------------------





        //  Interaction --------------------------------------------------------

        if (Input.GetButton("Interact"))
        {
            playerControllerScript.Interact();

        }


        //  Skill abilities ---------------------------------------------------

        //  Skill 1
        if (Input.GetButton("Skill1"))
        {
            playerSkillScript.SkillButtonDown();
        }
        else
        {
            playerSkillScript.SkillButtonUp();
        }


        //  Weapon handling ----------------------------------------------------

        //  Reloading
        if (Input.GetButton("Reload"))
        {
            weaponShootingScript.StartReload();
        }

        //  Shooting
        if (Input.GetButton("Fire1") || (Input.GetAxis("Fire1") > 0))
        {
            weaponShootingScript.StartShoot();
        }

        //  Weapon swapping

        //  Weapon 1
        if ((Input.GetButton("Weapon1")) || (Input.GetAxis("Weapon1") > 0))
        {
            weaponSwapScript.changeWeapon(0);
        }

        //  Weapon 2
        if ((Input.GetButton("Weapon2")) || (Input.GetAxis("Weapon2") > 0))
        {
            weaponSwapScript.changeWeapon(1);
        }

        //  Weapon 3
        if ((Input.GetButton("Weapon3")) || (Input.GetAxis("Weapon3") > 0))
        {
            weaponSwapScript.changeWeapon(2);
        }

        //  Weapon 4 - Not currently in use
        /*
        if ((Input.GetButton("Weapon3")) || (Input.GetAxis("Weapon3") > 0))
        {
            weaponSwapScript.changeWeapon(3);
        }
        */

        




    }
}
