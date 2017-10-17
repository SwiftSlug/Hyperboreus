using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/CrawlerSearchTimePassed")]
public class CrawlerSearchTimePassedDecision : Decision {

    public override bool Decide(StateController controller)
    {
        return CrawlerSearchTimePassed(controller);
    }

    private bool CrawlerSearchTimePassed(StateController controller)
    {

        if (controller.CheckIfCountDownElapsed(controller.searchWaitTime))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

}
