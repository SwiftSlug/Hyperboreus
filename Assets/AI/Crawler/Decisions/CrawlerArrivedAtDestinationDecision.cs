using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/CrawlerArrivedAtDestination")]
public class CrawlerArrivedAtDestinationDecision : Decision {

    public float stoppingDistance = 3.0f;

    public override bool Decide(StateController controller)
    {
        return CrawlerArrivedAtDestination(controller);
    }

    private bool CrawlerArrivedAtDestination(StateController controller)
    {
        if (controller.navMeshAgent.remainingDistance < stoppingDistance)
        {            
            return true;
        }
        else
        {            
            return false;
        }
    }
}