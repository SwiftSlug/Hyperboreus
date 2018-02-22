using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "PluggableAI/Decisions/CrawlerTargetDead")]
public class CrawlerTargetDeadDecision : Decision {

    public override bool Decide(StateController controller)
    {
        return CrawlerTargetDead(controller);
    }

    private bool CrawlerTargetDead(StateController controller)
    {

        if (controller.target.GetComponent<PlayerStats>().isDead)
        {
            return true;
        }

            return false;

    }

}