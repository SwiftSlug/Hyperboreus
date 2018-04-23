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
        /*
        if (controller.target != null)
        {
            if (controller.target.GetComponent<TestBuildingController>())
            {
                if (controller.target.GetComponent<TestBuildingController>().hitPoints > 0)
                {
                    // If AI already has a building target and it has health
                    return true;
                }
            }
        }

        if (controller.target != null)
        {
            if (controller.target.GetComponent<PlayerStats>())
            {
                if (controller.target.GetComponent<PlayerStats>().currentHealth > 0)
                {
                    // If AI already has a player target and it has health
                    return false;
                }
            }
        }
        */

        Collider[] hitColliders = Physics.OverlapSphere(controller.transform.position, controller.detectionRange);

        foreach (Collider hit in hitColliders)
        {
            //Debug.Log(hit.name);
            if (hit.gameObject.GetComponentInParent<TestBuildingController>())
            {
                //Debug.Log("Building set as AI target by DetectBuildingDecision");

                //controller.target = hit.transform.parent.gameObject;
                controller.setTarget(hit.transform.parent.gameObject);
                //Debug.Log("Target Set to " + controller.target);
                return true;
            }
        }
        //Debug.Log("No building controller found");

        return false;
    }

}