using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/MoveToLocationAction")]
public class MoveToLocationAction : Action
{

    //public bool test;

    public override void Act(StateController controller)
    {
        MoveToLocation(controller);
    }

    private void MoveToLocation(StateController controller)
    {
        //Debug.Log("Moving to location ! ");
        controller.navMeshAgent.destination = controller.moveCommandLocation;
    }

}

