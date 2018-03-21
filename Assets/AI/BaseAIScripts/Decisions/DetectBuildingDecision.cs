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

        foreach (Collider hit in hitColliders)
        {
            //Debug.Log(hit.name);
            if (hit.gameObject.GetComponentInParent<TestBuildingController>())
            {
                //Debug.Log("Found Building Controller");
                
                controller.target = hit.gameObject;
                Debug.Log("Target Set to " + controller.target);
                return true;
            }
        }
        //Debug.Log("No building controller found");

        return false;
    }

}