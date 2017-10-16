using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/CrawlerBasicPlayerDetection")]
public class CrawlerBasicPlayerDetectionDecision : Decision {

    public float basicDetectionMultiplier = 1.5f;

    public override bool Decide(StateController controller)
    {
        return CrawlerBasicPlayerDetection(controller);
    }

    private bool CrawlerBasicPlayerDetection(StateController controller)
    {
        Collider[] hitColliders = Physics.OverlapSphere(controller.transform.position, (controller.detectionRange * basicDetectionMultiplier));

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject.CompareTag("Player"))
            {  
                controller.target = hitColliders[i].gameObject;
                return true;
            }
        }

        return false;

    }

}
