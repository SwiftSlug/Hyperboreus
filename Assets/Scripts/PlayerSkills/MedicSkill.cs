using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicSkill : Skill {

    //  Line renderer used to represent the laser sight of the syringe
    LineRenderer laser;
    //  Time it takes to charge the syringe shot
    public float chargeTime;
    //  The current time spent charging the ability
    float currentChargeTime;

    //  The game object used to instantiate the fired syringe
    GameObject syringe;

    public override void Init()
    {
        if(cooldown == 0.0f)
        {
            //  Sets the cooldown to 2 seconds if no other value is set
            cooldown = 2.0f;
        }
        if(chargeTime == 0.0f)
        {
            //  Sets the chargetime to 2 seconds if no other value is set
            chargeTime = 1.0f;
        }
        
        //  Add the line renderer component to the player
        laser = playerReference.AddComponent<LineRenderer>();
        //  Set the laser to default off
        laser.enabled = false;
        //  Load the syringe gameobject
        syringe = Instantiate(Resources.Load("MedicalSyringe", typeof(GameObject))) as GameObject;

    }

    public override bool SkillAction()
    {
        if (Time.time > lastUsedTime + cooldown)
        {
            laser.SetPosition(0, playerReference.transform.position);           //  Sets laser start location to player
            laser.SetPosition(1, playerReference.transform.forward * 1000);     //  Sets laser end to 1000 in front of player
            laser.enabled = true;   //  Turn laser on

            //  Increase the charge time
            currentChargeTime += Time.deltaTime;

            if (currentChargeTime > chargeTime)
            {
                //  Fire a syringe
                if (syringe)
                {
                    //  Create the syringe game object
                    Instantiate(syringe, transform.position, transform.rotation);
                }
                
                currentChargeTime = 0.0f;   //  Reset the current charge time
                laser.enabled = false;      //  Turn laser off
                lastUsedTime = Time.time;   //  Set last firing time
                return true;    
            }

            
        }
        return false;
    }

    public override void buttonRelease()
    {
        currentChargeTime = 0.0f;       //  Reset the current charge time
        laser.enabled = false;          //  Turn laser off
        //Debug.Log("Button released");
    }

}
