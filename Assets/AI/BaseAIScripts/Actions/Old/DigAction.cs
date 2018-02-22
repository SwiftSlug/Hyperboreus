using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Dig")]
public class DigAction : Action {

    public override void Act(StateController controller)
    {
        Dig(controller);
    }

    private void Dig(StateController controller)
    {
        //Debug.Log("Diggin here !");
        controller.navMeshAgent.SetDestination(controller.transform.position);
    }
	
}
