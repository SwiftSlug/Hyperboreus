using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/TargetSetDecision")]
public class TargetSetDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        return TargetSet(controller);
    }

    private bool TargetSet(StateController controller)
    {

        if (controller.target != false)
        {
            
            if (controller.target.GetComponent<PlayerStats>() || controller.target.GetComponent<BuildingController>())
            {
                return true;
            }
            
        }
        return false;


    }
}

