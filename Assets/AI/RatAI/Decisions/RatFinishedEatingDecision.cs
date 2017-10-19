using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/RatFinishedEating")]
public class RatFinishedEatingDecision : Decision {

    public override bool Decide(StateController controller)
    {
        return FinishedEating(controller);
    }

    private bool FinishedEating(StateController controller)
    {
        if (controller.CheckIfCountDownElapsed(5))
        {
            return true;
        }
        else
        {
            return false;
        }
            
    }

}
