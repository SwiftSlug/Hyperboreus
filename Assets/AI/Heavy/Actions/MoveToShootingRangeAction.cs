using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/MoveToAttackRange")]
public class MoveToAttackRangeAction : Action
{

    public override void Act(StateController controller)
    {
        MoveToAttack(controller);
    }

    private void MoveToAttack(StateController controller)
    {
        if (controller.target)
        {

            //Vector3 vectorToTarget = controller.transform.position - controller.target.transform.position;

            controller.navMeshAgent.destination = controller.target.transform.position;
        }                

    }

}