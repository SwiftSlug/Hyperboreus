using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
                    //controller.navMeshAgent.speed = controller.runSpeed;

                    //  Ensure there is a path to the player before setting it as a target
                    if (CanPathToPlayer(controller, hitColliders[i].gameObject))
                    {
                        controller.target = hitColliders[i].gameObject;
                        return true;
                    }
                    else
                    {
                        //controller.target = null;
                        return false;
                    }
                }
            }
        }

        //Debug.Log("Speed Set");

        controller.navMeshAgent.speed = controller.walkSpeed;
        return false;

    }

    bool CanPathToPlayer(StateController controller, GameObject targetToPathTo)
    {
        if (targetToPathTo.GetComponent<PlayerStats>())
        {
            NavMeshHit navMeshHit;

            NavMeshPath pathToPlayer = new NavMeshPath();

            if (NavMesh.SamplePosition(controller.transform.position, out navMeshHit, controller.detectionRange, NavMesh.AllAreas))
            {
                NavMesh.CalculatePath(navMeshHit.position, targetToPathTo.transform.position, NavMesh.AllAreas, pathToPlayer);

                if (pathToPlayer.status == NavMeshPathStatus.PathComplete)
                {
                    //Debug.Log("can reach player");
                    return true;
                }
                else
                {
                    //Debug.Log("Can't reach player");
                }
            }


        }
        return false;
    }
}

