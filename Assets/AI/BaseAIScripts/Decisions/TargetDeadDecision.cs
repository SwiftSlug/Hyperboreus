
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/TargetDeadDecision")]
public class TargetDeadDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        return TargetDead(controller);
    }

    private bool TargetDead(StateController controller)
    {

        if (controller.target == null)
        {
            return false;
        }
        if (controller.target.GetComponent<PlayerStats>())
        {
            if (controller.target.GetComponent<PlayerStats>().isDead)
            {
                //  Target is dead so set AI target to null
                controller.target = null;

                return true;
            }
        }
        else if (controller.target.GetComponent<TestPlayerBuilingController>())
        {
            /*
            if(controller.target.GetComponent<TestPlayerBuilingController>().buildingTemplate.GetComponent<BuildingController>().Hitpoints <= 0)
            {
                controller.target = null;
                return true;
            }
            */
        }       

        return false;

    }

}