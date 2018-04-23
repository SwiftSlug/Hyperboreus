
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
            //  Target is a player
            if (controller.target.GetComponent<PlayerStats>().isDead)
            {
                //  Target is dead so set AI target to null
                //controller.target = null;
                controller.setTarget(null);
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (controller.target.GetComponent<TestBuildingController>())
        {
            //  Target is a building
            if(controller.target.GetComponent<TestBuildingController>().hitPoints <= 0)
            {
                //  Target has no hit points left so set target to null
                //controller.target = null;
                controller.setTarget(null);
                return true;
            }
            else
            {
                return false;
            }
            
        }

        return true;

    }

}