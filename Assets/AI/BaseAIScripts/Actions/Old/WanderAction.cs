using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Wander")]
public class WanderAction : Action {

    public override void Act(StateController controller)
    {
        Wander(controller);
    }

    private void Wander(StateController controller)
    {

        //Debug.Log("Wander called");

        if (controller.navMeshAgent.remainingDistance < 2)
        {
            var randomPosition = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));

            Vector3 newPost = controller.transform.position + randomPosition;

            controller.navMeshAgent.SetDestination(newPost);
            
            //Debug.Log("Wandering to new location");
        }
    }


	
}
