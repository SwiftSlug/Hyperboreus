
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
            //  There is no target to it must have already been destroyed
            return true;
        }
        if (controller.target.GetComponent<PlayerStats>())
        {
            //  Target is a player
            if (controller.target.GetComponent<PlayerStats>().isDead)
            {
                //  Target was a player and is dead so set AI target to null

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
                //  Target was a building so set target back to previously targeted player if possible

                if (controller.previousPlayerTarget != null)
                {
                    controller.setTarget(controller.previousPlayerTarget);
                    Debug.Log("AI Target set back to previous player");
                }
                else
                {

                    //controller.target = null;
                    controller.setTarget(null);
                    Debug.Log("AI Target set to null");
                }
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