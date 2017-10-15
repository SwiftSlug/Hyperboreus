using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/RatFindFood")]
public class RatFindFoodDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        return FindFood(controller);
    }

    private bool FindFood(StateController controller)
    {
        Collider[] hitColliders = Physics.OverlapSphere(controller.transform.position, controller.detectionRange);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].GetComponent<RatFood>())
            {
                if (!hitColliders[i].GetComponent<RatFood>().eaten)
                {
                    controller.target = hitColliders[i].gameObject;
                    return true;
                }
            }
        }
        return false;
    }

}
