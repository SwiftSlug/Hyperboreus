using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "PluggableAI/Decisions/CrawlerBasicPlayerDetection")]
public class CrawlerBasicPlayerDetectionDecision : Decision {

    public float basicDetectionMultiplier = 2.0f;

    public override bool Decide(StateController controller)
    {
        return CrawlerBasicPlayerDetection(controller);
    }

    private bool CrawlerBasicPlayerDetection(StateController controller)
    {
        Collider[] hitColliders = Physics.OverlapSphere(controller.transform.position, (controller.detectionRange * basicDetectionMultiplier));

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject.CompareTag("NetworkedPlayer"))
            {
                if (!hitColliders[i].gameObject.GetComponent<PlayerStats>().isDead)
                {
                    //controller.target = hitColliders[i].gameObject;
                    controller.setTarget(hitColliders[i].gameObject);
                    return true;
                }
                
            }
        }

        return false;

    }

}
