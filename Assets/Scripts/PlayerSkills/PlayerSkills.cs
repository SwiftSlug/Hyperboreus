using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSkills : NetworkBehaviour
{
    //  Current equipped player skill
    public Skill playerSkill;

    //  Flags if the ability button is held down
    bool skillButtonDown;

	void Start () {
        //  Finds the skill attached to the player and assigns that to its equipped skill
        playerSkill = GetComponent<Skill>();
        //  Calls the init function of the skill, if needed
        playerSkill.Init();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButton("Skill1"))
        {
            skillButtonDown = true;
            playerSkill.buttonDownTime = playerSkill.buttonDownTime + Time.deltaTime;
        }
        else
        {
            skillButtonDown = false;
            playerSkill.buttonDownTime = 0.0f;
        }
        if (skillButtonDown)
        {
            playerSkill.SkillAction();
        }

    }
}
