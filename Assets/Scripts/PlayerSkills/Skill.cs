using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class Skill : NetworkBehaviour
{
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

}
