using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSkills : NetworkBehaviour
{
    //  Current equipped player skill
    public Skill playerSkill1;

    //  Flags if the ability button is held down
    bool skillButton1Down;

	void Start () {
        Debug.Log("Start run on playerSkills");
        //  Finds the skill attached to the player and assigns that to its equipped skill
        playerSkill1 = GetComponent<Skill>();
        //  Calls the init function of the skill, if needed
        playerSkill1.playerReference = transform.gameObject;
        playerSkill1.Init();        
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButton("Skill1"))
        {
            skillButton1Down = true;
            playerSkill1.buttonDownTime = playerSkill1.buttonDownTime + Time.deltaTime;
        }
        else
        {
            if (skillButton1Down)
            {
                playerSkill1.buttonRelease();
                skillButton1Down = false;
            }
            
            playerSkill1.buttonDownTime = 0.0f;
        }

        if (skillButton1Down)
        {
            playerSkill1.SkillAction();
        }

    }
}
