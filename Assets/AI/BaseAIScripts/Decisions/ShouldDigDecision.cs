using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/ShouldDig")]
public class ShouldDigDecision : Decision {

    public bool test;

    public override bool Decide(StateController controller)
    {
        bool shouldDig = ShouldDig(controller);
        return shouldDig;
    }

    private bool ShouldDig(StateController controller)
    {

        if (controller.CheckIfCountDownElapsed(5))
        {
            //Debug.Log("Should Dig returning true !");
            return true;
            
        }
        else
        {
            //Debug.Log("Should Dig returning false !");
            return false;
        }

        
    }

}
