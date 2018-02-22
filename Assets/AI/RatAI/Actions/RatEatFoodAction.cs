using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/RatEatFood")]
public class RatEatFoodAction : Action {

    //public bool test;

    public float arriveTime;

    public override void Act(StateController controller)
    {
        EatFood(controller);
    }

    private void EatFood(StateController controller)
    {
        controller.navMeshAgent.destination = controller.transform.position;

        controller.target.GetComponent<RatFood>().Eat();

        //  Rats growing functionality

        //  Make sure rat scale is not larger than the max value
        if (controller.transform.localScale.magnitude < (new Vector3(20.0f, 20.0f, 20.0f).magnitude))
        {
            //  Only grow if food has been eaten
            if (controller.target.GetComponent<RatFood>().eaten == true)
            {
                //  Only grow if eaten time was not long ago
                if (controller.target.GetComponent<RatFood>().lastEaten + 0.01 < Time.time)
                {
                    controller.transform.localScale = controller.transform.localScale + new Vector3(1.0f, 1.0f, 1.0f);
                }
            }
        }
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
