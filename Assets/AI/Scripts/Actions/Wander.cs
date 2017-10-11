using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Wander")]
public class Wander : Action {

    public override void Act(StateController controller)
    {
        WanderAbout(controller);
    }

    private void WanderAbout(StateController controller)
    {
        if (controller.navMeshAgent.remainingDistance < 1)
        {
            var randomPosition = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));
            controller.navMeshAgent.SetDestination(randomPosition);
            Debug.Log("New Location Set");
        }
    }
	
}
