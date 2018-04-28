using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/FindBuildingsNearPlayer")]
public class FindBuildingsNearPlayerDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        return FindBuildingsNearPlayer(controller);
    }

    private bool FindBuildingsNearPlayer(StateController controller)
    {

        if (controller.previousPlayerTarget == null)
        {
            //return false;
        }

        //int numberOfRuns = 0;


        //  Get a building target from the director for the target player and set that as target
        controller.setTarget(controller.directorReference.GetTargetableBuilding(controller.previousPlayerTarget));

        if (controller.target.GetComponent<TestBuildingController>())
        {
            return true;
        }
        else
        {
            return false;
        }

        /*

        Collider[] hitColliders = Physics.OverlapSphere(controller.previousPlayerTarget.transform.position, controller.detectionRange);

        foreach (Collider hit in hitColliders)
        {
            
            if (hit.gameObject.GetComponentInParent<TestBuildingController>())
            {
                //  Found a building object near the player

                float offset = 10.0f;    //  Offset multiplier

                Vector3 vectorToAI = (hit.gameObject.transform.position - controller.transform.position).normalized;    //  Vector from the buidling to the ai unit

                Vector3 positionOffset = vectorToAI * offset;   //  Vector used to offset building position for AI pathfinding

                Vector3 searchLocation = positionOffset + hit.transform.position;

                //  Check if AI can path to buidling
                NavMeshHit navMeshHit;
                NavMeshPath pathToBuilding = new NavMeshPath();

                if (NavMesh.SamplePosition(controller.transform.position, out navMeshHit, controller.detectionRange, NavMesh.AllAreas))
                {

                    //NavMesh.CalculatePath(navMeshHit.position, controller.previousPlayerTarget.transform.position, NavMesh.AllAreas, pathToBuilding);
                    NavMesh.CalculatePath(navMeshHit.position, searchLocation, NavMesh.AllAreas, pathToBuilding);

                    numberOfRuns++;

                    if (pathToBuilding.status == NavMeshPathStatus.PathComplete)
                    {
                        //  Can path to building so set as target

                        controller.setTarget(hit.transform.parent.gameObject);

                        //Debug.Log("Found a building near the player");

                        Debug.Log("One found with number of runs = " + numberOfRuns);

                        return true;
                    }
                }

            }            

        }
        Debug.Log("None found with number of runs = " + numberOfRuns);

        

        return false;

    */

    }

}


