using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/RatMoveToFood")]
public class RatMoveToFoodAction : Action {    

    public override void Act(StateController controller)
    {
        RatMoveToFood(controller);
    }

    private void RatMoveToFood(StateController controller)
    {
        
        controller.navMeshAgent.destination = controller.target.transform.position;
        
        /*
        Collider[] hitColliders = Physics.OverlapSphere(controller.transform.position, controller.detectionRange);
        
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].GetComponent<RatFood>())
            {               
                if (!hitColliders[i].GetComponent<RatFood>().eaten)
                {                    
                    controller.navMeshAgent.destination = hitColliders[i].transform.position;                   
                }
            }
        }
        */
    }

}