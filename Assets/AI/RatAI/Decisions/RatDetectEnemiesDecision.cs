using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/RatDetectEnemiesDecision")]
public class RatDetectEnemiesDecision : Decision {

    public Transform gizmoLocation;

    public override bool Decide(StateController controller)
    {
        return RatDetectEnemy(controller);
    }

    private bool RatDetectEnemy(StateController controller)
    {
        //OnDrawGizmosSelected(controller);
        gizmoLocation = controller.transform;
        Collider[] hitColliders = Physics.OverlapSphere(controller.transform.position, controller.detectionRange);
        
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject.CompareTag("Player"))
            {
                Debug.Log("Player Seen Run Away !");
                controller.navMeshAgent.speed = controller.runSpeed;
                controller.target = hitColliders[i].gameObject;
                return true;
            }
        }

        Debug.Log("Ah nice and safe!");

        controller.navMeshAgent.speed = controller.walkSpeed;
        return false;

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(gizmoLocation.transform.position, 15);
    }

}



    
