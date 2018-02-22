using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableAI/Decisions/DetectPlayerDecision")]
public class DetectPlayerDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        return DetectPlayer(controller);
    }

    private bool DetectPlayer(StateController controller)
    {
        Collider[] hitColliders = Physics.OverlapSphere(controller.transform.position, controller.detectionRange);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject.CompareTag("NetworkedPlayer"))
            {
                if (!hitColliders[i].gameObject.GetComponent<PlayerStats>().isDead)
                {
                    //Debug.Log("Player Seen Run Away !");
                    controller.navMeshAgent.speed = controller.runSpeed;
                    controller.target = hitColliders[i].gameObject;
                    return true;
                }
            }
        }

        //Debug.Log("Speed Set");

        controller.navMeshAgent.speed = controller.walkSpeed;
        return false;

    }


}

