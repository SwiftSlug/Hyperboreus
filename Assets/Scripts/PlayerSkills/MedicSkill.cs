using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicSkill : Skill {

    public override void Init()
    {
        if(cooldown == 0.0f)
        {
            //  Sets the cooldown to 5 seconds if no other value is set
            cooldown = 2.0f;
        }
    }

    public override bool SkillAction()
    {

        if (Time.time > lastUsedTime + cooldown)
        {
            Debug.Log("Cooldown is good !");
            if (buttonDownTime > 5.0f)
            {
                Debug.Log("Skill Activate");
                GetComponent<PlayerStats>().currentHealth = GetComponent<PlayerStats>().currentHealth + 50;
                lastUsedTime = Time.time;
                return true;
            }

        }
        return false;
    }
}
