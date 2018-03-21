using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/CanReachPlayerDecision")]
public class CanReachPlayerDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        return CanReachPlayer(controller);
    }

    private bool CanReachPlayer(StateController controller)
    {
        if(controller.target != null){

            if (controller.target.GetComponent<PlayerStats>())
            {
                NavMeshHit navMeshHit;

                NavMeshPath pathToPlayer = new NavMeshPath();

                if (NavMesh.SamplePosition(controller.transform.position, out navMeshHit, controller.detectionRange, NavMesh.AllAreas))
                {
                    NavMesh.CalculatePath(navMeshHit.position, controller.target.transform.position, NavMesh.AllAreas, pathToPlayer);

                    if (pathToPlayer.status == NavMeshPathStatus.PathComplete)
                    {
                        Debug.Log("can reach player");
                        return true;
                    }
                    else
                    {
                        Debug.Log("Can't reach player");
                    }
                }

            }
        }
       
        return false;

    }

}
