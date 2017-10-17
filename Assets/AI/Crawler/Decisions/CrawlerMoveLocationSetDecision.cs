using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/CrawlerMoveLocationSet")]
public class CrawlerMoveLocationSetDecision : Decision {

    public override bool Decide(StateController controller)
    {
        return CrawlerMoveLocationSet(controller);
    }

    private bool CrawlerMoveLocationSet(StateController controller)
    {

        if (controller.targetLocation == null)
        {
            return false;
        }
        else
        {            
            return true;
        }

    }

}

