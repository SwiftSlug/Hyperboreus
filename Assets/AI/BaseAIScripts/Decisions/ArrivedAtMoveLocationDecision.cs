using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/BLEBPLBEPLBEPLBEPLArrivedAtMoveLocationDecision")]
public class ArrivedAtMoveLocationDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        return ArrivedAtMoveLocation(controller);
    }

    private bool ArrivedAtMoveLocation(StateController controller)
    {
        if (controller.navMeshAgent.remainingDistance < controller.stopDistance)
        {
            controller.moveCommandLocation = Vector3.zero;
            return true;
        }
        else
        {
            return false;
        }
    }
}

