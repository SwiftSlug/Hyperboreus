
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/TargetDeadDecision")]
public class TargetDeadDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        return TargetDead(controller);
    }

    private bool TargetDead(StateController controller)
    {

        if (controller.target == null)
        {
            return false;
        }

        if (controller.target.GetComponent<PlayerStats>().isDead)
        {
            return true;
        }

        return false;

    }

}