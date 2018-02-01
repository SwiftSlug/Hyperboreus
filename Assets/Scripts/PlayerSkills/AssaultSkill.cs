using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AssaultSkill : Skill
{
    //  Time it takes to charge the syringe shot
    public float chargeTime;

    // Time taken before airstrike hits
    public float airstrikeTime;

    //  The current time spent charging the ability
    float currentChargeTime;

    Transform gunPos;

    //void Update()
    //{
    //    Debug.Log("INIT");
    //    Vector3 mousePosUpdate = Input.mousePosition;
    //}

    public override void Init()
    {
        Debug.Log("INIT");

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

        //gunPos = GetComponent<WeaponShooting>().gunEnd;
    }

    public override bool SkillAction()
    {
        if (isLocalPlayer)
        {
            if (Time.time > lastUsedTime + cooldown)
            {
                currentChargeTime += Time.deltaTime;

                //draw marker on mouse location update
                Vector3 mousePosUpdate = Input.mousePosition;
                Debug.Log("drawing marker");

                if (currentChargeTime > chargeTime)
                {
                    //get mouse location and draw marker at mouse location
                    Vector3 mousePos = Input.mousePosition;
                    //wait for air strike time
                    //delete marker


                    CmdSpawnStrike();
                    Debug.Log("BOOM");
                    currentChargeTime = 0.0f;   //  Reset the current charge time
                    lastUsedTime = Time.time;   //  Set last firing time
                    return true;
                }
            }
        }
        return false;
    }

    public override void ButtonRelease()
    {
        currentChargeTime = 0.0f;       //  Reset the current charge time  
        Debug.Log("release");
        //Disable marker after airstrike fires
    }

    //edit for missile from set z value above and so its facing down and velocity is facing down
    [Command]
    void CmdSpawnStrike()
    {
        Debug.Log("strike");
        //GameObject syringe = Resources.Load("MedicalSyringe", typeof(GameObject)) as GameObject;

        //GameObject syringeRef = Instantiate(syringe, spawnPosition, spawnRotation);

        ////  Assign player reference on scripts
        //syringeRef.GetComponentInChildren<MedicalSyringeScript>().player = currentPlayerReference;
        //syringeRef.GetComponentInChildren<trackingSphereScript>().player = currentPlayerReference;

        //NetworkServer.Spawn(syringeRef);
    }

    //if missile hits something damagable, then damage
    //else explode on hit
    //on explode do small shere trace from hit location with less damage
    //repeat sphere trace getting bigger, but less damage
}