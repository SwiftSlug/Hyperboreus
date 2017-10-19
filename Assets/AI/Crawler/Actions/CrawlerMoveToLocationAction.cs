using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/CrawlerMoveToLocationAction")]
public class CrawlerMoveToLocationAction : Action
{

    //public bool test;

    public override void Act(StateController controller)
    {
        CrawlerMoveToLocation(controller);
    }

    private void CrawlerMoveToLocation(StateController controller)
    {
        //Debug.Log("Moving to location ! ");
        controller.navMeshAgent.destination = controller.moveCommandLocation;
    }

}
