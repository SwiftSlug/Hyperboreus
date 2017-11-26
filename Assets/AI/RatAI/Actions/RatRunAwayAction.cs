using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/RatRunAway")]
public class RatRunAwayAction : Action {

    public override void Act(StateController controller)
    {
        RatRunAway(controller);   
    }

    private void RatRunAway(StateController controller)
    {
        //controller.navMeshAgent.speed = controller.runSpeed;
        if (controller.target)
        {
            Vector3 reverseVector = (-(controller.target.transform.position - controller.transform.position).normalized) * 50;
            controller.navMeshAgent.destination = reverseVector;
        }
    }

}
