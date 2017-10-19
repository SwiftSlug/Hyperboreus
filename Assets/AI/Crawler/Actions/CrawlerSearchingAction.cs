using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/CrawlerSearch")]
public class CrawlerSearchingAction : Action {

    public override void Act(StateController controller)
    {
        CrawlerSerach(controller);
    }

    private void CrawlerSerach(StateController controller)
    {

        Vector3 randomUnitVector = new Vector3(Random.Range(-1, 1), 0.0f, Random.Range(-1, 1));
        //randomVector = randomVector.normalized;

        randomUnitVector = randomUnitVector * controller.wanderRange;

        //controller.navMeshAgent.destination = Vector3.Scale(controller.target.transform.position, randomUnitVector);

        //controller.navMeshAgent.destination = controller.navMeshAgent.destination + randomUnitVector;

        controller.navMeshAgent.destination = controller.target.transform.position;

    }


}
