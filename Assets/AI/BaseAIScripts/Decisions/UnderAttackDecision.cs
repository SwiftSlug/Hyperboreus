using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/UnderAttackDecision")]
public class UnderAttackDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        return UnderAttack(controller);
    }

    private bool UnderAttack(StateController controller)
    {

        if (controller.underAttack != false)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}