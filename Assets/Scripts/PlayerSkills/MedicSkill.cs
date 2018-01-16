using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicSkill : Skill {

    //LineRenderer laser = gameobject.AddComponent<LineRenderer>();

    //  Line renderer used to represent the laser sight of the syringe
    LineRenderer laser;
    //  Time it takes to charge the syringe shot
    public float chargeTime;    


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

        laser = playerReference.AddComponent<LineRenderer>();
        laser.enabled = false;
    }

    public override bool SkillAction()
    {

        if (Time.time > lastUsedTime + cooldown)
        {
            laser.SetPosition(0, playerReference.transform.position);           //  Sets laser start location to player
            laser.SetPosition(1, playerReference.transform.forward * 1000);     //  Sets laser end to 1000 in front of player
            laser.enabled = true;   //  Turn laser on

            if (buttonDownTime > chargeTime)
            {
                Debug.Log("Skill Activate");
                GetComponent<PlayerStats>().currentHealth = GetComponent<PlayerStats>().currentHealth + 50;

                laser.enabled = false;  //  Turn laser off
                lastUsedTime = Time.time;
                return true;
            }

        }
        return false;
    }

    public override void buttonRelease()
    {
        laser.enabled = false;  //  Turn laser off
        Debug.Log("Button released");
    }

}
