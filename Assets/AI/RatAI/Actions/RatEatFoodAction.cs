using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/RatEatFood")]
public class RatEatFoodAction : Action {

    public bool test;

    public float arriveTime;

    public override void Act(StateController controller)
    {
        EatFood(controller);
    }

    private void EatFood(StateController controller)
    {
        controller.navMeshAgent.destination = controller.transform.position;

        controller.target.GetComponent<RatFood>().Eat();



        /*
        Collider[] hitColliders = Physics.OverlapSphere(controller.transform.position, controller.detectionRange);

        controller.navMeshAgent.destination = controller.transform.position;

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].GetComponent<RatFood>())
            {
                if (!hitColliders[i].GetComponent<RatFood>().eaten)
                {
                    hitColliders[i].GetComponent<RatFood>().Eat();
                }
            }
        }
        */
    }

}
