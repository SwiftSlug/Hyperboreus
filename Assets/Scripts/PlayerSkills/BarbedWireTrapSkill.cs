using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BarbedWireTrapSkill : Skill
{
    //  Time it takes to charge the barbed wire trap
    public float chargeTime;
    //  The current time spent charging the barbed wire trap
    float currentChargeTime;

    public Transform TrapPos;

    //public bool belp;

    public override void Init()
    {
        if (cooldown == 0.0f)
        {
            //  Sets the cooldown to 2 seconds if no other value is set
            cooldown = 2.0f;
        }
        if (chargeTime == 0.0f)
        {
            //  Sets the chargetime to 2 seconds if no other value is set
            chargeTime = 1.0f;
        }
        playerReference = this.gameObject;

        //        gunPos = GetComponent<WeaponShooting>().gunEnd;
    }

    public override bool SkillAction()
    {
        if (isLocalPlayer)
        {
            if (Time.time > lastUsedTime + cooldown)
            {
                //laser.SetPosition(0, gunPos.position);           //  Sets laser start location to player
                //laser.SetPosition(1, transform.forward * 1000);     //  Sets laser end to 1000 in front of player

                //laser.enabled = true;   //  Turn laser on

                //  Increase the charge time
                currentChargeTime += Time.deltaTime;

                if (currentChargeTime > chargeTime)
                {
                    // Fire a syringe
                    //if (syringe)
                    //{
                    //GameObject syringeRef = Instantiate(syringe, transform.position + (transform.forward * 5.0f ), transform.rotation);

                    //Debug.Log("New Syringe spawned");

                    /*
                    //Create the syringe game object
                    GameObject syringeRef = Instantiate(syringe, gunPos.position, transform.rotation);

                    //Assign player reference on scripts
                    syringeRef.GetComponentInChildren<MedicalSyringeScript>().player = playerReference;
                    syringeRef.GetComponentInChildren<trackingSphereScript>().player = playerReference;
                    */

                    CmdSpawnBarbedWireTrap(TrapPos.position, transform.rotation); //, playerReference);
                    //}

                    currentChargeTime = 0.0f;   //  Reset the current charge time
                    lastUsedTime = Time.time;   //  Set last firing time
                    return true;
                }


            }

        }
        return false;
    }

    public override void buttonRelease()
    {
        currentChargeTime = 0.0f;       //  Reset the current charge time
        //laser.enabled = false;          //  Turn laser off
        //Debug.Log("Button released");
    }

    [Command]
    void CmdSpawnBarbedWireTrap(Vector3 spawnPosition, Quaternion spawnRotation) //, GameObject currentPlayerReference)
    {
        GameObject BarbedWireTrap = Resources.Load("BarbedWireTrap", typeof(GameObject)) as GameObject;

        GameObject BarbedWireTrapRef = Instantiate(BarbedWireTrap, spawnPosition, spawnRotation);

        //  Assign player reference on scripts
        //BarbedWireTrapRef.GetComponentInChildren<MedicalSyringeScript>().player = currentPlayerReference;
        //syringeRef.GetComponentInChildren<trackingSphereScript>().player = currentPlayerReference;

        NetworkServer.Spawn(BarbedWireTrapRef);
    }

}

