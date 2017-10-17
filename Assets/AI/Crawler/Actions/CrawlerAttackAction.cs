using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/CrawlerAttack")]
public class CrawlerAttackAction : Action {

    public override void Act(StateController controller)
    {
        CrawlerAttack(controller);
    }

    private void CrawlerAttack(StateController controller)
    {
        controller.navMeshAgent.speed = controller.runSpeed;
        controller.navMeshAgent.destination = controller.target.transform.position;

    }

}