using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "PluggableAI/Decisions/CrawlerArrivedAtMoveLocationDecision")]
public class CrawlerArrivedAtMoveLocationDecsion : Decision {
    
    public override bool Decide(StateController controller)
    {
        return CrawlerArrivedAtMoveLocation(controller);
    }

    private bool CrawlerArrivedAtMoveLocation(StateController controller)
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

