using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/FindBuildingsNearPlayer")]
public class FindBuildingsNearPlayerDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        return FindBuildingsNearPlayer(controller);
    }

    private bool FindBuildingsNearPlayer(StateController controller)
    {

        if(controller.previousPlayerTarget == null)
        {
            return false;
        }

        Collider[] hitColliders = Physics.OverlapSphere(controller.previousPlayerTarget.transform.position, controller.detectionRange);

        foreach (Collider hit in hitColliders)
        {

            if (hit.gameObject.GetComponentInParent<TestBuildingController>())
            {
                controller.setTarget(hit.transform.parent.gameObject);

                Debug.Log("Found a building near the player");

                return true;
            }
        }

        return false;

    }

}


