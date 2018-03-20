using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/DetectBuildingDecision")]
public class DetectBuildingDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        return DetectBuilding(controller);
    }

    private bool DetectBuilding(StateController controller)
    {

        Collider[] hitColliders = Physics.OverlapSphere(controller.transform.position, controller.detectionRange);

        foreach (Collider hits in hitColliders)
        {
            if (hits.GetComponent<AIStats>())
            {

            }

        }


        return false;
    }

}