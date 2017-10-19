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

        if (controller.moveCommandLocation != Vector3.zero) //  May be better to use a move to bool instead of the zero comparison
        {
            //Debug.Log("MoveLocation Set !");
            return true;
        }
        else
        {            
            return false;
        }

    }

}

