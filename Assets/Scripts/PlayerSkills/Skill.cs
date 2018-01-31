using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class Skill : NetworkBehaviour
{
    //  This will act as the base class for all our skills, It will be required for the
    //  PlayerSkill script to detect and use different skills assigned to the player blueprint

    //  Reference to the attached player
    public GameObject playerReference;

    //  Cooldown time for skill
    public float cooldown;

    //  The time the button has been held for
    public float buttonDownTime;

    //  The time of the last skill use
    public float lastUsedTime = 0.0f;

    //  Init function for each skill
    public abstract void Init();

    //  The primary action of the skill, this will run as long as the button is held
    public abstract bool SkillAction();

    //  Function call for when the skill button is released
    public abstract void ButtonRelease();

}
