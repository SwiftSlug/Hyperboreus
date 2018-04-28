using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/PreviousPlayerTargetSetDecision")]
public class PreviousPlayerTargetSetDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        return PreviousPlayerTargetSet(controller);
    }

    private bool PreviousPlayerTargetSet(StateController controller)
    {
        if (controller.previousPlayerTarget != null)
        {
            //  Set target to previous target and return true
            controller.setTarget(controller.previousPlayerTarget);
            return true;
        }
        else
        {
            return false;
        }


    }

}
